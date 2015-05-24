using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Verst.Models.SiteAccountsTableAdapters;

namespace Verst.Models
{
    public class CustomRoleProvider : RoleProvider
    {

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            var user = AccountsProcessing.GetTable<SiteAccounts.UsersDataTable>().Where(x=>x.Username==username);
            if (user.Any())
            {
                var userRoles = from t1 in user
                                join t2 in AccountsProcessing.GetTable<SiteAccounts.UsersInRoleDataTable>()
                                    on t1.UserID equals t2.UserID
                                join roles in AccountsProcessing.GetTable<SiteAccounts.RolesDataTable>()
                                    on t2.RoleID equals roles.RoleID
                                select new {usrRoles = roles.RoleName};
                string usrroles = userRoles.Select(x=>x.usrRoles).Aggregate((current, next) => current +"/"+ next);
                return usrroles.Split('/');
            }
            return new string[] { "none" };
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName { get; set; }
    }
}