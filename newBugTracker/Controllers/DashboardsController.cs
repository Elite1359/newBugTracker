using Microsoft.AspNet.Identity;
using newBugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace newBugTracker.Controllers
{
        public class DashboardsController : Controller
        {
            private ApplicationDbContext db = new ApplicationDbContext();

            // GET: Dashboard
            public ActionResult AdminDashboard()
            {
                var userId = User.Identity.GetUserId();
                var model = new DashboardViewModel
                {
                    Projects = db.Users.Find(userId).Project.ToList(),
                    AllTickets = db.Tickets.OrderByDescending(x => x.Created).Take(5).ToList(),
                    AllUsers = db.Users.ToList(),
                };
                return View(model);
            }

            // GET: Dashboard
            public ActionResult ProjectManagerDashboard()
            {
                var userId = User.Identity.GetUserId();
                var model = new DashboardViewModel
                {
                    Tickets = db.Users.Find(userId).Project.SelectMany(t => t.Ticket).OrderByDescending(x => x.Created).Take(5).ToList(),
                    Projects = db.Users.Find(userId).Project.ToList(),
                    AllTickets = db.Tickets.ToList(),
                    AllUsers = db.Users.ToList(),
                };
                return View(model);
            }

            // GET: Dashboard
            public ActionResult DeveloperDashboard()
            {
                var userId = User.Identity.GetUserId();
                var model = new DashboardViewModel
                {
                    Tickets = db.Tickets.Where(u => u.AssignedToUserId == userId).OrderByDescending(x => x.Created).Take(5).ToList(),
                    Projects = db.Users.Find(userId).Project.ToList(),
                    AllTickets = db.Tickets.ToList(),
                    AllUsers = db.Users.ToList(),
                    Notifications = db.TicketNotifications.Where(n => n.Ticket.AssignedToUserId == userId).ToList()
                };
                return View(model);
            }

            // GET: Dashboard
            public ActionResult SubmitterDashboard()
            {
                var userId = User.Identity.GetUserId();
                var model = new DashboardViewModel
                {
                    Tickets = db.Tickets.Where(u => u.OwnerUserId == userId).OrderByDescending(x => x.Created).Take(5).ToList(),
                    Projects = db.Users.Find(userId).Project.ToList(),
                    AllTickets = db.Tickets.ToList(),
                    AllUsers = db.Users.ToList(),
                };
                return View(model);
            }

        [Authorize]
        public ActionResult Navigator()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("AdminDashboard", "Dashboards");
            }
            else if (User.IsInRole("ProjectManager"))
            {
                return RedirectToAction("ProjectManagerDashboard", "Dashboards");
            }
            else if (User.IsInRole("Developer"))
            {
                return RedirectToAction("DeveloperDashboard", "Dashboards");
            }
            else if (User.IsInRole("Submitter"))
            {
                return RedirectToAction("SubmitterDashboard", "Dashboards");
            }
            return RedirectToAction("Login", "Account");
        }

    }
}