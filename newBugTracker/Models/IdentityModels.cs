using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using newBugTracker.Models;

namespace newBugTracker.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string FullName { get; set; }
        public string FullName
        {
            get { return this.FirstName + " " + this.LastName; }
        }

        public virtual ICollection<TicketNotification> TicketNotification { get; set; }
        public virtual ICollection<TicketHistory> TicketHistory { get; set; }
        public virtual ICollection<TicketComment> TicketComment { get; set; }
        public virtual ICollection<TicketAttachment> TicketAttachment { get; set; }

        public virtual ICollection<Ticket> AssignedToUser { get; set; }
        public virtual ICollection<Ticket> OwnerUser { get; set; }


        public virtual ICollection<Project> Project { get; set; }

        public ApplicationUser()
        {
            TicketNotification = new HashSet<TicketNotification>();
            TicketHistory = new HashSet<TicketHistory>();
            TicketComment = new HashSet<TicketComment>();
            TicketAttachment = new HashSet<TicketAttachment>();
            //???????
            AssignedToUser = new HashSet<Ticket>();
            OwnerUser = new HashSet<Ticket>();
            //????????
            Project = new HashSet<Project>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketComment> TicketComments { get; set; }
        public DbSet<TicketAttachment> TicketAttachments { get; set; }
        public DbSet<TicketHistory> TicketHistories { get; set; }
        public DbSet<TicketNotification> TicketNotifications { get; set; }
        public DbSet<TicketStatus> TicketStatuses { get; set; }
        public DbSet<TicketPriority> TicketPriorities { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<Project> Projects { get; set; }
    }
}