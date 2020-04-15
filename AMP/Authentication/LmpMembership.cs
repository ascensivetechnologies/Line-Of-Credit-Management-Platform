using AMP.Authentication;
using AMP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace AMP.Authentication
{
    public class LmpMembership : MembershipProvider
    {
        public override bool ValidateUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }
            using (AMPEntities context = new AMPEntities())
            {
                var user = context.TBL_Users.Where(us => string.Compare(username, us.Username, StringComparison.OrdinalIgnoreCase) == 0).FirstOrDefault();  
                return (user != null) ? true : false;
            }
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            using (AMPEntities context = new AMPEntities())
            {
                var user = context.TBL_Users.Where(us => string.Compare(username, us.Username, StringComparison.OrdinalIgnoreCase) == 0).FirstOrDefault();

                if (user == null)
                {
                    return null;
                }

                var selectedUser = new LmpMemberShipUser(user);

                return selectedUser;
            }
        }
        public override string GetUserNameByEmail(string email)
        {
            using (AMPEntities context = new AMPEntities())
            {
                var user = context.TBL_Users.Where(us => string.Compare(email, us.EmailAddress, StringComparison.OrdinalIgnoreCase) == 0).FirstOrDefault();
                if(user != null)
                {
                    return string.Empty;
                }
                else
                {
                    return user.Username;
                }
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
                
            }
        }

        public override bool EnablePasswordReset
        {
            get
            {
                return false;
            }
        }

        public override bool EnablePasswordRetrieval
        {
            get
            {
                return false;
            }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                return 3;
            }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                return 0;
            }
        }

        public override int MinRequiredPasswordLength
        {
            get
            {
                return 0;
            }
        }

        public override int PasswordAttemptWindow
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string PasswordStrengthRegularExpression
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool RequiresUniqueEmail
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }
        
    }
}