using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

namespace VirusBlokAda.CC.DataBase
{
    internal sealed class ScanningObjectManager
    {
        private readonly String connectionString;
		private readonly DbProviderFactory factory;

        #region Constructors
        public ScanningObjectManager(String connectionString):this(connectionString,"System.Data.SqlClient")
        {            
        }

        public ScanningObjectManager(String connectionString,String DbFactoryName):this(connectionString,DbProviderFactories.GetFactory(DbFactoryName))
        {            
        }
        public ScanningObjectManager(String connectionString, DbProviderFactory factory)
        {
            this.connectionString = connectionString;
            this.factory = factory;
            
        }
        #endregion
		
		#region Methods

        /// <summary>
        /// Add comment in database
        /// </summary>
        internal void AddComment(ScanningObjectEntity entity)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AddCommentByIP";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@IP";
                param.Value = entity.IPAddress;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@Comment";
                param.Value = entity.Comment;
                cmd.Parameters.Add(param);
               
                con.Open();
                cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Delete comment in database
        /// </summary>
        internal void DeleteComment(String ip)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteCommentByIP";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@IP";
                param.Value = ip;
                cmd.Parameters.Add(param);
               
                con.Open();
                cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Get comment by IPAddress
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        internal String GetComment(String ip)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetCommentByIP";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@IP";
                param.Value = ip;
                cmd.Parameters.Add(param);

                con.Open();
                Object res = cmd.ExecuteScalar();
                return res != null ? res.ToString() : String.Empty;
            }
        }
		
		#endregion
    }
}
