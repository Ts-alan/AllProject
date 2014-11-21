using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;

namespace VirusBlokAda.CC.DataBase
{
    /// <summary>
    /// IMPORTANT: This class should never be manually edited.
    /// Generated by VlslV CodeSmith Template.
    /// This class is used to manage the ProcessesEntity object.
    /// </summary>
    internal sealed class ProcessesManager
    {
        private readonly String connectionString;
        private readonly DbProviderFactory factory;

        #region Constructors
        public ProcessesManager(String connectionString):this(connectionString,"System.Data.SqlClient")
        {            
        }

        public ProcessesManager(String connectionString,String DbFactoryName):this(connectionString,DbProviderFactories.GetFactory(DbFactoryName))
        {            
        }
        public ProcessesManager(String connectionString, DbProviderFactory factory)
        {
            this.connectionString = connectionString;
            this.factory = factory;
            
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get process page
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <param name="order">Sort query</param>
        /// <param name="page">Page index</param>
        /// <param name="size">Page size</param>
        /// <returns></returns>
        internal List<ProcessesEntity> List(String where, String order, Int32 page, Int32 size)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetProcessesPage";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@page";
                param.Value = page;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@rowcount";
                param.Value = size;
                cmd.Parameters.Add(param);
               
                param=cmd.CreateParameter();
                param.ParameterName="@where";
                param.Value=where;
                cmd.Parameters.Add(param);

                param=cmd.CreateParameter();
                param.ParameterName="@orderby";
                param.Value=where;
                cmd.Parameters.Add(param);
                
                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<ProcessesEntity> list = new List<ProcessesEntity>();
                while (reader.Read())
                {
                    ProcessesEntity process = new ProcessesEntity();
                    if (reader.GetValue(0) != DBNull.Value)
                        process.ComputerName = reader.GetString(0);
                    if (reader.GetValue(1) != DBNull.Value)
                        process.ProcessName = reader.GetString(1);
                    if (reader.GetValue(2) != DBNull.Value)
                        process.MemorySize = reader.GetInt32(2);
                    if (reader.GetValue(3) != DBNull.Value)
                        process.LastDate = reader.GetDateTime(3);

                    list.Add(process);
                }

                reader.Close();
                return list;
            }
        }

        /// <summary>
        /// Get count of records with filter
        /// </summary>
        /// <param name="where">where clause</param>
        /// <returns></returns>
        internal Int32 Count(String where)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetProcessesCount";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@where";
                param.Value = where;
                cmd.Parameters.Add(param);

                con.Open();
                return (Int32)cmd.ExecuteScalar();
            }
        }

        #endregion
    }
}