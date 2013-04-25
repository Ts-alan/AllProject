using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;

using ARM2_dbcontrol.DataBase;

namespace VirusBlokAda.Vba32CC.Policies.Devices.Policy
{
    internal class DevicePolicyManager
    {
        private SqlConnection _connection;

        public SqlConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }


        public DevicePolicyManager(SqlConnection conn)
        {
            _connection = conn;
        }

        internal string GetPolicyToComputer(string computerName)
        {

            List<DevicePolicy> policies =
                GetDeviceEntitiesFromComputer(computerName);

            return ConvertDeviceEntitiesToPolicy(policies);
        }

        internal void Add(DevicePolicy devicePolicy)
        {

            //query to db
           // try
           // {
                SqlCommand command = new SqlCommand("AddDevicePolicy", Connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ComputerID", devicePolicy.Computer.ID);
                command.Parameters.AddWithValue("@DeviceID", devicePolicy.Device.ID);
                command.Parameters.AddWithValue("@StateName", devicePolicy.State.ToString());

                command.ExecuteScalar();
            //}
            //catch(Exception ex )
            //{
            //    System.Diagnostics.Debug.Write(ex);
            //}
        }


        internal DevicePolicy GetDevicePolicyByID(int id)
        {
            SqlCommand command = new SqlCommand("GetDevicePolicyByID", Connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ID", id);
            SqlDataReader reader = command.ExecuteReader();

            DevicePolicy dp = new DevicePolicy();
            if (reader.Read())
            {
                ComputersEntity comp = new ComputersEntity();
                Device device = new Device();

                dp.ID = reader.GetInt32(0);

                comp.ID = reader.GetInt16(1);
                comp.ComputerName = reader.GetString(2);

                device.ID = reader.GetInt16(3);

                dp.State = (DevicePolicyState)Enum.Parse(typeof(DevicePolicyState),
                    reader.GetString(4));

                device.SerialNo = reader.GetString(5);

                if (reader.GetValue(6) != DBNull.Value)
                device.Type = (DeviceType)Enum.Parse(typeof(DeviceType),
                    reader.GetString(6));

            if (reader.GetValue(7) != DBNull.Value)
                    device.Comment = reader.GetString(7);


                dp.Computer = comp;
                dp.Device = device;
            }

            reader.Close();

            return dp;
        }

        internal void DeleteDevicePolicyByID(int id)
        {

            SqlCommand command = new SqlCommand("DeleteDevicePolicyByID", Connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ID", id);
            command.ExecuteScalar();
        }



        /// <summary>
        /// Return device policies to specific computer
        /// </summary>
        /// <param name="computerName">Computer name</param>
        /// <param name="conn">Database connection</param>
        /// <returns></returns>
        internal List<DevicePolicy>
            GetDeviceEntitiesFromComputer(string computerName)
        {
            //request to db

            SqlCommand command = new SqlCommand("GetDeviceEntitiesFromComputer", Connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ComputerName", computerName);
            SqlDataReader reader = command.ExecuteReader();

            List<DevicePolicy> devicePolicies = new List<DevicePolicy>();
            try
            {
                while (reader.Read())
                {
                    DevicePolicy dp = new DevicePolicy();

                    ComputersEntity comp = new ComputersEntity();
                    Device device = new Device();

                    dp.ID = reader.GetInt32(0);

                    comp.ID = reader.GetInt16(1);
                    comp.ComputerName = reader.GetString(2);

                    device.ID = reader.GetInt16(3);

                    dp.State = (DevicePolicyState)Enum.Parse(typeof(DevicePolicyState),
                                                              reader.GetString(4));

                    device.SerialNo = reader.GetString(5);

                    if (reader.GetValue(6) != DBNull.Value)
                        device.Type = (DeviceType)Enum.Parse(typeof(DeviceType),
                                                              reader.GetString(6));

                    if (reader.GetValue(7) != DBNull.Value)
                        device.Comment = reader.GetString(7);

                    if (reader.GetValue(8) != DBNull.Value)
                        dp.LatestInsert = reader.GetDateTime(8);

                    dp.Computer = comp;
                    dp.Device = device;

                    devicePolicies.Add(dp);

                }
            }
            finally
            {
                if (!reader.IsClosed)
                    reader.Close();
            }

            return devicePolicies;
        }

        private string
            ConvertDeviceEntitiesToPolicy(List<DevicePolicy> lists)
        {
            //if (lists.Count == 0)
            //    return "";
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
                string s = "";
                switch (item.State)
                {
                    case DevicePolicyState.Enabled:
                        s = 'A' + GetHash(GetHash(item.Device.SerialNo)); 
                        break;
                    case DevicePolicyState.Disabled:
                        s = 'D' + GetHash(item.Device.SerialNo);
                        break;
                    default:
                        continue;
                }
                policy.AppendFormat("<{0}>reg_sz:{1}</{0}>",s,item.Device.SerialNo);
            }
            policy.Append("</Settings>");
            policy.AppendFormat("<Exclusion>{0}</Exclusion>", @"<DEVICE_PROTECT />");
            policy.Append("</SetRegistrySettings>");
            policy.Append("</Options>");
            policy.Append("</TaskCustomAction>");
            policy.Append("</Content>");
            policy.Append("</Task>");

            return policy.ToString();

        }

        private static string GetHash(string source)
        {
              MD5 md5Hasher = MD5.Create();
                byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(source));

                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                    sBuilder.Append(data[i].ToString("x2"));

            return sBuilder.ToString();
        }

        internal List<ComputersEntity> GetComputersByDevice(Device device)
        {

            List<DevicePolicy> deviceList = this.GetPoliciesByDevice(device);

            List<ComputersEntity> computers = new List<ComputersEntity>();
            foreach (DevicePolicy item in deviceList)
                computers.Add(item.Computer);

            return computers;
        }

        #region Admin action at device policy

        internal void ChangeDevicePolicyStatusForComputer(DevicePolicy devicePolicy)
        {
            //query to db
            SqlCommand command = new SqlCommand("UpdateDevicePolicyStatusForComputer", Connection);
            command.CommandType = CommandType.StoredProcedure;

            //command.Parameters.AddWithValue("@ComputerID", devicePolicy.Computer.ID);
            command.Parameters.AddWithValue("@ID", devicePolicy.ID);
            command.Parameters.AddWithValue("@StateName", devicePolicy.State.ToString());

            command.ExecuteNonQuery();

            Console.WriteLine("Query to DB from devicePolicy.ChangeDeviceStatusForComputer() args: Comp={0}, ID={1} State={2} ",
                devicePolicy.Computer.ComputerName, devicePolicy.Device.SerialNo, devicePolicy.State);
        }

        #endregion

        #region Statistics


        internal List<DevicePolicy> GetDevicesPolicyPage(int index, int pageCount,
            string where, string orderBy)
        {
            SqlCommand command = new SqlCommand("GetDevicesPolicyPage", Connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Page", index);
            command.Parameters.AddWithValue("@RowCount", pageCount);
            command.Parameters.AddWithValue("@OrderBy", orderBy);
            command.Parameters.AddWithValue("@Where", where);

            SqlDataReader reader = command.ExecuteReader();

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

                dp.State = (DevicePolicyState)Enum.Parse(typeof(DevicePolicyState),
                    reader.GetString(4));

                device.SerialNo = reader.GetString(5);
                if (reader.GetValue(6) != DBNull.Value)
                    device.Type = (DeviceType)Enum.Parse(typeof(DeviceType),
                        reader.GetString(6));
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


        internal int GetDevicesPolicyPageCount(string where)
        {
            SqlCommand command = new SqlCommand("GetDevicesPolicyPageCount", Connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Where", where);

            return (int)command.ExecuteScalar();

        }

        internal List<DevicePolicy> GetPoliciesByDevice(Device device)
        {

            //query to db
            SqlCommand command = new SqlCommand("GetPoliciesByDevice", Connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@DeviceID", device.ID);
            SqlDataReader reader = command.ExecuteReader();

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

                dp.State = (DevicePolicyState)Enum.Parse(typeof(DevicePolicyState),
                    reader.GetString(4));

                device.SerialNo = reader.GetString(5);

                if (reader.GetValue(6) != DBNull.Value)
                    device.Type = (DeviceType)Enum.Parse(typeof(DeviceType),
                    reader.GetString(6));
                
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

        #endregion
    }
}