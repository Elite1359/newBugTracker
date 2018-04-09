using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace newBugTracker.Models
{
    public class DashboardViewModel
    {
        public ApplicationUser User { get; set; }
        public List<ApplicationUser> AllUsers { get; set; }
        public List<Project> Projects { get; set; }
        public List<Ticket> Tickets { get; set; }
        public List<Ticket> AllTickets { get; set; }
        public List<TicketNotification> Notifications { get; set; }
    }
}