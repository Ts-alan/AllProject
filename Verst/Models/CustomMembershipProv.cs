using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Security;
using Verst.Models.SiteAccountsTableAdapters;

namespace Verst.Models
{
    public class CustomMembershipProvider : MembershipProvider
    {
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer,
                                                  bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            try
            {
                var ta = new UsersTableAdapter() { Connection = new OleDbConnection(AccountsProcessing.GetConnection()) };
                ta.AddUser(username, password, "", "", "");
            }
            catch (Exception)
            {
                throw new Exception();
            }
            status = MembershipCreateStatus.Success;
            return null;
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion,string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            try
            {
                var password = AccountsProcessing.GetTable<SiteAccounts.UsersDataTable>()
                                      .First(x => x.Username == username)
                                      .Password;
                return password;
            }
            catch (Exception)
            {
                return "Error: User not found";
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            try
            {
                var UserID = AccountsProcessing.GetTable<SiteAccounts.UsersDataTable>().First(x => x.Username == username).UserID;
                var t1 = AccountsProcessing.GetTable<SiteAccounts.UsersDataTable>().FindByUserID(UserID);
                t1.Password = newPassword;
                t1.AcceptChanges();

               // AccountsProcessing.SaveChanges(table)
                var tr1 = new SiteAccounts().Users;
                tr1.AsEnumerable().First(x => x.Username == username).Password = newPassword;
                tr1.AcceptChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override string ResetPassword(string username, string answer)
        {
            try
            {
                var tr1 = new SiteAccounts().Users;
                tr1.AsEnumerable().First(x => x.Username == username).Password = "12345";
                tr1.AcceptChanges();
                return "Password was successfully reseted";
            }
            catch (Exception)
            {
                return "Password reset failed";
            }

            /*
            DataTable data = new UsersTableAdapter().GetData();
            var results = from t in (Users.UsersDataTable)data
                          where t.Login == username
                          select t.Password = "12345";
            if (results.Any())
            {
                data.AcceptChanges();
                return "Password was successfully reseted";
            }
            else
            {
                return "Password reset failed";
            }*/
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            var results =
                AccountsProcessing.GetTable<SiteAccounts.UsersDataTable>()
                                  .Where(x => x.Username == username && x.Password == password);
            if (results.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            try
            {
                var tr1 = new SiteAccounts().Users;
                tr1.AsEnumerable().First(x => x.Username == username).Delete();
                tr1.AcceptChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override string ApplicationName { get; set; }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }
    }
}