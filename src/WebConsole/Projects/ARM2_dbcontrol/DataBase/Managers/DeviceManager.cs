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