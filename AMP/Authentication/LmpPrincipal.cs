using AMP.Models;
using AMP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace AMP.Authentication
{
    public class LmpPrincipal : IPrincipal
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string EmployeeNo { get; set; }
        public string Department { get; set; }
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }
        public int ContactId { get; set; }
        public byte[] Image { get; set; }
        public string[] Roles { get; set; }

        public IIdentity Identity
        {
            get; private set;
        }

        public bool IsInRole(string role)
        {
            if(Roles == null)
            {
                Roles = new string[0];
            }
            if (Roles.Any(r => role.Contains(r)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public LmpPrincipal(TBL_Users user)
        {
            UserId = user.Id;
            Username = user.Username;
            EmployeeNo = user.EmployeeNo;
            Department = user.Department;
            DisplayName = user.DisplayName;
            EmailAddress = user.EmailAddress;
            ContactId = user.ContactId;
            Image = user.TBL_Contacts.ContactImg;
            Roles = user.TBL_UserRoleMap.Select(r => r.TBL_Roles.RoleName).ToArray();
        }

        public LmpPrincipal(string username)
        {
            Identity = new GenericIdentity(username);
        }

        
    }
}