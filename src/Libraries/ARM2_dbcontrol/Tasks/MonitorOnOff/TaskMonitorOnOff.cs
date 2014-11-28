using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.CC.Common.Xml;

namespace ARM2_dbcontrol.Tasks
{
    public class TaskMonitorOnOff: IConfigureTask
    {
        #region Fields

        private String _Vba32CCUser;
        private Boolean _IsMonitorOn = true;

        #endregion

        #region Properties

        public Boolean IsMonitorOn
        {
            get { return _IsMonitorOn; }
            set { _IsMonitorOn = value; }
        }

        public String Vba32CCUser
        {
            get { return _Vba32CCUser; }
            set { _Vba32CCUser = value; }
        }

        public String TaskType
        {
            get { return IsMonitorOn ? "MonitorOn" : "MonitorOff"; }
        }

        #endregion

        #region Constructors

        public TaskMonitorOnOff()
        { }

        #endregion

        #region IConfigureTask Members
        /// <summary>
        /// Сохранить в xml
        /// </summary>
        /// <returns></returns>
        public String SaveToXml()
        {
            XmlBuilder xml = new XmlBuilder("task");

            xml.AddNode("IsMonitorOn", IsMonitorOn ? "1" : "0");
            xml.AddNode("Vba32CCUser", Vba32CCUser);
            xml.AddNode("Type", TaskType);

            xml.Generate();

            return xml.Result;
        }
        /// <summary>
        /// Выдача задачи
        /// </summary>
        /// <returns></returns>
        public String GetTask()
        {
            StringBuilder result = new StringBuilder(256);

            result.Append("<VsisCommand>");
            result.Append("<Args>");

            result.Append(@"<command><arg><key>module-id</key><value>{7C62F84A-A362-4CAA-800C-DEA89110596C}</value></arg>");
            result.Append(@"<arg><key>command</key><value>apply_settings</value></arg>");
            result.Append(@"<arg><key>settings</key><value><config><id>Normal</id><module><id>{A3F5FCA0-46DC-4328-8568-5FDF961E87E6}</id>");
            result.AppendFormat(@"<param><id>Enable</id><type>string</type><value>{0}</value></param></module></config></value></arg></command>", IsMonitorOn ? "On" : "Off");

            result.Append(@"</Args>");
            result.Append(@"<Async>0</Async>");
            result.Append(@"</VsisCommand>");

            return result.ToString();
        }
        /// <summary>
        /// Загрузить из xml
        /// </summary>
        /// <param name="xml"></param>
        public void LoadFromXml(String xml)
        {
            if (String.IsNullOrEmpty(xml))
                return;

            XmlTaskParser pars = new XmlTaskParser(xml);
            IsMonitorOn = pars.GetValue("IsMonitorOn") == "1";
        }

        public void LoadFromRegistry(string reg)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
