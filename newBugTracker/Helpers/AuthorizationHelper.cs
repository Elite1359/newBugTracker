using Microsoft.AspNet.Identity;
using newBugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace newBugTracker.Helpers
{
    public class AuthorizationHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public static bool Authorization(string ownerId, string assignedToUser, int projectId)
        {
            ProjectHelper projHelper = new ProjectHelper();
            UserRolesHelper userRoleHelper = new UserRolesHelper();
            var user = HttpContext.Current.User.Identity.GetUserId().ToString();
            if (userRoleHelper.IsUserInRole(user, "Admin"))
            {
                return true;
            }
            if (userRoleHelper.IsUserInRole(user, "Submitter") && user == ownerId)
            {
                return true;
            }
            if (userRoleHelper.IsUserInRole(user, "ProjectManager") && projHelper.IsUserOnProject(user, projectId))
            {
                return true;
            }
            if (userRoleHelper.IsUserInRole(user, "Developer") && user == assignedToUser)
            {
                return true;
            }
            return false;
        }
    }
}