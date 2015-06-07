using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace Crystal.Authentication
{
    public static class AuthService
    {
        public static IMRPService mrpService;
        static AuthService()
        {
            //mrpService = new Mr        
        }

        public static IPrincipal GetPrincipal(IPrincipal principal)
        {
            return mrpService.GetPrincipal(principal);
        }

        public static bool ValidateUser(string userName, string userPassw)
        {
            return mrpService.ValidateUser(userName, userPassw);
        }

        public static int GetUserKeyHashCode(IPrincipal principal)
        {
            return mrpService.GetUserKeyHashCode(principal);
        }

        public static string GetUserProfileProperty(string userName, string propertyName)
        {
            return mrpService.GetUserProfileProperty(userName, propertyName);
        }
    }
}
