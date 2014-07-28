using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using VirusBlokAda.CC.Common.Xml;
using VirusBlokAda.CC.Common;
using System.Xml.Serialization;
using ARM2_dbcontrol.Tasks.ConfigureJournalEvent;
using System.IO;

namespace ARM2_dbcontrol.Tasks
{
    [Serializable]
    public class TaskConfigureMonitor: IConfigureTask
    {
        #region Fields

        private String _TaskType = "ConfigureMonitor";
        public const String DefaultFilters = @"COM.EXE.DLL.DRV.SYS.OV?.VXD.SCR.CPL.OCX.BPL.AX.PIF.DO?.XL?.HLP.RTF.WI?.WZ?.MSI.MSC.HT*.VB*.JS.JSE.ASP*.CGI.PHP*.?HTML.BAT.CMD.EML.NWS.MSG.XML.MSO.WPS.PPT.PUB.JPG.JPEG.ANI.INF.SWF.PDF";

        private String _Vba32CCUser;
        private XmlSerializer serializer;
        private JournalEvent _journalEvent;

        private Boolean _MONITOR_ON;

        private String _FileExtensions;
        private String _FileExtensionsExcluded;

        private List<String> _ExcludingFoldersAndFilesDelete;

        private Boolean _IsSaveInfectedToQuarantine;
        private Boolean _IsSaveSuspiciousToQuarantine;
        private MonitorInfectedActions _InfectedAction1;
        private MonitorInfectedActions _InfectedAction2;
        private MonitorInfectedActions _InfectedAction3;
        private MonitorSuspiciousActions _SuspiciousAction1;
        private MonitorSuspiciousActions _SuspiciousAction2;

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

        public List<String> ExcludingFoldersAndFilesDelete
        {
            get { return _ExcludingFoldersAndFilesDelete; }
            set { _ExcludingFoldersAndFilesDelete = value; }
        }

        public Boolean MONITOR_ON
        {
            get { return _MONITOR_ON; }
            set { _MONITOR_ON = value; }
        }

        public Boolean IsSaveInfectedToQuarantine
        {
            get { return _IsSaveInfectedToQuarantine; }
            set { _IsSaveInfectedToQuarantine = value; }
        }

        public  Boolean IsSaveSuspiciousToQuarantine
        {
            get { return _IsSaveSuspiciousToQuarantine; }
            set { _IsSaveSuspiciousToQuarantine = value; }
        }


        public MonitorInfectedActions InfectedAction1
        {
            get { return _InfectedAction1; }
            set { _InfectedAction1 = value; }
        }
        public MonitorInfectedActions InfectedAction2
        {
            get { return _InfectedAction2; }
            set { _InfectedAction2 = value; }
        }
        public MonitorInfectedActions InfectedAction3
        {
            get { return _InfectedAction3; }
            set { _InfectedAction3 = value; }
        }


        public MonitorSuspiciousActions SuspiciousAction1
        {
            get { return _SuspiciousAction1; }
            set { _SuspiciousAction1 = value; }
        }
        public MonitorSuspiciousActions SuspiciousAction2
        {
            get { return _SuspiciousAction2; }
            set { _SuspiciousAction2 = value; }
        }

        #endregion

        #region Constructors

        public TaskConfigureMonitor()
        {
            serializer = new XmlSerializer(this.GetType());
            journalEvent = new JournalEvent();
            _ExcludingFoldersAndFilesDelete = new List<String>();
            _FileExtensions = DefaultFilters;
        }

        public TaskConfigureMonitor(String[]eventNames)
        {
            serializer = new XmlSerializer(this.GetType());
            journalEvent = new JournalEvent(eventNames);
            _ExcludingFoldersAndFilesDelete = new List<String>();
            _FileExtensions = DefaultFilters;
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



            result.AppendFormat(@"<param><id>Enable</id><type>string</type><value>{0}</value></param>", MONITOR_ON ? "On" : "Off");

            result.Append(journalEvent.GetTask());

            result.Append(@"<param><id>Journal</id><type>stringmap</type><value>");
            result.AppendFormat(@"<string><id>0</id><key>Enable</key><val>On</val></string>");
            result.AppendFormat(@"<string><id>1</id><key>FullFileList</key><val>Off</val></string>");
            result.Append(@"</value></param>");

            result.AppendFormat(@"<param><id>MntExcludedFiles</id><type>stringlist</type><value>
                <string>
                    <id>0</id>
                    <val>C:\pagefile.sys</val>
                </string>
                <string>
                    <id>1</id>
                    <val>C:\swapfile.sys</val>
                </string>
                <string>
                    <id>2</id>
                    <val>C:\hiberfil.sys</val>
                </string>
            </value></param>");

            result.AppendFormat(@"<param><id>MntExcludedFolders</id><type>stringlist</type><value>");
            for(int i=0;i<ExcludingFoldersAndFilesDelete.Count;i++)
            {
                result.AppendFormat(@"<string><id>{0}</id><val>{1}</val></string>",i,ExcludingFoldersAndFilesDelete[i]);
            }
            result.AppendFormat(@"</value></param>");

            #region ScanSettings
            Int32 index = 0;
            result.AppendFormat(@"<param><id>ScanSettings</id><type>stringmap</type><value>");

            #region Default parameters
            result.AppendFormat(@"<string><id>{0}</id><key>AddSpyRiskWareAnalyze</key><val>On</val></string>",index++);
            result.AppendFormat(@"<string><id>{0}</id><key>ArchiveAnalyze</key><val>Off</val></string>", index++);
            result.AppendFormat(@"<string><id>{0}</id><key>Authenticode</key><val>On</val></string>", index++);
            result.AppendFormat(@"<string><id>{0}</id><key>Cache</key><val>On</val></string>", index++);
            result.AppendFormat(@"<string><id>{0}</id><key>Heuristic</key><val>On</val></string>", index++);
            result.AppendFormat(@"<string><id>{0}</id><key>MailAnalyze</key><val>Off</val></string>", index++);
            result.AppendFormat(@"<string><id>{0}</id><key>MaxArchiveSize</key><val>0</val></string>", index++);
            result.AppendFormat(@"<string><id>{0}</id><key>SfxAnalyze</key><val>On</val> </string>", index++);
            result.AppendFormat(@"<string><id>{0}</id><key>DefaultExtensions</key><val>{1}</val></string>", index++, DefaultFilters);
            #endregion 

            //Not Default

            result.AppendFormat(@"<string><id>{0}</id><key>FirstInfectedAction</key><val>{1}</val></string>", index++, _InfectedAction1.ToString());
            result.AppendFormat(@"<string><id>{0}</id><key>SecondInfectedAction</key><val>{1}</val></string>", index++, _InfectedAction2.ToString());
            result.AppendFormat(@"<string><id>{0}</id><key>ThirdInfectedAction</key><val>{1}</val></string>", index++, _InfectedAction3.ToString());
            result.AppendFormat(@"<string><id>{0}</id><key>InfectedQuarantine</key><val>{1}</val></string>", index++, _IsSaveInfectedToQuarantine == true ? "On" : "Off");

            result.AppendFormat(@"<string><id>{0}</id><key>FirstSuspectedAction</key><val>{1}</val></string>", index++, _SuspiciousAction1.ToString());
            result.AppendFormat(@"<string><id>{0}</id><key>SecondSuspectedAction</key><val>{1}</val></string>", index++, _SuspiciousAction2.ToString());
            result.AppendFormat(@"<string><id>{0}</id><key>SuspiciousQuarantine</key><val>{1}</val></string>", index++, _IsSaveSuspiciousToQuarantine == true ? "On" : "Off");


            if (!String.IsNullOrEmpty(FileExtensions))
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

        public void LoadFromXml(String Xml)
        {
            if (String.IsNullOrEmpty(Xml))
                return;
            TaskConfigureMonitor task;
            using (TextReader reader = new StringReader(Xml))
            {
                task = (TaskConfigureMonitor)serializer.Deserialize(reader);
            }
            this._ExcludingFoldersAndFilesDelete = task.ExcludingFoldersAndFilesDelete;
            this._FileExtensions = task.FileExtensions;
            this._FileExtensionsExcluded = task.FileExtensionsExcluded;
            this._InfectedAction1 = task.InfectedAction1;
            this._InfectedAction2 = task.InfectedAction2;
            this._InfectedAction3 = task.InfectedAction3;
            this._IsSaveInfectedToQuarantine = task.IsSaveInfectedToQuarantine;
            this._SuspiciousAction1 = task.SuspiciousAction1;
            this._SuspiciousAction2 = task.SuspiciousAction2;
            this._IsSaveSuspiciousToQuarantine = task.IsSaveSuspiciousToQuarantine;
            this.MONITOR_ON = task.MONITOR_ON;

            this._Vba32CCUser = task.Vba32CCUser;
            this._journalEvent = task.journalEvent;
        }

        public void Clear()
        {
            this._ExcludingFoldersAndFilesDelete.Clear();
            this._FileExtensions = DefaultFilters;
            this._FileExtensionsExcluded = String.Empty;
            this._InfectedAction1 = MonitorInfectedActions.None;
            this._InfectedAction2 = MonitorInfectedActions.None;
            this._InfectedAction3 = MonitorInfectedActions.None;
            this._IsSaveInfectedToQuarantine = false;
            this._SuspiciousAction1 = MonitorSuspiciousActions.None;
            this._SuspiciousAction2 = MonitorSuspiciousActions.None;
            this._IsSaveSuspiciousToQuarantine = false;
            this.MONITOR_ON = true;

            this._journalEvent.ClearEvents();
        }

        #endregion

        #region IConfigureTask Members
        public void LoadFromRegistry(String reg)
        {
            throw new NotImplementedException();
            /*
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

            foreach (System.Xml.XmlNode node in doc.GetElementsByTagName("Key"))
            {
                if (node.Attributes["name"] != null)
                {
                    switch (node.Attributes["name"].Value)
                    {
                        case "idlecheck":
                            ListOfPathToScan.Clear();
                            foreach (System.Xml.XmlNode child in node.ChildNodes)
                            {
                                ListOfPathToScan.Add(TaskHelper.GetValueForList(child));
                            }
                            break;
                        case "exclusions":
                            ExcludingFoldersAndFilesDelete.Clear();
                            foreach (System.Xml.XmlNode child in node.ChildNodes)
                            {
                                ExcludingFoldersAndFilesDelete.Add(TaskHelper.GetValueForList(child));
                            }
                            break;
                    }
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
			<Value name="ADD_TO_REPORT" type="1" value="1"/>
			<Value name="CHECK_MODE" type="1" value="0"/>
			<Value name="DETECT_RISKWARE" type="2" value="1"/>
			<Value name="FAST_MODE" type="1" value="0"/>
			<Value name="HEURISTIC" type="1" value="1"/>
			<Value name="IDLE_AUTORUN" type="1" value="1"/>
			<Value name="IDLE_CHECK" type="1" value="0"/>
			<Value name="IDLE_DISK" type="1" value="1"/>
			<Value name="IDLE_DISK_VALUE" type="1" value="20"/>
			<Value name="IDLE_MOUSE" type="1" value="1"/>
			<Value name="IDLE_MOUSE_VALUE" type="1" value="50"/>
			<Value name="IDLE_NOTEBOOK" type="1" value="1"/>
			<Value name="IDLE_NOTEBOOK_VALUE" type="1" value="90"/>
			<Value name="IDLE_PROCESSOR" type="1" value="1"/>
			<Value name="IDLE_PROCESSOR_VALUE" type="1" value="20"/>
			<Value name="IDLE_USERFILES" type="1" value="1"/>
			<Value name="InfectedAction1" type="1" value="2"/>
			<Value name="InfectedAction2" type="1" value="3"/>
			<Value name="InfectedAction3" type="1" value="3"/>
			<Value name="InfectedCopy1" type="1" value="1"/>
			<Value name="InfectedCopy2" type="1" value="1"/>
			<Value name="InfectedCopy3" type="1" value="0"/>
			<Value name="LIMIT_REPORT" type="1" value="1"/>
			<Value name="LIMIT_REPORT_VALUE" type="1" value="256"/>
			<Value name="NOTIFY" type="1" value="1"/>
			<Value name="REPORT" type="1" value="1"/>
			<Value name="REPORT_NAME" type="2" value="Vba32mNt.log"/>
			<Value name="SHOW_OK" type="1" value="0"/>
			<Value name="SuspiciousAction1" type="1" value="0"/>
			<Value name="SuspiciousAction2" type="1" value="0"/>
			<Value name="SuspiciousCopy1" type="1" value="1"/>
			<Value name="SuspiciousCopy2" type="1" value="1"/>
			<Key name="idlecheck">
				<Value name="N0B070A0F050B0F0C010D0B0E08080709" type="2" value="C:\WINDOWS\"/>
				<Value name="N000002090C070F080506040F0C030204" type="2" value="C:\WINDOWS\system32\"/>
				<Value name="S01040A000E0D020A0D0C0A040C0F0E0C" type="2" value="C:\Program Files\"/>
			</Key>
              
             */
        }
        
     
        #endregion
    }

    public enum MonitorInfectedActions
    {
        Block = 0,
        Cure = 1,
        Delete = 2,        
        None = 3
    }

    public enum MonitorSuspiciousActions
    {
        Block = 0,
        Delete = 1,
        None = 2
    }
}
