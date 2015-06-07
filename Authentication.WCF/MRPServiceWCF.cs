using Crystal.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace Authentication.WCF
{
    public class MRPServiceWCF : IMRPService
    {
        public IPrincipal GetPrincipal(IPrincipal originalPrincipal)
        {
            return new WCFPrincipal(originalPrincipal);
        }

        public int GetUserKeyHashCode(IPrincipal principal)
        {
            return ((WCFPrincipal)principal).UserKeyHashCode;
        }


        public bool ValidateUser(string userName, string userPassword)
        {
            AuthenticationServiceClient authService = new AuthenticationServiceClient();
            var rez = authService.ValidateUser(userName, userPassword, "");
            authService.Close();
            return rez;
        }


        public string GetUserProfileProperty(string userName, string propertyName)
        {
            string result;
            MRPProviderSvcWrapClient mrpClient = new MRPProviderSvcWrapClient();
            {
                result = mrpClient.GetUserProfileProperty(userName, propertyName);
            }
            mrpClient.Close();
            return result;
        }
    }

    class WCFPrincipal : IPrincipal
    {
        private IPrincipal principal;
        private int userKeyHashCode;
        private string[] userRoles;
        public int UserKeyHashCode { get { return userKeyHashCode; } }

        public WCFPrincipal(IPrincipal originalPrincipal)
        {
            principal = originalPrincipal;
            MRPProviderSvcWrapClient mrpClient = new MRPProviderSvcWrapClient();
            {
                userKeyHashCode = mrpClient.GetUserKeyHashCode(principal.Identity.Name);
                userRoles = mrpClient.GetRolesForUser(principal.Identity.Name);
            }
            mrpClient.Close();
        }

        public System.Security.Principal.IIdentity Identity
        {
            get { return principal.Identity; }
        }

        public bool IsInRole(string role)
        {
            return userRoles.Contains(role);
            //using (MRPProviderSvcWrapClient client = new MRPProviderSvcWrapClient())
            //{
            //    return client.IsUserInRole(principal.Identity.Name, role);
            //}
        }
    }
}
