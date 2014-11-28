using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using VirusBlokAda.CC.Common.Xml;
using VirusBlokAda.CC.Common;
using ARM2_dbcontrol.Tasks.ConfigureJournalEvent;
using System.Xml.Serialization;
using System.IO;

namespace ARM2_dbcontrol.Tasks
{
    public class TaskConfigureQuarantine: IConfigureTask
    {
        #region Fields

        public const String TaskType = "ConfigureQuarantine";
        public const String PassPrefix = "HER!%&$";



        private String _Type = "ConfigureQarantine";
        private String _Vba32CCUser;
        private XmlSerializer serializer;
        private JournalEvent _journalEvent;
        #endregion

        #region Properties

     

        public String Vba32CCUser
        {
            get { return _Vba32CCUser; }
            set { _Vba32CCUser = value; }
        }
        public String Type
        {
            get { return _Type; }
            set { _Type = "ConfigureQarantine"; }
        }

        public JournalEvent journalEvent
        {
            get { return _journalEvent; }
            set { _journalEvent = value; }
        }
        #endregion

        #region Constructors

        public TaskConfigureQuarantine()
        {
            serializer = new XmlSerializer(this.GetType());
            journalEvent = new JournalEvent();
        }
        public TaskConfigureQuarantine(String[] eventNames)
        {
            serializer = new XmlSerializer(this.GetType());
            journalEvent = new JournalEvent(eventNames);
        }
        #endregion

        #region IConfigureTask Members
        /// <summary>
        /// Сохранение в xml
        /// </summary>
        /// <returns></returns>
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
            result.Append(@"<arg><key>settings</key><value><config><id>Normal</id><module><id>{67E7B7BE-BE06-4A0D-A812-7E1A0142C0F6}</id>");

           
            result.Append(journalEvent.GetTask());
            
           

            result.Append(@"</module></config></value></arg></command>");

            result.Append(@"</Args>");
            result.Append(@"<Async>0</Async>");
            result.Append(@"</VsisCommand>");

            return result.ToString();
        }
        /// <summary>
        /// Загрузка из xml
        /// </summary>
        /// <param name="Xml"></param>
        public void LoadFromXml(String Xml)
        {
            if (String.IsNullOrEmpty(Xml))
                return;
            TaskConfigureQuarantine task;
            using (TextReader reader = new StringReader(Xml))
            {
                task = (TaskConfigureQuarantine)serializer.Deserialize(reader);
            }
           
            this._Vba32CCUser = task.Vba32CCUser;
            this._journalEvent = task.journalEvent;
        }

        public void LoadFromRegistry(String reg)
        {
            throw new NotImplementedException();
       /*     System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
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
