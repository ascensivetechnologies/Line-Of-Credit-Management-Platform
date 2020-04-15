using AMP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace AMP.Authentication
{
    
    public class LmpMemberShipUser : MembershipUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string EmployeeNo { get; set; }
        public string Department { get; set; }
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }
        public int ContactId { get; set; }
        public TBL_Contacts Contact { get; set; }
        public ICollection<TBL_UserRoleMap> Roles { get; set; }

        public LmpMemberShipUser(TBL_Users user):base("LmpMembership", user.Username, user.Id, user.EmailAddress, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)  
        {
            Id = user.Id;
            Username = user.Username;
            EmployeeNo = user.EmployeeNo;
            Department = user.Department;
            DisplayName = user.DisplayName;
            EmailAddress = user.EmailAddress;
            ContactId = user.ContactId;
            Contact = user.TBL_Contacts;
            Roles = user.TBL_UserRoleMap;
        }
    }
}