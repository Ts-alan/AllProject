using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

namespace VirusBlokAda.CC.DataBase
{
    internal sealed class TemporaryGroupManager
    {
        private readonly String connectionString;
        private readonly DbProviderFactory factory;

        #region Constructors
        public TemporaryGroupManager(String connectionString):this(connectionString,"System.Data.SqlClient")
        {            
        }

        public TemporaryGroupManager(String connectionString,String DbFactoryName):this(connectionString,DbProviderFactories.GetFactory(DbFactoryName))
        {            
        }
        public TemporaryGroupManager(String connectionString, DbProviderFactory factory)
        {
            this.connectionString = connectionString;
            this.factory = factory;
            
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get computer name list by type
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="where">Filter query</param>
        /// <returns>Computer name list</returns>
        internal List<String> GetComputerNameList(String type, String where)
        {
            switch (type)
            {
                case "COMPONENTS":
                    return GetComputerNameListFromComponents(where);
                case "COMPUTERS":
                    return GetComputerNameListFromComputers(where);
                case "EVENTS":
                    return GetComputerNameListFromEvents(where);
                case "PROCESSES":
                    return GetComputerNameListFromProcesses(where);
                case "TASKS":
                    return GetComputerNameListFromTasks(where);
            }

            return null;
        }

        /// <summary>
        /// Get computer name list from process
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <returns>Computer name list</returns>
        private List<String> GetComputerNameListFromProcesses(String where)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetComputerNameListFromProcesses";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@Where";
                param.Value = where;
                cmd.Parameters.Add(param);

                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<String> list = new List<String>();
                while (reader.Read())
                {
                    if (reader.GetValue(0) != DBNull.Value)
                        list.Add(reader.GetString(0));
                }

                reader.Close();
                return list;
            }
        }

        /// <summary>
        /// Get computer name list from event
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <returns>Computer name list</returns>
        private List<String> GetComputerNameListFromEvents(String where)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetComputerNameListFromEvents";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@Where";
                param.Value = where;
                cmd.Parameters.Add(param);
               
                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<String> list = new List<String>();
                while (reader.Read())
                {
                    if (reader.GetValue(0) != DBNull.Value)
                        list.Add(reader.GetString(0));
                }

                reader.Close();
                return list;
            }
        }

        /// <summary>
        /// Get computer name list from task
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <returns>Computer name list</returns>
        private List<String> GetComputerNameListFromTasks(String where)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetComputerNameListFromTasks";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@Where";
                param.Value = where;
                cmd.Parameters.Add(param);

                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<String> list = new List<String>();
                while (reader.Read())
                {
                    if (reader.GetValue(0) != DBNull.Value)
                        list.Add(reader.GetString(0));
                }

                reader.Close();
                return list;
            }
        }

        /// <summary>
        /// Get computer name list from component
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <returns>Computer name list</returns>
        private List<String> GetComputerNameListFromComponents(String where)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetComputerNameListFromComponents";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@Where";
                param.Value = where;
                cmd.Parameters.Add(param);
               
                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<String> list = new List<String>();
                while (reader.Read())
                {
                    if (reader.GetValue(0) != DBNull.Value)
                        list.Add(reader.GetString(0));
                }

                reader.Close();
                return list;
            }
        }

        /// <summary>
        /// Get computer name list from computer
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <returns>Computer name list</returns>
        private List<String> GetComputerNameListFromComputers(String where)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetComputerNameListFromComputers";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@Where";
                param.Value = where;
                cmd.Parameters.Add(param);

                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<String> list = new List<String>();
                while (reader.Read())
                {
                    if (reader.GetValue(0) != DBNull.Value)
                        list.Add(reader.GetString(0));
                }

                reader.Close();
                return list;
            }
        }

        #endregion
    }
}
