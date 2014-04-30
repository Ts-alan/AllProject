using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Win32;

namespace VirusBlokAda.CC.DataBase
{
    internal sealed class PolicyManager
    {
        private VlslVConnection database;

        public PolicyManager(VlslVConnection conn)
        {
            database = conn;
        }

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
            IDbCommand command = database.CreateCommand("GetPolicyID", true);

            database.AddCommandParameter(command, "@TypeName", DbType.String, policy.Name, ParameterDirection.Input);
            database.AddCommandParameter(command, "@Params", DbType.String, policy.Content, ParameterDirection.Input);
            database.AddCommandParameter(command, "@Comment", DbType.String, policy.Comment, ParameterDirection.Input);
            database.AddCommandParameter(command, "@InsertIfNotExists", DbType.Boolean, true, ParameterDirection.Input);

            SqlDataReader reader = command.ExecuteReader() as SqlDataReader;
            if (reader.Read())
                policy.ID = reader.GetInt16(0);
            reader.Close();

            return policy;
        }

        /// <summary>
        /// Remove policy
        /// </summary>
        /// <param name="policy">policy name</param>
        internal void Delete(Policy policy)
        {
            IDbCommand command = database.CreateCommand("DeletePolicy", true);

            database.AddCommandParameter(command, "@ID", DbType.Int32, policy.ID, ParameterDirection.Input);
            
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Edit selected rule
        /// </summary>
        /// <param name="policy">Policy name</param>
        /// <param name="content">Body of policy</param>
        internal void Edit(Policy policy)
        {
            IDbCommand command = database.CreateCommand("UpdatePolicy", true);

            database.AddCommandParameter(command, "@TypeName", DbType.String, policy.Name, ParameterDirection.Input);
            database.AddCommandParameter(command, "@Params", DbType.String, policy.Content, ParameterDirection.Input);
            database.AddCommandParameter(command, "@Comment", DbType.String, policy.Comment, ParameterDirection.Input);

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Get policy by its name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal Policy GetPolicyByName(String name)
        {
            IDbCommand command = database.CreateCommand("GetPolicyByName", true);

            database.AddCommandParameter(command, "@TypeName", DbType.String, name, ParameterDirection.Input);

            SqlDataReader reader = command.ExecuteReader() as SqlDataReader;
            Policy policy = new Policy();
            if (reader.Read())
            {
                policy = GetPolicyFromReader(reader);
            }
            reader.Close();

            return policy;
        }

        /// <summary>
        /// Add list of computer to selected policy
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="computers"></param>
        internal void AddComputersToPolicy(Policy policy,List<String> computers)
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
            IDbCommand command = database.CreateCommand("AddComputerToPolicy", true);

            database.AddCommandParameter(command, "@ComputerName", DbType.String, compName, ParameterDirection.Input);
            database.AddCommandParameter(command, "@PolicyID", DbType.Int32, policy.ID, ParameterDirection.Input);

            command.ExecuteNonQuery();
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
            IDbCommand command = database.CreateCommand("RemoveComputerFromAllPolicies", true);

            database.AddCommandParameter(command, "@ComputerName", DbType.String, compName, ParameterDirection.Input);

            command.ExecuteNonQuery();
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
            IDbCommand command = database.CreateCommand("RemoveComputerFromPolicy", true);

            database.AddCommandParameter(command, "@ComputerName", DbType.String, compName, ParameterDirection.Input);
            database.AddCommandParameter(command, "@PolicyID", DbType.Int32, policy.ID, ParameterDirection.Input);

            command.ExecuteNonQuery();
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
            IDbCommand command = database.CreateCommand("GetComputersByPolicy", true);

            database.AddCommandParameter(command, "@PolicyID", DbType.Int32, policy.ID, ParameterDirection.Input);

            List<ComputersEntity> computers = null;

            SqlDataReader reader = command.ExecuteReader() as SqlDataReader;
            computers = ComputersManager.GetComputersFromReader(reader);
            reader.Close();

            return computers;
        }

        /// <summary>
        /// Get computers page without policy
        /// </summary>
        /// <param name="index"></param>
        /// <param name="pageCount"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        internal List<ComputersEntity> GetComputersWithoutPolicyPage(Int32 index, Int32 pageCount,String orderBy)
        {
            IDbCommand command = database.CreateCommand("GetComputersWithoutPolicyPage", true);

            database.AddCommandParameter(command, "@Page", DbType.Int32, index, ParameterDirection.Input);
            database.AddCommandParameter(command, "@RowCount", DbType.Int32, pageCount, ParameterDirection.Input);
            database.AddCommandParameter(command, "@OrderBy", DbType.Int32, orderBy, ParameterDirection.Input);

            List<ComputersEntity> computers = null;

            SqlDataReader reader = command.ExecuteReader() as SqlDataReader;
            computers = ComputersManager.GetComputersFromReader(reader);
            reader.Close();

            return computers;
        }

        /// <summary>
        /// Get computer count without policy
        /// </summary>
        /// <returns></returns>
        internal Int32 GetComputersWithoutPolicyCount()
        {
            IDbCommand command = database.CreateCommand("GetComputersWithoutPolicyPageCount", true);

            return (Int32)command.ExecuteScalar();
        }

        /// <summary>
        /// Get policies to computer by computer name
        /// </summary>
        /// <param name="computerName"></param>
        /// <returns></returns>
        internal Policy GetPolicyToComputer(String computerName)
        {
            IDbCommand command = database.CreateCommand("GetPoliciesToComputer", true);

            database.AddCommandParameter(command, "@ComputerName", DbType.String, computerName, ParameterDirection.Input);

            SqlDataReader reader = command.ExecuteReader() as SqlDataReader;
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
            IDbCommand command = database.CreateCommand("GetPolicyTypes", true);

            SqlDataReader reader = command.ExecuteReader() as SqlDataReader;
            List<Policy> policies = GetPoliciesFromReader(reader);
            reader.Close();

            return policies;
        }

        /// <summary>
        /// Remove all policies
        /// </summary>
        internal void ClearAllPolicy()
        {
            IDbCommand command = database.CreateCommand("ClearAllPolicy", true);

            command.ExecuteNonQuery();
        }

        #endregion
    }
}