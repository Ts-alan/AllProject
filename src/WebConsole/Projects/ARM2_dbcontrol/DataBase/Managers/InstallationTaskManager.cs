using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;

namespace VirusBlokAda.CC.DataBase
{
    internal sealed class InstallationTaskManager
    {
        private readonly String connectionString;
		private readonly DbProviderFactory factory;

        #region Constructors
        public InstallationTaskManager(String connectionString):this(connectionString,"System.Data.SqlClient")
        {            
        }

        public InstallationTaskManager(String connectionString,String DbFactoryName):this(connectionString,DbProviderFactories.GetFactory(DbFactoryName))
        {            
        }
        public InstallationTaskManager(String connectionString, DbProviderFactory factory)
        {
            this.connectionString = connectionString;
            this.factory = factory;
            
        }
        #endregion
		#region Methods

        /// <summary>
		/// Update task in database
		/// </summary>
        internal void UpdateTask(InstallationTaskEntity task)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "UpdateInstallationTask";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ID";
                param.Value = task.ID;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@Status";
                param.Value = task.Status;
                cmd.Parameters.Add(param);

                param=cmd.CreateParameter();
                param.ParameterName="@Exitcode";
                param.Value=task.ExitCode;
                cmd.Parameters.Add(param);

                param=cmd.CreateParameter();
                param.ParameterName="@Error";
                param.Value=task.Error;
                cmd.Parameters.Add(param);
               
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Insert task in database
        /// </summary>
        internal Int64 InsertTask(InstallationTaskEntity task)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "InsertInstallationTask";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ComputerName";
                param.Value = task.ComputerName;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@IPAddress";
                param.Value = task.IPAddress;
                cmd.Parameters.Add(param);

                param=cmd.CreateParameter();
                param.ParameterName="@Status";
                param.Value=task.Status;
                cmd.Parameters.Add(param);

                param=cmd.CreateParameter();
                param.ParameterName="@Date";
                param.Value=DateTime.Now;
                cmd.Parameters.Add(param);

                param=cmd.CreateParameter();
                param.ParameterName="@ExitCode";
                param.Value=task.ExitCode;
                cmd.Parameters.Add(param);
               
                con.Open();
                return Convert.ToInt64(cmd.ExecuteScalar());
            }
        }

        /// <summary>
        /// Get installation task page
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <param name="order">Sort query</param>
        /// <param name="page">Page index</param>
        /// <param name="size">Page size</param>
        /// <returns></returns>
        internal List<InstallationTaskEntity> List(String where, String order, Int16 page, Int16 size)
		{
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetInstallationTasks";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@Page";
                param.Value = page;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@RowCount";
                param.Value = size;
                cmd.Parameters.Add(param);
               
                param=cmd.CreateParameter();
                param.ParameterName="@Where";
                param.Value=where;
                cmd.Parameters.Add(param);

                param =cmd.CreateParameter();
                param.ParameterName="@OrderBy";
                param.Value=order;
                cmd.Parameters.Add(param);

                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<InstallationTaskEntity> list = new List<InstallationTaskEntity>();
                while (reader.Read())
                {
                    InstallationTaskEntity task = new InstallationTaskEntity();
                    if (reader.GetValue(0) != DBNull.Value)
                        task.ID = reader.GetInt32(0);
                    if (reader.GetValue(1) != DBNull.Value)
                        task.ComputerName = reader.GetString(1);
                    if (reader.GetValue(2) != DBNull.Value)
                        task.IPAddress = reader.GetString(2);
                    if (reader.GetValue(3) != DBNull.Value)
                        task.Status = reader.GetString(3);
                    if (reader.GetValue(4) != DBNull.Value)
                        task.InstallationDate = reader.GetDateTime(4);
                    if (reader.GetValue(5) != DBNull.Value)
                        task.ExitCode = reader.GetInt16(5);
                    if (reader.GetValue(6) != DBNull.Value)
                        task.Error = reader.GetString(6);

                    list.Add(task);
                }

                reader.Close();
                return list;
            }
		}

        /// <summary>
        /// Get installation task count
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <returns>Count of installation tasks</returns>
        internal Int32 Count(String where)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetInstallationTasksCount";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@Where";
                param.Value = where;
                cmd.Parameters.Add(param);
                
                con.Open();
                return (Int32)cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Get installation task status list
        /// </summary>
        /// <returns></returns>
        internal List<String> GetStatuses()
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetListInstallationStatus";

                
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
        /// Get Vba32 version list
        /// </summary>
        /// <returns></returns>
        internal List<String> GetVba32Versions()
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetListVba32Versions";
               
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
        /// Get computer name list
        /// </summary>
        /// <returns></returns>
        internal List<String> GetComputerNames()
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandText = "SELECT DISTINCT([ComputerName]) FROM InstallationTasks ORDER BY [ComputerName] ASC";

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
