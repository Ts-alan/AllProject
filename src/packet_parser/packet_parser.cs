using System.Text;

namespace Vba32CC
{
    //Common
    using System;
    using System.IO;
    using System.Globalization;
    using Vba32.ControlCenter.NotificationService;

    //xml-parsing
    using System.Xml;

    //Arrays
    using System.Collections;
    using System.Collections.Specialized;

    //Database
    using System.Data;
    using System.Data.SqlClient;

    //Registry
    using Microsoft.Win32;

    //COM-Server
    using System.Runtime.InteropServices;

    //Multithreading
    using System.Threading;

    //Type reflection
    using System.Reflection;

    using System.Diagnostics;
    using Vba32CC.TaskAssignment;
    using Vba32CC.TaskAssignment.Tasks;

    /// <summary>
    /// Event sinks class
    /// </summary>
    [ComVisible(true)]
    public interface IPacketParserCallBacks
    {
        [ComVisible(true)]
        void OnConnectionFalse();

        [ComVisible(true)]
        void OnConnectionTrue();
    }


    /// <summary>
    /// Parse Xml-formatting events to DataBase
    /// </summary>
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    public class PacketParser
    {

        private Int32 CompCount = 0;
        private Int32 CompUsbCount = 0;

        private readonly String AppPath = String.Empty;
        private const String logFileName = @"Vba32PacketParser.log";
        private const String keyFileName = @"vba32.key";
        private Logger log;
        private Logger Log
        {
            get {
                if (log == null)
                    log = new Logger(AppPath + logFileName);

                return log;
            }
        }


        // Constructor.
        public PacketParser()
        {
            AppPath = Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(PacketParser)).Location) + @"\";

            if (IsConnectionNull())
            {
                m_sql_connection = new SqlConnection();
            }
            if (IsConnectionClosed())
            {
                ConnectDB();
            }

            m_server_name = ReadServerName();
            m_user_name = ReadUserName();

            //Set ConnectionString into Automatically tasks
            AutomaticallyTasks.ConnectionString = GetConnectionString();

            // start connection control thread
            Thread connection_control_thread = new Thread(new ThreadStart(ConnectionControlThread));
            connection_control_thread.Start();

            Thread.CurrentThread.Name = "PacketParserThread";
            connection_control_thread.Name = "ConnectionControlThread";

            ParseKeyFile();
        }

        /// <summary>
        /// Parse key file. Set count of computers and count of usb.
        /// </summary>
        private void ParseKeyFile()
        {
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(AppPath + keyFileName);
                
                String keyFileContent = reader.ReadToEnd();
                CompCount = GetLicensesCount(keyFileContent, @"VBA32AAW");
                CompUsbCount = GetLicensesCount(keyFileContent, @"VBA32USB");

                reader.Close();
            }
            catch
            {
                Log.Write("Key file is not exist or invalid.");
                CompCount = 0;
                CompUsbCount = 0;

                if (reader != null)
                    reader.Close();
            }
        }

        private Int32 GetLicensesCount(string keyFileContent, string keyName)
        {
            Int32 indexKey = keyFileContent.IndexOf(String.Format(@"[{0}]", keyName));
            if (indexKey == -1) return 0;
            Int32 indexBegin = keyFileContent.IndexOf("ComputersLimit=", indexKey) + 15;
            Int32 indexEnd = keyFileContent.IndexOfAny(new char[] { ' ', '\r', '\n' }, indexBegin);

            String val;
            if(indexEnd != -1)
                val = keyFileContent.Substring(indexBegin, indexEnd - indexBegin);
            else val = keyFileContent.Substring(indexBegin);

            try
            {
                return Int32.Parse(val);
            }
            catch
            {
                throw new Exception();                
            }
        }

        /// <summary>
        /// Get from client event handlers
        /// </summary>
        private object m_event_handlers = null;
        [ComVisible(true)]
        public bool SetCallbacks(IPacketParserCallBacks event_handlers)
        {
            lock (this)
            {
                bool result = true;
                try
                {
                    m_event_handlers = event_handlers;
                    if (IsConnectionClosed())
                    {
                        NotifyFalseConnection();
                    }
                    else
                    {
                        NotifyTrueConnection();
                    }
                }
                catch (Exception exception)
                {
                    m_error_info = exception.Message;
                    result = false;
                }
                return result;
            }
        }

        [ComVisible(true)]
        public bool CloseConnection()
        {
            lock (this)
            {
                try
                {
                    m_sql_connection.Close();
                }
                catch (Exception e)
                {
                    m_error_info = e.Message;
                    return false;
                }
                return true;
            }
        }

        [ComVisible(true)]
        public bool ParseXmlToDB(string xml_fragment)
        {            
            lock (this)
            {

                // Check the connection
                if (IsConnectionNull())
                {
                    m_error_info = "Connection is not established";
                    if (m_was_opened) // was opened now closed
                    {
                        NotifyFalseConnection();
                        m_was_opened = false;
                    }
                    return false;
                }
                if (IsConnectionClosed()) // have to check after checking IsConnectionNull()
                {
                    m_error_info = "Connection is not established";
                    if (m_was_opened) // was opened now closed
                    {
                        NotifyFalseConnection();
                        m_was_opened = false;
                    }
                    return false;
                }
                m_was_opened = true;

                bool result = true;
                StringReader string_reader = null;
                XmlTextReader xml_reader = null;

                try
                {
                    string_reader = new StringReader(xml_fragment);
                    xml_reader = new XmlTextReader(string_reader);
                    result = ParseXmlFragment(xml_reader);
                }
                catch (Exception exception)
                {
                    result = false;
                    m_error_info = exception.Message;
                }

                //Close readers
                if (xml_reader != null)
                {
                    xml_reader.Close();
                }
                if (string_reader != null)
                {
                    string_reader.Close();
                }

                return result;
            }
        }

        public string GetLastError()
        {
            lock (this)
            {
                string result = m_error_info;
                m_error_info = "Operation successfully";
                return result;
            }
        }

        #region Element Parsing Functions
        private bool ParseXmlFragment(XmlTextReader xml_reader)
        {
            bool result = true;
            try
            {
                while (xml_reader.Read())
                {
                    if (xml_reader.NodeType == XmlNodeType.Element)
                    {
                        switch (xml_reader.Name)
                        {
                            case "ComponentSettings":
                                result = ParseComponentSettings(xml_reader);
                                if (!result)
                                    return result;
                                break;
                            case "ComponentState":
                                result = ParseComponentState(xml_reader);
                                if (!result)
                                    return result;
                                break;
                            case "Events":
                                result = ParseEvents(xml_reader);
                                if (!result)
                                    return result;
                                break;
                            case "ProcessInfo":
                                result = ParseProcessInfo(xml_reader);
                                if (!result)
                                    return result;
                                break;
                            case "SystemInfo":
                                result = ParseSystemInfo(xml_reader);
                                if (!result)
                                    return result;
                                break;
                            case "TaskStates":
                                result = ParseTaskStates(xml_reader);
                                if (!result)
                                    return result;
                                break;
                            case "TaskState":
                                result = ParseTaskState(xml_reader);
                                if (!result)
                                    return result;
                                break;
                            case "SettingsStates":
                                result = ParseSettingsStates(xml_reader);
                                if (!result)
                                    return result;
                                break;
                            default:
                                result = false;
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                m_error_info = e.Message;
                result = false;
            }
            return result;
        }

        private bool ParseCommonElement(XmlTextReader xml_reader, StringDictionary name_value_map)
        {
            bool result = true;
            string element_name = null;
            string element_value = null;
            while (xml_reader.Read())
            {
                switch (xml_reader.NodeType)
                {
                    case XmlNodeType.Element:
                        element_name = xml_reader.Name;
                        break;
                    case XmlNodeType.Text:
                        element_value = xml_reader.Value;
                        break;
                    case XmlNodeType.EndElement:
                        if ((element_name != null) /*&& (element_value != null)*/)
                        {
                            try
                            {
                                name_value_map.Add(element_name, element_value);
                            }
                            catch (Exception e)
                            {
                                m_error_info = e.Message;
                                result = false;
                                return result;
                            }
                            element_name = null;
                            element_value = null;
                        }
                        else
                        {
                            return result;
                        }
                        break;
                    default:
                        break;
                }
            }
            return result;
        }

        private bool ParseComponentSettings(XmlTextReader xml_reader)
        {
            bool result = true;
            string computer_name = null;
            if (xml_reader.MoveToFirstAttribute())
            {
                computer_name = xml_reader.Value;
            }
            string element_name = null;
            string element_value = null;
            while (xml_reader.Read())
            {
                if ((xml_reader.NodeType == XmlNodeType.Element) && (xml_reader.Name.Equals("Component")))
                {
                    if (xml_reader.MoveToFirstAttribute())
                    {
                        element_name = xml_reader.Value;
                        if (xml_reader.MoveToElement())
                        {
                            element_value = xml_reader.ReadInnerXml();
                            result = InsertComponentSettings(computer_name, element_name, element_value);
                            if (!result)
                                return result;
                        }
                    }
                }
            }
            return result;
        }

        private Boolean ParseSettingsStates(XmlTextReader xml_reader)
        {
            Boolean result = true;
            xml_reader.MoveToContent();
            String xml = xml_reader.ReadInnerXml();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            String computerName = String.Empty;
            String macAddress = String.Empty;
            foreach (XmlNode node in doc.GetElementsByTagName("SettingsState"))
            {
                if (node.Attributes["computerName"] != null && node.Attributes["mac"] != null)
                {
                    computerName = node.Attributes["computerName"].Value;
                    macAddress = node.Attributes["mac"].Value;
                    break;
                }
            }

            if (String.IsNullOrEmpty(computerName) || String.IsNullOrEmpty(macAddress))
            {
                m_error_info = "ParseSettingsStates() :: ComputerName or MAC are empty.";
                return false;
            }


            foreach (XmlNode node in doc.GetElementsByTagName("Key"))
            {
                if (node.Attributes["name"].Value == "Vba32")
                {
                    IConfigureTask task;
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        if (child.Name.ToLower() == "key")
                        {
                            switch (child.Attributes["name"].Value.ToLower())
                            {
                                case "loader":
                                    task = new TaskConfigureLoader();
                                    task.LoadFromRegistry(child.OuterXml);
                                    result = InsertSettings(macAddress, "Vba32 Loader", task.SaveToXml());
                                    break;
                                case "monitor":
                                    task = new TaskConfigureMonitor();
                                    task.LoadFromRegistry(child.OuterXml);
                                    result = InsertSettings(macAddress, "Vba32 Monitor", task.SaveToXml());
                                    break;
                                case "qtn":
                                    task = new TaskConfigureQuarantine();
                                    task.LoadFromRegistry(child.OuterXml);
                                    result = InsertSettings(macAddress, "Vba32 Quarantine", task.SaveToXml());
                                    break;
                            }
                        }
                    }
                    break;
                }
            }

            return result;
        }

        private bool ParseComponentState(XmlTextReader xml_reader)
        {
            bool result = true;
            string computer_name = null;
            if (xml_reader.MoveToFirstAttribute())
            {
                computer_name = xml_reader.Value;
            }
            while (xml_reader.Read())
            {
                if ((xml_reader.NodeType == XmlNodeType.Element) && (xml_reader.Name.Equals("Component")))
                {
                    StringDictionary name_value_map = new StringDictionary();
                    result = ParseCommonElement(xml_reader, name_value_map);
                    if (!result)
                        return result;
                    name_value_map.Add("ComputerName", computer_name);
                    result = InsertComponentState(name_value_map);
                    if (!result)
                        return result;
                }
            }
            return result;
        }

        private bool ParseEvents(XmlTextReader xml_reader)
        {
            bool result = true;
            while (xml_reader.Read())
            {
                if ((xml_reader.NodeType == XmlNodeType.Element) && (xml_reader.Name.Equals("Event")))
                {
                    StringDictionary name_value_map = new StringDictionary();
                    result = ParseCommonElement(xml_reader, name_value_map);
                    if (!result)
                        return result;
                    result = InsertEvent(name_value_map);
                    if (!result)
                        return result;
                }
            }
            return result;
        }

        private bool ParseProcessInfo(XmlTextReader xml_reader)
        {
            bool result = true;
            string computer_name = null;
            if (xml_reader.MoveToFirstAttribute())
            {
                computer_name = xml_reader.Value;
                result = DeleteProcessInfo(computer_name);
            }
            if (!result)
            {
                return result;
            }
            while (xml_reader.Read())
            {
                if ((xml_reader.NodeType == XmlNodeType.Element) && (xml_reader.Name.Equals("Process")))
                {
                    StringDictionary name_value_map = new StringDictionary();
                    result = ParseCommonElement(xml_reader, name_value_map);
                    if (!result)
                        return result;
                    name_value_map.Add("ComputerName", computer_name);
                    result = InsertProcessInfo(name_value_map);
                    if (!result)
                        return result;
                }
            }
            return result;
        }

        private bool ParseSystemInfo(XmlTextReader xml_reader)
        {
            bool result = true;
            StringDictionary name_value_map = new StringDictionary();
            result = ParseCommonElement(xml_reader, name_value_map);
            if (!result)
                return result;
            result = InsertSystemInfo(name_value_map);
            return result;
        }

        private bool ParseTaskStates(XmlTextReader xml_reader)
        {
            bool result = true;
            while (xml_reader.Read())
            {
                if ((xml_reader.NodeType == XmlNodeType.Element) && (xml_reader.Name.Equals("TaskState")))
                {
                    result = ParseTaskState(xml_reader);
                }
                if (!result)
                {
                    return result;
                }
            }
            return result;
        }

        private bool ParseTaskState(XmlTextReader xml_reader)
        {
            bool result = true;
            StringDictionary name_value_map = new StringDictionary();
            result = ParseCommonElement(xml_reader, name_value_map);
            if (!result)
                return result;
            result = InsertTaskState(name_value_map);
            return result;
        }

        #endregion
        #region DataBase Basic Functions
        private bool ExecuteStoredProcedure(SqlCommand named_command_with_parameters)
        {
            bool result = true;
            int rows = 0;
            try
            {
                named_command_with_parameters.Connection = m_sql_connection;
                named_command_with_parameters.CommandType = CommandType.StoredProcedure;
                rows = named_command_with_parameters.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                m_error_info = e.Message;
                result = false;
            }
            return result;
        }

        private bool ReadObjectFromDataBase(SqlCommand named_command_with_parameters, ref object data_object)
        {
            bool result = true;
            SqlDataReader reader = null;
            try
            {
                named_command_with_parameters.Connection = m_sql_connection;
                named_command_with_parameters.CommandType = CommandType.StoredProcedure;
                reader = named_command_with_parameters.ExecuteReader();
                if (reader.Read())
                {
                    data_object = reader[0];
                }
            }
            catch (Exception e)
            {
                m_error_info = e.Message;
                result = false;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return result;
        }

        private bool ConnectDB()
        {
            bool result = true;
            try
            {
                m_sql_connection.Close();
            }
            catch { }
            try
            {
                m_sql_connection.ConnectionString = GetConnectionString();
                m_sql_connection.Open();
            }
            catch (Exception e)
            {
                m_error_info = e.Message;
                Log.Write(e.Message);
                result = false;
            }
            return result;
        }

        private string GetConnectionString()
        {
            string connection_string = "SERVER=";
            connection_string += ReadServerName();
            connection_string += ";DATABASE=vbaControlCenterDB;UID=";
            connection_string += ReadUserName();
            connection_string += ";PWD=";
            connection_string += ReadPassword();
            return connection_string;
        }

        #endregion
        #region Database Updating Functions
        private bool DeleteProcessInfo(string computer_name)
        {
            bool result = true;
            try
            {
                SqlCommand command = new SqlCommand("DeleteProcessInfo");
                command.Parameters.Add("@ComputerName", SqlDbType.NVarChar, 64);
                command.Parameters[0].Value = computer_name;
                result = ExecuteStoredProcedure(command);
            }
            catch (Exception e)
            {
                m_error_info = e.Message;
                result = false;
            }
            return result;
        }

        private bool InsertComponentSettings(string computer_name, string component_name, string settings)
        {
            bool result = true;
            try
            {
                SqlCommand command = new SqlCommand("UpdateComponentSettings");
                command.Parameters.Add("@ComputerName", SqlDbType.NVarChar, 64);
                command.Parameters[0].Value = computer_name;
                command.Parameters.Add("@ComponentName", SqlDbType.NVarChar, 64);
                command.Parameters[1].Value = component_name;
                command.Parameters.Add("@ComponentSettings", SqlDbType.Text);
                command.Parameters[2].Value = settings;
                result = ExecuteStoredProcedure(command);
            }
            catch (Exception e)
            {
                m_error_info = e.Message;
                result = false;
            }
            return result;
        }

        private bool InsertComponentState(StringDictionary name_value_map)
        {
            bool result = true;
            try
            {
                SqlCommand command = new SqlCommand("UpdateComponentState");
                command.Parameters.Add("@ComputerName", SqlDbType.NVarChar, 64);
                command.Parameters[0].Value = name_value_map["ComputerName"];
                command.Parameters.Add("@ComponentName", SqlDbType.NVarChar, 64);
                command.Parameters[1].Value = name_value_map["Name"];
                command.Parameters.Add("@ComponentState", SqlDbType.NVarChar, 32);
                command.Parameters[2].Value = name_value_map["State"];
                command.Parameters.Add("@Version", SqlDbType.NVarChar, 64);
                command.Parameters[3].Value = name_value_map["Version"];
                result = ExecuteStoredProcedure(command);
            }
            catch (Exception e)
            {
                m_error_info = e.Message;
                result = false;
            }
            return result;
        }

        private bool InsertEvent(StringDictionary name_value_map)
        {
            bool result = true;
            try
            {
                SqlCommand command = new SqlCommand("AddEvent");
                command.Parameters.Add("@ComputerName", SqlDbType.NVarChar, 64);
                command.Parameters[0].Value = name_value_map["Computer"];
                command.Parameters.Add("@EventName", SqlDbType.NVarChar, 128);
                command.Parameters[1].Value = name_value_map["EventName"];

                command.Parameters.Add("@EventTime", SqlDbType.DateTime);
                IFormatProvider format = new CultureInfo("ru-RU");
                DateTime date_time = DateTime.Parse(name_value_map["EventTime"], format);
                command.Parameters[2].Value = date_time;

                command.Parameters.Add("@ComponentName", SqlDbType.NVarChar, 64);
                if (name_value_map["Component"] == null)
                {
                    name_value_map["Component"] = "(unknown)";
                }
                command.Parameters[3].Value = name_value_map["Component"];


                //A device has been inserted. 
                //We must check whether the device registered in the database
                if (name_value_map["EventName"] == "vba32.device.inserted")
                    OnDeviceInsert(new EventsEntity(name_value_map));

                //If this event is device's module we must change some 
                //properties in this event
                if (name_value_map["Component"] == "Vba32 Device Module")
                    ModifyDeviceEvent(ref name_value_map);
    

                command.Parameters.Add("@Object", SqlDbType.NVarChar, 260);
                command.Parameters[4].Value = name_value_map["Object"];
                command.Parameters.Add("@Comment", SqlDbType.NVarChar, 256);
                command.Parameters[5].Value = name_value_map["Comment"];
                result = ExecuteStoredProcedure(command);
                if (result)
                {
                    //Automatically tasks
                    AutomaticallyTasks.GiveTask(new EventsEntity(name_value_map));

                    OnEventInsert(name_value_map);
                }
            }
            catch (Exception e)
            {
                m_error_info = e.Message;
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Add convertation logic if this event is related to devices 
        /// This is a kostyl.
        /// </summary>
        /// <param name="name_value_map"></param>
        private void ModifyDeviceEvent(ref StringDictionary name_value_map)
        {
            try
            {
                string serial = name_value_map["Object"];

                //Is this operation thread-safe?
                SqlCommand command = new SqlCommand("GetDevicesPage", m_sql_connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Page", 1);
                command.Parameters.AddWithValue("@RowCount", 1);
                command.Parameters.AddWithValue("@OrderBy", "SerialNo ASC");


                command.Parameters.AddWithValue("@Where",
                                                //Is a SQL-injection possible?
                                                String.Format("SerialNo like '{0}'", serial));

                //The ORM-tool is the best to do it.. 
                string comment = "";
                SqlDataReader reader = null;
                try
                {
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {

                        if (reader.GetValue(3) != DBNull.Value)
                            comment = reader.GetString(3);
                    }
                }
                finally
                {
                    if ((reader != null)&&(!reader.IsClosed))
                        reader.Close();
                }

                if (String.IsNullOrEmpty(comment))
                {
                    byte[] bs = Convert.FromBase64String(serial);
                    comment = Encoding.UTF8.GetString(bs);
                }

                name_value_map["Comment"] = comment;
            }
            catch (Exception ex)
            {
                //debug
                //name_value_map["Comment"] = ex.Message;
            }
        }



        private bool InsertProcessInfo(StringDictionary name_value_map)
        {
            bool result = true;
            try
            {
                SqlCommand command = new SqlCommand("UpdateProcessInfo");
                command.Parameters.Add("@ComputerName", SqlDbType.NVarChar, 64);
                command.Parameters[0].Value = name_value_map["ComputerName"];
                command.Parameters.Add("@ProcessName", SqlDbType.NVarChar, 260);
                command.Parameters[1].Value = name_value_map["ProcessName"];
                command.Parameters.Add("@MemorySize", SqlDbType.Int);
                command.Parameters[2].Value = name_value_map["MemorySize"];
                command.Parameters.Add("@Date", SqlDbType.DateTime);
                command.Parameters[3].Value = DateTime.Now;

                result = ExecuteStoredProcedure(command);
            }
            catch (Exception e)
            {
                m_error_info = e.Message;
                result = false;
            }
            return result;
        }

        private bool InsertSystemInfo(StringDictionary name_value_map)
        {
            bool result = true;
            try
            {
                SqlCommand command = new SqlCommand("UpdateComputerSystemInfo");
                command.Parameters.Add("@ComputerName", SqlDbType.NVarChar, 64);
                command.Parameters[0].Value = name_value_map["ComputerName"];
                command.Parameters.Add("@IPAddress", SqlDbType.NVarChar, 16);
                command.Parameters[1].Value = name_value_map["IPAddress"];
                command.Parameters.Add("@DomainName", SqlDbType.NVarChar, 256);
                command.Parameters[2].Value = name_value_map["DomainName"];
                command.Parameters.Add("@UserLogin", SqlDbType.NVarChar, 256);
                command.Parameters[3].Value = name_value_map["UserLogin"];
                command.Parameters.Add("@OSName", SqlDbType.NVarChar, 128);
                command.Parameters[4].Value = name_value_map["OSName"];
                command.Parameters.Add("@RAM", SqlDbType.SmallInt);
                command.Parameters[5].Value = name_value_map["RAM"];
                command.Parameters.Add("@CPUClock", SqlDbType.SmallInt);
                command.Parameters[6].Value = name_value_map["CPUClock"];
                command.Parameters.Add("@Vba32Version", SqlDbType.NVarChar, 256);
                command.Parameters[7].Value = name_value_map["Vba32Version"];
                command.Parameters.Add("@Vba32Integrity", SqlDbType.Bit);
                command.Parameters[8].Value = name_value_map["Vba32Integrity"];
                command.Parameters.Add("@Vba32KeyValid", SqlDbType.Bit);
                command.Parameters[9].Value = name_value_map["Vba32KeyValid"];
                command.Parameters.Add("@ControlCenter", SqlDbType.Bit);
                command.Parameters[10].Value = name_value_map["ControlCenter"];
                command.Parameters.Add("@LicenseCount", SqlDbType.SmallInt);
                command.Parameters[11].Value = CompCount;
                command.Parameters.Add("@MACAddress", SqlDbType.NVarChar, 64);
                command.Parameters[12].Value = name_value_map["MACAddress"];
                result = ExecuteStoredProcedure(command);
            }
            catch (Exception e)
            {
                m_error_info = e.Message;
                result = false;
            }
            return result;
        }

        private bool InsertTaskState(StringDictionary name_value_map)
        {
            bool result = true;
            try
            {
                SqlCommand command = new SqlCommand("UpdateTaskState");
                command.Parameters.Add("@TaskID", SqlDbType.BigInt);
                command.Parameters[0].Value = name_value_map["ID"];
                command.Parameters.Add("@TaskState", SqlDbType.NVarChar, 32);
                command.Parameters[1].Value = name_value_map["State"];

                command.Parameters.Add("@Date", SqlDbType.DateTime);
                IFormatProvider format = new CultureInfo("ru-RU");
                DateTime date = DateTime.Parse(name_value_map["Date"], format);
                command.Parameters[2].Value = date;

                command.Parameters.Add("@Description", SqlDbType.NVarChar, 256);
                command.Parameters[3].Value = name_value_map["Description"];

                result = ExecuteStoredProcedure(command);
            }
            catch (Exception e)
            {
                m_error_info = e.Message;
                result = false;
            }
            return result;
        }

        private Boolean InsertSettings(String macAddress, String componentName, String settings)
        {
            Boolean result = true;
            try
            {
                SqlCommand command = new SqlCommand("InsertSettings");
                command.Parameters.Add("@MAC", SqlDbType.NVarChar, 64);
                command.Parameters[0].Value = macAddress;
                command.Parameters.Add("@ComponentName", SqlDbType.NVarChar, 64);
                command.Parameters[1].Value = componentName;
                command.Parameters.Add("@Settings", SqlDbType.NText);
                command.Parameters[2].Value = settings;

                result = ExecuteStoredProcedure(command);
            }
            catch (Exception e)
            {
                m_error_info = e.Message;
                result = false;
            }
            return result;
        }

        #endregion
        #region Registry Reading Functions

        private const string gc_database_regkey = "SOFTWARE\\Vba32\\ControlCenter\\DataBase";

        private string ReadRegistryValue(RegistryKey registry_key, string sub_key, string var_name)
        {
            string result = "";
            RegistryKey reg_key = registry_key;
            try
            {
                reg_key = reg_key.OpenSubKey(sub_key);
                result = reg_key.GetValue(var_name).ToString();
            }
            catch (Exception e)
            {
                m_error_info = e.Message;
            }
            return result;
        }

        private string DecryptBinaryToString(RegistryKey registry_key, string sub_key, string var_name)
        {
            RegistryKey reg_key = registry_key;
            string password = "";
            try
            {
                reg_key = reg_key.OpenSubKey(sub_key);
                Byte[] buffer = (Byte[])reg_key.GetValue(var_name);
                int buffer_length = buffer.Length;
                for (int i = 0; i < buffer_length; ++i)
                {
                    buffer[i] ^= 0x17;
                }
                password = System.Text.Encoding.UTF8.GetString(buffer);
            }
            catch (Exception e)
            {
                m_error_info = e.Message;
            }
            return password;
        }

        private string ReadServerName()
        {
            return ReadRegistryValue(Registry.LocalMachine, gc_database_regkey, "DataSource");
        }

        private string ReadUserName()
        {
            return ReadRegistryValue(Registry.LocalMachine, gc_database_regkey, "UserName");
        }

        private string ReadPassword()
        {
            return DecryptBinaryToString(Registry.LocalMachine, gc_database_regkey, "Password");
        }

        #endregion

        #region Connection control functions
        private System.Data.SqlClient.SqlConnection m_sql_connection = null;

        private const int c_delay = 120000; // = 2 min
        bool m_was_closed = false;
        bool m_was_opened = false;
        private void ConnectionControlThread()
        {
            while (true)
            {
                Thread.Sleep(c_delay);

                lock (this)
                {
                    if (IsConnectionNull())
                    {
                        m_sql_connection = new SqlConnection();
                    }

                    if (IsConnectionClosed())
                    {
                        ConnectDB();
                        m_was_closed = true;
                    }
                    else
                    {
                        string current_server_name = ReadServerName();
                        string current_user_name = ReadUserName();
                        // server name or user name has been changed
                        if ((m_server_name.ToLower() != current_server_name.ToLower()) ||
                            (m_user_name.ToLower() != current_user_name.ToLower()))
                        {
                            m_server_name = current_server_name;
                            m_user_name = current_user_name;
                            if (CloseConnection())
                            {
                                m_was_opened = true;
                                ConnectDB();
                            }
                        }
                    }
                    if (IsConnectionClosed())
                    {
                        if (m_was_opened) //was opened now closed
                        {
                            NotifyFalseConnection();
                            m_was_opened = false;
                        }
                    }
                    else
                    {
                        if (m_was_closed) //was closed now opened
                        {
                            NotifyTrueConnection();
                            m_was_closed = false;
                        }
                    }
                }
            }
        }

        // Create event that connection is established
        private void NotifyTrueConnection()
        {
            try
            {
                if (m_event_handlers == null)
                {
                    return;
                }
                Type events_handler_type = m_event_handlers.GetType();
                object[] args = { };
                events_handler_type.InvokeMember("OnConnectionTrue", BindingFlags.InvokeMethod, null, m_event_handlers, args);
            }
            catch (Exception exception)
            {
                m_error_info = exception.Message;
            }
        }

        // Create event that connection is not established
        private void NotifyFalseConnection()
        {
            try
            {
                if (m_event_handlers == null)
                {
                    return;
                }
                Type events_handler_type = m_event_handlers.GetType();
                object[] args = { };
                events_handler_type.InvokeMember("OnConnectionFalse", BindingFlags.InvokeMethod, null, m_event_handlers, args);
            }
            catch (Exception exception)
            {
                m_error_info = exception.Message;
            }
        }

        private bool IsConnectionNull()
        {
            bool result = m_sql_connection == null;
            return result;
        }

        private bool IsConnectionClosed()
        {
            bool result = ((m_sql_connection.State == ConnectionState.Closed) || (m_sql_connection.State == ConnectionState.Broken));
            return result;
        }
        #endregion
        private string m_server_name = "";
        private string m_user_name = "";
        private string m_error_info = "Operation successfully";

        #region Notification functions
        // Check if the notification is needed
        private bool GetEventTypeNotify(string event_name, ref bool notification_is_needed)
        {
            bool result = true;
            try
            {
                SqlCommand command = new SqlCommand("GetEventTypeNotify");
                command.Parameters.Add("@EventName", SqlDbType.NVarChar, 128);
                command.Parameters[0].Value = event_name;
                object data_object = null;
                result = ReadObjectFromDataBase(command, ref data_object);
                notification_is_needed = Convert.ToBoolean(data_object);
            }
            catch (Exception exception)
            {
                m_error_info = exception.Message;
                result = false;
            }
            return result;
        }

        // Get computer ip-address by computer name
        private bool GetComputerIPAddress(string computer_name, ref string ip_address)
        {
            bool result = true;
            try
            {
                SqlCommand command = new SqlCommand("GetComputerIPAddress");
                command.Parameters.Add("@ComputerName", SqlDbType.NVarChar, 64);
                command.Parameters[0].Value = computer_name;
                object data_object = null;
                result = ReadObjectFromDataBase(command, ref data_object);
                ip_address = data_object.ToString();
            }
            catch (Exception exception)
            {
                m_error_info = exception.Message;
                result = false;
            }
            return result;
        }

        private const string mc_ipc_url = "ipc://Vba32NS/Vba32NS.rem";
        private bool OnEventInsert(StringDictionary name_value_map)
        {
            bool result = true;
            try
            {
                string ip_address = "";
                result = GetComputerIPAddress(name_value_map["Computer"], ref ip_address);
                if (!result || ip_address == null || ip_address == "")
                {
                    return result;
                }
                else
                {
                    name_value_map.Add("IPAddress", ip_address);
                }

                /*if (name_value_map["EventName"] == "vba32.device.inserted")
                {
                    OnDeviceInsert(new EventsEntity(name_value_map));
                }*/

                bool notification_is_needed = false;
                result = GetEventTypeNotify(name_value_map["EventName"], ref notification_is_needed);

                if ((!result || !notification_is_needed) &&
                    (name_value_map["EventName"] != virusFoundEvent))
                {
                    return result;
                }


                if (IsNeedSendNotify(ref name_value_map))
                {
                    //virus.found has been detected, but not it marked to send and no epidemy
                    if ((name_value_map["EventName"] == virusFoundEvent) &&
                         (!notification_is_needed))
                        return result;

                    Debug.WriteLine("Need send event " + name_value_map["EventName"]);
                    INotification notifier = (Vba32.ControlCenter.NotificationService.INotification)Activator.GetObject(
                        typeof(Vba32.ControlCenter.NotificationService.INotification), mc_ipc_url);

                  

                    notifier.OnRegisteredMessage(name_value_map);
                }
                else
                {
                    Debug.WriteLine("Sending event was skipped: " + name_value_map["EventName"]);
                }
            }
            catch (Exception exception)
            {
                m_error_info = exception.Message;
                result = false;
            }
            return result;
        }

        #endregion


        #region Flow analysis

        private const string notifyRegistryKey = "SOFTWARE\\Vba32\\ControlCenter\\Notification";
        private const string globalEpidemyEvent = "vba32.cc.GlobalEpidemy";
        private const string localHearthEvent = "vba32.cc.LocalHearth";
        private const string virusFoundEvent = "vba32.virus.found";

        private bool globalNotifyStatus = false; //Поле оптимизировать

        private bool IsNeedSendNotify(ref StringDictionary dic)
        {
            Debug.WriteLine("IsNeedSendNotify is called");

            Debug.WriteLine("read values from registry...");

            int useFlowAnalysis = ReadRegistryDwordValue(
                Registry.LocalMachine,
                notifyRegistryKey,
                "UseFlowAnalysis");

            //Не используем обработку потока уведомлений
            if ((useFlowAnalysis < 1)||(useFlowAnalysis==10)) return true; 

            EventsFlowManager flow = new EventsFlowManager(m_sql_connection);
            flow.GlobalEpidemyLimit = ReadRegistryDwordValue(
                Registry.LocalMachine, 
                notifyRegistryKey, 
                "GlobalEpidemyLimit");
            //Debug.WriteLine("GlobalEpidemyLimit is " + flow.GlobalEpidemyLimit);

            flow.GlobalEpidemyTimeLimit = ReadRegistryDwordValue(
                Registry.LocalMachine,
                notifyRegistryKey,
                "GlobalEpidemyTimeLimit");
            //Debug.WriteLine("GlobalEpidemyTimeLimit is " + flow.GlobalEpidemyTimeLimit);

            flow.LocalHearthLimit = ReadRegistryDwordValue(
                Registry.LocalMachine,
                notifyRegistryKey,
                "LocalHearthLimit");
           // Debug.WriteLine("LocalHearthLimit is " + flow.LocalHearthLimit);

            flow.LocalHearthTimeLimit = ReadRegistryDwordValue(
                Registry.LocalMachine,
                notifyRegistryKey,
                "LocalHearthTimeLimit");
            //Debug.WriteLine("LocalHearthTimeLimit is " + flow.LocalHearthTimeLimit);

            flow.Limit = ReadRegistryDwordValue(
                Registry.LocalMachine,
                notifyRegistryKey,
                "Limit");
            Debug.WriteLine("Limit is " + flow.Limit);

            flow.TimeLimit = ReadRegistryDwordValue(
                Registry.LocalMachine,
                notifyRegistryKey,
                "TimeLimit");
           // Debug.WriteLine("TimeLimit is " + flow.TimeLimit);

            flow.GlobalEpidemyCompCount = ReadRegistryDwordValue(
               Registry.LocalMachine,
               notifyRegistryKey,
               "GlobalEpidemyCompCount");
            Debug.WriteLine("TimeLimit is " + flow.GlobalEpidemyCompCount);

            EventsEntity ev = new EventsEntity(dic);

            Debug.WriteLine("Event is "+ev.Event);

            if (ev.Event == virusFoundEvent)
            {
                //Впервые обнаружена глобальная эпидемия
                bool gResult = flow.IsGlobalEpidemy(ev);
                if (gResult && (!globalNotifyStatus))
                {
                    Debug.WriteLine("is first vba32.cc.GlobalEpidemy");
                    globalNotifyStatus = true;
                    dic["EventName"] = globalEpidemyEvent;
                    dic["Object"] = flow.GlobalEpidemyCompList;
                    InsertEventWithoutNotify(dic);
                    return true;
                }
                else
                    if (!gResult)
                    {
                        globalNotifyStatus = false;
                    }
                    else
                    {
                        Debug.WriteLine("is older vba32.cc.GlobalEpidemy");
                        return false;
                    }
                   /* //Повторно обнаружена глоб. эпидемия. Уведомлять не следует
                    if (gResult && (!flow.IsNeedSendGlobalEpidemyWarning))
                    {
                        Debug.WriteLine("is older vba32.cc.GlobalEpidemy");
                        return false;
                    }*/

                bool lResult = flow.IsLocalHearth(ev);

                //Впервые обнаружен очаг заражения.
                if (lResult && (flow.IsNeedSendLocalHearthWarning))
                {
                    Debug.WriteLine("is first vba32.cc.LocalHearth");
                    dic["EventName"] = localHearthEvent;
                    InsertEventWithoutNotify(dic);
                    return true;
                }
                else
                    //Повторно обнаружен очаг заражения. Уведомлять не следует
                    if (lResult && (!flow.IsNeedSendLocalHearthWarning))
                    {
                        Debug.WriteLine("is older vba32.cc.LocalHearth");
                        return false;
                    }

                //Обрубим дальнейшую обработку события vba32.virus.found
                Debug.WriteLine("is simply virus.found event");
                return true;
            }

            Debug.WriteLine("flow analysis...");
            return flow.FlowAnalysis(ev);
        }

        /// <summary>
        /// Считывает из реестра dword значение
        /// </summary>
        /// <param name="registry_key">Ветка реестра</param>
        /// <param name="sub_key">Раздел</param>
        /// <param name="var_name">ключ</param>
        /// <returns>Значение:10 в случае ошибки</returns>
        private int ReadRegistryDwordValue(RegistryKey registry_key, string sub_key, string var_name)
        {
            RegistryKey reg_key = registry_key;
            try
            {
                reg_key = reg_key.OpenSubKey(sub_key);
                int? tmp = (int?) reg_key.GetValue(var_name);

                if (tmp.HasValue)
                    return tmp.Value;

            }
            catch (Exception e)
            {
                m_error_info = e.Message;
            }
            return 10;
       }

        /// <summary>
        /// Фактически дублирует метод InsertEvent, но без попытки вызвать Vba32NS
        /// Возможно, стоит как-то объединить два этих метода
        /// </summary>
        private void InsertEventWithoutNotify(StringDictionary name_value_map)
        {
            try
            {
                SqlCommand command = new SqlCommand("AddEvent");
                command.Parameters.Add("@ComputerName", SqlDbType.NVarChar, 64);
                command.Parameters[0].Value = name_value_map["Computer"];
                command.Parameters.Add("@EventName", SqlDbType.NVarChar, 128);
                command.Parameters[1].Value = name_value_map["EventName"];

                command.Parameters.Add("@EventTime", SqlDbType.DateTime);
                IFormatProvider format = new CultureInfo("ru-RU");
                DateTime date_time = DateTime.Parse(name_value_map["EventTime"], format);
                command.Parameters[2].Value = date_time;

                command.Parameters.Add("@ComponentName", SqlDbType.NVarChar, 64);
                if (name_value_map["Component"] == null)
                {
                    name_value_map["Component"] = "(unknown)";
                }
                command.Parameters[3].Value = name_value_map["Component"];

                command.Parameters.Add("@Object", SqlDbType.NVarChar, 260);
                command.Parameters[4].Value = name_value_map["Object"];
                command.Parameters.Add("@Comment", SqlDbType.NVarChar, 256);
                command.Parameters[5].Value = name_value_map["Comment"];
                ExecuteStoredProcedure(command);
            }
            catch (Exception ex)
            {
                Debug.Write("InsertEventWithoutNotify: "+ex);
            }
        }

        #endregion


        #region
        private void OnDeviceInsert(EventsEntity ev)
        {
            try
            {
                SqlCommand command = new SqlCommand("OnInsertingDevice");

                command.Parameters.Add("@SerialNo", SqlDbType.NVarChar, 256);
                command.Parameters[0].Value = ev.Object;

                command.Parameters.Add("@ComputerName", SqlDbType.NVarChar, 64);
                command.Parameters[1].Value = ev.Computer;

                command.Parameters.Add("@Comment", SqlDbType.NVarChar, 128);
                command.Parameters[2].Value = Anchor.GetCommentFromSerial(ev.Object);

                command.Parameters.Add("@LicenseCount", SqlDbType.SmallInt);
                command.Parameters[3].Value = CompUsbCount;
                
                ExecuteStoredProcedure(command);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex);
            }
        }
        #endregion
    }
}