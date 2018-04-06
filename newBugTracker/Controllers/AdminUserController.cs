using newBugTracker.Helpers;
using newBugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace newBugTracker.Controllers
{
    public class AdminUserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult UserIndex()
        {
            return View(db.Users.ToList());
        }

        ////Assign Roles
        //public ActionResult RoleAssign(string id)
        //{
        //    UserRolesHelper roleHelper = new UserRolesHelper();

        //    var occupiedRole = roleHelper.ListUserRoles(id).FirstOrDefault();
        //    ViewBag.UserId = id;
        //    var mostRoles = db.Roles.Where(r => r.Name != "Admin");
        //    ViewBag.AllRoles = new SelectList(mostRoles, "Name", "Name", occupiedRole);
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult RoleAssign(string UserId, string AllRoles)
        //{
        //    UserRolesHelper roleHelper = new UserRolesHelper();

        //    //Add back any role that was selected
        //    if (String.IsNullOrEmpty(UserId))
        //    {
        //        return RedirectToAction("RoleAssign", new { id = UserId });
        //    }

        //    //Spin through all roles currently occupied removing the user from them all
        //    foreach (var role in roleHelper.ListUserRoles(UserId))
        //    {
        //        roleHelper.RemoveUserFromRole(UserId, role);
        //    }

        //    //Adding code to assign the user to the selected role
        //    if (!String.IsNullOrEmpty(AllRoles))
        //    {
        //        roleHelper.AddUserToRole(UserId, AllRoles);
        //    }
        //    return RedirectToAction("Index");
        //}

#region Old Code Role Assign

        [Authorize(Roles = "Admin")]
        public ActionResult RoleAssign(string id)
        {
            UserRolesHelper roleHelper = new UserRolesHelper();

            var occupiedRole = roleHelper.ListUserRoles(id).FirstOrDefault();
            ViewBag.UserId = id;
            var mostRoles = db.Roles.Where(r => r.Name != "Admin");
            ViewBag.AllRoles = new SelectList(mostRoles, "Name", "Name", occupiedRole);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAssign(string UserId, string AllRoles)
        {
            UserRolesHelper roleHelper = new UserRolesHelper();

            //Add back any role that was selected
            if (String.IsNullOrEmpty(UserId))
            {
                return RedirectToAction("RoleAssign", new { id = UserId });
            }

            //Spin through all roles currently occupied removing the user from them all
            foreach (var role in roleHelper.ListUserRoles(UserId))
            {
                roleHelper.RemoveUserFromRole(UserId, role);
            }

            //Adding code to assign the user to the selected role
            if (!String.IsNullOrEmpty(AllRoles))
            {
                roleHelper.AddUserToRole(UserId, AllRoles);
            }
            return RedirectToAction("UserIndex");
        }

        #endregion

#region Old Code Project Assign
        // GET: Admin
        public ActionResult ProjectAssign(string id)
        {
            ProjectHelper projectHelper = new ProjectHelper();
            //Get a list of projects i am already assigned to
            var myProjectsIds = projectHelper.ListUserProjects(id).Select(p => p.Id);
            ViewBag.UserId = id;
            ViewBag.AllProjects = new MultiSelectList(db.Projects, "Id", "Name", myProjectsIds);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectAssign(string UserId, List<int> AllProjects)
        {
            ProjectHelper projectHelper = new ProjectHelper();
            //First use the Project helper to remove this user from all Projects
            foreach (var project in projectHelper.ListUserProjects(UserId))
            {
                projectHelper.RemoveUserFromProject(UserId, project.Id);
            }
      
            //Next add the user to the selected Projects (AllProjects)
            if (AllProjects != null)
            {
                foreach (var projectId in AllProjects)
                {
                    projectHelper.AddUserToProject(UserId, projectId);
                }
            }
            return RedirectToAction("UserIndex","AdminUser");
        }
        #endregion



        // GET: Admin
        public ActionResult TicketAssign(string id)
        {
            TicketsHelper ticketHelper = new TicketsHelper();
            //Get a list of tickets i am already assigned to
            var myTicketIds = ticketHelper.ListUserTickets(id).Select(t => t.Id);
            ViewBag.UserId = id;
            ViewBag.AllTickets = new MultiSelectList(db.Tickets, "Id", "Title", myTicketIds);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TicketAssign(string UserId, List<int> AllTickets)
        {
            TicketsHelper ticketHelper = new TicketsHelper();
            //First use the Project helper to remove this user from all Tickets
            foreach (var ticket in ticketHelper.ListUserTickets(UserId))
            {
                ticketHelper.RemoveUserFromTicket(UserId, ticket.Id);
            }

            //Next add the user to the selected Projects (AllTickets)
            if (AllTickets != null)
            {
                foreach (var ticketId in AllTickets)
                {
                    ticketHelper.AddUserToTicket(UserId, ticketId);
                }
            }
            return RedirectToAction("UserIndex", "AdminUser");
        }
                       
        //Remover User Roles

        //Get
        public ActionResult EditUser(string id)
        {
            var user = db.Users.Find(id);
            AdminUserViewModel AdminModel = new AdminUserViewModel();
            UserRolesHelper helper = new UserRolesHelper();
            var selected = helper.ListUserRoles(id);
            AdminModel.Roles = new MultiSelectList(db.Roles, "Name", "Name", selected);
            AdminModel.User = new ApplicationUser();
            AdminModel.User.Id = user.Id;
            //AdminModel.User.FullName = user.FullName;

            return View(AdminModel);
        }

        //POST:  EditUser
        public ActionResult EditUser(AdminUserViewModel model)
        {
            var user = db.Users.Find(model.User.Id);
            UserRolesHelper helper = new UserRolesHelper();
            foreach (var role in db.Roles.Select(r => r.Name).ToList())
            {
                helper.RemoveUserFromRole(user.Id, role);
            }

            foreach (var role in model.SelectedRoles)
            {
                helper.AddUserToRole(user.Id, role);
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}