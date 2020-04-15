using AMP.Models;
using AMP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.Authentication
{
    public class UserConversion
    {
        public static LmpPrincipal Convertuser()
        {
            string contextuser = HttpContext.Current.User.Identity.Name;// HttpContext.User.Identity.Name;
            string[] splituser = AMPUtilities.SplitLoginName(contextuser);
            string username = splituser[0];
            if (splituser.Length > 1)
                username = splituser[1];
            using (AMPEntities context = new AMPEntities())
            {
                var user = context.TBL_Users.Where(us => string.Compare(username, us.Username, StringComparison.OrdinalIgnoreCase) == 0).FirstOrDefault();
                if (user != null)
                {
                    return new LmpPrincipal(user);
                }
                else
                    return new LmpPrincipal(new TBL_Users()
                    {
                        ContactId = 0,
                        Department = "Guest",
                        DisplayName = "Guest",
                        EmailAddress = "",
                        EmployeeNo = "",
                        Username = "Guest",
                        TBL_Contacts = new TBL_Contacts()
                    }
                    );
            }
        }
    }
}