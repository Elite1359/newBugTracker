using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace newBugTracker.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Ticket> Ticket { get; set; }
        public virtual ICollection<ApplicationUser> User { get; set; }

        public Project()
        {
            Ticket = new HashSet<Ticket>();
            User = new HashSet<ApplicationUser>();
        }
    }
}