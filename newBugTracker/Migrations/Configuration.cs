namespace newBugTracker.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using newBugTracker.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<newBugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(newBugTracker.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole();

                if (!context.Roles.Any(r => r.Name == "Admin"))
                {
                    role = new IdentityRole { Name = "Admin" };
                    manager.Create(role);
                }

                if (!context.Roles.Any(r => r.Name == "Developer"))
                {
                    role = new IdentityRole { Name = "Developer" };
                    manager.Create(role);
                }

                if (!context.Roles.Any(r => r.Name == "Submitter"))
                {
                    role = new IdentityRole { Name = "Submitter" };
                    manager.Create(role);
                }

                if (!context.Roles.Any(r => r.Name == "ProjectManager"))
                {
                    role = new IdentityRole { Name = "ProjectManager" };
                    manager.Create(role);
                }

                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                if (!context.Users.Any(u => u.Email == "admin@demo.com"))
                {
                    var user = new ApplicationUser
                    {
                        UserName = "admin@demo.com",
                        Email = "admin@demo.com",
                        FirstName = "William",
                        LastName = "Robertson"
                        //DisplayName = "ADMIN"
                    };

                    userManager.Create(user, "Abc&123!");

                    userManager.AddToRoles(user.Id,
                        new string[] {
                        "Admin"
                        });
                }
                if (!context.Users.Any(u => u.Email == "manager@demo.com"))
                {
                    var user = new ApplicationUser
                    {
                        UserName = "manager@demo.com",
                        Email = "manager@demo.com",
                        FirstName = "Tyler",
                        LastName = "Longbottom"
                        //DisplayName = "MANGR"
                    };

                    userManager.Create(user, "Abc&123!");

                    userManager.AddToRoles(user.Id,
                        new string[] {
                        "ProjectManager"
                        });
                }
                if (!context.Users.Any(u => u.Email == "developer@demo.com"))
                {
                    var user = new ApplicationUser
                    {
                        UserName = "developer@demo.com",
                        Email = "developer@demo.com",
                        FirstName = "John",
                        LastName = "Mathias"
                        //DisplayName = "DEVPR"
                    };

                    userManager.Create(user, "Abc&123!");

                    userManager.AddToRoles(user.Id,
                        new string[] {
                        "Developer"
                        });
                }
                if (!context.Users.Any(u => u.Email == "submitter@demo.com"))
                {
                    var user = new ApplicationUser
                    {
                        UserName = "submitter@demo.com",
                        Email = "submitter@demo.com",
                        FirstName = "Reid",
                        LastName = "Thompson"
                        //DisplayName = "SUBMT"
                    };

                    userManager.Create(user, "Abc&123!");

                    userManager.AddToRoles(user.Id,
                        new string[] {
                        "Submitter"
                        });
                }


                if (!context.TicketPriorities.Any(u => u.Name == "High"))
                { context.TicketPriorities.Add(new TicketPriority { Name = "High" }); }

                if (!context.TicketPriorities.Any(u => u.Name == "Medium"))
                { context.TicketPriorities.Add(new TicketPriority { Name = "Medium" }); }

                if (!context.TicketPriorities.Any(u => u.Name == "Low"))
                { context.TicketPriorities.Add(new TicketPriority { Name = "Low" }); }

                if (!context.TicketPriorities.Any(u => u.Name == "Urgent"))
                { context.TicketPriorities.Add(new TicketPriority { Name = "Urgent" }); }

                if (!context.TicketTypes.Any(u => u.Name == "Production Fix"))
                { context.TicketTypes.Add(new TicketType { Name = "Production Fix" }); }

                if (!context.TicketTypes.Any(u => u.Name == "Project Task"))
                { context.TicketTypes.Add(new TicketType { Name = "Project Task" }); }

                if (!context.TicketTypes.Any(u => u.Name == "Software Update"))
                { context.TicketTypes.Add(new TicketType { Name = "Software Update" }); }

                if (!context.TicketStatuses.Any(u => u.Name == "New"))
                { context.TicketStatuses.Add(new TicketStatus { Name = "New" }); }

                if (!context.TicketStatuses.Any(u => u.Name == "In Development"))
                { context.TicketStatuses.Add(new TicketStatus { Name = "In Development" }); }

                if (!context.TicketStatuses.Any(u => u.Name == "Completed"))
                { context.TicketStatuses.Add(new TicketStatus { Name = "Completed" }); }


            }
        }
    }
}