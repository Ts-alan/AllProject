using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Win32;
using System.Data.Common;

namespace VirusBlokAda.CC.DataBase
{
    internal sealed class PolicyManager
    {
        private readonly String connectionString;
        private readonly DbProviderFactory factory;

        #region Constructors
        public PolicyManager(String connectionString):this(connectionString,"System.Data.SqlClient")
        {            
        }

        public PolicyManager(String connectionString,String DbFactoryName):this(connectionString,DbProviderFactories.GetFactory(DbFactoryName))
        {            
        }
        public PolicyManager(String connectionString, DbProviderFactory factory)
        {
            this.connectionString = connectionString;
            this.factory = factory;
            
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get policy from SqlDataReader
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        internal static Policy GetPolicyFromReader(IDataReader reader)
        {
            Policy p = new Policy();

            if (reader.GetValue(0) != DBNull.Value)
                p.ID = reader.GetInt16(0);
            if (reader.GetValue(1) != DBNull.Value)
                p.Name = reader.GetString(1);
            if (reader.GetValue(2) != DBNull.Value)
                p.Content = reader.GetString(2);
            if (reader.GetValue(3) != DBNull.Value)
                p.Comment = reader.GetString(3);

            return p;
        }

        /// <summary>
        /// Get policies from SqlDataReader
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        internal static List<Policy> GetPoliciesFromReader(IDataReader reader)
        {
            List<Policy> p = new List<Policy>();

            while (reader.Read())
            {
                p.Add(GetPolicyFromReader(reader));
            }

            return p;
        }

        /// <summary>
        /// Add new policy to database
        /// </summary>
        /// <param name="policy">Policy name</param>
        /// <param name="content">Body of policy</param>
        internal Policy Add(Policy policy)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetPolicyID";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@TypeName";
                param.Value = policy.Name;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@Params";
                param.Value = policy.Content;
                cmd.Parameters.Add(param);
               
                param=cmd.CreateParameter();
                param.ParameterName="@Comment";
                param.Value=policy.Comment;
                cmd.Parameters.Add(param);

                param=cmd.CreateParameter();
                param.ParameterName="@InsertIfNotExists";
                param.Value=true;
                cmd.Parameters.Add(param);

                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (reader.Read())
                    policy.ID = reader.GetInt16(0);
                reader.Close();

                return policy;
            }
        }

        /// <summary>
        /// Remove policy
        /// </summary>
        /// <param name="policy">policy name</param>
        internal void Delete(Policy policy)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeletePolicy";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ID";
                param.Value = policy.ID;
                cmd.Parameters.Add(param);
             
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Edit selected rule
        /// </summary>
        /// <param name="policy">Policy name</param>
        /// <param name="content">Body of policy</param>
        internal void Edit(Policy policy)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "UpdatePolicy";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@TypeName";
                param.Value = policy.Name;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@Params";
                param.Value = policy.Content;
                cmd.Parameters.Add(param);
               
                param=cmd.CreateParameter();
                param.ParameterName="@Comment";
                param.Value=policy.Comment;
                cmd.Parameters.Add(param);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Get policy by its name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal Policy GetPolicyByName(String name)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetPolicyByName";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@TypeName";
                param.Value = name;
                cmd.Parameters.Add(param);

                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                Policy policy = new Policy();
                if (reader.Read())
                {
                    policy = GetPolicyFromReader(reader);
                }
                reader.Close();

                return policy;
            }
        }

        /// <summary>
        /// Add list of computer to selected policy
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="computers"></param>
        internal void AddComputersToPolicy(Policy policy, List<String> computers)
        {
            foreach (String compName in computers)
                AddComputerToPolicy(policy, compName);
        }

        /// <summary>
        /// Add computer to policy
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="compName"></param>
        private void AddComputerToPolicy(Policy policy, String compName)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AddComputerToPolicy";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ComputerName";
                param.Value = compName;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@PolicyID";
                param.Value = policy.ID;
                cmd.Parameters.Add(param);
               
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Remove computers from all policies
        /// </summary>
        /// <param name="computers">List of computers</param>
        internal void RemoveComputersFromAllPolicies(List<String> computers)
        {
            foreach (String compName in computers)
                RemoveComputerFromAllPolicies(compName);
        }

        /// <summary>
        /// Remove computer from all policies
        /// </summary>
        /// <param name="compName"></param>
        internal void RemoveComputerFromAllPolicies(String compName)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "RemoveComputerFromAllPolicies";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ComputerName";
                param.Value = compName;
                cmd.Parameters.Add(param);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Remove selected computers from policy
        /// </summary>
        internal void RemoveComputersFromPolicy(Policy policy, List<String> computers)
        {
            foreach (String compName in computers)
                RemoveComputerFromAllPolicies(compName);
        }

        /// <summary>
        /// Remove Computer from policy
        /// </summary>
        /// <param name="policy">Policy</param>
        /// <param name="compName">Computer name</param>
        private void RemoveComputerFromPolicy(Policy policy, String compName)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "RemoveComputerFromPolicy";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ComputerName";
                param.Value = compName;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@RowCount";
                param.Value = policy.ID;
                cmd.Parameters.Add(param);
               
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Remove computer from policy
        /// </summary>
        /// <param name="compName"></param>
        private void RemoveComputerFromPolicy(String compName)
        {
            Policy policy = new Policy();
            policy.ID = 0;
            RemoveComputerFromPolicy(policy, compName);
        }

        /// <summary>
        /// Get computers by policy
        /// </summary>
        /// <param name="policy"></param>
        /// <returns></returns>
        internal List<ComputersEntity> GetComputersByPolicy(Policy policy)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetComputersByPolicy";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@PolicyID";
                param.Value = policy.ID;
                cmd.Parameters.Add(param);
               
                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<ComputersEntity> computers = ComputersManager.GetComputersFromReader(reader);
                reader.Close();

                return computers;
            }
        }

        /// <summary>
        /// Get computers page without policy
        /// </summary>
        /// <param name="index"></param>
        /// <param name="pageCount"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        internal List<ComputersEntity> GetComputersWithoutPolicyPage(Int32 index, Int32 pageCount, String orderBy)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetComputersWithoutPolicyPage";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@Page";
                param.Value = index;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@RowCount";
                param.Value = pageCount;
                cmd.Parameters.Add(param);
               
                param=cmd.CreateParameter();
                param.ParameterName="@OrderBy";
                param.Value=orderBy;
                cmd.Parameters.Add(param);

                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<ComputersEntity> computers = ComputersManager.GetComputersFromReader(reader);
                reader.Close();

                return computers;
            }
        }

        /// <summary>
        /// Get computer count without policy
        /// </summary>
        /// <returns></returns>
        internal Int32 GetComputersWithoutPolicyCount()
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetComputersWithoutPolicyPageCount";

                con.Open();
                return (Int32)cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Get policies to computer by computer name
        /// </summary>
        /// <param name="computerName"></param>
        /// <returns></returns>
        internal Policy GetPolicyToComputer(String computerName)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetPoliciesToComputer";

                
                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                Policy policy = new Policy();

                if (reader.Read())
                {
                    policy = GetPolicyFromReader(reader);
                    reader.Close();
                }
                else
                {
                    reader.Close();
                    String defaultPolicy = GetDefaultPolicyName();
                    if (!String.IsNullOrEmpty(defaultPolicy))
                        policy = GetPolicyByName(defaultPolicy);
                }

                return policy;
            }
        }

        /// <summary>
        /// Get default policy name
        /// </summary>
        /// <returns></returns>
        internal static String GetDefaultPolicyName()
        {
            String registryControlCenterKeyName;
            RegistryKey key;

            if (System.Runtime.InteropServices.Marshal.SizeOf(typeof(IntPtr)) == 8)
                registryControlCenterKeyName = "SOFTWARE\\Wow6432Node\\Vba32\\ControlCenter\\";
            else
                registryControlCenterKeyName = "SOFTWARE\\Vba32\\ControlCenter\\";

            key = Registry.LocalMachine.OpenSubKey(registryControlCenterKeyName); ;

            return (String)key.GetValue("DefaultPolicy");
        }

        /// <summary>
        /// Get policy type list
        /// </summary>
        /// <returns></returns>
        internal List<Policy> GetPolicyType()
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetPolicyTypes";

                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<Policy> policies = GetPoliciesFromReader(reader);
                reader.Close();

                return policies;
            }
        }

        /// <summary>
        /// Remove all policies
        /// </summary>
        internal void ClearAllPolicy()
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ClearAllPolicy";

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        #endregion
    }
}