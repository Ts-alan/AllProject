using System.Data;
using System.Data.OleDb;
using System.Web;
using System.Web.Mvc;
using MvcApplication.Models;
using Crystal.Models.SiteAccountsTableAdapters;

namespace Crystal.Models
{
    public class AccountsProcessing
    {
        static readonly OleDbConnection DbConn = new OleDbConnection(GetConnection());
        static OleDbDataAdapter _adapter = new OleDbDataAdapter();
        static DataTable _tbl;

        /// <summary>
        /// Получает строку подключения к БД из webConfig
        /// </summary>
        /// <returns></returns>
        public static string GetConnection()
        {
            const string expectedConnectionString = "SiteAccounts";
            System.Configuration.ConnectionStringSettings connectionStringSettings = System.Configuration.ConfigurationManager.ConnectionStrings[expectedConnectionString];
            if (connectionStringSettings == null)
            {
                throw new System.Configuration.ConfigurationErrorsException(string.Format("Error ConnectionString in Web.config\nExpected: {0}", expectedConnectionString));
            }
            return connectionStringSettings.ConnectionString;
        }

        /// <summary>
        /// Получить таблицу из базы данных пользователей
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetTable<T>() where T : global::System.Data.DataTable, new()
        {
            _tbl = new T();
            string dbName = _tbl.TableName;
            _adapter.SelectCommand = new OleDbCommand("select * from " + dbName) { Connection = DbConn };
                _adapter.Fill(_tbl);
            return (T)_tbl;
        }

        public static UsersTableAdapter GetUsersTableAdapter()
        {
            var tableAdapter = new UsersTableAdapter { Connection = DbConn };
            return tableAdapter;
        }

        public static UsersInRoleTableAdapter GetUsersInRoleTableAdapter()
        {
            var tableAdapter = new UsersInRoleTableAdapter { Connection = DbConn };
            return tableAdapter;
        }

        public static RolesTableAdapter GetRolesTableAdapter()
        {
            var tableAdapter = new RolesTableAdapter { Connection = DbConn };
            return tableAdapter;
        }
        
    }

    public class PlantAuthorizationAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return base.AuthorizeCore(httpContext) && httpContext.User.IsInRole(FoxRepo.GetPlant().ToString());
        }
    }
}