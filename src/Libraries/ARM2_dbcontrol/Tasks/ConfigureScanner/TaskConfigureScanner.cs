using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using ARM2_dbcontrol.Tasks.ConfigureJournalEvent;

namespace ARM2_dbcontrol.Tasks.ConfigureScanner
{
    [Serializable]
    public class TaskConfigureScanner : IConfigureTask
    {
        #region Fields

        private String _Type = "ConfigureScanner";
        private String _Vba32CCUser;
        private XmlSerializer serializer;

        private JournalEvent _journalEvent;

        private String _FileExtensions;
        private String _FileExtensionsExcluded;
        private Boolean _IsEnableCache;

        private Boolean _IsCheckMail;
        private Boolean _IsCheckArchives;
        private Boolean _IsFindPotential;
        private Boolean _IsFindVirusInstalls;
        private Boolean _IsArchiveMaxSize;
        private Int32 _ArchiveMaxSize;
        private Boolean _IsAuthenticode;

        private Boolean _IsSaveInfectedToQuarantine;
        private Boolean _IsSaveSuspiciousToQuarantine;
        private ScannerActions _InfectedAction;
        private ScannerActions _InfectedCases;
        private ScannerActions _SuspiciousAction;
        private ScannerActions _SuspiciousCases;
               
        #endregion

        #region Properties
        public JournalEvent journalEvent
        {
            get { return _journalEvent; }
            set { _journalEvent = value; }
        }

        public String FileExtensions
        {
            get { return _FileExtensions; }
            set { _FileExtensions = value; }
        }

        public String FileExtensionsExcluded
        {
            get { return _FileExtensionsExcluded; }
            set { _FileExtensionsExcluded = value; }
        }

        public Boolean IsEnableCache
        {
            get { return _IsEnableCache; }
            set { _IsEnableCache = value; }
        }

        public Boolean IsCheckMail
        {
            get { return _IsCheckMail; }
            set { _IsCheckMail = value; }
        }

        public Boolean IsCheckArchives
        {
            get { return _IsCheckArchives; }
            set { _IsCheckArchives = value; }
        }

        public Boolean IsFindPotential
        {
            get { return _IsFindPotential; }
            set { _IsFindPotential = value; }
        }

        public Boolean IsFindVirusInstalls
        {
            get { return _IsFindVirusInstalls; }
            set { _IsFindVirusInstalls = value; }
        }

        public Boolean IsArchivesMaxSize
        {
            get { return _IsArchiveMaxSize; }
            set { _IsArchiveMaxSize = value; }
        }

        public Int32 ArchiveMaxSize
        {
            get { return _ArchiveMaxSize; }
            set { _ArchiveMaxSize = value; }
        }

        public Boolean IsAuthenticode
        {
            get { return _IsAuthenticode; }
            set { _IsAuthenticode = value; }
        }

        public Boolean IsSaveInfectedToQuarantine
        {
            get { return _IsSaveInfectedToQuarantine; }
            set { _IsSaveInfectedToQuarantine = value; }
        }
        public Boolean IsSaveSuspiciousToQuarantine
        {
            get { return _IsSaveSuspiciousToQuarantine; }
            set { _IsSaveSuspiciousToQuarantine = value; }
        }
        public ScannerActions InfectedAction
        {
            get { return _InfectedAction; }
            set { _InfectedAction = value; }
        }

        public ScannerActions InfectedCases
        {
            get { return _InfectedCases; }
            set { _InfectedCases = value; }
        }

        public ScannerActions SuspiciousAction
        {
            get { return _SuspiciousAction; }
            set { _SuspiciousAction = value; }
        }

        public ScannerActions SuspiciousCases
        {
            get { return _SuspiciousCases; }
            set { _SuspiciousCases = value; }
        }
      

        public String Vba32CCUser
        {
            get { return _Vba32CCUser; }
            set { _Vba32CCUser = value; }
        }
        public String Type
        {
            get { return _Type; }
            set { _Type = "ConfigureScanner"; }
        }

        #endregion

        #region Constructors

        public TaskConfigureScanner()
        {
            serializer = new XmlSerializer(this.GetType());
            journalEvent = new JournalEvent();
        }
        public TaskConfigureScanner(String[]eventNames)
        {
            serializer = new XmlSerializer(this.GetType());
            journalEvent = new JournalEvent(eventNames);
        }
        #endregion

        #region Methods
        /// <summary>
        ///  Сохранить в xml
        /// </summary>
        /// <returns></returns>
        public String SaveToXml()
        {
            StringWriter sw = new StringWriter();
            serializer.Serialize(sw, this);
            return sw.ToString();
        }
        /// <summary>
        /// Выдача задачи
        /// </summary>
        /// <returns></returns>
        public String GetTask()
        {
            StringBuilder result = new StringBuilder(512);

            result.Append("<VsisCommand>");
            result.Append("<Args>");

            result.Append(@"<command><arg><key>module-id</key><value>{7C62F84A-A362-4CAA-800C-DEA89110596C}</value></arg>");
            result.Append(@"<arg><key>command</key><value>apply_settings</value></arg>");
            result.Append(@"<arg><key>settings</key><value><config><id>Normal</id><module><id>{2E406790-5472-4E0C-9EBF-88D081AA09AC}</id>");

            result.Append(journalEvent.GetTask());
            #region Settings
            Int32 index = 0;

           
            index = 0;
            result.Append(@"<param><id>ScanSettings</id><type>stringmap</type><value>");
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "AddSpyRiskWareAnalyze", IsFindPotential ? "On" : "Off");
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "ArchiveAnalyze", IsCheckArchives ? "On" : "Off");
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "Authenticode", IsAuthenticode ? "On" : "Off");
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "Cache", IsEnableCache ? "On" : "Off");
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "Heuristic", "On");
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "MailAnalyze", IsCheckMail ? "On" : "Off");
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "MaxArchiveSize", ArchiveMaxSize);
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "SfxAnalyze", IsFindVirusInstalls ? "On" : "Off");


            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "FirstInfectedAction", InfectedAction.ToString());
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "SecondInfectedAction", InfectedCases.ToString());           
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "ThirdInfectedAction", "None");

            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "FirstSuspectedAction", SuspiciousAction.ToString());
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "SecondSuspectedAction", "None");
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "ThirdSuspectedAction", "None");

            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "InfectedQuarantine", IsSaveInfectedToQuarantine ? "On" : "Off");
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "SuspectedQuarantine", IsSaveSuspiciousToQuarantine ? "On" : "Off");

            if(!String.IsNullOrEmpty(FileExtensions))
                result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "ProcessedExtensions", FileExtensions);
            
            if (!String.IsNullOrEmpty(FileExtensionsExcluded))
                result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "ExcludedExtensions", FileExtensionsExcluded);
            else
                result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val/></string>", index++, "ExcludedExtensions");
            result.Append(@"</value></param>");

            #endregion

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
            TaskConfigureScanner task;
            using (TextReader reader = new StringReader(Xml))
            {
                task = (TaskConfigureScanner)serializer.Deserialize(reader);
            }

            this._ArchiveMaxSize = task.ArchiveMaxSize;
            this._FileExtensions = task.FileExtensions;
            this._FileExtensionsExcluded = task.FileExtensionsExcluded;
            this._InfectedAction = task.InfectedAction;
            this._InfectedCases = task.InfectedCases;
            this._IsArchiveMaxSize = task.IsArchivesMaxSize;
            this._IsAuthenticode = task.IsAuthenticode;
            this._IsCheckArchives = task.IsCheckArchives;
            this._IsCheckMail = task.IsCheckMail;
            this._IsEnableCache = task._IsEnableCache;
            this._IsFindPotential = task.IsFindPotential;
            this._IsFindVirusInstalls = task.IsFindVirusInstalls;
            this._IsSaveInfectedToQuarantine = task.IsSaveInfectedToQuarantine;
            this._IsSaveSuspiciousToQuarantine = task.IsSaveSuspiciousToQuarantine;
            this._SuspiciousAction = task.SuspiciousAction;
            this._SuspiciousCases = task.SuspiciousCases;

            this._Vba32CCUser = task.Vba32CCUser;
            this._journalEvent = task.journalEvent;
        }

        #endregion

        #region IConfigureTask Members

        public void LoadFromRegistry(string reg)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    public enum ScannerActions
    {
        Cure = 0,
        Delete = 1,
        None = 2

    };
}
