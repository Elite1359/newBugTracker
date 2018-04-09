using Microsoft.AspNet.Identity;
using newBugTracker.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace newBugTracker.Helpers
{
    public class TicketsHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public List<Ticket> GetProjectTickets(int id)
        {
            return db.Tickets.Where(t => t.ProjectId == id).ToList();
        }

        public ICollection<Ticket> ListUserTickets(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);
            var tickets = db.Tickets.Where(t => t.AssignedToUserId == userId).ToList();
            //var tickets = user.AssignedToUser.ToList();
            return (tickets);
        }

        public ICollection<Ticket> ListSubmitterTickets(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);
            var tickets = db.Tickets.Where(t => t.OwnerUserId == userId).ToList();
            //var tickets = user.AssignedToUser.ToList();
            return (tickets);
        }

        public bool IsUserOnTicket(string userId, int ticketId)
        {
            var AssignedToId = db.Tickets.FirstOrDefault(t => t.Id == ticketId).AssignedToUserId;
            //var userId = db.Users.Find(userId).Id;
            //var result = db.Tickets.Where(t => t.AssignedToUserId == userId).ToList;
            return (AssignedToId == userId);
        }


        //public bool IsUserOnProject(string UserId, int ProjectId)
        //{
        //    try
        //    {
        //        var usr = db.Users.Find(UserId);
        //        var result = db.Projects.Find(ProjectId).Users.Contains(usr);
        //        return result;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
        public void AddUserToTicket(string userId, int ticketId)
        {
            if (!IsUserOnTicket(userId, ticketId))
            {
                Ticket tick = db.Tickets.Find(ticketId);
                var user = db.Users.Find(userId);
                tick.AssignedToUser = user;
                db.SaveChanges();
            }
        }

        public void RemoveUserFromTicket(string userId, int ticketId)
        {
            if (IsUserOnTicket(userId, ticketId))
            {
                Ticket tick = db.Tickets.Find(ticketId);
                var delUser = db.Users.Find(userId);

                //tick.AssignedToUser.Remove(delUser);
                tick.AssignedToUser = null;
                db.Entry(tick).State = EntityState.Modified; //just saves this obj instance
                db.SaveChanges();
            }
        }

        #region Antonios Histories

        #endregion
        public void AddTicketHistory(Ticket oldTicket, Ticket newTicket)
        {
            //Each of these properties can trigger a history if they change
            var propList = new List<string>
            {
                "Title",
                "Description",
                "TicketTypeId",
                "TicketStatusId",
                "TicketPriorityId",
                "AssignedToUserId",
                "ProjectId"
            };

            //Write a for loop that loops through the properties of a Ticket
            foreach (var property in propList)
            {
                //Add an extra test for null property values
                if (newTicket.GetType().GetProperty(property).GetValue(newTicket) == null)
                    newTicket.GetType().GetProperty(property).SetValue(newTicket, "");

                if (oldTicket.GetType().GetProperty(property).GetValue(oldTicket) == null)
                {
                    oldTicket.GetType().GetProperty(property).SetValue(oldTicket, "");
                }
                //Having an issue with null property values...AssignToUserId
                var newValue = newTicket.GetType().GetProperty(property) == null ? "" : newTicket.GetType().GetProperty(property).GetValue(newTicket).ToString();
                var oldValue = oldTicket.GetType().GetProperty(property) == null ? "" : oldTicket.GetType().GetProperty(property).GetValue(oldTicket).ToString();

                if (newValue != oldValue)
                {
                    //Add TicketHistory...
                    var newTicketHistory = new TicketHistory();
                    newTicketHistory.UserId = HttpContext.Current.User.Identity.GetUserId();
                    newTicketHistory.Changed = DateTime.Now;
                    newTicketHistory.TicketId = newTicket.Id;

                    //Record Property name and Values
                    newTicketHistory.Property = property;
                    newTicketHistory.OldValue = oldValue;
                    newTicketHistory.NewValue = newValue;

                    db.TicketHistories.Add(newTicketHistory);
                    db.SaveChanges();
                }
            }
        }

        //Add an overloaded AddTicketNotification method
        public async Task AddTicketNotification(int ticketId, string oldAssignedToId, string newAssignedToId)
        {
            //if (String.IsNullOrEmpty(oldAssignedToId) && !String.IsNullOrEmpty(newAssignedToId))
            //    return;

            //Assigning
            if (!String.IsNullOrEmpty(oldAssignedToId) && !String.IsNullOrEmpty(newAssignedToId))
            {
                //Grab a copy of the Ticket
                var ticket = db.Tickets.AsNoTracking().Include("Project").FirstOrDefault(t => t.Id == ticketId);

                //Create a new TicketNotification and set some default properties
                var userId = HttpContext.Current.User.Identity.GetUserId();
                var ticketNotifications = new TicketNotification();
                ticketNotifications.SenderId = userId;
                ticketNotifications.Created = DateTime.Now;
                ticketNotifications.TicketId = ticketId;
                ticketNotifications.RecipentId = newAssignedToId;

                //Assemble body of the message
                var msgBody = new StringBuilder();
                msgBody.AppendFormat("Hello {0}, ", db.Users.FirstOrDefault(u => u.Id == newAssignedToId).FirstName);
                msgBody.AppendLine("");
                msgBody.AppendLine("you have been assigned to a Ticket, the pertinent information can be found below: ");
                msgBody.AppendLine(" Ticket Id: " + ticketId);
                msgBody.AppendLine(" Ticket Title: " + ticket.Title);
                msgBody.AppendLine(" Project Id: " + ticket.ProjectId);
                msgBody.AppendLine(" Project Title: " + ticket.Project.Name);
                msgBody.AppendLine("");
                msgBody.AppendLine("Let me know if you have any questions!");
                msgBody.AppendLine("");
                msgBody.AppendLine("Thank you,");
                msgBody.AppendLine(db.Users.FirstOrDefault(u => u.Id == userId).FirstName);

                //Set Body
                ticketNotifications.Body = msgBody.ToString();

                db.TicketNotifications.Add(ticketNotifications);
                db.SaveChanges();

                //Here I can also send an email notification...
                //Start code: Add code here to email password reset link
                var from = "BugTracker<wscottrobertson336@gmail.com>";
                var to = db.Users.Find(newAssignedToId).Email;
                var email = new MailMessage(from, to)
                {
                    Subject = "You have been assigned to a Ticket",
                    Body = msgBody.ToString(),
                    IsBodyHtml = true
                };

                var svc = new PersonalEmail();
                await svc.SendAsync(email);
                //End code:
            }

            //Re - Assigning
            //Add code here...
        }

    }

}