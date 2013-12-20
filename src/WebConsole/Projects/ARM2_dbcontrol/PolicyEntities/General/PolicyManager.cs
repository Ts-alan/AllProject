using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;

using Microsoft.Win32;

using ARM2_dbcontrol.DataBase;

namespace VirusBlokAda.Vba32CC.Policies.General
{
    internal class PolicyManager
    {

        private SqlConnection _connection = null;

        public SqlConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        public PolicyManager(SqlConnection conn)
        {
            _connection = conn;
        }

        /// <summary>
        /// Add new policy to database
        /// </summary>
        /// <param name="policy">Policy name</param>
        /// <param name="content">Body of policy</param>
        internal Policy Add(Policy policy)
        {
            //query to db
            SqlCommand command = new SqlCommand("GetPolicyID", Connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@TypeName", policy.Name);
            command.Parameters.AddWithValue("@Params", policy.Content);
            command.Parameters.AddWithValue("@Comment", policy.Comment);
            command.Parameters.AddWithValue("@InsertIfNotExists", 1);

            SqlDataReader reader = command.ExecuteReader();
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

            //query to db
            SqlCommand command = new SqlCommand("DeletePolicy", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ID", policy.ID);
            command.ExecuteNonQuery();

        }

        /// <summary>
        /// Edit selected rule
        /// </summary>
        /// <param name="policy">Policy name</param>
        /// <param name="content">Body of policy</param>
        internal void Edit(Policy policy)
        {
            //query to db

            SqlCommand command = new SqlCommand("UpdatePolicy", Connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@TypeName", policy.Name);
            command.Parameters.AddWithValue("@Params", policy.Content);
            command.Parameters.AddWithValue("@Comment", policy.Comment);


            command.ExecuteNonQuery();
            
        }

        /// <summary>
        /// Get policy by its name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal Policy GetPolicyByName(string name)
        {
            //query to db
            SqlCommand command = new SqlCommand("GetPolicyByName", Connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@TypeName", name);

            SqlDataReader reader = command.ExecuteReader();
            Policy policy = new Policy();
            try
            {
                
                
                if (reader.Read())
                {
                    policy.ID = reader.GetInt16(0);
                    policy.Name = reader.GetString(1);
                    policy.Content = reader.GetString(2);
                    policy.Comment = reader.GetString(3);
                }
            }
            finally
            {
                if (!reader.IsClosed)
                    reader.Close();
            }

            return policy;
        }

        /// <summary>
        /// Add list of computer to selected policy
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="computers"></param>
        internal void AddComputersToPolicy(Policy policy,
            List<string> computers)
        {

            foreach (string compName in computers)
                AddComputerToPolicy(policy, compName);

        }

        /// <summary>
        /// Add computer to policy
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="compName"></param>
        private void AddComputerToPolicy(Policy policy, string compName)
        {
            SqlCommand command = new SqlCommand("AddComputerToPolicy", Connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ComputerName", compName);
            command.Parameters.AddWithValue("@PolicyID", policy.ID);

            command.ExecuteNonQuery();
        }


        internal void RemoveComputersFromAllPolicies(List<string> computers)
        {
            foreach (string compName in computers)
                RemoveComputerFromAllPolicies(compName);
        }

        internal void RemoveComputerFromAllPolicies(string compName)
        {
            SqlCommand command = new SqlCommand("RemoveComputerFromAllPolicies", Connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ComputerName", compName);

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Remove selected computers from policy
        /// </summary>
        /// <param name="computers"></param>
        internal void
            RemoveComputersFromPolicy(Policy policy, List<string> computers)
        {
            foreach (string compName in computers)
                RemoveComputerFromAllPolicies(compName);//Policy(compName);

        }

        private void RemoveComputerFromPolicy(Policy policy, string compName)
        {
            SqlCommand command = new SqlCommand("RemoveComputerFromPolicy", Connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ComputerName", compName);
            command.Parameters.AddWithValue("@PolicyID", policy.ID);

            command.ExecuteNonQuery();
        }

        private void RemoveComputerFromPolicy(string compName)
        {
            Policy policy = new Policy();
            policy.ID = 0;
            RemoveComputerFromPolicy(policy,compName);
        }

        internal List<ComputersEntity> GetComputersByPolicy(Policy policy)
        {
            //request to db

            SqlCommand command = new SqlCommand("GetComputersByPolicy", Connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@PolicyID", policy.ID);

            List<ComputersEntity> computers = new List<ComputersEntity>();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ComputersEntity comp = new ComputersEntity();
                comp.ID = reader.GetInt16(0);
                comp.ComputerName = reader.GetString(1);

                computers.Add(comp);
            }
            reader.Close();


            return computers;
        }


        internal List<ComputersEntity> GetComputersByPolicyPage(int index, int pageCount,
            string where, string orderBy)
        {
            //request to db

            SqlCommand command = new SqlCommand("GetComputersByPolicyPage", Connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Page", index);
            command.Parameters.AddWithValue("@RowCount", pageCount);
            command.Parameters.AddWithValue("@OrderBy", orderBy);
            command.Parameters.AddWithValue("@Where", where);

            List<ComputersEntity> computers = new List<ComputersEntity>();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                #region Fill entity
                ComputersEntity comp = new ComputersEntity();
                if (reader.GetValue(0) != DBNull.Value)
                    comp.ID = reader.GetInt16(0);

                if (reader.GetValue(1) != DBNull.Value)
                    comp.ComputerName = reader.GetString(1);

                if (reader.GetValue(2) != DBNull.Value)
                    comp.IPAddress = reader.GetString(2);

                if (reader.GetValue(3) != DBNull.Value)
                    comp.ControlCenter = reader.GetBoolean(3);

                if (reader.GetValue(4) != DBNull.Value)
                    comp.DomainName = reader.GetString(4);

                if (reader.GetValue(5) != DBNull.Value)
                    comp.UserLogin = reader.GetString(5);

                if (reader.GetValue(6) != DBNull.Value)
                    comp.OSName = reader.GetString(6);

                if (reader.GetValue(7) != DBNull.Value)
                    comp.RAM = reader.GetInt16(7);

                if (reader.GetValue(8) != DBNull.Value)
                    comp.CPUClock = reader.GetInt16(8);

                if (reader.GetValue(9) != DBNull.Value)
                    comp.RecentActive = reader.GetDateTime(9);

                if (reader.GetValue(10) != DBNull.Value)
                    comp.LatestUpdate = reader.GetDateTime(10);

                if (reader.GetValue(11) != DBNull.Value)
                    comp.Vba32Version = reader.GetString(11);

                if (reader.GetValue(12) != DBNull.Value)
                    comp.LatestInfected = reader.GetDateTime(12);

                if (reader.GetValue(13) != DBNull.Value)
                    comp.LatestMalware = reader.GetString(13);

                if (reader.GetValue(14) != DBNull.Value)
                    comp.Vba32Integrity = reader.GetBoolean(14);

                if (reader.GetValue(15) != DBNull.Value)
                    comp.Vba32KeyValid = reader.GetBoolean(15);

                if (reader.GetValue(16) != DBNull.Value)
                    comp.Description = reader.GetString(16);
                
                if (reader.GetValue(17) != DBNull.Value)
                    comp.AdditionalInfo.ControlDeviceType = ControlDeviceTypeEnumExtensions.Get(reader.GetString(17));
                #endregion
                computers.Add(comp);

            }
            reader.Close();


            return computers;
        }


        internal int GetComputersByPolicyCount(string where)
        {
            //request to db

            SqlCommand command = new SqlCommand("GetComputersByPolicyCount", Connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Where", where);

            
            return (int) command.ExecuteScalar();

        }

        //!OPTM - Шаблонный код. Нужно разбить отдельно парсинг IDataReader
        internal List<ComputersEntity> GetComputersWithoutPolicyPage(int index, int pageCount,
            string orderBy)
        {
            //request to db

            SqlCommand command = new SqlCommand("GetComputersWithoutPolicyPage", Connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Page", index);
            command.Parameters.AddWithValue("@RowCount", pageCount);
            command.Parameters.AddWithValue("@OrderBy", orderBy);

            List<ComputersEntity> computers = new List<ComputersEntity>();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                #region Fill entity
                ComputersEntity comp = new ComputersEntity();
                if (reader.GetValue(0) != DBNull.Value)
                    comp.ID = reader.GetInt16(0);

                if (reader.GetValue(1) != DBNull.Value)
                    comp.ComputerName = reader.GetString(1);

                if (reader.GetValue(2) != DBNull.Value)
                    comp.IPAddress = reader.GetString(2);

                if (reader.GetValue(3) != DBNull.Value)
                    comp.ControlCenter = reader.GetBoolean(3);

                if (reader.GetValue(4) != DBNull.Value)
                    comp.DomainName = reader.GetString(4);

                if (reader.GetValue(5) != DBNull.Value)
                    comp.UserLogin = reader.GetString(5);

                if (reader.GetValue(6) != DBNull.Value)
                    comp.OSName = reader.GetString(6);

                if (reader.GetValue(7) != DBNull.Value)
                    comp.RAM = reader.GetInt16(7);

                if (reader.GetValue(8) != DBNull.Value)
                    comp.CPUClock = reader.GetInt16(8);

                if (reader.GetValue(9) != DBNull.Value)
                    comp.RecentActive = reader.GetDateTime(9);

                if (reader.GetValue(10) != DBNull.Value)
                    comp.LatestUpdate = reader.GetDateTime(10);

                if (reader.GetValue(11) != DBNull.Value)
                    comp.Vba32Version = reader.GetString(11);

                if (reader.GetValue(12) != DBNull.Value)
                    comp.LatestInfected = reader.GetDateTime(12);

                if (reader.GetValue(13) != DBNull.Value)
                    comp.LatestMalware = reader.GetString(13);

                if (reader.GetValue(14) != DBNull.Value)
                    comp.Vba32Integrity = reader.GetBoolean(14);

                if (reader.GetValue(15) != DBNull.Value)
                    comp.Vba32KeyValid = reader.GetBoolean(15);

                if (reader.GetValue(16) != DBNull.Value)
                    comp.Description = reader.GetString(16);
                #endregion
                computers.Add(comp);

            }
            reader.Close();

            return computers;
        }

        internal int GetComputersWithoutPolicyCount()
        {
            //request to db

            SqlCommand command = new SqlCommand("GetComputersWithoutPolicyPageCount", Connection);
            command.CommandType = CommandType.StoredProcedure;

            return (int)command.ExecuteScalar();

        }

        internal Policy GetPolicyToComputer(string computerName)
        {
            //request to db

            SqlCommand command = new SqlCommand("GetPoliciesToComputer", Connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ComputerName", computerName);

            SqlDataReader reader = command.ExecuteReader();
            Policy policy = new Policy();
            try
            {
                
                if (reader.Read())
                {
                    policy.ID = reader.GetInt16(0);
                    policy.Name = reader.GetString(1);
                    policy.Content = reader.GetString(2);
                    policy.Comment = reader.GetString(3);
                }
                else
                {
                    reader.Close();
                    string defaultPolicy = GetDefaultPolicyName();
                    if (!String.IsNullOrEmpty(defaultPolicy))
                        policy = GetPolicyByName(defaultPolicy);
                }
            }
            finally
            {
                if (!reader.IsClosed)
                    reader.Close();
            }

            return policy;
        }

        internal string GetDefaultPolicyName()
        {
            string registryControlCenterKeyName;
            RegistryKey key;
            //try
            //{
                //!-OPTM Вынести такую проверку в App_Code и юзать один код
                if (System.Runtime.InteropServices.Marshal.SizeOf(typeof(IntPtr)) == 8)
                    registryControlCenterKeyName = "SOFTWARE\\Wow6432Node\\Vba32\\ControlCenter\\";
                else
                    registryControlCenterKeyName = "SOFTWARE\\Vba32\\ControlCenter\\";

                key = Registry.LocalMachine.OpenSubKey(registryControlCenterKeyName); ;

            //}
            //catch (Exception ex)
            //{
            //    System.Diagnostics.Debug.Write("Registry open 'ControlCenter' key error: " + ex.Message);
            //}

            return (string)key.GetValue("DefaultPolicy");
        }

        internal List<Policy> GetPolicyType()
        {
            SqlCommand command = new SqlCommand("GetPolicyTypes", Connection);
            command.CommandType = CommandType.StoredProcedure;
           

            SqlDataReader reader = command.ExecuteReader();
            List<Policy> policies = new List<Policy>();
            while (reader.Read())
            {
                Policy policy = new Policy();
                policy.ID = reader.GetInt16(0);
                policy.Name = reader.GetString(1);
                policy.Content = reader.GetString(2);
                policy.Comment = reader.GetString(3);

                policies.Add(policy);
            }
            reader.Close();

            return policies;

        }

        internal void ClearAllPolicy()
        {
            SqlCommand command = new SqlCommand("ClearAllPolicy", Connection);
            command.CommandType = CommandType.StoredProcedure;

            command.ExecuteNonQuery();
        }

    }
}
