using System.Text;

namespace Vba32CC
{
    using System;
    using System.Runtime.InteropServices;
    using VirusBlokAda.CC.DataBase;
    using System.IO;
    using System.Xml;
    using System.Reflection;

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
    public partial class PacketParser
    {
        private PParserProvider db;
        private readonly Object lockToken = new Object();

        private Int32 CompCount = 0;
        private Int32 CompUsbCount = 0;

        private readonly String AppPath = String.Empty;
        private const String logFileName = @"Vba32PacketParser.log";
        private const String keyFileName = @"vba32.key";

        // Constructor.
        public PacketParser()
        {
            AppPath = Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(PacketParser)).Location) + @"\";

            CheckDBProvider();
            CheckConnectDB();

            m_server_name = ReadServerName();
            m_user_name = ReadUserName();

            ParseKeyFile();
        }

        private void CheckDBProvider()
        {
            if (db == null)
                db = new PParserProvider(GetConnectionString());
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
                LoggerPP.log.Error("Key file is not exist or invalid.");
                CompCount = 0;
                CompUsbCount = 0;

                if (reader != null)
                    reader.Close();
            }
        }

        private Int32 GetLicensesCount(String keyFileContent, String keyName)
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
        public Boolean SetCallbacks(IPacketParserCallBacks event_handlers)
        {
            lock (lockToken)
            {
                Boolean result = true;
                try
                {
                    m_event_handlers = event_handlers;
                    if (!CheckConnectDB())
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
        public Boolean CloseConnection()
        {
            return true;
        }

        [ComVisible(true)]
        public Boolean ParseXmlToDB(String xml_fragment)
        {            
            lock (lockToken)
            {
                // Check the connection
                if (!CheckConnectDB())
                {
                    m_error_info = "Connection is not established";
                        NotifyFalseConnection();
                    return false;
                }

                Boolean result = true;
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

        public String GetLastError()
        {
            lock (lockToken)
            {
                String result = m_error_info;
                m_error_info = "Operation successfully";
                return result;
            }
        }

        
        #region DataBase Basic Functions

        private Boolean CheckConnectDB()
        {
            Exception e;
            if(!DataBaseProvider.CheckConnection(GetConnectionString(), out e))
            {
                m_error_info = e.Message;
                LoggerPP.log.Error(e.Message);
                return false;
            }
            return true;
        }

        private String GetConnectionString()
        {
            String connection_string = "SERVER=";
            connection_string += ReadServerName();
            connection_string += ";DATABASE=vbaControlCenterDB;UID=";
            connection_string += ReadUserName();
            connection_string += ";PWD=";
            connection_string += ReadPassword();

            return connection_string;
        }

        #endregion              

        #region Connection control functions

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

        #endregion

        private string m_server_name = "";
        private string m_user_name = "";
        private string m_error_info = "Operation successfully";
    }
}