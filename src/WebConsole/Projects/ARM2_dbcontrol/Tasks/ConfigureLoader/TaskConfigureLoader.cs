using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using VirusBlokAda.CC.Common.Xml;
using VirusBlokAda.CC.Common;

namespace ARM2_dbcontrol.Tasks
{
    public class TaskConfigureLoader: IConfigureTask
    {
        #region Fields

        public const String TaskType = "ConfigureLoader";
        public const String PassPrefix = "HER!%&$";

        private String _Vba32CCUser;
        private Int32 _AUTO_START;
        private Int32 _MONITOR_AUTO_START;
        private Int32 _PROTECT_LOADER;
        private Int32 _SHOW_WINDOW;
        private Int32 _AUTO_CHECK_MEMORY;
        private Int32 _CHECK_MEMORY_MODE = -1;
        private Int32 _AUTO_CHECK_BOOT;
        private Int32 _AUTO_CHECK_BOOT_FLOPPY;
        private Int32 _LOG_LIMIT;
        private Int32 _SOUND;
        private Int32 _ANIMATION;
        private Int32 _UPDATE_TIME;
        private Int32 _UPDATE_INTERACTIVE;
        private Int32 _PROXY_USAGE;
        private Int32 _PROXY_AUTHORIZE;
        private String _LOG_NAME;
        private Int32 _LOG_LIMIT_VALUE = -1;
        private Int32 _UPDATE_TIME_VALUE = -1;
        private String _UPDATE_FOLDER;
        private List<String> _UPDATE_FOLDER_LIST;
        private String _PROXY_ADDRESS;
        private Int32 _PROXY_PORT;
        private String _PROXY_USER;
        private String _PROXY_PASSWORD;
        private Int32 _SCAN_USB;

        #endregion

        #region Properties

        public Int32 SCAN_USB
        {
            get { return _SCAN_USB; }
            set { _SCAN_USB = value; }
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

        public List<String> UPDATE_FOLDER_LIST
        {
            get { return _UPDATE_FOLDER_LIST; }
            set { _UPDATE_FOLDER_LIST = value; }
        }

        public String UPDATE_FOLDER
        {
            get { return _UPDATE_FOLDER; }
            set { _UPDATE_FOLDER = value; }
        }

        public Int32 UPDATE_TIME_VALUE
        {
            get { return _UPDATE_TIME_VALUE; }
            set { _UPDATE_TIME_VALUE = value; }
        }

        public Int32 LOG_LIMIT_VALUE
        {
            get { return _LOG_LIMIT_VALUE; }
            set { _LOG_LIMIT_VALUE = value; }
        }

        public String LOG_NAME
        {
            get { return _LOG_NAME; }
            set { _LOG_NAME = value; }
        }

        public Int32 PROXY_AUTHORIZE
        {
            get { return _PROXY_AUTHORIZE; }
            set { _PROXY_AUTHORIZE = value; }
        }

        public Int32 PROXY_USAGE
        {
            get { return _PROXY_USAGE; }
            set { _PROXY_USAGE = value; }
        }

        public Int32 UPDATE_INTERACTIVE
        {
            get { return _UPDATE_INTERACTIVE; }
            set { _UPDATE_INTERACTIVE = value; }
        }

        public Int32 UPDATE_TIME
        {
            get { return _UPDATE_TIME; }
            set { _UPDATE_TIME = value; }
        }

        public Int32 ANIMATION
        {
            get { return _ANIMATION; }
            set { _ANIMATION = value; }
        }

        public Int32 SOUND
        {
            get { return _SOUND; }
            set { _SOUND = value; }
        }

        public Int32 LOG_LIMIT
        {
            get { return _LOG_LIMIT; }
            set { _LOG_LIMIT = value; }
        }

        public Int32 AUTO_CHECK_BOOT_FLOPPY
        {
            get { return _AUTO_CHECK_BOOT_FLOPPY; }
            set { _AUTO_CHECK_BOOT_FLOPPY = value; }
        }

        public Int32 AUTO_CHECK_BOOT
        {
            get { return _AUTO_CHECK_BOOT; }
            set { _AUTO_CHECK_BOOT = value; }
        }

        public Int32 CHECK_MEMORY_MODE
        {
            get { return _CHECK_MEMORY_MODE; }
            set { _CHECK_MEMORY_MODE = value; }
        }

        public Int32 AUTO_CHECK_MEMORY
        {
            get { return _AUTO_CHECK_MEMORY; }
            set { _AUTO_CHECK_MEMORY = value; }
        }

        public Int32 SHOW_WINDOW
        {
            get { return _SHOW_WINDOW; }
            set { _SHOW_WINDOW = value; }
        }

        public Int32 PROTECT_LOADER
        {
            get { return _PROTECT_LOADER; }
            set { _PROTECT_LOADER = value; }
        }

        public Int32 MONITOR_AUTO_START
        {
            get { return _MONITOR_AUTO_START; }
            set { _MONITOR_AUTO_START = value; }
        }
        
        public Int32 AUTO_START
        {
            get { return _AUTO_START; }
            set { _AUTO_START = value; }
        }

        public String Vba32CCUser
        {
            get { return _Vba32CCUser; }
            set { _Vba32CCUser = value; }
        }

        #endregion

        #region Constructors

        public TaskConfigureLoader()
        {
            _UPDATE_FOLDER_LIST = new List<String>();
        }

        #endregion

       
        #region IConfigureTask Members

        public String SaveToXml()
        {
            XmlBuilder xml = new XmlBuilder("loader");
            xml.Top = String.Empty;

            xml.AddNode("AUTO_START", "reg_dword:" + AUTO_START.ToString());
            xml.AddNode("MONITOR_AUTO_START", "reg_dword:" + MONITOR_AUTO_START.ToString());
            xml.AddNode("PROTECT_LOADER", "reg_dword:" + PROTECT_LOADER.ToString());
            xml.AddNode("SHOW_WINDOW", "reg_dword:" + SHOW_WINDOW.ToString());
            xml.AddNode("AUTO_CHECK_MEMORY", "reg_dword:" + AUTO_CHECK_MEMORY.ToString());

            if (CHECK_MEMORY_MODE > -1)
                xml.AddNode("CHECK_MEMORY_MODE", "reg_dword:" + CHECK_MEMORY_MODE.ToString());

            xml.AddNode("AUTO_CHECK_BOOT", "reg_dword:" + AUTO_CHECK_BOOT.ToString());
            xml.AddNode("AUTO_CHECK_BOOT_FLOPPY", "reg_dword:" + AUTO_CHECK_BOOT_FLOPPY.ToString());
            xml.AddNode("LOG_LIMIT", "reg_dword:" + LOG_LIMIT.ToString());
            xml.AddNode("SOUND", "reg_dword:" + SOUND.ToString());
            xml.AddNode("ANIMATION", "reg_dword:" + ANIMATION.ToString());
            xml.AddNode("UPDATE_TIME", "reg_dword:" + UPDATE_TIME.ToString());
            xml.AddNode("UPDATE_INTERACTIVE", "reg_dword:" + UPDATE_INTERACTIVE.ToString());
            xml.AddNode("PROXY_USAGE", "reg_dword:" + PROXY_USAGE.ToString());
            xml.AddNode("PROXY_AUTHORIZE", "reg_dword:" + PROXY_AUTHORIZE.ToString());

            xml.AddNode("LOG_NAME", "reg_sz:" + (String.IsNullOrEmpty(LOG_NAME) ? "Vba32Ldr.log" : LOG_NAME));
            if (LOG_LIMIT_VALUE > -1)
            {
                xml.AddNode("LOG_LIMIT_VALUE", "reg_dword:" + LOG_LIMIT_VALUE.ToString());
            }

            if (UPDATE_TIME_VALUE > -1)
            {
                xml.AddNode("UPDATE_TIME_VALUE", "reg_dword:" + UPDATE_TIME_VALUE.ToString());
            }

            if (!String.IsNullOrEmpty(UPDATE_FOLDER))
                xml.AddNode("UPDATE_FOLDER", "reg_sz:" + UPDATE_FOLDER);
            if (UPDATE_FOLDER_LIST.Count > 0)
            {
                StringBuilder str = new StringBuilder();
                foreach (String item in UPDATE_FOLDER_LIST)
                {
                    str.AppendFormat(item + "?");
                }
                str.Append("?");
                xml.AddNode("UPDATE_FOLDER_LIST", "reg_multi_sz:" + str.ToString());
            }

            if (!String.IsNullOrEmpty(PROXY_ADDRESS))
            {
                xml.AddNode("PROXY_ADDRESS", "reg_sz:" + PROXY_ADDRESS);
                xml.AddNode("PROXY_PORT", "reg_dword:" + PROXY_PORT.ToString());
            }

            if (!String.IsNullOrEmpty(PROXY_USER))
            {
                xml.AddNode("PROXY_USER", "reg_sz:" + PROXY_USER);

                if (!PROXY_PASSWORD.Contains(PassPrefix))
                {
                    Byte[] bytes = Encoding.Unicode.GetBytes(PROXY_PASSWORD);

                    Byte xorValue = 0xAA;
                    Byte delta = 0x1;

                    for (Int32 i = 0; i < bytes.Length; i++)
                    {
                        bytes[i] ^= xorValue;
                        delta = Convert.ToByte(delta % 3 + 1);
                        xorValue += delta;
                    }
                    xml.AddNode("PROXY_PASSWORD", "reg_binary:" + Anchor.ConvertToDumpString(bytes));
                }
                else
                {
                    xml.AddNode("PROXY_PASSWORD", PROXY_PASSWORD.Replace(PassPrefix, ""));
                }
            }

            xml.AddNode("SCAN_USB", "reg_dword:" + SCAN_USB.ToString());

            xml.AddNode("Vba32CCUser", Vba32CCUser);
            xml.AddNode("Type", TaskType);

            xml.Generate();

            return xml.Result;
        }

        public void LoadFromXml(String xml)
        {
            XmlTaskParser pars = new XmlTaskParser(xml);

            Int32.TryParse(pars.GetValue("AUTO_START", "reg_dword:"), out _AUTO_START);
            Int32.TryParse(pars.GetValue("MONITOR_AUTO_START", "reg_dword:"), out _MONITOR_AUTO_START);
            Int32.TryParse(pars.GetValue("PROTECT_LOADER", "reg_dword:"), out _PROTECT_LOADER);
            Int32.TryParse(pars.GetValue("SHOW_WINDOW", "reg_dword:"), out _SHOW_WINDOW);
            Int32.TryParse(pars.GetValue("AUTO_CHECK_MEMORY", "reg_dword:"), out _AUTO_CHECK_MEMORY);
            Int32.TryParse(pars.GetValue("AUTO_CHECK_BOOT", "reg_dword:"), out _AUTO_CHECK_BOOT);
            Int32.TryParse(pars.GetValue("AUTO_CHECK_BOOT_FLOPPY", "reg_dword:"), out _AUTO_CHECK_BOOT_FLOPPY);
            Int32.TryParse(pars.GetValue("LOG_LIMIT", "reg_dword:"), out _LOG_LIMIT);
            Int32.TryParse(pars.GetValue("SOUND", "reg_dword:"), out _SOUND);
            Int32.TryParse(pars.GetValue("ANIMATION", "reg_dword:"), out _ANIMATION);
            Int32.TryParse(pars.GetValue("UPDATE_TIME", "reg_dword:"), out _UPDATE_TIME);
            Int32.TryParse(pars.GetValue("UPDATE_INTERACTIVE", "reg_dword:"), out _UPDATE_INTERACTIVE);
            Int32.TryParse(pars.GetValue("PROXY_USAGE", "reg_dword:"), out _PROXY_USAGE);
            Int32.TryParse(pars.GetValue("PROXY_AUTHORIZE", "reg_dword:"), out _PROXY_AUTHORIZE);
            Int32.TryParse(pars.GetValue("CHECK_MEMORY_MODE", "reg_dword:"), out _CHECK_MEMORY_MODE);
            Int32.TryParse(pars.GetValue("SCAN_USB", "reg_dword:"), out _SCAN_USB);

            _LOG_NAME = pars.GetValue("LOG_NAME", "reg_sz:") != String.Empty ? pars.GetValue("LOG_NAME", "reg_sz:") : "Vba32Ldr.log";

            Int32.TryParse(pars.GetValue("LOG_LIMIT_VALUE", "reg_dword:"), out _LOG_LIMIT_VALUE);
            Int32.TryParse(pars.GetValue("UPDATE_TIME_VALUE", "reg_dword:"), out _UPDATE_TIME_VALUE);

            _UPDATE_FOLDER = pars.GetValue("UPDATE_FOLDER", "reg_sz:");

            String updateSourceList = pars.GetValue("UPDATE_FOLDER_LIST", "reg_multi_sz:");
            _UPDATE_FOLDER_LIST.Clear();
            if (!String.IsNullOrEmpty(updateSourceList))
            {
                String[] splitted = updateSourceList.Split('?');
                foreach (String str in splitted)
                    if (!String.IsNullOrEmpty(str))
                        _UPDATE_FOLDER_LIST.Add(str);
            }

            _PROXY_ADDRESS = pars.GetValue("PROXY_ADDRESS", "reg_sz:");
            Int32.TryParse(pars.GetValue("PROXY_PORT", "reg_dword:"), out _PROXY_PORT);
            _PROXY_USER = pars.GetValue("PROXY_USER", "reg_sz:");
            PROXY_PASSWORD = PassPrefix + pars.GetValue("PROXY_PASSWORD", "reg_binary:");
        }

        public void LoadFromRegistry(String reg)
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
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

        public String GetTask()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
