using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.Security;

namespace WebApp
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IRoleProviderSvcWrap" в коде и файле конфигурации.
    [ServiceContract]
    public interface IMRPProviderSvcWrap
    {
        [OperationContract]
        bool IsUserInRole(string userName, string roleName);

        [OperationContract]
        int GetUserKeyHashCode(string userName);

        [OperationContract]
        string[] GetRolesForUser(string userName);

        [OperationContract]
        string GetUserProfileProperty(string userName, string propertyName);
    }
}
