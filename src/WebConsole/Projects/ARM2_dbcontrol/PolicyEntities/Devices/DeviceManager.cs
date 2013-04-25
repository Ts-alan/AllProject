using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;

namespace VirusBlokAda.Vba32CC.Policies.Devices
{
    internal class DeviceManager
    {

        private SqlConnection _connection;

        public SqlConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        public DeviceManager(SqlConnection conn)
        {
            _connection = conn;
        }

        /// <summary>
        /// Add new device to db 
        /// </summary>
        /// <param name="id"></param>
        internal Device Add(Device device)
        {
            //query to db
            SqlCommand command = new SqlCommand("GetDeviceID", Connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@SerialNo", device.SerialNo);
            command.Parameters.AddWithValue("@Type", device.Type);
            command.Parameters.AddWithValue("@Comment", device.Comment);
            command.Parameters.AddWithValue("@InsertIfNotExists", 1);

            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
                device.ID = reader.GetInt16(0);
            reader.Close();



            Console.WriteLine("Query to DB from DevicePolicy.Add() args: deviceID={0} ", device.SerialNo);

            return device;
        }

        internal Device Edit(Device device)
        {
            //query to db
            SqlCommand command = new SqlCommand("UpdateDevice", Connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ID", device.ID);
            command.Parameters.AddWithValue("@Comment", device.Comment);

            command.ExecuteScalar();


            return device;
        }

        /// <summary>
        /// Remove device with this id from db
        /// </summary>
        /// <param name="id"></param>
        internal void Delete(Device device)
        {
            //query to db
            SqlCommand command = new SqlCommand("DeleteDevice", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ID", device.ID);
            command.ExecuteNonQuery();

        }

        internal Device GetDevice(int id)
        {
            SqlCommand command = new SqlCommand("GetDeviceByID", Connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ID", id);

            SqlDataReader reader = command.ExecuteReader();
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

        internal int GetDevicesCount(string where)
        {
            SqlCommand command = new SqlCommand("GetDevicesPageCount", Connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Where", where);

            return (int)command.ExecuteScalar();
        }

        internal List<Device> GetDevicesList(int index, int pageCount,
            string where, string orderBy)
        {
            //query to db
            SqlCommand command = new SqlCommand("GetDevicesPage", Connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Page", index);
            command.Parameters.AddWithValue("@RowCount", pageCount);
            command.Parameters.AddWithValue("@OrderBy", orderBy);
            command.Parameters.AddWithValue("@Where", where);


            SqlDataReader reader = command.ExecuteReader();
            List<Device> devices = new List<Device>();
            while (reader.Read())
            {
                try
                {
                    Device device = new Device();

                    device.ID = reader.GetInt16(0);
                    device.SerialNo = reader.GetString(1);

                    if (reader.GetValue(2) != DBNull.Value)
                        device.Type = (DeviceType)Enum.Parse(typeof(DeviceType),
                            reader.GetString(2));
                    if (reader.GetValue(3) != DBNull.Value)
                        device.Comment = reader.GetString(3);

                    if (reader.GetValue(4) != DBNull.Value)
                        device.LastComputer = reader.GetString(4);

                    if (reader.GetValue(5) != DBNull.Value)
                        device.LastInserted = reader.GetDateTime(5);

                    devices.Add(device);
                }
                catch
                {
                }
            }
            reader.Close();

            return devices;

        }

       /* /// <summary>
        /// Get Device by its ID
        /// </summary>
        /// <param name="serial"></param>
        /// <returns></returns>
        internal Device GetDeviceBySerial(string serial)
        {
            //query to db
            SqlCommand command = new SqlCommand("GetDeviceBySerial", Connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@SerialNo", serial);



            SqlDataReader reader = command.ExecuteReader();
            Device device = new Device();
            if (reader.Read())
            {
                

                device.ID = reader.GetInt16(0);
                device.SerialNo = reader.GetString(1);
                device.Type = (DeviceType)Enum.Parse(typeof(DeviceType),
                    reader.GetString(2));
                device.Comment = reader.GetString(3);

            }
            reader.Close();

            return device;

        }
        */

    }
}
