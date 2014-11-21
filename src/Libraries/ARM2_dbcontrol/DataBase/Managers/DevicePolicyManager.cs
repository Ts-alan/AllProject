using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using VirusBlokAda.CC.Common;
using System.Text.RegularExpressions;
using System.Data.Common;

namespace VirusBlokAda.CC.DataBase
{
    internal sealed class DevicePolicyManager
    {
        private readonly String connectionString;
        private readonly DbProviderFactory factory;

        #region Constructors
        public DevicePolicyManager(String connectionString):this(connectionString,"System.Data.SqlClient")
        {            
        }

        public DevicePolicyManager(String connectionString,String DbFactoryName):this(connectionString,DbProviderFactories.GetFactory(DbFactoryName))
        {            
        }

        public DevicePolicyManager(String connectionString, DbProviderFactory factory)
        {
            this.connectionString = connectionString;
            this.factory = factory;            
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get policy to computer
        /// </summary>
        /// <param name="computerName">Computer name</param>
        /// <returns>Policy string</returns>
        internal String GetPolicyToComputer(String computerName, String jounalEvents)
        {
            DeviceClassManager dcMngr = new DeviceClassManager(connectionString);
            ComputersManager compMngr = new ComputersManager(connectionString);
            ComputersEntity comp = new ComputersEntity();
            comp.ComputerName = computerName;
            comp.ID = compMngr.GetComputerID(computerName);

            return ConvertDeviceEntitiesToPolicy(GetDeviceEntitiesFromComputer(computerName, DeviceType.USB), GetDeviceEntitiesFromComputer(computerName, DeviceType.NET), dcMngr.GetPolicyList(comp), dcMngr.GetDeviceClassList(), jounalEvents);
        }

        /// <summary>
        /// Add device policy
        /// </summary>
        /// <param name="devicePolicy">New device policy entity</param>
        internal void Add(DevicePolicy devicePolicy)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AddDevicePolicy";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ComputerID";
                param.Value = devicePolicy.Computer.ID;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@DeviceID";
                param.Value = devicePolicy.Device.ID;
                cmd.Parameters.Add(param);

                param=cmd.CreateParameter();
                param.ParameterName="@StateName";
                cmd.Parameters.Add(param);

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
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetDevicePolicyByID";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ID";
                param.Value = id;
                cmd.Parameters.Add(param);

                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                DevicePolicy dp = new DevicePolicy();
                if (reader.Read())
                {
                    ComputersEntity comp = new ComputersEntity();
                    Device device = new Device();

                    dp.ID = reader.GetInt32(0);

                    comp.ID = reader.GetInt16(1);
                    comp.ComputerName = reader.GetString(2);

                    device.ID = reader.GetInt16(3);

                    dp.State = DeviceModeExtensions.Get(reader.GetString(4));

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
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteDevicePolicyByID";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ID";
                param.Value = id;
                cmd.Parameters.Add(param);
               
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
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "RemoveDevicePolicyFromGroup";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@GroupID";
                param.Value = groupID;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@DeviceID";
                param.Value = devID;
                cmd.Parameters.Add(param);

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

            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "RemoveDevicePolicyFromWithoutGroup";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@DeviceID";
                param.Value = devID;
                cmd.Parameters.Add(param);
               
                con.Open();
                cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Return device policies to specific computer
        /// </summary>
        /// <param name="computerName">Computer name</param>
        /// <param name="device_type">Device type</param>
        /// <returns></returns>
        internal List<DevicePolicy> GetDeviceEntitiesFromComputer(String computerName, DeviceType type)
        {

            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetDeviceEntitiesFromComputer";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ComputerName";
                param.Value = computerName;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@DeviceType";
                param.Value = type.ToString();
                cmd.Parameters.Add(param);
               
                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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

                    dp.State = DeviceModeExtensions.Get(reader.GetString(4));

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
        private String ConvertDeviceEntitiesToPolicy(List<DevicePolicy> listDP_USB, List<DevicePolicy> listDP_NET, List<DeviceClassPolicy> listDCP, List<DeviceClass> listDC, String journalEvents)
        {
            Int32 index = 0;
            Regex reg = new Regex(RegularExpressions.GUID);
            List<DeviceClass> listUsbClassesAll = new List<DeviceClass>();
            List<DeviceClassPolicy> listUsbClasses = new List<DeviceClassPolicy>();
            StringBuilder policy = new StringBuilder(1024);
            
            policy.Append("<Task>");
            policy.Append("<Content>");
            policy.Append("<TaskCustomAction><Options>");
            policy.Append("<VsisCommand>");
            policy.Append("<Args>");

            policy.Append("<command>");
            policy.Append(@"<arg><key>module-id</key><value>{7C62F84A-A362-4CAA-800C-DEA89110596C}</value></arg>");
            policy.Append("<arg><key>command</key><value>apply_settings</value></arg>");
            policy.Append("<arg><key>settings</key><value><config><id>Normal</id><module><id>{87005109-1276-483A-B0A9-F3119AFA4E5B}</id>");

            //AcceptableClassesRules
            if (listDC.Count != 0)
            {
                policy.Append("<param><id>AcceptableClassesRules</id><type>stringlist</type><value>");

                foreach (DeviceClass dc in listDC)
                {
                    if (reg.IsMatch(dc.UID))
                        policy.AppendFormat("<string><id>{0}</id><val>{1}</val></string>", index++, dc.UID);
                    else
                        listUsbClassesAll.Add(dc);
                }

                policy.Append("</value></param>");
            }

            //ClassesRules
            if (listDCP.Count != 0)
            {
                index = 0;
                StringBuilder tmp = new StringBuilder(256);
                policy.Append("<param><id>ClassesRules</id><type>stringlist</type><value>");
                foreach (DeviceClassPolicy dcp in listDCP)
                {
                    if (reg.IsMatch(dcp.ClassOfDevice.UID))
                        tmp.AppendFormat("<string><id>{0}</id><val>{1}={2}</val></string>", index++, dcp.ClassOfDevice.UID, (Byte)dcp.Mode);
                    else
                        listUsbClasses.Add(dcp);
                }
                policy.Append(tmp.ToString());
                policy.Append("</value></param>");
            }

            //Events
            if (!String.IsNullOrEmpty(journalEvents))
                policy.Append(journalEvents);

            //UsbClasses
            if (listUsbClasses.Count != 0 || listUsbClassesAll.Count != 0)
            {
                index = 0;
                policy.Append("<param><id>UsbClasses</id><type>stringlist</type><value>");
                foreach (DeviceClassPolicy dcp in listUsbClasses)
                {
                    policy.AppendFormat("<string><id>{0}</id><val>{1}</val></string>", index++,
                        DeviceClassManager.SerializeToBase64(new CLASS_INFO(Convert.ToByte(dcp.ClassOfDevice.UID), (Byte)(dcp.Mode == DeviceClassMode.Enabled ? 1 : 0))));

                    listUsbClassesAll.Remove(listDC.Find(delegate(DeviceClass dev)
                    {
                        return (dev.ID == dcp.ClassOfDevice.ID);
                    }));
                }
                foreach (DeviceClass dc in listUsbClassesAll)
                {
                    policy.AppendFormat("<string><id>{0}</id><val>{1}</val></string>", index++,
                        DeviceClassManager.SerializeToBase64(new CLASS_INFO(Convert.ToByte(dc.UID), 1)));
                }
                policy.Append("</value></param>");
            }

            StringBuilder sb;

            //NETDevices
            if (listDP_NET.Count != 0)
            {
                sb = new StringBuilder(512);
                index = 0;
                foreach (DevicePolicy item in listDP_NET)
                {
                    if (item.State != DeviceMode.Undefined)
                    {
                        sb.AppendFormat("<string><id>{0}</id><val>{1}</val></string>", index++, DeviceManager.ChangeDeviceMode(item.Device.SerialNo, DeviceType.NET, item.State));
                    }
                }

                if (sb.Length != 0)
                {
                    policy.Append(@"<param><id>NetProtect</id><type>string</type><value>enable</value></param>");

                    policy.Append("<param><id>NetDevices</id><type>stringlist</type><value>");
                    policy.Append(sb.ToString());
                    policy.Append("</value></param>");
                }
            }

            //UsbDevices
            if (listDP_USB.Count != 0)
            {
                sb = new StringBuilder(512);
                
                index = 0;
                foreach (DevicePolicy item in listDP_USB)
                {
                    if (item.State != DeviceMode.Undefined)
                    {
                        sb.AppendFormat("<string><id>{0}</id><val>{1}</val></string>", index++, DeviceManager.ChangeDeviceMode(item.Device.SerialNo, DeviceType.USB, item.State));
                    }
                }

                if (sb.Length != 0)
                {
                    policy.Append("<param><id>UsbDevices</id><type>stringlist</type><value>");
                    policy.Append(sb.ToString());
                    policy.Append("</value></param>");
                }
            }

            policy.Append("</module></config></value></arg>");
            policy.Append("</command>");

            policy.Append("</Args>");
            policy.Append("<Async>0</Async>");
            policy.Append("</VsisCommand>");
            policy.Append("</Options></TaskCustomAction>");
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
        internal List<DevicePolicy> GetDeviceEntitiesFromGroup(Int32 groupID, DeviceType type)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetDevicesPageForGroup";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@GroupID";
                param.Value = groupID;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@DeviceType";
                param.Value = type.ToString();
                cmd.Parameters.Add(param);

                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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

                    dp.State = DeviceModeExtensions.Get(reader.GetString(4));

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
        internal List<DevicePolicy> GetDeviceEntitiesWithoutGroup(DeviceType type)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetDevicesPageWithoutGroup";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@DeviceType";
                param.Value = type.ToString();
                cmd.Parameters.Add(param);
                
                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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

                    dp.State = DeviceModeExtensions.Get(reader.GetString(4));

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
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "UpdateDevicePolicyStatusForComputer";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ID";
                param.Value = devicePolicy.ID;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@StateName";
                param.Value = devicePolicy.State.ToString();
                cmd.Parameters.Add(param);
               
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
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "UpdateDevicePolicyStatesToComputer";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@DeviceID";
                param.Value = deviceID;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@ComputerID";
                param.Value = computerID;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@StateName";
                param.Value = state;
                cmd.Parameters.Add(param);
                
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
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "UpdateDevicePolicyStatesToGroup";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@DeviceID";
                param.Value = deviceID;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@GroupID";
                param.Value = groupID;
                cmd.Parameters.Add(param);

                param=cmd.CreateParameter();
                param.ParameterName="@StateName";
                param.Value=state;
                cmd.Parameters.Add(param);
                
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
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "UpdateDevicePolicyStatesToWithoutGroup";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@DeviceID";
                param.Value = deviceID;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@StateName";
                param.Value = state;
                cmd.Parameters.Add(param);
                
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
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetDevicesPolicyPage";

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

                param=cmd.CreateParameter();
                param.ParameterName="@Where";
                param.Value=where;
                cmd.Parameters.Add(param);
                
                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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

                    dp.State = DeviceModeExtensions.Get(reader.GetString(4));

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
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetDevicesPolicyPageCount";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@Where";
                param.Value = where;
                cmd.Parameters.Add(param);
              
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
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetPoliciesByDevice";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@DeviceID";
                param.Value = device.ID;
                cmd.Parameters.Add(param);
                
                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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

                    dp.State = DeviceModeExtensions.Get(reader.GetString(4));

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
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AddDevicePolicyToComputer";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ComputerID";
                param.Value = devicePolicy.Computer.ID;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@SerialNo";
                param.Value = devicePolicy.Device.SerialNo;
                cmd.Parameters.Add(param);

                param=cmd.CreateParameter();
                param.ParameterName="@StateName";
                param.Value=devicePolicy.State.ToString();
                cmd.Parameters.Add(param);
             
                con.Open();
                Device device = new Device();
                DevicePolicy dp = new DevicePolicy();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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

                    if (reader.GetValue(4) != DBNull.Value)
                        device.Type = DeviceTypeExtensions.Get(reader.GetString(4));
                }
                reader.Close();

                dp.Device = device;
                dp.Computer = devicePolicy.Computer;
                dp.State = DeviceMode.Undefined;

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
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AddDevicePolicyToGroup";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@GroupID";
                param.Value = groupID;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@SerialNo";
                param.Value = dev.SerialNo;
                cmd.Parameters.Add(param);

                param=cmd.CreateParameter();
                param.ParameterName="@StateName";
                param.Value=DeviceMode.Undefined.ToString();
                cmd.Parameters.Add(param);
                
                con.Open();
                Device device = new Device();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (reader.Read())
                {
                    if (reader.GetValue(0) != DBNull.Value)
                        device.ID = reader.GetInt16(0);
                    if (reader.GetValue(1) != DBNull.Value)
                        device.SerialNo = reader.GetString(1);
                    if (reader.GetValue(2) != DBNull.Value)
                        device.Comment = reader.GetString(2);
                    if (reader.GetValue(3) != DBNull.Value)
                        device.Type = DeviceTypeExtensions.Get(reader.GetString(3));
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
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AddDevicePolicyToWithoutGroup";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@SerialNo";
                param.Value = dev.SerialNo;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@StateName";
                param.Value = DeviceMode.Undefined.ToString();
                cmd.Parameters.Add(param);

                con.Open();
                Device device = new Device();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (reader.Read())
                {

                    if (reader.GetValue(0) != DBNull.Value)
                        device.ID = reader.GetInt16(0);
                    if (reader.GetValue(1) != DBNull.Value)
                        device.SerialNo = reader.GetString(1);
                    if (reader.GetValue(2) != DBNull.Value)
                        device.Comment = reader.GetString(2);
                    if (reader.GetValue(3) != DBNull.Value)
                        device.Type = DeviceTypeExtensions.Get(reader.GetString(3));
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
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetDevicePolicyStates";
                
                con.Open();
                List<String> policyStates = new List<String>();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetComputerListByDeviceID";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@DeviceID";
                param.Value = device.ID;
                cmd.Parameters.Add(param);
             
                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<DevicePolicy> devicePolicies = new List<DevicePolicy>();
                while (reader.Read())
                {
                    DevicePolicy dp = new DevicePolicy();

                    ComputersEntity comp = new ComputersEntity();
                    device = new Device();

                    dp.ID = reader.GetInt32(0);

                    comp.ID = reader.GetInt16(1);
                    comp.ComputerName = reader.GetString(2);

                    dp.State = DeviceModeExtensions.Get(reader.GetString(3));

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
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetUnknownDevicesPage";

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

                param=cmd.CreateParameter();
                param.ParameterName="@Where";
                param.Value=where;
                cmd.Parameters.Add(param);

                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetCountUnknownDevices";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@Where";
                param.Value = where;
                cmd.Parameters.Add(param);
                
                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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