using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace ARM2_dbcontrol.Tasks
{
    [Serializable]
    public class TaskConfigureScheduler: IConfigureTask
    {
        #region Fields

        private String _TaskType = "ConfigureScheduler";
        private String _Vba32CCUser;
        private XmlSerializer serializer;
        private List<SchedulerTask> _schedulerTasksList;
        #endregion
        #region Properties

        public String Vba32CCUser
        {
            get { return _Vba32CCUser; }
            set { _Vba32CCUser = value; }
        }

        public String TaskType
        {
            get { return _TaskType; }
        }

        public List<SchedulerTask> SchedulerTasksList
        {
            get { return _schedulerTasksList; }
            set { _schedulerTasksList = value; }
        }
        #endregion

        #region Constructors
        public TaskConfigureScheduler()
        {
            serializer = new XmlSerializer(this.GetType());
            _schedulerTasksList = new List<SchedulerTask>();
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
            result.Append(@"<arg><key>settings</key><value><config><id>Normal</id><module><id>{AE8BC729-4C9D-4123-B1CC-28B6EBEA6EAA}</id>");

            result.Append(@"<param><id><tasks></id><type>stringlist</type><value>");
            for (int i = 0; i < _schedulerTasksList.Count; i++)
            {
                result.AppendFormat(@"string><id>{0}</id><val>{1}_{2}</val></string>", i, SchedulerTasksList[i].Type.ToString(), i + 1);
            }
            result.Append(@"</param>");

            for (int i = 0; i < _schedulerTasksList.Count; i++)
            {
                result.AppendFormat(@"<param><id>{0}_{1}_type</id><type>ulong</type><value>{2}</value></param>", SchedulerTasksList[i].Type.ToString(), i + 1, (UInt32)SchedulerTasksList[i].Period);
                result.AppendFormat(@"<param><id>{0}_{1}_time</id><type>string</type><value>{2}</value></param>", SchedulerTasksList[i].Type.ToString(), i + 1, SchedulerTasksList[i].TaskDateTime);
                result.AppendFormat(@"<param><id>{0}_{1}_command</id><type>stringmap</type><value>",SchedulerTasksList[i].Type.ToString(), i + 1);
                result.AppendFormat(@"<string><id>0</id><key>command</key><val>{0}</val></string>",SchedulerTasksList[i].Type.ToString());
                String guid = "";
                if (SchedulerTasksList[i].Type == ActionTypeEnum.SCHD_SCANNER)
                {
                    guid = "{2E406790-5472-4E0C-9EBF-88D081AA09AC}";
                }
                else guid = "{D4041472-FEC0-41B5-A133-8AAC758C1006}";
                result.AppendFormat(@"<string><id>1</id><key>module-id</key><val>{0}</val></string></value></param>",guid);
            }  
            result.Append(@"</module></config></value></arg></command>");

            result.Append(@"</Args>");
            result.Append(@"<Async>0</Async>");
            result.Append(@"</VsisCommand>");

            return result.ToString();
        }

        public void LoadFromXml(String Xml)
        {
            if (String.IsNullOrEmpty(Xml))
                return;
            TaskConfigureScheduler task;
            using (TextReader reader = new StringReader(Xml))
            {
                task = (TaskConfigureScheduler)serializer.Deserialize(reader);
            }
            this._schedulerTasksList = task.SchedulerTasksList;
            this._Vba32CCUser = task.Vba32CCUser;
        }
        #endregion

        #region IConfigureTask Members
        public void LoadFromRegistry(string reg)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
    public struct SchedulerTask
    {
        public ActionTypeEnum Type;
        public PeriodicityEnum Period;
        public DateTime TaskDateTime;
    }
    public enum PeriodicityEnum
    {
        AtSystemStartUp = 0,
        SomeDateTime = 1,
        EveryHour = 2,
        EveryDayWeek = 3,
        EveryDayMonth = 4
    }

    public enum ActionTypeEnum
    {
        SCHD_SCANNER = 0,
        SCHD_LOADER = 1
    }  
}