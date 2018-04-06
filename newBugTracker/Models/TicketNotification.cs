using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace newBugTracker.Models
{
    public class TicketNotification
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime Created { get; set; }
        public int TicketId { get; set; }
        public string UserId { get; set; }
        public string SenderId { get; set; }
        public string RecipentId { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationUser Sender { get; set; }
        public virtual ApplicationUser Recipient { get; set; }

    }
}