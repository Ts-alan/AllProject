using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace VirusBlokAda.CC.DataBase
{
    internal sealed class DeviceManager
    {
        private readonly String connectionString;

        public DeviceManager(String connectionString)
        {
            this.connectionString = connectionString;
        }

        #region Methods

        /// <summary>
        /// Deserialize Base64 string to DEVICE_INFO
        /// </summary>
        /// <param name="text">Base64 string</param>
        /// <returns>DEVICE_INFO entity</returns>
        internal static DEVICE_INFO DeserializeFromBase64(String text)
        {
            Byte[] buffer = Convert.FromBase64String(text);
            Byte[] tmp = new Byte[buffer.Length];

            DEVICE_INFO di = new DEVICE_INFO();

            Array.Copy(buffer, 0, tmp, 0, 4);
            di.size = BitConverter.ToUInt32(tmp, 0);

            di.time = new Byte[8];

            di.mount = 0;
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

        /// <summary>
        /// Serialize DEVICE_INFO entity to Base64 string
        /// </summary>
        /// <param name="di">DEVICE_INFO entity</param>
        /// <returns>Base64 string</returns>
        internal static String SerializeToBase64(DEVICE_INFO di)
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

        /// <summary>
        /// Add new device to db 
        /// </summary>
        /// <param name="id"></param>
        internal Device Add(Device device)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetDeviceID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@SerialNo", device.SerialNo);
                cmd.Parameters.AddWithValue("@Type", device.Type);
                cmd.Parameters.AddWithValue("@Comment", device.Comment);
                cmd.Parameters.AddWithValue("@InsertIfNotExists", true);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UpdateDevice", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", device.ID);
                cmd.Parameters.AddWithValue("@Comment", device.Comment);

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
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DeleteDevice", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", device.ID);

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
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetDeviceByID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", id);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetDevicesPageCount", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Where", where);

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
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetDevicesPage", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Page", index);
                cmd.Parameters.AddWithValue("@RowCount", pageCount);
                cmd.Parameters.AddWithValue("@OrderBy", orderBy);
                cmd.Parameters.AddWithValue("@Where", where);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetCountDevices", con);
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

        /// <summary>
        /// Get device by serialNo
        /// </summary>
        /// <param name="serial">SerialNo</param>
        /// <returns>Device entity</returns>
        internal Device GetDeviceBySerial(String serial)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetDeviceBySerial", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@SerialNo", serial);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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

        #endregion
    }
}