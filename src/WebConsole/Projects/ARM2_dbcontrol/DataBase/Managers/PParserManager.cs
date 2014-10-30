using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace VirusBlokAda.CC.DataBase
{
    internal sealed class PParserManager
    {
        private readonly String connectionString;
        private readonly DbProviderFactory factory;

        #region Constructors
        public PParserManager(String connectionString):this(connectionString,"System.Data.SqlClient")
        {            
        }

        public PParserManager(String connectionString,String DbFactoryName):this(connectionString,DbProviderFactories.GetFactory(DbFactoryName))
        {            
        }
        public PParserManager(String connectionString, DbProviderFactory factory)
        {
            this.connectionString = connectionString;
            this.factory = factory;
            
        }
        #endregion

        #region Methods

        /// <summary>
        /// Delete process info
        /// </summary>
        /// <param name="computer_name"></param>
        internal void DeleteProcessInfo(String computer_name)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteProcessInfo";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ComputerName";
                param.Value = computer_name;
                cmd.Parameters.Add(param);
             
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
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "UpdateComponentSettings";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ComputerName";
                param.Value = computer_name;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@ComponentName";
                param.Value = component_name;
                cmd.Parameters.Add(param);

                param=cmd.CreateParameter();
                param.ParameterName="@ComponentSettings";
                param.Value=settings;

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
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "UpdateComponentState";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ComputerName";
                param.Value = computerName;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@ComponentName";
                param.Value = componentName;
                cmd.Parameters.Add(param);

                param=cmd.CreateParameter();
                param.ParameterName="@ComponentState";
                param.Value=componentState;
                cmd.Parameters.Add(param);

                param=cmd.CreateParameter();
                param.ParameterName="@Version";
                param.Value=version;
                cmd.Parameters.Add(param);
               
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
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AddEvent";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ComputerName";
                param.Value = ev.ComputerName;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@EventName";
                param.Value = ev.EventName;
                cmd.Parameters.Add(param);

                param=cmd.CreateParameter();
                param.ParameterName="@EventTime";
                param.Value=ev.EventTime;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName="@ComponentName";
                param.Value=String.IsNullOrEmpty(ev.ComponentName) ? "(unknown)" : ev.ComponentName;
                cmd.Parameters.Add(param);

                param=cmd.CreateParameter();
                param.ParameterName="@Object";
                param.Value=ev.Object;
                cmd.Parameters.Add(param);

                param=cmd.CreateParameter();
                param.ParameterName="@Comment";
                param.Value=ev.Comment;
                cmd.Parameters.Add(param);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Add convertation logic if this event is related to devices 
        /// </summary>
        /// <param name="name_value_map"></param>
        internal void ModifyEvent(EventsEntity ev)
        {
            if (!String.IsNullOrEmpty(ev.Comment))
                return;

            Int32 index = 0;
            if (ev.EventName == "JE_VDD_DEVICE")
            {
                index = 1;
            }

            StringBuilder sb = new StringBuilder(256);
            while (index < ev.Comment_parts.Length)
            {
                if (index != 0)
                    sb.Append(@"<br>");
                sb.Append(ev.Comment_parts[index]);
                
                index++;
            }

            ev.Comment = sb.ToString();
        }
        
        /// <summary>
        /// Insert process info
        /// </summary>
        /// <param name="computerName"></param>
        /// <param name="processName"></param>
        /// <param name="memorySize"></param>
        internal void InsertProcessInfo(String computerName, String processName, Int32 memorySize)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "UpdateProcessInfo";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ComputerName";
                param.Value = computerName;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@ProcessName";
                param.Value = processName;
                cmd.Parameters.Add(param);

                param=cmd.CreateParameter();
                param.ParameterName="@MemorySize";
                param.Value=memorySize;
                cmd.Parameters.Add(param);

                param=cmd.CreateParameter();
                param.ParameterName="@Date";
                param.Value=DateTime.Now;
                cmd.Parameters.Add(param);

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
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "UpdateComputerSystemInfo";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ComputerName";
                param.Value = comp.ComputerName;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@IPAddress";
                param.Value = comp.IPAddress;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@DomainName";
                param.Value = comp.DomainName;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@UserLogin";
                param.Value = comp.UserLogin;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@OSName";
                param.Value = comp.OSName;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@RAM";
                param.Value = comp.RAM;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@CPUClock";
                param.Value = comp.CPUClock;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@Vba32Version";
                param.Value = comp.Vba32Version;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@Vba32Integrity";
                param.Value = comp.Vba32Integrity;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@Vba32KeyValid";
                param.Value = comp.Vba32KeyValid;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@ControlCenter";
                param.Value = comp.ControlCenter;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@LicenseCount";
                param.Value = licenceCount;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@MACAddress";
                param.Value = comp.MacAddress;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@ControlName";
                param.Value = comp.AdditionalInfo.ControlDeviceType.ToString();
                cmd.Parameters.Add(param);

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
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "UpdateTaskState";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@TaskID";
                param.Value = task.ID;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@TaskState";
                param.Value = task.TaskState;
                cmd.Parameters.Add(param);

                param=cmd.CreateParameter();
                param.ParameterName="@Date";
                param.Value=task.DateUpdated;
                cmd.Parameters.Add(param);

                param=cmd.CreateParameter();
                param.ParameterName="@Description";
                param.Value=task.TaskDescription;
                cmd.Parameters.Add(param);
               
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
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "InsertSettings";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@MAC";
                param.Value = macAddress;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@ComponentName";
                param.Value = componentName;
                cmd.Parameters.Add(param);

                param=cmd.CreateParameter();
                param.ParameterName="@Settings";
                param.Value=settings;
                cmd.Parameters.Add(param);
                
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
            if (ev.Comment_parts[2] != "VDD_INSERTED")
                return;

            DeviceType type = DeviceTypeExtensions.Get(ev.Comment_parts[3]);
            String serial = DeviceManager.ChangeDeviceMode(ev.Comment_parts[0], type, DeviceMode.Undefined);
            String comment = ev.Comment_parts[1];

            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "OnInsertingDevice";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@SerialNo";
                param.Value = serial;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@ComputerName";
                param.Value = ev.ComputerName;
                cmd.Parameters.Add(param);

                param=cmd.CreateParameter();
                param.ParameterName="@Comment";
                param.Value=comment;
                cmd.Parameters.Add(param);

                param=cmd.CreateParameter();
                param.ParameterName="@TypeName";
                param.Value=type.ToString();
                cmd.Parameters.Add(param);

                param=cmd.CreateParameter();
                param.ParameterName="@LicenseCount";
                param.Value=licenseCount;
                cmd.Parameters.Add(param);

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
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetComputerIPAddress";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ComputerName";
                param.Value = computerName;
                cmd.Parameters.Add(param);

               
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
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetEventTypeNotify";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@EventName";
                param.Value = event_name;
                cmd.Parameters.Add(param);
             
                con.Open();
                Boolean isNeed = false;
                isNeed = Convert.ToBoolean(cmd.ExecuteScalar());

                return isNeed;
            }
        }

        #endregion
    }
}
