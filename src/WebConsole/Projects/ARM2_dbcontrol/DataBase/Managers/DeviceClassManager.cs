using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace VirusBlokAda.CC.DataBase
{
    internal sealed class DeviceClassManager
    {
        private readonly String connectionString;

        public DeviceClassManager(String connectionString)
        {
            this.connectionString = connectionString;
        }

        #region Methods

        #region Device Class

        /// <summary>
        /// Get device class entity by uid
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        internal DeviceClass GetDeviceClass(String uid)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetDeviceClass", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UID", uid);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                DeviceClass d = null;
                while (reader.Read())
                {
                    d = new DeviceClass();
                    if (reader.GetValue(0) != DBNull.Value)
                        d.ID = reader.GetInt16(0);
                    if (reader.GetValue(1) != DBNull.Value)
                        d.UID = reader.GetString(1);
                    if (reader.GetValue(2) != DBNull.Value)
                        d.Class = reader.GetString(2);
                    if (reader.GetValue(3) != DBNull.Value)
                        d.ClassName = reader.GetString(3);
                }
                reader.Close();
                return d;
            }
        }

        /// <summary>
        /// Add new device class to DB
        /// </summary>
        /// <param name="deviceClass"></param>
        /// <returns></returns>
        internal DeviceClass Add(DeviceClass deviceClass)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("AddNewDeviceClass", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UID", deviceClass.UID);
                cmd.Parameters.AddWithValue("@Class", deviceClass.Class);
                cmd.Parameters.AddWithValue("@ClassName", deviceClass.ClassName);

                DeviceClass d = (DeviceClass)deviceClass.Clone();
                con.Open();
                d.ID = Convert.ToInt16(cmd.ExecuteScalar());
                return d;
            }
        }

        /// <summary>
        /// Delete class of devices by UID
        /// </summary>
        /// <param name="deviceClass"></param>
        internal void Delete(DeviceClass deviceClass)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DeleteDeviceClassByUID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UID", deviceClass.UID);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Change comment
        /// </summary>
        /// <param name="deviceClass"></param>
        internal void ChangeComment(DeviceClass deviceClass)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("ChangeCommentDeviceClassByUID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UID", deviceClass.UID);
                cmd.Parameters.AddWithValue("@Comment", deviceClass.Class);

                con.Open();
                cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Get Device class list
        /// </summary>
        /// <returns></returns>
        internal List<DeviceClass> GetDeviceClassList()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetDeviceClassList", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Close();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<DeviceClass> list = new List<DeviceClass>();
                DeviceClass deviceClass;
                while (reader.Read())
                {
                    deviceClass = new DeviceClass();
                    if (reader.GetValue(0) != DBNull.Value)
                        deviceClass.ID = reader.GetInt16(0);
                    if (reader.GetValue(1) != DBNull.Value)
                        deviceClass.UID = reader.GetString(1);
                    if (reader.GetValue(2) != DBNull.Value)
                        deviceClass.Class = reader.GetString(2);
                    if (reader.GetValue(3) != DBNull.Value)
                        deviceClass.ClassName = reader.GetString(3);

                    list.Add(deviceClass);
                }
                reader.Close();

                return list;
            }
        }

        /// <summary>
        /// Get Device class list by filter
        /// </summary>
        /// <returns></returns>
        internal List<DeviceClass> GetDeviceClassListByFilter(String where, String orderBy, Int32 index, Int32 pageCount)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetDeviceClassListByFilter", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Page", index);
                cmd.Parameters.AddWithValue("@RowCount", pageCount);
                cmd.Parameters.AddWithValue("@OrderBy", orderBy);
                cmd.Parameters.AddWithValue("@Where", where);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<DeviceClass> list = new List<DeviceClass>();
                DeviceClass deviceClass;
                while (reader.Read())
                {
                    deviceClass = new DeviceClass();
                    if (reader.GetValue(0) != DBNull.Value)
                        deviceClass.ID = reader.GetInt16(0);
                    if (reader.GetValue(1) != DBNull.Value)
                        deviceClass.UID = reader.GetString(1);
                    if (reader.GetValue(2) != DBNull.Value)
                        deviceClass.Class = reader.GetString(2);
                    if (reader.GetValue(3) != DBNull.Value)
                        deviceClass.ClassName = reader.GetString(3);

                    list.Add(deviceClass);
                }
                reader.Close();

                return list;
            }
        }

        /// <summary>
        /// Get Device class count by filter
        /// </summary>
        /// <returns></returns>
        internal Int32 GetDeviceClassCount(String where)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetDeviceClassCount", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Where", where);

                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        #endregion

        #region Device Class Policy
        #region Add
        /// <summary>
        /// Add new device class policy to computer
        /// </summary>
        /// <param name="policy">ComputerID, DeviceClassID, DeviceClassMode</param>
        /// <returns></returns>
        internal DeviceClassPolicy AddPolicy(DeviceClassPolicy policy)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("AddDeviceClassPolicy", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ComputerID", policy.Computer.ID);
                cmd.Parameters.AddWithValue("@DeviceClassID", policy.ClassOfDevice.ID);
                cmd.Parameters.AddWithValue("@DeviceClassMode", policy.Mode.ToString());

                DeviceClassPolicy d = (DeviceClassPolicy)policy.Clone();
                con.Open();
                d.ID = Convert.ToInt32(cmd.ExecuteScalar());

                return d;
            }
        }

        /// <summary>
        /// Add new device class policy to group
        /// </summary>
        /// <param name="policy">DeviceClassID, DeviceClassMode</param>
        /// <param name="group">groupID</param>
        /// <returns>added policy count</returns>
        internal Int32 AddPolicy(DeviceClassPolicy policy, Group group)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("AddDeviceClassPolicyToGroup", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@GroupID", group.ID);
                cmd.Parameters.AddWithValue("@DeviceClassID", policy.ClassOfDevice.ID);
                cmd.Parameters.AddWithValue("@DeviceClassMode", policy.Mode.ToString());

                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        /// <summary>
        /// Add new device class policy to group
        /// </summary>
        /// <param name="policy">DeviceClassID, DeviceClassMode</param>
        /// <param name="group">groupID</param>
        internal Int32 AddPolicyWithoutGroup(DeviceClassPolicy policy)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("AddDeviceClassPolicyWithoutGroup", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@DeviceClassID", policy.ClassOfDevice.ID);
                cmd.Parameters.AddWithValue("@DeviceClassMode", policy.Mode.ToString());

                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        /// <summary>
        /// Change mode to device class policy to group
        /// </summary>
        /// <param name="policy">DeviceClassID, DeviceClassMode</param>
        /// <param name="group">groupID</param>
        internal void ChangeMode(DeviceClassPolicy policy, Group group)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("ChangeDeviceClassModeToGroup", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@GroupID", group.ID);
                cmd.Parameters.AddWithValue("@DeviceClassID", policy.ClassOfDevice.ID);
                cmd.Parameters.AddWithValue("@DeviceClassMode", policy.Mode.ToString());

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Change mode to device class policy without group
        /// </summary>
        /// <param name="policy">DeviceClassID, DeviceClassMode</param>
        internal void ChangeModeWithoutGroup(DeviceClassPolicy policy)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("ChangeDeviceClassModeWithoutGroup", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@DeviceClassID", policy.ClassOfDevice.ID);
                cmd.Parameters.AddWithValue("@DeviceClassMode", policy.Mode.ToString());

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Change mode to device class policy
        /// </summary>
        /// <param name="policy"></param>
        /// <returns>ID updated item, 0 - not update</returns>
        internal Int32 ChangeMode(DeviceClassPolicy policy)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("ChangeModeToDeviceClassPolicy", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ComputerID", policy.Computer.ID);
                cmd.Parameters.AddWithValue("@DeviceClassID", policy.ClassOfDevice.ID);
                cmd.Parameters.AddWithValue("@DeviceClassMode", policy.Mode.ToString());

                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        #endregion
        #region Delete
        /// <summary>
        /// Delete device class policy from computer
        /// </summary>
        /// <param name="policy">ComputerID, DeviceClassID</param>
        internal void DeletePolicy(DeviceClassPolicy policy)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DeleteDeviceClassPolicy", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ComputerID", policy.Computer.ID);
                cmd.Parameters.AddWithValue("@DeviceClassID", policy.ClassOfDevice.ID);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Delete device class policy by DeviceClassID
        /// </summary>
        /// <param name="deviceClass">ID</param>        
        internal void DeletePolicy(DeviceClass deviceClass)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DeleteDeviceClassPolicyByDeviceClassID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@DeviceClassID", deviceClass.ID);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Delete device class policy By ComputerID
        /// </summary>
        /// <param name="computer">ID</param>
        internal void DeletePolicy(ComputersEntity computer)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DeleteDeviceClassPolicyByComputerID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ComputerID", computer.ID);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Delete device class policy from group
        /// </summary>
        /// <param name="deviceClass">ID</param>
        /// <param name="group">ID</param>
        internal void DeletePolicy(DeviceClass deviceClass, Group group)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DeleteDeviceClassPolicyFromGroup", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@GroupID", group.ID);
                cmd.Parameters.AddWithValue("@DeviceClassID", deviceClass.ID);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Delete device class policy from group
        /// </summary>
        /// <param name="deviceClass">ID</param>
        /// <param name="group">ID</param>
        internal void DeletePolicyWithoutGroup(DeviceClass deviceClass)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DeleteDeviceClassPolicyWithoutGroup", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@DeviceClassID", deviceClass.ID);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        
        /// <summary>
        /// Delete all device class policy from group
        /// </summary>
        /// <param name="group">ID</param>
        internal void DeletePolicy(Group group)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DeleteDeviceClassPolicyFromGroupByGroupID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@GroupID", group.ID);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        #endregion
        #region Get
        /// <summary>
        /// Get device class policy list by computerID
        /// </summary>
        /// <param name="computer">ID</param>
        /// <returns></returns>
        internal List<DeviceClassPolicy> GetPolicyList(ComputersEntity computer)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetDeviceClassPolicyListByComputerID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ComputerID", computer.ID);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<DeviceClassPolicy> list = new List<DeviceClassPolicy>();
                DeviceClassPolicy policy;
                while (reader.Read())
                {
                    policy = new DeviceClassPolicy();
                    policy.Computer.ID = computer.ID;
                    if (reader.GetValue(0) != DBNull.Value)
                        policy.ID = reader.GetInt32(0);
                    if (reader.GetValue(1) != DBNull.Value)
                        policy.ClassOfDevice.ID = reader.GetInt16(1);
                    if (reader.GetValue(2) != DBNull.Value)
                        policy.ClassOfDevice.UID = reader.GetString(2);
                    if (reader.GetValue(3) != DBNull.Value)
                        policy.ClassOfDevice.Class = reader.GetString(3);
                    if (reader.GetValue(4) != DBNull.Value)
                        policy.ClassOfDevice.ClassName = reader.GetString(4);
                    if (reader.GetValue(5) != DBNull.Value)
                        policy.Mode = DeviceClassModeExtensions.Get(reader.GetString(5));

                    list.Add(policy);
                }
                reader.Close();

                return list;
            }
        }

        /// <summary>
        /// Get device class policy list by groupID
        /// </summary>
        /// <param name="group">ID</param>
        /// <returns>DeviceClassPolicy.ID is [All]-flag (1 - this policy is applied to all computer from group)</returns>
        internal List<DeviceClassPolicy> GetPolicyList(Group group)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetDeviceClassPolicyListByGroupID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@GroupID", group.ID);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<DeviceClassPolicy> list = new List<DeviceClassPolicy>();
                DeviceClassPolicy policy;
                while (reader.Read())
                {
                    policy = new DeviceClassPolicy();
                    if (reader.GetValue(0) != DBNull.Value)
                        policy.ClassOfDevice.ID = reader.GetInt16(0);
                    if (reader.GetValue(1) != DBNull.Value)
                        policy.ClassOfDevice.UID = reader.GetString(1);
                    if (reader.GetValue(2) != DBNull.Value)
                        policy.ClassOfDevice.Class = reader.GetString(2);
                    if (reader.GetValue(3) != DBNull.Value)
                        policy.ClassOfDevice.ClassName = reader.GetString(3);
                    if (reader.GetValue(4) != DBNull.Value)
                        policy.Mode = DeviceClassModeExtensions.Get(reader.GetString(4));
                    if (reader.GetValue(5) != DBNull.Value)
                        policy.ID = reader.GetInt32(5);

                    list.Add(policy);
                }
                reader.Close();

                return list;
            }
        }

        /// <summary>
        /// Get device class policy list without groups
        /// </summary>
        /// <returns>DeviceClassPolicy.ID is [All]-flag (1 - this policy is applied to all computer from group)</returns>
        internal List<DeviceClassPolicy> GetPolicyList()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetDeviceClassPolicyListWithoutGroup", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<DeviceClassPolicy> list = new List<DeviceClassPolicy>();
                DeviceClassPolicy policy;
                while (reader.Read())
                {
                    policy = new DeviceClassPolicy();
                    if (reader.GetValue(0) != DBNull.Value)
                        policy.ClassOfDevice.ID = reader.GetInt16(0);
                    if (reader.GetValue(1) != DBNull.Value)
                        policy.ClassOfDevice.UID = reader.GetString(1);
                    if (reader.GetValue(2) != DBNull.Value)
                        policy.ClassOfDevice.Class = reader.GetString(2);
                    if (reader.GetValue(3) != DBNull.Value)
                        policy.ClassOfDevice.ClassName = reader.GetString(3);
                    if (reader.GetValue(4) != DBNull.Value)
                        policy.Mode = DeviceClassModeExtensions.Get(reader.GetString(4));
                    if (reader.GetValue(5) != DBNull.Value)
                        policy.ID = reader.GetInt32(5);

                    list.Add(policy);
                }
                reader.Close();

                return list;
            }
        }

        /// <summary>
        /// Get computer list by deviceID
        /// </summary>
        /// <param name="deviceClass">ID</param>
        /// <returns>DeviceClassPolicy.ID is [GroupID] (0 - if computer without group)</returns>
        internal List<DeviceClassPolicy> GetComputerList(DeviceClass deviceClass)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetComputerListByDeviceClassID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@DeviceClassID", deviceClass.ID);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<DeviceClassPolicy> list = new List<DeviceClassPolicy>();
                DeviceClassPolicy policy;
                while (reader.Read())
                {
                    policy = new DeviceClassPolicy();
                    policy.ID = -1;
                    if (reader.GetValue(0) != DBNull.Value)
                        policy.ClassOfDevice.ID = reader.GetInt16(0);
                    if (reader.GetValue(1) != DBNull.Value)
                        policy.Computer.ID = reader.GetInt16(1);
                    if (reader.GetValue(2) != DBNull.Value)
                        policy.Computer.ComputerName = reader.GetString(2);
                    if (reader.GetValue(3) != DBNull.Value)
                        policy.Mode = DeviceClassModeExtensions.Get(reader.GetString(3));
                    if (reader.GetValue(4) != DBNull.Value)
                        policy.ID = reader.GetInt32(4);

                    list.Add(policy);
                }
                reader.Close();

                return list;
            }
        }

        /// <summary>
        /// Get list of device class mode
        /// </summary>
        /// <returns></returns>
        internal List<String> GetDeviceClassModeList()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetDeviceClassModeList", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<String> list = new List<String>();
                while (reader.Read())
                {
                    /* [ID]
                    if (reader.GetValue(0) != DBNull.Value)
                        reader.GetInt16(0);
                    */
                    if (reader.GetValue(1) != DBNull.Value)
                        list.Add(reader.GetString(1));
                }
                reader.Close();

                return list;
            }
        }

        #endregion
        #endregion

        #endregion
    }
}