using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

namespace VirusBlokAda.CC.DataBase
{
    internal sealed class DeviceManager
    {
        private readonly String connectionString;
        private readonly DbProviderFactory factory;

        #region Constructors
        public DeviceManager(String connectionString):this(connectionString,"System.Data.SqlClient")
        {            
        }

        public DeviceManager(String connectionString,String DbFactoryName):this(connectionString,DbProviderFactories.GetFactory(DbFactoryName))
        {            
        }
        public DeviceManager(String connectionString, DbProviderFactory factory)
        {
            this.connectionString = connectionString;
            this.factory = factory;
            
        }
        #endregion
        #region Methods

        internal static String ChangeDeviceMode(String serial, DeviceType type, DeviceMode mode)
        {
            String result = String.Empty;
            switch (type)
            {
                case DeviceType.USB:
                    DEVICE_INFO di = (DEVICE_INFO)DeviceManager.DeserializeFromBase64(serial, type);
                    di.mount = (Byte)mode;
                    result = DeviceManager.SerializeToBase64(di, type);
                    break;
                case DeviceType.NET:
                    NET_DEVICE_INFO ndi = (NET_DEVICE_INFO)DeviceManager.DeserializeFromBase64(serial, type);
                    ndi.mount = (Byte)mode;
                    result = DeviceManager.SerializeToBase64(ndi, type);
                    break;
            }

            return result;
        }

        /// <summary>
        /// Deserialize Base64 string to DEVICE_INFO
        /// </summary>
        /// <param name="text">Base64 string</param>
        /// <returns>DEVICE_INFO entity</returns>
        internal static Object DeserializeFromBase64(String text, DeviceType type)
        {
            Object result = null;
            switch (type)
            {
                case DeviceType.USB:
                    result = DeserializeDeviceInfo(text);
                    break;
                case DeviceType.NET:
                    result = DeserializeNetDeviceInfo(text);
                    break;
            }

            return result;
        }

        private static DEVICE_INFO DeserializeDeviceInfo(String text)
        {
            Byte[] buffer = Convert.FromBase64String(text);
            Byte[] tmp = new Byte[buffer.Length];

            DEVICE_INFO di = new DEVICE_INFO();

            Array.Copy(buffer, 0, tmp, 0, 4);
            di.size = BitConverter.ToUInt32(tmp, 0);

            di.time = new Byte[8];

            di.mount = buffer[12];
            di.insert = buffer[13];

            di.dev_class = buffer[14];
            di.dev_subclass = buffer[15];
            di.dev_protocol = buffer[16];

            tmp = new Byte[2];
            Array.Copy(buffer, 17, tmp, 0, 2);
            di.id_vendor = BitConverter.ToUInt16(tmp, 0);

            Array.Copy(buffer, 19, tmp, 0, 2);
            di.id_product = BitConverter.ToUInt16(tmp, 0);



            Array.Copy(buffer, 21, tmp, 0, 2);
            di.manufacturer_length = BitConverter.ToUInt16(tmp, 0);

            Array.Copy(buffer, 23, tmp, 0, 2);
            di.product_length = BitConverter.ToUInt16(tmp, 0);

            Array.Copy(buffer, 25, tmp, 0, 2);
            di.serial_number_length = BitConverter.ToUInt16(tmp, 0);

            di.strings = Encoding.Unicode.GetString(buffer, 27, di.manufacturer_length + di.product_length + di.serial_number_length);

            return di;
        }

        private static NET_DEVICE_INFO DeserializeNetDeviceInfo(String text)
        {
            Byte[] buffer = Convert.FromBase64String(text);
            Byte[] tmp = new Byte[buffer.Length];

            NET_DEVICE_INFO di = new NET_DEVICE_INFO();

            Array.Copy(buffer, 0, tmp, 0, 4);
            di.size = BitConverter.ToUInt32(tmp, 0);

            di.time = new Byte[8];

            di.mount = buffer[12];
            di.insert = buffer[13];

            Array.Copy(buffer, 14, tmp, 0, 2);
            di.dev_descr_length = BitConverter.ToUInt16(tmp, 0);

            Array.Copy(buffer, 16, tmp, 0, 2);
            di.hardware_id_length = BitConverter.ToUInt16(tmp, 0);

            Array.Copy(buffer, 18, tmp, 0, 2);
            di.friendly_name_length = BitConverter.ToUInt16(tmp, 0);

            di.strings = Encoding.Unicode.GetString(buffer, 20, di.dev_descr_length + di.hardware_id_length + di.friendly_name_length);

            return di;
        }

        /// <summary>
        /// Serialize DEVICE_INFO entity to Base64 string
        /// </summary>
        /// <param name="di">DEVICE_INFO entity</param>
        /// <returns>Base64 string</returns>
        internal static String SerializeToBase64(Object obj, DeviceType type)
        {
            String result = String.Empty;
            switch (type)
            {
                case DeviceType.USB:
                    result = SerializeDeviceInfo((DEVICE_INFO)obj);
                    break;
                case DeviceType.NET:
                    result = SerializeNetDeviceInfo((NET_DEVICE_INFO)obj);
                    break;
            }
            
            return result;
        }

        private static String SerializeDeviceInfo(DEVICE_INFO di)
        {
            Byte[] buffer = new Byte[di.size];

            Array.Copy(BitConverter.GetBytes(di.size), 0, buffer, 0, 4);
            Array.Copy(di.time, 0, buffer, 4, 8);

            Array.Copy(BitConverter.GetBytes(di.mount), 0, buffer, 12, 1);
            Array.Copy(BitConverter.GetBytes(di.insert), 0, buffer, 13, 1);

            Array.Copy(BitConverter.GetBytes(di.dev_class), 0, buffer, 14, 1);
            Array.Copy(BitConverter.GetBytes(di.dev_subclass), 0, buffer, 15, 1);
            Array.Copy(BitConverter.GetBytes(di.dev_protocol), 0, buffer, 16, 1);

            Array.Copy(BitConverter.GetBytes(di.id_vendor), 0, buffer, 17, 2);
            Array.Copy(BitConverter.GetBytes(di.id_product), 0, buffer, 19, 2);

            Array.Copy(BitConverter.GetBytes(di.manufacturer_length), 0, buffer, 21, 2);
            Array.Copy(BitConverter.GetBytes(di.product_length), 0, buffer, 23, 2);
            Array.Copy(BitConverter.GetBytes(di.serial_number_length), 0, buffer, 25, 2);

            Array.Copy(Encoding.Unicode.GetBytes(di.strings), 0, buffer, 27, di.manufacturer_length + di.product_length + di.serial_number_length);

            return Convert.ToBase64String(buffer);
        }

        private static String SerializeNetDeviceInfo(NET_DEVICE_INFO di)
        {
            Byte[] buffer = new Byte[di.size];

            Array.Copy(BitConverter.GetBytes(di.size), 0, buffer, 0, 4);
            Array.Copy(di.time, 0, buffer, 4, 8);

            Array.Copy(BitConverter.GetBytes(di.mount), 0, buffer, 12, 1);
            Array.Copy(BitConverter.GetBytes(di.insert), 0, buffer, 13, 1);

            Array.Copy(BitConverter.GetBytes(di.dev_descr_length), 0, buffer, 14, 2);
            Array.Copy(BitConverter.GetBytes(di.hardware_id_length), 0, buffer, 16, 2);
            Array.Copy(BitConverter.GetBytes(di.friendly_name_length), 0, buffer, 18, 2);

            Array.Copy(Encoding.Unicode.GetBytes(di.strings), 0, buffer, 20, di.dev_descr_length + di.hardware_id_length + di.friendly_name_length);

            return Convert.ToBase64String(buffer);
        }

        /// <summary>
        /// Add new device to db 
        /// </summary>
        /// <param name="id"></param>
        internal Device Add(Device device)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetDeviceID";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@SerialNo";
                param.Value = device.SerialNo;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@Type";
                param.Value = device.Type;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@Comment";
                param.Value = device.Comment;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@InsertIfNotExists";
                param.Value = true;
                cmd.Parameters.Add(param);

                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (reader.Read())
                    device.ID = reader.GetInt16(0);
                reader.Close();

                return device;
            }
        }

        /// <summary>
        /// Update device
        /// </summary>
        /// <param name="device">New device version</param>
        /// <returns>Updated device</returns>
        internal Device Edit(Device device)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "UpdateDevice";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ID";
                param.Value = device.ID;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@Comment";
                param.Value = device.Comment;
                cmd.Parameters.Add(param);

                con.Open();
                cmd.ExecuteScalar();

                return device;
            }
        }

        /// <summary>
        /// Remove device with this id from db
        /// </summary>
        /// <param name="device">Device</param>
        internal void Delete(Device device)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteDevice";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ID";
                param.Value = device.ID;
                cmd.Parameters.Add(param);
              
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Get device by ID
        /// </summary>
        /// <param name="id">Device ID</param>
        /// <returns>Device entity</returns>
        internal Device GetDevice(Int32 id)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetDeviceByID";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ID";
                param.Value = id;
                cmd.Parameters.Add(param);               

                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                Device device = new Device();
                if (reader.Read())
                {
                    device.ID = reader.GetInt16(0);
                    device.SerialNo = reader.GetString(1);
                    if (reader.GetValue(2) != DBNull.Value)
                        device.Comment = reader.GetString(2);
                }
                reader.Close();

                return device;
            }
        }

        /// <summary>
        /// Get device count by filter
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <returns>Device count</returns>
        internal Int32 GetDevicesCount(String where)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetDevicesPageCount";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@Where";
                param.Value = where;
                cmd.Parameters.Add(param);

                con.Open();
                return (Int32)cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Get page of devices
        /// </summary>
        /// <param name="index">Page index</param>
        /// <param name="pageCount">Page size</param>
        /// <param name="where">Filter query</param>
        /// <param name="orderBy">Sort query</param>
        /// <returns></returns>
        internal List<Device> GetDevicesList(Int32 index, Int32 pageCount, String where, String orderBy)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetDevicesPage";

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
                param.ParameterName="Where";
                param.Value=where;
                cmd.Parameters.Add(param);
                
                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<Device> devices = new List<Device>();
                while (reader.Read())
                {
                    Device device = new Device();

                    if (reader.GetValue(0) != DBNull.Value)
                        device.ID = reader.GetInt16(0);
                    if (reader.GetValue(1) != DBNull.Value)
                        device.SerialNo = reader.GetString(1);

                    if (reader.GetValue(2) != DBNull.Value)
                        device.Comment = reader.GetString(2);

                    devices.Add(device);
                }

                reader.Close();

                return devices;
            }
        }

        /// <summary>
        /// Get device count by filter
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <returns>Device count</returns>
        internal Int32 GetDeviceCount(String where)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetCountDevices";

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

        /// <summary>
        /// Get device by serialNo
        /// </summary>
        /// <param name="serial">SerialNo</param>
        /// <returns>Device entity</returns>
        internal Device GetDeviceBySerial(String serial)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetDeviceBySerial";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@SerialNo";
                param.Value = serial;
                cmd.Parameters.Add(param);

                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                Device device = new Device();
                if (reader.Read())
                {
                    device.ID = reader.GetInt16(0);
                    device.SerialNo = reader.GetString(1);
                    device.Type = DeviceTypeExtensions.Get(reader.GetString(2));
                    device.Comment = reader.GetString(3);
                }
                reader.Close();

                return device;
            }
        }

        /// <summary>
        /// Get device type list
        /// </summary>
        /// <returns></returns>
        internal List<String> GetDeviceTypes()
        {
            List<String> list = new List<String>();
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetDeviceTypes";

                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while(reader.Read())
                {
                    if (reader.GetValue(0) != DBNull.Value)
                        list.Add(reader.GetString(0));
                }
                reader.Close();
            }

            return list;
        }

        #endregion
    }
}