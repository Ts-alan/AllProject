using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Web.Profile;
using System.Web.Security;

namespace WebApp
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "RoleProviderSvcWrap" в коде, SVC-файле и файле конфигурации.
    // ПРИМЕЧАНИЕ. Чтобы запустить клиент проверки WCF для тестирования службы, выберите элементы RoleProviderSvcWrap.svc или RoleProviderSvcWrap.svc.cs в обозревателе решений и начните отладку.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class MRPProviderSvcWrap : IMRPProviderSvcWrap
    {
        
        public Boolean IsUserInRole(string userName, string roleName) {
            return Roles.IsUserInRole(userName, roleName);
        }


        public int GetUserKeyHashCode(string userName)
        {
            var user = Membership.GetUser(userName);
            if (user == null) return 0;
            else return user.ProviderUserKey.GetHashCode();
        }


        public string[] GetRolesForUser(string userName)
        {
            return Roles.GetRolesForUser(userName);
        }

        public string GetUserProfileProperty(string userName, string propertyName)
        { 
            //System.Web.HttpContext.Current.Profile.PropertyValues
            return (string)ProfileBase.Create(userName).GetPropertyValue(propertyName);
        }

    }
}
