using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace VirusBlokAda.CC.DataBase
{
    internal sealed class DeviceManager
    {
        private VlslVConnection database;

        public DeviceManager(VlslVConnection conn)
        {
            database = conn;
        }

        #region Methods

        /// <summary>
        /// Add new device to db 
        /// </summary>
        /// <param name="id"></param>
        internal Device Add(Device device)
        {
            //query to db
            IDbCommand command = database.CreateCommand("GetDeviceID", true);

            database.AddCommandParameter(command, "@SerialNo",
                DbType.String, device.SerialNo, ParameterDirection.Input);
            database.AddCommandParameter(command, "@Type",
                DbType.Int16, device.Type, ParameterDirection.Input);
            database.AddCommandParameter(command, "@Comment",
                DbType.String, device.Comment, ParameterDirection.Input);
            database.AddCommandParameter(command, "@InsertIfNotExists",
                DbType.Boolean, true, ParameterDirection.Input);

            SqlDataReader reader = command.ExecuteReader() as SqlDataReader;
            if (reader.Read())
                device.ID = reader.GetInt16(0);
            reader.Close();

            return device;
        }

        /// <summary>
        /// Update device
        /// </summary>
        /// <param name="device">New device version</param>
        /// <returns>Updated device</returns>
        internal Device Edit(Device device)
        {
            IDbCommand command = database.CreateCommand("UpdateDevice", true);

            database.AddCommandParameter(command, "@ID",
                DbType.Int32, device.ID, ParameterDirection.Input);
            database.AddCommandParameter(command, "@Comment",
                DbType.String, device.Comment, ParameterDirection.Input);

            command.ExecuteScalar();

            return device;
        }

        /// <summary>
        /// Remove device with this id from db
        /// </summary>
        /// <param name="device">Device</param>
        internal void Delete(Device device)
        {
            IDbCommand command = database.CreateCommand("DeleteDevice", true);

            database.AddCommandParameter(command, "@ID",
                DbType.Int32, device.ID, ParameterDirection.Input);

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Get device by ID
        /// </summary>
        /// <param name="id">Device ID</param>
        /// <returns>Device entity</returns>
        internal Device GetDevice(Int32 id)
        {
            IDbCommand command = database.CreateCommand("GetDeviceByID", true);

            database.AddCommandParameter(command, "@ID",
                DbType.Int32, id, ParameterDirection.Input);

            SqlDataReader reader = command.ExecuteReader() as SqlDataReader;
            Device device = new Device();
            if (reader.Read())
            {
                try
                {
                    device.ID = reader.GetInt16(0);
                    device.SerialNo = reader.GetString(1);
                    if (reader.GetValue(2) != DBNull.Value)
                        device.Comment = reader.GetString(2);
                }
                catch { }
            }
            reader.Close();

            return device;
        }

        /// <summary>
        /// Get device count by filter
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <returns>Device count</returns>
        internal Int32 GetDevicesCount(String where)
        {
            IDbCommand command = database.CreateCommand("GetDevicesPageCount", true);

            database.AddCommandParameter(command, "@Where",
                DbType.String, where, ParameterDirection.Input);

            return (Int32)command.ExecuteScalar();
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
            IDbCommand command = database.CreateCommand("GetDevicesPage", true);

            database.AddCommandParameter(command, "@Page",
                DbType.Int32, index, ParameterDirection.Input);
            database.AddCommandParameter(command, "@RowCount",
                DbType.Int32, pageCount, ParameterDirection.Input);
            database.AddCommandParameter(command, "@OrderBy",
                DbType.String, orderBy, ParameterDirection.Input);
            database.AddCommandParameter(command, "@Where",
                DbType.String, where, ParameterDirection.Input);

            SqlDataReader reader = command.ExecuteReader() as SqlDataReader;
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

        /// <summary>
        /// Get device count by filter
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <returns>Device count</returns>
        internal Int32 GetDeviceCount(String where)
        {
            IDbCommand command = database.CreateCommand("GetCountDevices", true);

            database.AddCommandParameter(command, "@Where",
                DbType.String, where, ParameterDirection.Input);

            SqlDataReader reader = command.ExecuteReader() as SqlDataReader;
            Int32 count = 0;
            if (reader.Read())
            {
                if (reader.GetValue(0) != DBNull.Value)
                    count = reader.GetInt32(0);
            }
            reader.Close();

            return count;
        }

        /// <summary>
        /// Get device by serialNo
        /// </summary>
        /// <param name="serial">SerialNo</param>
        /// <returns>Device entity</returns>
        internal Device GetDeviceBySerial(String serial)
        {
            IDbCommand command = database.CreateCommand("GetDeviceBySerial", true);

            database.AddCommandParameter(command, "@SerialNo",
                DbType.String, serial, ParameterDirection.Input);

            SqlDataReader reader = command.ExecuteReader() as SqlDataReader;
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
        
        #endregion
    }
}