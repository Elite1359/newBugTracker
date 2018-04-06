using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace newBugTracker.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public int ProjectId { get; set; }
        public int TicketTypeId { get; set; }
        public int TicketPriorityId { get; set; }
        public int TicketStatusId { get; set; }
        public string OwnerUserId { get; set; }
        public string AssignedToUserId { get; set; }

        public virtual ICollection<TicketComment> TicketComment { get; set; }
        public virtual ICollection<TicketAttachment> TicketAttachment { get; set; }
        public virtual ICollection<TicketHistory> TicketHistory { get; set; }
        public virtual ICollection<TicketNotification> TicketNotification { get; set; }

        public Ticket()
        {
            TicketComment = new HashSet<TicketComment>();
            TicketAttachment = new HashSet<TicketAttachment>();
            TicketHistory = new HashSet<TicketHistory>();
            TicketNotification = new HashSet<TicketNotification>();
        }

        public virtual TicketStatus TicketStatus { get; set; }
        public virtual TicketPriority TicketPriority { get; set; }
        public virtual TicketType TicketType { get; set; }
        public virtual Project Project { get; set; }
        public virtual ApplicationUser OwnerUser { get; set; }
        public virtual ApplicationUser AssignedToUser { get; set; }
    }
}