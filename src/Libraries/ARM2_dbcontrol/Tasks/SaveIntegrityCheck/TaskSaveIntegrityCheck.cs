using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace ARM2_dbcontrol.Tasks
{
    [Serializable]
    public class TaskSaveIntegrityCheck : IConfigureTask
    {
        #region Fields

        private String _TaskType = "SaveCheckIntegrity";
        private String _Vba32CCUser;
        private XmlSerializer serializer;

        private String _Integrity;
        private String _Command;

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

        public String Integrity
        {
            get { return _Integrity; }
            set { _Integrity = value; }
        }

        public String Command
        {
            get { return _Command; }
            set { _Command = value; }
        }

        #endregion

        #region Constructors
        
        public TaskSaveIntegrityCheck()
        {
            serializer = new XmlSerializer(this.GetType());
        }

        #endregion

        #region IConfigureTask Members

        public string SaveToXml()
        {
            StringWriter sw = new StringWriter();
            serializer.Serialize(sw, this);
            return sw.ToString();
        }

        public void LoadFromXml(string xml)
        {
            if (String.IsNullOrEmpty(xml))
                return;
            TaskSaveIntegrityCheck task;
            using (TextReader reader = new StringReader(xml))
            {
                task = (TaskSaveIntegrityCheck)serializer.Deserialize(reader);
            }
            this._Integrity = task.Integrity;
            this._Command = task.Command;
            this._Vba32CCUser = task.Vba32CCUser;
        }

        public void LoadFromRegistry(string reg)
        {
            throw new NotImplementedException();
        }

        public string GetTask()
        {
            StringBuilder result = new StringBuilder(512);

            result.Append("<VsisCommand>");
            result.Append("<Args>");

            result.Append(@"<command><arg><key>module-id</key><value>{B4A234EB-3BAF-4822-9262-2467B035856C}</value></arg>");
            result.AppendFormat(@"<arg><key>command</key><value>{0}</value></arg>", Command);
            result.AppendFormat(@"<arg><key>integrity</key><value>{0}</value></arg>", Integrity);
            result.Append(@"</command>");

            result.Append(@"</Args>");
            result.Append(@"<Async>0</Async>");
            result.Append(@"</VsisCommand>");

            return result.ToString();
        }

        #endregion
    }
}
