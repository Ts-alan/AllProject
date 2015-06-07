using Crystal.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

using System.Web.Security;

namespace Authentication.Membership
{
    public class MRPServiceBase : IMRPService
    {
        public IPrincipal GetPrincipal(IPrincipal originalPrincipal)
        {
            return originalPrincipal;
        }

        public int GetUserKeyHashCode(IPrincipal principal)
        {
            var user = System.Web.Security.Membership.GetUser(principal.Identity.Name);
            if (user == null) return 0;
            else return user.ProviderUserKey.GetHashCode();
        }


        public bool ValidateUser(string userName, string userPassword)
        {
            return System.Web.Security.Membership.ValidateUser(userName, userPassword);
        }


        public string GetUserProfileProperty(string userName, string propertyName)
        {
            return (string)System.Web.Profile.ProfileBase.Create(userName).GetPropertyValue(propertyName);
        }
    }
}
