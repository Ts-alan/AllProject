using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace WebConsoleTests.ARM2_DbControl.Database.Provider
{
    static class DBHelper
    {
        public static String connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString;
        public static  void InitDataBase()
        {
            string script = File.ReadAllText(@"..\..\..\..\src\DB\VbaControlCenterDB\Scripts\Post-Deployment\Script.PostDeployment.sql");

            // split script on GO command
            IEnumerable<string> commandStrings = Regex.Split(script, @"^\s*GO\s*$",
                                     RegexOptions.Multiline | RegexOptions.IgnoreCase);
            using (SqlConnection Connection = new SqlConnection (connectionString))
            {
                Connection.Open();
                foreach (string commandString in commandStrings)
                {
                    if (commandString.Trim() != "")
                    {
                        new SqlCommand(commandString, Connection).ExecuteNonQuery();
                    }
                }
            }
        }
        
    }
}
