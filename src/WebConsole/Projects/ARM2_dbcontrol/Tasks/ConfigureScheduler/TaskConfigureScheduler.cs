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
            result.Append(@"<arg><key>settings</key><value><config><id>Normal</id><module><id>{A3F5FCA0-46DC-4328-8568-5FDF961E87E6}</id>");



           


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
        Scan=0,
        Update
    }

  
}