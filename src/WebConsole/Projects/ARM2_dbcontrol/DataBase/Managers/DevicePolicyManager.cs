using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using VirusBlokAda.CC.Common;

namespace VirusBlokAda.CC.DataBase
{
    internal sealed class DevicePolicyManager
    {
        private readonly String connectionString;

        public DevicePolicyManager(String connectionString)
        {
            this.connectionString = connectionString;
        }

        #region Methods

        /// <summary>
        /// Get policy to computer
        /// </summary>
        /// <param name="computerName">Computer name</param>
        /// <returns>Policy string</returns>
        internal String GetPolicyToComputer(String computerName)
        {
            List<DevicePolicy> policies = GetDeviceEntitiesFromComputer(computerName);

            return ConvertDeviceEntitiesToPolicy(policies);
        }

        /// <summary>
        /// Add device policy
        /// </summary>
        /// <param name="devicePolicy">New device policy entity</param>
        internal void Add(DevicePolicy devicePolicy)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("AddDevicePolicy", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ComputerID", devicePolicy.Computer.ID);
                cmd.Parameters.AddWithValue("@DeviceID", devicePolicy.Device.ID);
                cmd.Parameters.AddWithValue("@StateName", devicePolicy.State.ToString());

                con.Open();
                cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Get device policy by ID
        /// </summary>
        /// <param name="id">Device policy ID</param>
        /// <returns>Device policy entity</returns>
        internal DevicePolicy GetDevicePolicyByID(Int32 id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetDevicePolicyByID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", id);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                DevicePolicy dp = new DevicePolicy();
                if (reader.Read())
                {
                    ComputersEntity comp = new ComputersEntity();
                    Device device = new Device();

                    dp.ID = reader.GetInt32(0);

                    comp.ID = reader.GetInt16(1);
                    comp.ComputerName = reader.GetString(2);

                    device.ID = reader.GetInt16(3);

                    dp.State = DevicePolicyStateExtensions.Get(reader.GetString(4));

                    device.SerialNo = reader.GetString(5);

                    if (reader.GetValue(6) != DBNull.Value)
                        device.Type = DeviceTypeExtensions.Get(reader.GetString(6));

                    if (reader.GetValue(7) != DBNull.Value)
                        device.Comment = reader.GetString(7);


                    dp.Computer = comp;
                    dp.Device = device;
                }

                reader.Close();

                return dp;
            }
        }

        /// <summary>
        /// Remove device policy by ID
        /// </summary>
        /// <param name="id">Device policy ID</param>
        internal void DeleteDevicePolicyByID(Int32 id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DeleteDevicePolicyByID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", id);

                con.Open();
                cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Remove Device policy from group by device ID
        /// </summary>
        /// <param name="devID">Device ID</param>
        /// <param name="groupID">Group ID</param>
        internal void RemoveDevicePolicyGroup(Int16 devID, Int32 groupID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("RemoveDevicePolicyFromGroup", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@GroupID", groupID);
                cmd.Parameters.AddWithValue("@DeviceID", devID);

                con.Open();
                cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Remove Device policy from without group by device ID
        /// </summary>
        /// <param name="devID">Device ID</param>
        internal void RemoveDevicePolicyWithoutGroup(Int16 devID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("RemoveDevicePolicyFromWithoutGroup", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@DeviceID", devID);

                con.Open();
                cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Return device policies to specific computer
        /// </summary>
        /// <param name="computerName">Computer name</param>
        /// <param name="conn">Database connection</param>
        /// <returns></returns>
        internal List<DevicePolicy> GetDeviceEntitiesFromComputer(String computerName)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetDeviceEntitiesFromComputer", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ComputerName", computerName);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<DevicePolicy> devicePolicies = new List<DevicePolicy>();
                while (reader.Read())
                {
                    DevicePolicy dp = new DevicePolicy();

                    ComputersEntity comp = new ComputersEntity();
                    Device device = new Device();

                    dp.ID = reader.GetInt32(0);

                    comp.ID = reader.GetInt16(1);
                    comp.ComputerName = reader.GetString(2);

                    device.ID = reader.GetInt16(3);

                    dp.State = DevicePolicyStateExtensions.Get(reader.GetString(4));

                    device.SerialNo = reader.GetString(5);

                    if (reader.GetValue(6) != DBNull.Value)
                        device.Type = DeviceTypeExtensions.Get(reader.GetString(6));

                    if (reader.GetValue(7) != DBNull.Value)
                        device.Comment = reader.GetString(7);

                    if (reader.GetValue(8) != DBNull.Value)
                        dp.LatestInsert = reader.GetDateTime(8);

                    dp.Computer = comp;
                    dp.Device = device;

                    devicePolicies.Add(dp);

                }
                reader.Close();

                return devicePolicies;
            }
        }

        /// <summary>
        /// Convert device policies to policy string
        /// </summary>
        /// <param name="lists">Device policy list</param>
        /// <returns>Policy string</returns>
        private String ConvertDeviceEntitiesToPolicy(List<DevicePolicy> lists)
        {
            StringBuilder policy = new StringBuilder(256);
            policy.Append("<Task>");
            policy.Append("<Content>");
            policy.Append("<TaskCustomAction>");
            policy.Append("<Options>");
            policy.Append("<SetRegistrySettings>");
            policy.AppendFormat(@"<Common><RegisterPath>{0}</RegisterPath><IsDeleteOld>1</IsDeleteOld></Common>",
                    @"HKLM\SOFTWARE\Vba32\Loader\Devices");

            policy.Append("<Settings>");
            //Converting in internal driver format...
            foreach (DevicePolicy item in lists)
            {
                String s = "";
                switch (item.State)
                {
                    case DevicePolicyState.Enabled:
                        s = 'A' + Anchor.GetMd5Hash(Anchor.GetMd5Hash(item.Device.SerialNo));
                        break;
                    case DevicePolicyState.Disabled:
                        s = 'D' + Anchor.GetMd5Hash(item.Device.SerialNo);
                        break;
                    default:
                        continue;
                }
                policy.AppendFormat("<{0}>reg_sz:{1}</{0}>", s, item.Device.SerialNo);
            }
            policy.Append("</Settings>");
            policy.Append("<Exclusion>");
            policy.Append(@"<DEVICE_PROTECT />");
            policy.Append(@"<USE_DAILY_PROTECT />");
            policy.Append(@"<MONDAY />");
            policy.Append(@"<TUESDAY />");
            policy.Append(@"<WEDNESDAY />");
            policy.Append(@"<THURSDAY />");
            policy.Append(@"<FRIDAY />");
            policy.Append(@"<SATURDAY />");
            policy.Append(@"<SUNDAY />");
            policy.Append("</Exclusion>");
            policy.Append("</SetRegistrySettings>");
            policy.Append("</Options>");
            policy.Append("</TaskCustomAction>");
            policy.Append("</Content>");
            policy.Append("</Task>");

            return policy.ToString();
        }

        /// <summary>
        /// Get computer list by device
        /// </summary>
        /// <param name="device">Device</param>
        /// <returns>Computer list</returns>
        internal List<ComputersEntity> GetComputersByDevice(Device device)
        {
            List<DevicePolicy> deviceList = this.GetPoliciesByDevice(device);

            List<ComputersEntity> computers = new List<ComputersEntity>();
            foreach (DevicePolicy item in deviceList)
                computers.Add(item.Computer);

            return computers;
        }

        /// <summary>
        /// Return device policies to specific group
        /// </summary>
        /// <param name="groupID">Group ID</param>
        /// <param name="conn">Database connection</param>
        /// <returns></returns>
        internal List<DevicePolicy> GetDeviceEntitiesFromGroup(Int32 groupID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetDevicesPageForGroup", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@GroupID", groupID);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<DevicePolicy> devicePolicies = new List<DevicePolicy>();
                while (reader.Read())
                {
                    DevicePolicy dp = new DevicePolicy();

                    ComputersEntity comp = new ComputersEntity();
                    Device device = new Device();

                    dp.ID = reader.GetInt32(0);

                    comp.ID = reader.GetInt16(1);
                    if (reader.GetValue(2) != DBNull.Value)
                        comp.ComputerName = reader.GetString(2);

                    device.ID = reader.GetInt16(3);

                    dp.State = DevicePolicyStateExtensions.Get(reader.GetString(4));

                    device.SerialNo = reader.GetString(5);

                    if (reader.GetValue(6) != DBNull.Value)
                        device.Type = DeviceTypeExtensions.Get(reader.GetString(6));

                    if (reader.GetValue(7) != DBNull.Value)
                        device.Comment = reader.GetString(7);

                    if (reader.GetValue(8) != DBNull.Value)
                        dp.LatestInsert = reader.GetDateTime(8);
                    if (reader.GetValue(9) != DBNull.Value)
                        device.LastComputer = reader.GetString(9);

                    dp.Computer = comp;
                    dp.Device = device;

                    devicePolicies.Add(dp);

                }
                reader.Close();

                return devicePolicies;
            }
        }

        /// <summary>
        /// Return device policies without groups
        /// </summary>
        /// <param name="conn">Database connection</param>
        /// <returns></returns>
        internal List<DevicePolicy> GetDeviceEntitiesWithoutGroup()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetDevicesPageWithoutGroup", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<DevicePolicy> devicePolicies = new List<DevicePolicy>();
                while (reader.Read())
                {
                    DevicePolicy dp = new DevicePolicy();

                    ComputersEntity comp = new ComputersEntity();
                    Device device = new Device();

                    dp.ID = reader.GetInt32(0);

                    comp.ID = reader.GetInt16(1);
                    if (reader.GetValue(2) != DBNull.Value)
                        comp.ComputerName = reader.GetString(2);

                    device.ID = reader.GetInt16(3);

                    dp.State = DevicePolicyStateExtensions.Get(reader.GetString(4));

                    device.SerialNo = reader.GetString(5);

                    if (reader.GetValue(6) != DBNull.Value)
                        device.Type = DeviceTypeExtensions.Get(reader.GetString(6));

                    if (reader.GetValue(7) != DBNull.Value)
                        device.Comment = reader.GetString(7);

                    if (reader.GetValue(8) != DBNull.Value)
                        dp.LatestInsert = reader.GetDateTime(8);

                    if (reader.GetValue(9) != DBNull.Value)
                        device.LastComputer = reader.GetString(9);

                    dp.Computer = comp;
                    dp.Device = device;

                    devicePolicies.Add(dp);

                }
                reader.Close();

                return devicePolicies;
            }
        }

        /// <summary>
        /// Change device policy status for all computers by device policy ID
        /// </summary>
        /// <param name="devicePolicy">Device entity</param>
        internal void ChangeDevicePolicyStatusForComputer(DevicePolicy devicePolicy)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UpdateDevicePolicyStatusForComputer", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", devicePolicy.ID);
                cmd.Parameters.AddWithValue("@StateName", devicePolicy.State.ToString());

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Change device policy status for computer
        /// </summary>
        /// <param name="deviceID">Device ID</param>
        /// <param name="computerID">Computer ID</param>
        /// <param name="state">New status</param>
        internal void ChangeDevicePolicyStatusForComputer(Int16 deviceID, Int16 computerID, String state)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UpdateDevicePolicyStatesToComputer", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@DeviceID", deviceID);
                cmd.Parameters.AddWithValue("@ComputerID", computerID);
                cmd.Parameters.AddWithValue("@StateName", state);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Change device policy status for group
        /// </summary>
        /// <param name="deviceID">Device ID</param>
        /// <param name="groupID">Group ID</param>
        /// <param name="state">New status</param>
        internal void ChangeDevicePolicyStatusForGroup(Int16 deviceID, Int32 groupID, String state)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UpdateDevicePolicyStatesToGroup", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@DeviceID", deviceID);
                cmd.Parameters.AddWithValue("@GroupID", groupID);
                cmd.Parameters.AddWithValue("@StateName", state);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Change device policy status for without group
        /// </summary>
        /// <param name="deviceID">Device ID</param>
        /// <param name="state">New status</param>
        internal void ChangeDevicePolicyStatusToWithoutGroup(Int16 deviceID, String state)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UpdateDevicePolicyStatesToWithoutGroup", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@DeviceID", deviceID);
                cmd.Parameters.AddWithValue("@StateName", state);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Get device policy page
        /// </summary>
        /// <param name="index">Page index</param>
        /// <param name="pageCount">Page size</param>
        /// <param name="where">Filter query</param>
        /// <param name="orderBy">Sort query</param>
        /// <returns></returns>
        internal List<DevicePolicy> GetDevicesPolicyPage(Int32 index, Int32 pageCount, String where, String orderBy)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetDevicesPolicyPage", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Page", index);
                cmd.Parameters.AddWithValue("@RowCount", pageCount);
                cmd.Parameters.AddWithValue("@OrderBy", orderBy);
                cmd.Parameters.AddWithValue("@Where", where);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<DevicePolicy> devicePolicies = new List<DevicePolicy>();
                while (reader.Read())
                {
                    DevicePolicy dp = new DevicePolicy();

                    ComputersEntity comp = new ComputersEntity();
                    Device device = new Device();

                    dp.ID = reader.GetInt32(0);

                    comp.ID = reader.GetInt16(1);
                    comp.ComputerName = reader.GetString(2);

                    device.ID = reader.GetInt16(3);

                    dp.State = DevicePolicyStateExtensions.Get(reader.GetString(4));

                    device.SerialNo = reader.GetString(5);
                    if (reader.GetValue(6) != DBNull.Value)
                        device.Type = DeviceTypeExtensions.Get(reader.GetString(6));
                    if (reader.GetValue(7) != DBNull.Value)
                        device.Comment = reader.GetString(7);

                    if (reader.GetValue(8) != DBNull.Value)
                        dp.LatestInsert = reader.GetDateTime(8);

                    dp.Computer = comp;
                    dp.Device = device;

                    devicePolicies.Add(dp);

                }
                reader.Close();

                return devicePolicies;
            }
        }

        /// <summary>
        /// Get device policy page count
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <returns>Count of device policy pages</returns>
        internal Int32 GetDevicesPolicyPageCount(String where)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetDevicesPolicyPageCount", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Where", where);

                con.Open();
                return (Int32)cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Get policy list by device ID
        /// </summary>
        /// <param name="device">Device entity</param>
        /// <returns>List of policies</returns>
        internal List<DevicePolicy> GetPoliciesByDevice(Device device)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetPoliciesByDevice", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@DeviceID", device.ID);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<DevicePolicy> devicePolicies = new List<DevicePolicy>();
                while (reader.Read())
                {
                    DevicePolicy dp = new DevicePolicy();

                    ComputersEntity comp = new ComputersEntity();
                    device = new Device();

                    dp.ID = reader.GetInt32(0);

                    comp.ID = reader.GetInt16(1);
                    comp.ComputerName = reader.GetString(2);

                    device.ID = reader.GetInt16(3);

                    dp.State = DevicePolicyStateExtensions.Get(reader.GetString(4));

                    device.SerialNo = reader.GetString(5);

                    if (reader.GetValue(6) != DBNull.Value)
                        device.Type = DeviceTypeExtensions.Get(reader.GetString(6));

                    if (reader.GetValue(7) != DBNull.Value)
                        device.Comment = reader.GetString(7);

                    if (reader.GetValue(8) != DBNull.Value)
                        dp.LatestInsert = reader.GetDateTime(8);

                    dp.Computer = comp;
                    dp.Device = device;

                    devicePolicies.Add(dp);

                }
                reader.Close();

                return devicePolicies;
            }
        }

        /// <summary>
        /// Add device policy to computer
        /// </summary>
        /// <param name="devicePolicy">Device policy</param>
        /// <returns></returns>
        internal DevicePolicy AddToComputer(DevicePolicy devicePolicy)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("AddDevicePolicyToComputer", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ComputerID", devicePolicy.Computer.ID);
                cmd.Parameters.AddWithValue("@SerialNo", devicePolicy.Device.SerialNo);
                cmd.Parameters.AddWithValue("@StateName", devicePolicy.State.ToString());

                con.Open();
                Device device = new Device();
                DevicePolicy dp = new DevicePolicy();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (reader.Read())
                {
                    if (reader.GetValue(0) != DBNull.Value)
                        device.ID = reader.GetInt16(0);
                    if (reader.GetValue(1) != DBNull.Value)
                        device.SerialNo = reader.GetString(1);
                    if (reader.GetValue(2) != DBNull.Value)
                        device.Comment = reader.GetString(2);

                    Decimal dpID = 0;
                    if (reader.GetValue(3) != DBNull.Value)
                        dpID = reader.GetDecimal(3);
                    dp.ID = Convert.ToInt32(dpID);

                    device.Type = DeviceType.USB;
                }
                reader.Close();

                dp.Device = device;
                dp.Computer = devicePolicy.Computer;
                dp.State = DevicePolicyState.Undefined;

                return dp;
            }
        }

        /// <summary>
        /// Add device policy to group
        /// </summary>
        /// <param name="groupID">Group ID</param>
        /// <param name="dev">Device</param>
        /// <returns></returns>
        internal Device AddToGroup(Int32 groupID, Device dev)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("AddDevicePolicyToGroup", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@GroupID", groupID);
                cmd.Parameters.AddWithValue("@SerialNo", dev.SerialNo);
                cmd.Parameters.AddWithValue("@StateName", DevicePolicyState.Undefined.ToString());

                con.Open();
                Device device = new Device();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (reader.Read())
                {
                    if (reader.GetValue(0) != DBNull.Value)
                        device.ID = reader.GetInt16(0);
                    if (reader.GetValue(1) != DBNull.Value)
                        device.SerialNo = reader.GetString(1);
                    if (reader.GetValue(2) != DBNull.Value)
                        device.Comment = reader.GetString(2);
                    device.Type = DeviceType.USB;
                }
                reader.Close();

                return device;
            }
        }

        /// <summary>
        /// Add device policy to without group
        /// </summary>
        /// <param name="dev">Device</param>
        /// <returns></returns>
        internal Device AddToWithoutGroup(Device dev)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("AddDevicePolicyToWithoutGroup", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@SerialNo", dev.SerialNo);
                cmd.Parameters.AddWithValue("@StateName", DevicePolicyState.Undefined.ToString());

                con.Open();
                Device device = new Device();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (reader.Read())
                {

                    if (reader.GetValue(0) != DBNull.Value)
                        device.ID = reader.GetInt16(0);
                    if (reader.GetValue(1) != DBNull.Value)
                        device.SerialNo = reader.GetString(1);
                    if (reader.GetValue(2) != DBNull.Value)
                        device.Comment = reader.GetString(2);
                    device.Type = DeviceType.USB;
                }
                reader.Close();

                return device;
            }
        }

        /// <summary>
        /// Get list of policy states
        /// </summary>
        /// <returns>List of policy states</returns>
        internal List<String> GetPolicyStates()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetDevicePolicyStates", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                List<String> policyStates = new List<String>();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    if (reader.GetValue(0) != DBNull.Value)
                        policyStates.Add(reader.GetString(0));
                }
                reader.Close();

                return policyStates;
            }
        }

        /// <summary>
        /// Get computer list by device ID
        /// </summary>
        /// <param name="device">Device</param>
        /// <returns>Computer list</returns>
        internal List<DevicePolicy> GetComputerListByDeviceID(Device device)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetComputerListByDeviceID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@DeviceID", device.ID);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<DevicePolicy> devicePolicies = new List<DevicePolicy>();
                while (reader.Read())
                {
                    DevicePolicy dp = new DevicePolicy();

                    ComputersEntity comp = new ComputersEntity();
                    device = new Device();

                    dp.ID = reader.GetInt32(0);

                    comp.ID = reader.GetInt16(1);
                    comp.ComputerName = reader.GetString(2);

                    dp.State = DevicePolicyStateExtensions.Get(reader.GetString(3));

                    if (reader.GetValue(4) != DBNull.Value)
                        dp.LatestInsert = reader.GetDateTime(4);
                    //return GroupID
                    if (reader.GetValue(5) == DBNull.Value)
                        device.ID = -1;
                    else
                    {
                        device.ID = reader.GetInt32(5);
                    }

                    dp.Computer = comp;
                    dp.Device = device;

                    devicePolicies.Add(dp);

                }
                reader.Close();

                return devicePolicies;
            }
        }

        /// <summary>
        /// Get unknown device page
        /// </summary>
        /// <param name="index">Page index</param>
        /// <param name="pageCount">Page size</param>
        /// <param name="where">Filter query</param>
        /// <param name="orderBy">Sort query</param>
        /// <returns></returns>
        internal List<DevicePolicy> GetUnknownDevicesList(Int32 index, Int32 pageCount, String where, String orderBy)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetUnknownDevicesPage", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Page", index);
                cmd.Parameters.AddWithValue("@RowCount", pageCount);
                cmd.Parameters.AddWithValue("@OrderBy", orderBy);
                cmd.Parameters.AddWithValue("@Where", where);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<DevicePolicy> devicePolicies = new List<DevicePolicy>();
                while (reader.Read())
                {
                    DevicePolicy dp = new DevicePolicy();
                    Device device = new Device();
                    ComputersEntity comp = new ComputersEntity();

                    if (reader.GetValue(0) != DBNull.Value)
                        dp.ID = reader.GetInt32(0);
                    if (reader.GetValue(1) != DBNull.Value)
                        device.ID = reader.GetInt16(1);
                    if (reader.GetValue(2) != DBNull.Value)
                        device.SerialNo = reader.GetString(2);

                    if (reader.GetValue(3) != DBNull.Value)
                        device.Comment = reader.GetString(3);
                    if (reader.GetValue(4) != DBNull.Value)
                        comp.ComputerName = reader.GetString(4);
                    if (reader.GetValue(5) != DBNull.Value)

                        dp.LatestInsert = reader.GetDateTime(5);
                    dp.Device = device;
                    dp.Computer = comp;

                    devicePolicies.Add(dp);
                }
                reader.Close();

                return devicePolicies;
            }
        }

        /// <summary>
        /// Get unknown device count
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <returns>Count of unknown device</returns>
        internal Int32 GetUnknownDeviceCount(String where)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetCountUnknownDevices", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Where", where);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                Int32 count = 0;
                if (reader.Read())
                {
                    if (reader.GetValue(0) != DBNull.Value)
                        count = reader.GetInt32(0);
                }
                reader.Close();

                return count;
            }
        }

        #endregion
    }
}
