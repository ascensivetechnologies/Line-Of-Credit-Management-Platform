using AMP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace AMP.Authentication
{
    public class LmpRole : RoleProvider
    {

        public override bool IsUserInRole(string username, string roleName)
        {
            var userRoles = GetRolesForUser(username);
            return userRoles.Contains(roleName);
        }

        public override string[] GetRolesForUser(string username)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return null;
            }

            var userRoles = new string[] { };

            using (AMPEntities dbContext = new AMPEntities())
            {
                var selectedUser = (from us in dbContext.TBL_Users.Include("Roles")
                                    where string.Compare(us.Username, username, StringComparison.OrdinalIgnoreCase) == 0
                                    select us).FirstOrDefault();


                if (selectedUser != null)
                {
                    userRoles = selectedUser.TBL_UserRoleMap.Select(r => r.TBL_Roles.RoleName).ToArray();
                }

                return userRoles.ToArray();
            }
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}