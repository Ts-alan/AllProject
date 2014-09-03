using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace VirusBlokAda.CC.DataBase
{
    internal sealed class PParserManager
    {
        private readonly String connectionString;

        #region Constructors
        public PParserManager(String connectionString)
        {
            this.connectionString = connectionString;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Delete process info
        /// </summary>
        /// <param name="computer_name"></param>
        internal void DeleteProcessInfo(String computer_name)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DeleteProcessInfo", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ComputerName", computer_name);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Insert component settings by computer name
        /// </summary>
        /// <param name="computer_name"></param>
        /// <param name="component_name"></param>
        /// <param name="settings"></param>
        internal void InsertComponentSettings(String computer_name, String component_name, String settings)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UpdateComponentSettings", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ComputerName", computer_name);
                cmd.Parameters.AddWithValue("@ComponentName", component_name);
                cmd.Parameters.AddWithValue("@ComponentSettings", settings);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Insert component state
        /// </summary>
        /// <param name="computerName"></param>
        /// <param name="componentName"></param>
        /// <param name="componentState"></param>
        /// <param name="version"></param>
        internal void InsertComponentState(String computerName, String componentName, String componentState, String version)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UpdateComponentState", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ComputerName", computerName);
                cmd.Parameters.AddWithValue("@ComponentName", componentName);
                cmd.Parameters.AddWithValue("@ComponentState", componentState);
                cmd.Parameters.AddWithValue("@Version", version);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Insert event
        /// </summary>
        /// <param name="computerName"></param>
        /// <param name="eventName"></param>
        /// <param name="eventTime"></param>
        /// <param name="componentName"></param>
        internal void InsertEvent(EventsEntity ev)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("AddEvent", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ComputerName", ev.ComputerName);
                cmd.Parameters.AddWithValue("@EventName", ev.EventName);
                cmd.Parameters.AddWithValue("@EventTime", ev.EventTime);
                cmd.Parameters.AddWithValue("@ComponentName", String.IsNullOrEmpty(ev.ComponentName) ? "(unknown)" : ev.ComponentName);
                cmd.Parameters.AddWithValue("@Object", ev.Object);
                cmd.Parameters.AddWithValue("@Comment", ev.Comment);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Add convertation logic if this event is related to devices 
        /// </summary>
        /// <param name="name_value_map"></param>
        internal void ModifyDeviceEvent(EventsEntity ev)
        {
            String[] parts = ev.Comment.Split(new Char[] { '|' });

            String cleanSerial = DeviceManager.ChangeDeviceMode(parts[0], DeviceTypeExtensions.Get(parts[3]), DeviceMode.Undefined);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetDeviceBySN", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@SerialNo", cleanSerial);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                String comment = String.Empty;
                if (reader.Read())
                {
                    if (reader.GetValue(2) != DBNull.Value)
                        comment = reader.GetString(2);
                }
                reader.Close();
                if (String.IsNullOrEmpty(comment))
                {
                    comment = parts[1];
                }
                ev.Comment = String.Concat(comment, parts[2], parts[3]);
                ev.Object = cleanSerial;
            }
        }
        
        /// <summary>
        /// Insert process info
        /// </summary>
        /// <param name="computerName"></param>
        /// <param name="processName"></param>
        /// <param name="memorySize"></param>
        internal void InsertProcessInfo(String computerName, String processName, Int32 memorySize)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UpdateProcessInfo", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ComputerName", computerName);
                cmd.Parameters.AddWithValue("@ProcessName", processName);
                cmd.Parameters.AddWithValue("@MemorySize", memorySize);
                cmd.Parameters.AddWithValue("@Date", DateTime.Now);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Insert system info
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="licenceCount"></param>
        internal void InsertSystemInfo(ComputersEntity comp, Int16 licenceCount)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UpdateComputerSystemInfo", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ComputerName", comp.ComputerName);
                cmd.Parameters.AddWithValue("@IPAddress", comp.IPAddress);
                cmd.Parameters.AddWithValue("@DomainName", comp.DomainName);
                cmd.Parameters.AddWithValue("@UserLogin", comp.UserLogin);
                cmd.Parameters.AddWithValue("@OSName", comp.OSName);
                cmd.Parameters.AddWithValue("@RAM", comp.RAM);
                cmd.Parameters.AddWithValue("@CPUClock", comp.CPUClock);
                cmd.Parameters.AddWithValue("@Vba32Version", comp.Vba32Version);
                cmd.Parameters.AddWithValue("@Vba32Integrity", comp.Vba32Integrity);
                cmd.Parameters.AddWithValue("@Vba32KeyValid", comp.Vba32KeyValid);
                cmd.Parameters.AddWithValue("@ControlCenter", comp.ControlCenter);
                cmd.Parameters.AddWithValue("@LicenseCount", licenceCount);
                cmd.Parameters.AddWithValue("@MACAddress", comp.MacAddress);
                cmd.Parameters.AddWithValue("@ControlName", comp.AdditionalInfo.ControlDeviceType.ToString());

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Insert task state
        /// </summary>
        /// <param name="task"></param>
        internal void InsertTaskState(TaskEntity task)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UpdateTaskState", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TaskID", task.ID);
                cmd.Parameters.AddWithValue("@TaskState", task.TaskState);
                cmd.Parameters.AddWithValue("@Date", task.DateUpdated);
                cmd.Parameters.AddWithValue("@Description", task.TaskDescription);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Insert component settings by MAC address
        /// </summary>
        /// <param name="macAddress"></param>
        /// <param name="componentName"></param>
        /// <param name="settings"></param>
        internal void InsertSettings(String macAddress, String componentName, String settings)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("InsertSettings", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MAC", macAddress);
                cmd.Parameters.AddWithValue("@ComponentName", componentName);
                cmd.Parameters.AddWithValue("@Settings", settings);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Insert device
        /// </summary>
        /// <param name="ev"></param>
        /// <param name="licenseCount"></param>
        internal void OnDeviceInsert(EventsEntity ev, Int16 licenseCount)
        {
            String[] parts = ev.Comment.Split(new Char[] { '|' });
            if (parts[2] != "VDD_INSERTED")
                return;

            DeviceType type = DeviceTypeExtensions.Get(parts[3]);
            String serial = DeviceManager.ChangeDeviceMode(parts[0], type, DeviceMode.Undefined);
            String comment = parts[1];

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("OnInsertingDevice", con);
                cmd.CommandType = CommandType.StoredProcedure;                

                cmd.Parameters.AddWithValue("@SerialNo", serial);
                cmd.Parameters.AddWithValue("@ComputerName", ev.ComputerName);
                cmd.Parameters.AddWithValue("@Comment", comment);
                cmd.Parameters.AddWithValue("@TypeName", type.ToString());
                cmd.Parameters.AddWithValue("@LicenseCount", licenseCount);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Get computer IP-address by computer name
        /// </summary>
        /// <param name="computerName"></param>
        /// <returns></returns>
        internal String GetComputerIPAddress(String computerName)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetComputerIPAddress", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ComputerName", computerName);

                con.Open();
                String ipAddress = String.Empty;
                ipAddress = cmd.ExecuteScalar().ToString();

                return ipAddress;
            }
        }

        /// <summary>
        /// Get event type notify
        /// </summary>
        /// <param name="event_name"></param>
        /// <returns></returns>
        internal Boolean GetEventTypeNotify(String event_name)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetEventTypeNotify", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EventName", event_name);

                con.Open();
                Boolean isNeed = false;
                isNeed = Convert.ToBoolean(cmd.ExecuteScalar());

                return isNeed;
            }
        }

        #endregion
    }
}
