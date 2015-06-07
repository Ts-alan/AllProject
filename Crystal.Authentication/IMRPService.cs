using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace Crystal.Authentication
{
    public interface IMRPService
    {
        IPrincipal GetPrincipal(IPrincipal originalPrincipal);
        bool ValidateUser(string userName, string userPassword);
        int GetUserKeyHashCode(IPrincipal principal);
        string GetUserProfileProperty(string userName, string propertyName);
    }
}
