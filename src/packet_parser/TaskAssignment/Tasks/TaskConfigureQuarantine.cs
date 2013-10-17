using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Vba32CC.TaskAssignment.Tasks
{
    public class TaskConfigureQuarantine : IConfigureTask
    {
        #region Fields

        public const String TaskType = "ConfigureQuarantine";
        public const String PassPrefix = "HER!%&$";

        private String _Vba32CCUser;
        private String _StoragePath;
        private Int32 _UseProxy;
        private String _UserName;
        private String _Password;
        private String _Proxy;
        private Int32 _ProxyPort;
        private Int32 _TimeOutEx;
        private Int32 _TimeOut;
        private Int32 _MaxSizeEx;
        private Int32 _MaxSize;
        private Int32 _MaxTimeEx;
        private Int32 _MaxTime;
        private Int32 _AutoSend;
        private Int32 _INARACTIVE_MAINT;

        #endregion

        #region Properties

        public Int32 INARACTIVE_MAINT
        {
            get { return _INARACTIVE_MAINT; }
            set { _INARACTIVE_MAINT = value; }
        }

        public Int32 AutoSend
        {
            get { return _AutoSend; }
            set { _AutoSend = value; }
        }

        public Int32 MaxTime
        {
            get { return _MaxTime; }
            set { _MaxTime = value; }
        }

        public Int32 MaxTimeEx
        {
            get { return _MaxTimeEx; }
            set { _MaxTimeEx = value; }
        }

        public Int32 MaxSize
        {
            get { return _MaxSize; }
            set { _MaxSize = value; }
        }

        public Int32 MaxSizeEx
        {
            get { return _MaxSizeEx; }
            set { _MaxSizeEx = value; }
        }

        public Int32 TimeOut
        {
            get { return _TimeOut; }
            set { _TimeOut = value; }
        }

        public Int32 TimeOutEx
        {
            get { return _TimeOutEx; }
            set { _TimeOutEx = value; }
        }

        public Int32 ProxyPort
        {
            get { return _ProxyPort; }
            set { _ProxyPort = value; }
        }

        public String Proxy
        {
            get { return _Proxy; }
            set { _Proxy = value; }
        }

        public String Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

        public String UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }

        public Int32 UseProxy
        {
            get { return _UseProxy; }
            set { _UseProxy = value; }
        }

        public String StoragePath
        {
            get { return _StoragePath; }
            set { _StoragePath = value; }
        }

        public String Vba32CCUser
        {
            get { return _Vba32CCUser; }
            set { _Vba32CCUser = value; }
        }

        #endregion

        #region Constructors

        public TaskConfigureQuarantine()
        {
        }

        #endregion

        #region IConfigureTask Members

        public String SaveToXml()
        {
            XmlBuilder xml = new XmlBuilder("qtn");
            
            if (!String.IsNullOrEmpty(StoragePath))
                xml.AddNode("StoragePath", "reg_sz:" + StoragePath);

            xml.AddNode("UseProxy", "reg_dword:" + UseProxy.ToString());
            if (UseProxy == 1)
            {
                xml.AddNode("UserName", "reg_sz:" + UserName);
                
                //Шифруем пароль
                if (!Password.Contains(PassPrefix))
                {
                    Byte[] bytes = Encoding.Unicode.GetBytes(Password);

                    Byte xorValue = 0xAA;
                    Byte delta = 0x1;

                    for (Int32 i = 0; i < bytes.Length; i++)
                    {
                        bytes[i] ^= xorValue;
                        delta = Convert.ToByte(delta % 3 + 1);
                        xorValue += delta;
                    }

                    xml.AddNode("Password", "reg_binary:" + TaskHelper.ConvertToDumpString(bytes));
                }
                else
                {
                    xml.AddNode("Password", Password.Replace(PassPrefix, ""));
                }

                xml.AddNode("Proxy", "reg_sz:" + Proxy);
                xml.AddNode("ProxyPort", "reg_dword:" + ProxyPort.ToString());
            }

            //вкладка "Обслуживание"
            xml.AddNode("TimeOutEx", "reg_dword:" + TimeOutEx.ToString());
            if (TimeOutEx == 1)
            {
                xml.AddNode("TimeOut", "reg_dword:" + TimeOut);

                xml.AddNode("MaxSizeEx", "reg_dword:" + MaxSizeEx.ToString());
                if (MaxSizeEx == 1)
                    xml.AddNode("MaxSize", "reg_dword:" + MaxSize.ToString());

                xml.AddNode("MaxTimeEx", "reg_dword:" + MaxTimeEx.ToString());
                if (MaxTimeEx == 1)
                    xml.AddNode("MaxTime", "reg_dword:" + MaxTime.ToString());

                xml.AddNode("AutoSend", "reg_dword:" + AutoSend.ToString());
                xml.AddNode("INARACTIVE_MAINT", "reg_dword:" + INARACTIVE_MAINT.ToString());
            }
            xml.AddNode("Vba32CCUser", Vba32CCUser);
            xml.AddNode("Type", TaskType);

            xml.Generate();

            return xml.Result;
        }

        public void LoadFromXml(String xml)
        {
            XmlTaskParser pars = new XmlTaskParser(xml);

            _StoragePath = pars.GetValue("StoragePath", "reg_sz:");
            if (String.IsNullOrEmpty(_StoragePath))
                _StoragePath = @"http://www.anti-virus.by/cgi-bin/vbar.cgi";

            Int32.TryParse(pars.GetValue("UseProxy", "reg_dword:"), out _UseProxy);
            _UserName = pars.GetValue("UserName", "reg_sz:");
            if (pars.GetValue("Password", "reg_binary:") != String.Empty)
                _Password = PassPrefix + pars.GetValue("Password", "reg_binary:");
            _Proxy = pars.GetValue("Proxy", "reg_sz:");
            Int32.TryParse(pars.GetValue("ProxyPort", "reg_dword:"), out _ProxyPort);

            Int32.TryParse(pars.GetValue("TimeOutEx", "reg_dword:"), out _TimeOutEx);
            Int32.TryParse(pars.GetValue("TimeOut", "reg_dword:"), out _TimeOut);
            Int32.TryParse(pars.GetValue("MaxSizeEx", "reg_dword:"), out _MaxSizeEx);
            Int32.TryParse(pars.GetValue("MaxSize", "reg_dword:"), out _MaxSize);
            Int32.TryParse(pars.GetValue("MaxTimeEx", "reg_dword:"), out _MaxTimeEx);
            Int32.TryParse(pars.GetValue("MaxTime", "reg_dword:"), out _MaxTime);
            Int32.TryParse(pars.GetValue("AutoSend", "reg_dword:"), out _AutoSend);
            Int32.TryParse(pars.GetValue("INARACTIVE_MAINT", "reg_dword:"), out _INARACTIVE_MAINT);
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
        <Key name="Qtn">
			<Value name="AutoSend" type="1" value="0"/>
			<Value name="INARACTIVE_MAINT" type="1" value="0"/>
			<Value name="MaxSize" type="1" value="1024"/>
			<Value name="MaxSizeEx" type="1" value="1"/>
			<Value name="MaxTime" type="1" value="30"/>
			<Value name="MaxTimeEx" type="1" value="1"/>
			<Value name="TimeOut" type="1" value="1"/>
			<Value name="TimeOutEx" type="1" value="1"/>
			<Value name="StoragePath" type="2" value="C:\Program Files\Vba32\Qtn"/>
			<Value name="UseProxy" type="1" value="0"/>
		</Key>
             */
        }

        #endregion
    }
}
