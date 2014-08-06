using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using VirusBlokAda.CC.Common.Xml;
using VirusBlokAda.CC.Common;
using System.Xml.Serialization;
using System.IO;

namespace ARM2_dbcontrol.Tasks
{
    public class TaskConfigureLoader: IConfigureTask
    {
        #region Fields

        public const String PassPrefix = "HER!%&$";

        private String _TaskType = "ConfigureLoader";
        private String _Vba32CCUser;
        private XmlSerializer serializer;

        private List<String> _UPDATE_FOLDER_LIST;

        private Boolean _AUTHORIZE;
        private String _AUTH_USER;
        private String _AUTH_PASSWORD;


        private Boolean _PROXY_USAGE;
        private Boolean _PROXY_AUTHORIZE;
        private ProxyType _PROXY_TYPE;
        private String _PROXY_ADDRESS;
        private Int32 _PROXY_PORT;
        private String _PROXY_USER;
        private String _PROXY_PASSWORD;

        private List<UserLoginPassword> _USERS;

        #endregion

        #region Properties

        public Boolean AUTHORIZE
        {
            get { return _AUTHORIZE; }
            set { _AUTHORIZE = value; }
        }

        public String AUTH_PASSWORD
        {
            get { return _AUTH_PASSWORD; }
            set { _AUTH_PASSWORD = value; }
        }

        public String AUTH_USER
        {
            get { return _AUTH_USER; }
            set { _AUTH_USER = value; }
        }

        public String PROXY_PASSWORD
        {
            get { return _PROXY_PASSWORD; }
            set { _PROXY_PASSWORD = value; }
        }

        public String PROXY_USER
        {
            get { return _PROXY_USER; }
            set { _PROXY_USER = value; }
        }

        public Int32 PROXY_PORT
        {
            get { return _PROXY_PORT; }
            set { _PROXY_PORT = value; }
        }

        public String PROXY_ADDRESS
        {
            get { return _PROXY_ADDRESS; }
            set { _PROXY_ADDRESS = value; }
        }

        public ProxyType PROXY_TYPE
        {
            get { return _PROXY_TYPE; }
            set { _PROXY_TYPE = value; }
        }

        public List<String> UPDATE_FOLDER_LIST
        {
            get { return _UPDATE_FOLDER_LIST; }
            set { _UPDATE_FOLDER_LIST = value; }
        }

        public Boolean PROXY_AUTHORIZE
        {
            get { return _PROXY_AUTHORIZE; }
            set { _PROXY_AUTHORIZE = value; }
        }

        public Boolean PROXY_USAGE
        {
            get { return _PROXY_USAGE; }
            set { _PROXY_USAGE = value; }
        }

        public List<UserLoginPassword> USERS
        {
            get { return _USERS; }
            set { _USERS = value; }
        }
       
        public String Vba32CCUser
        {
            get { return _Vba32CCUser; }
            set { _Vba32CCUser = value; }
        }

        public String TaskType
        {
            get { return _TaskType; }
        }

        #endregion

        #region Constructors

        public TaskConfigureLoader()
        {
            serializer = new XmlSerializer(this.GetType());
            _UPDATE_FOLDER_LIST = new List<String>();
            _USERS = new List<UserLoginPassword>();
        }

        #endregion

        #region Methods

        public String SaveToXml()
        {
            StringWriter sw = new StringWriter();
            serializer.Serialize(sw, this);
            return sw.ToString();
        }

        public String GetTask()
        {
            StringBuilder result = new StringBuilder(512);

            result.Append("<VsisCommand>");
            result.Append("<Args>");

            result.Append(@"<command><arg><key>module-id</key><value>{7C62F84A-A362-4CAA-800C-DEA89110596C}</value></arg>");
            result.Append(@"<arg><key>command</key><value>apply_settings</value></arg>");
            result.Append(@"<arg><key>settings</key><value><config><id>Normal</id><module><id>{D4041472-FEC0-41B5-A133-8AAC758C1006}</id>");

            result.AppendFormat(@"<param><id>AuthorityName</id><type>string</type><value>{0}</value></param>", AUTH_USER);
            result.AppendFormat(@"<param><id>AuthorityPassword</id><type>string</type><value>{0}</value></param>", AUTH_PASSWORD);

            result.AppendFormat(@"<param><id>ProxyAddress</id><type>string</type><value>{0}</value></param>", PROXY_ADDRESS);
            result.AppendFormat(@"<param><id>ProxyAuthorityName</id><type>string</type><value>{0}</value></param>", PROXY_USER);
            result.AppendFormat(@"<param><id>ProxyAuthorityPassword</id><type>string</type><value>{0}</value></param>", PROXY_PASSWORD);

            result.AppendFormat(@"<param><id>ProxyPort</id><type>ulong</type><value>{0}</value></param>", PROXY_PORT);
            result.AppendFormat(@"<param><id>ProxyType</id><type>ulong</type><value>{0}</value></param>", (int)PROXY_TYPE);



            result.AppendFormat(@"<param><id>UpdatePathes</id><type>stringlist</type><value>");
            for (int i = 0; i < UPDATE_FOLDER_LIST.Count; i++)
            {
                result.AppendFormat(@"<string><id>{0}</id><val>{1}</val></string>", i, UPDATE_FOLDER_LIST[i]);
            }
            result.AppendFormat(@"</value></param>");

            result.AppendFormat(@"<param><id>Passwords</id><type>stringmap</type><value>");
            for (int i = 0; i < USERS.Count; i++)
            {
                result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", i, USERS[i].Login, CriptPassword(USERS[i]));
            }
            result.AppendFormat(@"</value></param>");

            result.Append(@"</module></config></value></arg></command>");
            result.Append(@"</Args>");
            result.Append(@"<Async>0</Async>");
            result.Append(@"</VsisCommand>");

            return result.ToString();
        }

        private String CriptPassword(UserLoginPassword user)
        {
            String str = user.Password + user.Login;

            Byte[] data = Encoding.Unicode.GetBytes(str);
            StringBuilder builder = new StringBuilder();
            for (Int32 i = 0; i < data.Length; i++)
            {
                data[i] = (Byte)(data[i] ^ 0xB3);
                builder.Append(data[i].ToString("x2").ToUpper());
            }

            return builder.ToString();
        }

        public void LoadFromXml(String Xml)
        {
            if (String.IsNullOrEmpty(Xml))
                return;
            TaskConfigureLoader task;
            using (TextReader reader = new StringReader(Xml))
            {
                task = (TaskConfigureLoader)serializer.Deserialize(reader);
            }

            this._PROXY_ADDRESS = task.PROXY_ADDRESS;
            this._PROXY_AUTHORIZE = task.PROXY_AUTHORIZE;
            this._PROXY_PASSWORD = task.PROXY_PASSWORD;
            this._PROXY_PORT = task.PROXY_PORT;
            this._PROXY_USAGE = task.PROXY_USAGE;
            this._PROXY_USER = task.PROXY_USER;
            this._PROXY_TYPE = task.PROXY_TYPE;
            this._UPDATE_FOLDER_LIST = task.UPDATE_FOLDER_LIST;
            this._USERS = task.USERS;

            this._AUTH_PASSWORD = task.AUTH_PASSWORD;
            this._AUTH_USER = task.AUTH_USER;
            this._AUTHORIZE = task.AUTHORIZE;

            this._Vba32CCUser = task.Vba32CCUser;
        }

        #endregion

        #region IConfigureTask Members
        
        public void LoadFromRegistry(String reg)
        {
          /*  System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(reg);

            foreach (System.Xml.XmlNode node in doc.GetElementsByTagName("Value"))
            {
                PropertyInfo prop = this.GetType().GetProperty(node.Attributes["name"].Value, BindingFlags.Public | BindingFlags.Instance);
                if (null != prop && prop.CanWrite)
                {
                    prop.SetValue(this, TaskHelper.GetValue(node), null);
                }
            }


            /*
             Если аттрибут encoded равен 1, то содержание аттрибута value кодировано в base64. 
             Мультистроковые значения в аттрибуте всегда кодируются в base64, при декодировании из которого сама строка кодирована в UTF-16.

      Типы поддерживаемых значений реестра:
        None = "0"
        Dword = "1"
        String = "2"
        Binary = "3"
        Int64 = "4"
        MultiString = "5"
             * 
             EXAMPLE:
             * 
        <Key name="Loader">
            <Value name="ANIMATION" type="1" value="1"/>
            <Value name="AUTO_START" type="1" value="0"/>
            <Value name="LANGUAGE" type="2" value="Vba32RU.lng"/>
            <Value name="AUTO_CHECK_AUTORUN" type="1" value="1"/>
            <Value name="AUTO_CHECK_BOOT" type="1" value="0"/>
            <Value name="AUTO_CHECK_BOOT_FLOPPY" type="1" value="0"/>
            <Value name="AUTO_CHECK_MEMORY" type="1" value="1"/>
            <Value name="AUTO_CHECK_MEMORY_FAST" type="1" value="1"/>
            <Value name="LOG" type="1" value="1"/>
            <Value name="LOG_ADD" type="1" value="1"/>
            <Value name="LOG_LIMIT" type="1" value="1"/>
            <Value name="LOG_LIMIT_VALUE" type="1" value="256"/>
            <Value name="LOG_NAME" type="2" value="Vba32Ldr.log"/>
            <Value name="MONITOR_AUTO_START" type="1" value="1"/>
            <Value name="MONITOR_AUTO_START_PARAM" type="2" value="TURN+"/>
            <Value name="NO_LICENSE_EXPIRE_MESSAGE" type="1" value="0"/>
            <Value name="PATH" type="2" value="C:\Program Files\Vba32"/>
            <Value name="POPUP_LOADER" type="1" value="1"/>
            <Value name="PROTECT_LOADER" type="1" value="1"/>
            <Value name="PROXY_ADDRESS" type="2" value=""/>
            <Value name="PROXY_PORT" type="1" value="8080"/>
            <Value name="PROXY_USAGE" type="1" value="0"/>
            <Value name="ROOTKIT_SEARCH" type="1" value="1"/>
            <Value name="SHOW_WINDOW" type="1" value="0"/>
            <Value name="SOUND" type="1" value="1"/>
            <Value name="UPDATE_INTERACTIVE" type="1" value="0"/>
            <Value name="UPDATE_TIME" type="1" value="1"/>
            <Value name="UPDATE_TIME_VALUE" type="1" value="1"/>
            <Value name="UPDATE_FOLDER" type="2" value="http://anti-virus.by/update/"/>
            <Value name="UPDATE_URL_01" type="2" value="http://anti-virus.by/update/"/>
            <Key name="Devices">
                <Value name="Ae87b1b967f912dc4cd5ab8b8555e9b00" type="2" value="qwertyiuobfgddsfasdgdfgsdfsd3"/>
            </Key>
            <Key name="Extensions">
                <Value name="Vba32PP3" type="2" value="{7E2D88AC-A2EB-498C-B666-61E5F38B553F}"/>
                <Value name="vba32sck" type="2" value="{114CABC1-F9CF-49ab-BDFF-2EE55F4FC652}"/>
                <Value name="VbaOLPL" type="2" value="{70C2BD0C-C48A-4921-8FB9-3050905FF76F}"/>
                <Value name="VBA32AntiDialer" type="2" value="{D4F18217-7CC2-48C8-A945-E75BF5B8A8B9}"/>
            </Key>
        </Key>

              
             */
        }

        #endregion
    }

    public struct UserLoginPassword
    {
        public String Login;
        public String Password;
    }

    public enum ProxyType
    {
        PROXY_NO = 0,
        PROXY_HTTP = 1,
        PROXY_SOCKS4 = 3,
        PROXY_SOCKS5 = 4
    }
}
