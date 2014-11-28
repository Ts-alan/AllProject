using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using ARM2_dbcontrol.Tasks.ConfigureFileCleanerCleaningTemplate;
using ARM2_dbcontrol.Tasks.ConfigureJournalEvent;


namespace ARM2_dbcontrol.Tasks.ConfigureFileCleaner
{
    [Serializable]
    public class TaskConfigureFileCleaner : IConfigureTask
    {
        #region Fields

        private String _Type = "FileCleaner";
        private List<SingleCleaningTemplate> _FullProgramList;
        private JournalEvent _journalEvent;
        public const String GUID = "{76DC546B-D814-4E18-AF4B-C7D17BC0AB90}";
        private String _Vba32CCUser;
        private XmlSerializer serializer;

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
            set { _Type = "FileCleaner"; }
        }

        public List<SingleCleaningTemplate> FullProgramList
        {
            get { return _FullProgramList; }
            set { _FullProgramList = value; }
        }

        public JournalEvent journalEvent
        {
            get { return _journalEvent; }
            set { _journalEvent = value; }
        }

        #endregion

        #region Constructor

        public TaskConfigureFileCleaner(String[] eventNames)
        {
            _FullProgramList = new List<SingleCleaningTemplate>();
            serializer = new XmlSerializer(this.GetType());
            _journalEvent = new JournalEvent(eventNames);
        }

        public TaskConfigureFileCleaner()
        {
            _FullProgramList = new List<SingleCleaningTemplate>();
            serializer = new XmlSerializer(this.GetType());
            _journalEvent = new JournalEvent();
        }

        #endregion

        #region Methods
        /// <summary>
        /// Сохранить в xml
        /// </summary>
        /// <returns></returns>
        public String SaveToXml()
        {
            StringWriter sw = new StringWriter();
            serializer.Serialize(sw, this);
            return sw.ToString();
        }
        /// <summary>
        ///  Загрузить из xml
        /// </summary>
        /// <param name="Xml"></param>
        public void LoadFromXml(String Xml)
        {
            if (String.IsNullOrEmpty(Xml))
                return;

            TaskConfigureFileCleaner task;
            using (TextReader reader = new StringReader(Xml))
            {
                task = (TaskConfigureFileCleaner)serializer.Deserialize(reader);
            }

            this._FullProgramList = task.FullProgramList;
            this._journalEvent = task.journalEvent;
            this._Type = task.Type;
            this._Vba32CCUser = task.Vba32CCUser;
        }
        /// <summary>
        /// Get XML for agent
        /// </summary>
        /// <returns></returns>
        public String GetTask()
        {
            StringBuilder builder = new StringBuilder(512);

            builder.Append(@"<VsisCommand><Args><command><arg><key>module-id</key><value>{7C62F84A-A362-4CAA-800C-DEA89110596C}</value></arg><arg><key>command</key><value>apply_settings</value></arg>");
            builder.AppendFormat(@"<arg><key>settings</key><value><config><id>Normal</id><module><id>{0}</id>", GUID);
            builder.Append("<param>");
            builder.Append("<id>ActiveProgramsList</id>");
            builder.Append("<type>stringlist</type>");
            builder.Append("<value>");
            int index = 0;
            for (Int32 i = 0; i < _FullProgramList.Count; i++)
            {
                if (_FullProgramList[i].IsActive)
                {
                    builder.AppendFormat("<string><id>{0}</id><val>{1}</val></string>", index.ToString(), FullProgramList[i].Name);
                    index++;
                }
            }
            builder.Append("</value></param>");
            

            for (Int32 i = 0; i < _FullProgramList.Count; i++)
            {                
                builder.Append(FullProgramList[i].GetTask());                
            } 

            builder.Append(journalEvent.GetTask());



            builder.Append("<param>");
            builder.Append("<id>FullProgramsList</id>");
            builder.Append("<type>stringlist</type>");
            builder.Append("<value>");
            for (Int32 i = 0; i < _FullProgramList.Count; i++)
            {
                builder.AppendFormat("<string><id>{0}</id><val>{1}</val></string>", i.ToString(), FullProgramList[i].Name);
            }
            builder.Append("</value></param>");
            builder.Append(@"</module></config></value></arg></command></Args><Async>0</Async></VsisCommand>");
            
            return builder.ToString();
        }      
        
        #endregion

        #region IConfigureTask Members


        public void LoadFromRegistry(string reg)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}