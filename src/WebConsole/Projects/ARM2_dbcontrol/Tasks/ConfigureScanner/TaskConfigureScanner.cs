using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.CC.Common.Xml;

namespace ARM2_dbcontrol.Tasks
{
    public class TaskConfigureScanner
    {
        #region Fields

        public const String TaskType = "ConfigureScanner";
        private String _Vba32CCUser;

        private Boolean _IsCheckArchives;
        private Boolean _IsCheckMacros;
        private Boolean _IsCheckMail;
        private Boolean _IsCheckMemory;
        private Boolean _IsCleanFiles;
        private Boolean _IsCheckCure;
        private Boolean _IsCheckCureBoot;
        private Boolean _IsDeleteArchives;
        private Boolean _IsDeleteMail;
        private Boolean _IsDetectAdware;
        private Boolean _IsEnableCach;
        private Boolean _IsExclude;
        private Boolean _IsSaveInfectedToQuarantine;
        private Boolean _IsSaveInfectedToReport;
        private Boolean _IsSaveSusToQuarantine;
        private Boolean _IsScanBootSectors;
        private Boolean _IsSFX;
        private Boolean _IsScanStartup;
        private Boolean _IsSet;
        private Boolean _IsKeep;
        private Boolean _IsAdd;
        private Boolean _IsAddArch;
        private Boolean _IsAddInf;
        private Boolean _IsUpdateBase;
        private Boolean _IsUncheckLargeArchives;
        private String _CheckObjects = "*:";
        private String _Keep;
        private String _PathToScanner = "%VBA32%";
        private String _SaveInfectedToReport;
        private String _ArchiveSize;
        private String _AddArch;
        private String _Exclude;
        private String _Set;
        private Int32 _Mode;
        private Int32 _HeuristicAnalysis;
        private Int32 _MultyThreading;
        private Boolean _IsShowScanProgress;
        private Boolean _IsMultyThreading;
        private Int32 _IfCureChecked;
        private String _Remove;

        private Int32 _SuspiciousMode;
        private Boolean _IsAuthenticode;
        private RemoteScanSettings _remoteClient;
        private RemoteScanSettings _remoteServer;
               
        #endregion

        #region Properties

        public Int32 SuspiciousMode
        {
            get { return _SuspiciousMode; }
            set { _SuspiciousMode = value; }
        }

        public Boolean IsAuthenticode
        {
            get { return _IsAuthenticode; }
            set { _IsAuthenticode = value; }
        }

        public Int32 RemoteClientPort
        {
            get { return _remoteClient.Port; }
        }

        public Int32 RemoteServerPort
        {
            get { return _remoteServer.Port; }
        }

        public String RemoteClientAddress
        {
            get { return _remoteClient.Address; }
            set { _remoteClient.Address = value; }
        }

        public Boolean IsRemoteClientEnabled
        {
            get { return _remoteClient.Enable; }
            set { _remoteClient.Enable = value; }
        }

        public String RemoteServerAddress
        {
            get { return _remoteServer.Address; }
            set { _remoteServer.Address = value; }
        }

        public Boolean IsRemoteServerEnabled
        {
            get { return _remoteServer.Enable; }
            set { _remoteServer.Enable = value; }
        }

        public Boolean IsMultyThreading
        {
            get { return _IsMultyThreading; }
            set { _IsMultyThreading = value; }
        }

        public Boolean IsUncheckLargeArchives
        {
            get { return _IsUncheckLargeArchives; }
            set { _IsUncheckLargeArchives = value; }
        }

        public Int32 IfCureChecked
        {
            get { return _IfCureChecked; }
            set { _IfCureChecked = value; }
        }

        public String Remove
        {
            get { return _Remove; }
            set { _Remove = value; }
        }

        public Int32 HeuristicAnalysis
        {
            get { return _HeuristicAnalysis; }
            set { _HeuristicAnalysis = value; }
        }

        public Int32 MultyThreading
        {
            get { return _MultyThreading; }
            set { _MultyThreading = value; }
        }

        public Boolean IsShowScanProgress
        {
            get { return _IsShowScanProgress; }
            set { _IsShowScanProgress = value; }
        }

        public Int32 Mode
        {
            get { return _Mode; }
            set { _Mode = value; }
        }

        public String SaveInfectedToReport
        {
            get { return _SaveInfectedToReport; }
            set { _SaveInfectedToReport = value; }
        }

        public String ArchiveSize
        {
            get { return _ArchiveSize; }
            set { _ArchiveSize = value; }
        }

        public String AddArch
        {
            get { return _AddArch; }
            set { _AddArch = value; }
        }

        public String Exclude
        {
            get { return _Exclude; }
            set { _Exclude = value; }
        }

        public String Set
        {
            get { return _Set; }
            set { _Set = value; }
        }

        public String CheckObjects
        {
            get { return _CheckObjects; }
            set { _CheckObjects = value; }
        }

        public String Keep
        {
            get { return _Keep; }
            set { _Keep = value; }
        }

        public String PathToScanner
        {
            get { return _PathToScanner; }
            set { _PathToScanner = value; }
        }

        public Boolean IsUpdateBase
        {
            get { return _IsUpdateBase; }
            set { _IsUpdateBase = value; }
        }

        public Boolean IsSet
        {
            get { return _IsSet; }
            set { _IsSet = value; }
        }

        public Boolean IsKeep
        {
            get { return _IsKeep; }
            set { _IsKeep = value; }
        }

        public Boolean IsAdd
        {
            get { return _IsAdd; }
            set { _IsAdd = value; }
        }

        public Boolean IsAddArch
        {
            get { return _IsAddArch; }
            set { _IsAddArch = value; }
        }

        public Boolean IsAddInf
        {
            get { return _IsAddInf; }
            set { _IsAddInf = value; }
        }

        public Boolean IsEnableCach
        {
            get { return _IsEnableCach; }
            set { _IsEnableCach = value; }
        }

        public Boolean IsExclude
        {
            get { return _IsExclude; }
            set { _IsExclude = value; }
        }

        public Boolean IsSaveInfectedToQuarantine
        {
            get { return _IsSaveInfectedToQuarantine; }
            set { _IsSaveInfectedToQuarantine = value; }
        }

        public Boolean IsSaveInfectedToReport
        {
            get { return _IsSaveInfectedToReport; }
            set { _IsSaveInfectedToReport = value; }
        }

        public Boolean IsSaveSusToQuarantine
        {
            get { return _IsSaveSusToQuarantine; }
            set { _IsSaveSusToQuarantine = value; }
        }

        public Boolean IsScanBootSectors
        {
            get { return _IsScanBootSectors; }
            set { _IsScanBootSectors = value; }
        }

        public Boolean IsSFX
        {
            get { return _IsSFX; }
            set { _IsSFX = value; }
        }

        public Boolean IsScanStartup
        {
            get { return _IsScanStartup; }
            set { _IsScanStartup = value; }
        }

        public Boolean IsCheckArchives
        {
            get { return _IsCheckArchives; }
            set { _IsCheckArchives = value; }
        }

        public Boolean IsCheckMacros
        {
            get { return _IsCheckMacros; }
            set { _IsCheckMacros = value; }
        }

        public Boolean IsCheckMail
        {
            get { return _IsCheckMail; }
            set { _IsCheckMail = value; }
        }

        public Boolean IsCheckMemory
        {
            get { return _IsCheckMemory; }
            set { _IsCheckMemory = value; }
        }

        public Boolean IsCleanFiles
        {
            get { return _IsCleanFiles; }
            set { _IsCleanFiles = value; }
        }

        public Boolean IsCheckCure
        {
            get { return _IsCheckCure; }
            set { _IsCheckCure = value; }
        }

        public Boolean IsCheckCureBoot
        {
            get { return _IsCheckCureBoot; }
            set { _IsCheckCureBoot = value; }
        }

        public Boolean IsDeleteArchives
        {
            get { return _IsDeleteArchives; }
            set { _IsDeleteArchives = value; }
        }

        public Boolean IsDeleteMail
        {
            get { return _IsDeleteMail; }
            set { _IsDeleteMail = value; }
        }

        public Boolean IsDetectAdware
        {
            get { return _IsDetectAdware; }
            set { _IsDetectAdware = value; }
        }

        public String Vba32CCUser
        {
            get { return _Vba32CCUser; }
            set { _Vba32CCUser = value; }
        }

        #endregion

        #region Constructors

        public TaskConfigureScanner()
        {
            _remoteClient = new RemoteScanSettings(17024);            
            _remoteServer = new RemoteScanSettings(17024);
        }

        #endregion

        #region Methods

        public String SaveToXml()
        {
            XmlBuilder xml = new XmlBuilder("task");

            xml.AddNode("IsCheckArchives", IsCheckArchives ? "1" : "0");
            xml.AddNode("IsCheckMacros", IsCheckMacros ? "1" : "0");
            xml.AddNode("IsCheckMail", IsCheckMail ? "1" : "0");
            xml.AddNode("IsCheckMemory", IsCheckMemory ? "1" : "0");
            xml.AddNode("IsCleanFiles", IsCleanFiles ? "1" : "0");
            xml.AddNode("IsCheckCure", IsCheckCure ? "1" : "0");
            xml.AddNode("IsCheckCureBoot", IsCheckCureBoot ? "1" : "0");
            xml.AddNode("IsDeleteArchives", IsDeleteArchives ? "1" : "0");
            xml.AddNode("IsDeleteMail", IsDeleteMail ? "1" : "0");
            xml.AddNode("IsDetectAdware", IsDetectAdware ? "1" : "0");
            xml.AddNode("IsEnableCach", IsEnableCach ? "1" : "0");
            xml.AddNode("IsExclude", IsExclude ? "1" : "0");
            xml.AddNode("IsSaveInfectedToQuarantine", IsSaveInfectedToQuarantine ? "1" : "0");
            xml.AddNode("IsSaveInfectedToReport", IsSaveInfectedToReport ? "1" : "0");
            xml.AddNode("IsSaveSusToQuarantine", IsSaveSusToQuarantine ? "1" : "0");
            xml.AddNode("IsScanBootSectors", IsScanBootSectors ? "1" : "0");
            xml.AddNode("IsSFX", IsSFX ? "1" : "0");
            xml.AddNode("IsScanStartup", IsScanStartup ? "1" : "0");
            xml.AddNode("IsSet", IsSet ? "1" : "0");
            xml.AddNode("IsKeep", IsKeep ? "1" : "0");
            xml.AddNode("IsAdd", IsAdd ? "1" : "0");
            xml.AddNode("IsAddArch", IsAddArch ? "1" : "0");
            xml.AddNode("IsAddInf", IsAddInf ? "1" : "0");
            xml.AddNode("IsUpdateBase", IsUpdateBase ? "1" : "0");
            xml.AddNode("IsMultyThreading", IsMultyThreading ? "1" : "0");
            xml.AddNode("IsUncheckLargeArchives", IsUncheckLargeArchives ? "1" : "0");
            xml.AddNode("IsShowScanProgress", IsShowScanProgress ? "1" : "0");
            xml.AddNode("IsAuthenticode", IsAuthenticode ? "1" : "0");

            xml.AddNode("CheckObjects", CheckObjects);

            if (IsKeep)
                xml.AddNode("Keep", Keep);
            
            xml.AddNode("PathToScanner", PathToScanner);
            
            if (IsSaveInfectedToReport)
                xml.AddNode("SaveInfectedToReport", SaveInfectedToReport);

            if (IsUncheckLargeArchives)
                xml.AddNode("ArchiveSize", ArchiveSize);

            if (IsAddArch)
                xml.AddNode("AddArch", AddArch);
            if (IsExclude)
                xml.AddNode("Exclude", Exclude);
            if (IsSet)
                xml.AddNode("Set", Set);

            xml.AddNode("Mode", Mode.ToString());
            xml.AddNode("HeuristicAnalysis", HeuristicAnalysis.ToString());
            
            if (IsMultyThreading)
            {
                xml.AddNode("MultyThreading", MultyThreading.ToString());
            }

            xml.AddNode("Remove", Remove);

            xml.AddNode("IfCureChecked", IfCureChecked.ToString());
            xml.AddNode("SuspiciousMode", SuspiciousMode.ToString());

            xml.AddNode("IsRemoteServerEnabled", IsRemoteServerEnabled ? "1" : "0");            
            xml.AddNode("IsRemoteClientEnabled", IsRemoteClientEnabled ? "1" : "0");
            xml.AddNode("RemoteClientAddress", RemoteClientAddress);

            xml.AddNode("Vba32CCUser", Vba32CCUser);
            xml.AddNode("Type", TaskType);

            xml.Generate();

            return xml.Result;
        }

        public String GetTaskForLoader()
        {
            String fileName = IsUpdateBase ? "VbaControlAgent.exe\" LCSU" : "vba32w.exe\"";

            StringBuilder commandLine = new StringBuilder(256);
            commandLine.Append('"' + PathToScanner + fileName + " " + '"' + CheckObjects + '"');

            commandLine.Append(" /AR" + (IsCheckArchives ? "+" : "-"));
            commandLine.Append(" /VM" + (IsCheckMacros ? "+" : "-"));
            commandLine.Append(" /ML" + (IsCheckMail ? "+" : "-"));
            commandLine.Append(" /MR" + (IsCheckMemory ? "+" : "-"));
            commandLine.Append(" /OK" + (IsCleanFiles ? "+" : "-"));
            commandLine.Append(" /FC" + (IsCheckCure ? "+" : "-"));
            commandLine.Append(" /BC" + (IsCheckCureBoot ? "+" : "-"));
            commandLine.Append(" /AD" + (IsDeleteArchives ? "+" : "-"));
            commandLine.Append(" /MD" + (IsDeleteMail ? "+" : "-"));
            commandLine.Append(" /RW" + (IsDetectAdware ? "+" : "-"));
            commandLine.Append(" /CH" + (IsEnableCach ? "+" : "-"));
            commandLine.Append(" /NA" + (IsAuthenticode ? "+" : "-"));
            if (IsExclude)
            {
                commandLine.Append(" /EXT-" + Exclude);
            }
            commandLine.Append(" /QI" + (IsSaveInfectedToQuarantine ? ("+[" + Remove + "]") : "-"));
            commandLine.Append(" /QS" + (IsSaveSusToQuarantine ? ("+[" + Remove + "]") : "-"));
            commandLine.Append(" /BT" + (IsScanBootSectors ? "+" : "-"));
            commandLine.Append(" /SFX" + (IsSFX ? "+" : "-"));
            commandLine.Append(" /AS" + (IsScanStartup ? "+" : "-"));
            if (IsSet)
            {
                commandLine.Append(" /EXT=" + Set);
            }

            if (IsKeep)
            {
                commandLine.Append(" /R" + (IsAdd ? "+" : "=") + '"' + Keep + '"');
            }

            if (IsAddArch)
                commandLine.Append(" /EXT+" + AddArch);

            if (IsSaveInfectedToReport)
            {
                commandLine.Append(" /L" + (IsAddInf ? "+" : "=") + SaveInfectedToReport);
            }

            switch (IfCureChecked)
            {
                case 2:
                    commandLine.Append(" /FD+");
                    break;
                case 3:
                    commandLine.Append(" /FR+");
                    break;
                case 4:
                    commandLine.Append(" /FM+" + '"' + Remove + '"');
                    break;
            }

            switch (SuspiciousMode)
            {
                case 2:
                    commandLine.Append(" /SD+");
                    break;
                case 3:
                    commandLine.Append(" /SR+");
                    break;
                case 4:
                    commandLine.Append(" /SM+" + '"' + Remove + '"');
                    break;
            }

            commandLine.Append(" /HA=" + HeuristicAnalysis.ToString());
            commandLine.Append(" /M=" + (Mode + 1).ToString());
            commandLine.Append(" /AL=" + ArchiveSize.ToString());
            commandLine.Append(" /J=" + (MultyThreading == 1 ? "Auto" : MultyThreading.ToString()));
            commandLine.Append(" /SP" + (IsShowScanProgress ? "+" : "-"));

            return commandLine.ToString();
        }

        public String GetTaskForVSIS()
        {
            StringBuilder result = new StringBuilder(512);

            result.Append("<VsisCommand>");
            result.Append("<Args>");

            result.Append(@"<command><arg><key>module-id</key><value>{7C62F84A-A362-4CAA-800C-DEA89110596C}</value></arg>");
            result.Append(@"<arg><key>command</key><value>apply_settings</value></arg>");
            result.Append(@"<arg><key>settings</key><value><config><id>Normal</id><module><id>{323C6C00-4FF9-4ADB-8F9A-1E394265E6FF}</id>");

            #region Settings
            Int32 index = 0;
            result.Append(@"<param><id>RemoteScanClientSettings</id><type>stringmap</type><value>");
            if (!String.IsNullOrEmpty(RemoteClientAddress))
                result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "Address", RemoteClientAddress);
            else 
                result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val/></string>", index++, "Address");
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "Enable", IsRemoteClientEnabled ? "On" : "Off");
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "Port", RemoteClientPort);
            result.Append(@"</value></param>");

            index = 0;
            result.Append(@"<param><id>RemoteScanServerSettings</id><type>stringmap</type><value>");
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "Enable", IsRemoteServerEnabled ? "On" : "Off");
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "Port", RemoteServerPort);
            result.Append(@"</value></param>");
            
            index = 0;
            result.Append(@"<param><id>ScanSettings</id><type>stringmap</type><value>");
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "AddSpyRiskWareAnalyze", IsDetectAdware ? "On" : "Off");
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "ArchiveAnalyze", IsCheckArchives ? "On" : "Off");
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "Authenticode", IsAuthenticode ? "On" : "Off");
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "Cache", IsEnableCach ? "On" : "Off");
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "Heuristic", HeuristicAnalysis == 0 ? "Off" : "On");
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "MailAnalyze", IsCheckMail ? "On" : "Off");
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "MaxArchiveSize", String.IsNullOrEmpty(ArchiveSize) ? "0" : ArchiveSize);
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "SfxAnalyze", IsSFX ? "On" : "Off");

            if (IsCheckCure)
            {
                result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "FirstInfectedAction", "Cure");
                result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "SecondInfectedAction", IfCureChecked == 2 ? "Delete" : "None");
            }
            else
            {
                result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "FirstInfectedAction", IfCureChecked == 2 ? "Delete" : "None");
                result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "SecondInfectedAction", "None");
            }
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "ThirdInfectedAction", "None");

            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "FirstSuspectedAction", SuspiciousMode == 2 ? "Delete" : "None");
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "SecondSuspectedAction", "None");
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "ThirdSuspectedAction", "None");

            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "InfectedQuarantine", IsSaveInfectedToQuarantine ? "On" : "Off");
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "SuspectedQuarantine", IsSaveSusToQuarantine ? "On" : "Off");

            if(IsSet && !String.IsNullOrEmpty(Set))
                result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "ProcessedExtensions", Set);
            
            if (IsExclude && !String.IsNullOrEmpty(Exclude))
                result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", index++, "ExcludedExtensions", Exclude);
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

        public void LoadFromXml(String xml)
        {
            XmlTaskParser pars = new XmlTaskParser(xml);

            IsCheckArchives = pars.GetValue("IsCheckArchives") == "1";
            IsCheckMacros = pars.GetValue("IsCheckMacros") == "1";
            IsCheckMail = pars.GetValue("IsCheckMail") == "1";
            IsCheckMemory = pars.GetValue("IsCheckMemory") == "1";
            IsCleanFiles = pars.GetValue("IsCleanFiles") == "1";
            IsCheckCure = pars.GetValue("IsCheckCure") == "1";
            IsCheckCureBoot = pars.GetValue("IsCheckCureBoot") == "1";
            IsDeleteArchives = pars.GetValue("IsDeleteArchives") == "1";
            IsDeleteMail = pars.GetValue("IsDeleteMail") == "1";
            IsDetectAdware = pars.GetValue("IsDetectAdware") == "1";
            IsEnableCach = pars.GetValue("IsEnableCach") == "1";
            IsExclude = pars.GetValue("IsExclude") == "1";
            IsSaveInfectedToQuarantine = pars.GetValue("IsSaveInfectedToQuarantine") == "1";
            IsSaveInfectedToReport = pars.GetValue("IsSaveInfectedToReport") == "1";
            IsSaveSusToQuarantine = pars.GetValue("IsSaveSusToQuarantine") == "1";
            IsScanBootSectors = pars.GetValue("IsScanBootSectors") == "1";
            IsSFX = pars.GetValue("IsSFX") == "1";
            IsScanStartup = pars.GetValue("IsScanStartup") == "1";
            IsSet = pars.GetValue("IsSet") == "1";
            IsKeep = pars.GetValue("IsKeep") == "1";
            IsAdd = pars.GetValue("IsAdd") == "1";
            IsAddArch = pars.GetValue("IsAddArch") == "1";
            IsAddInf = pars.GetValue("IsAddInf") == "1";
            IsUpdateBase = pars.GetValue("IsUpdateBase") == "1";
            IsUncheckLargeArchives = pars.GetValue("IsUncheckLargeArchives") == "1";
            IsShowScanProgress = pars.GetValue("IsShowScanProgress") == "1";
            IsMultyThreading = pars.GetValue("IsMultyThreading") == "1";
            IsAuthenticode = pars.GetValue("IsAuthenticode") == "1";

            CheckObjects = pars.GetValue("CheckObjects");
            if (CheckObjects == String.Empty)
                CheckObjects = "*:";

            Keep = pars.GetValue("Keep");

            PathToScanner = pars.GetValue("PathToScanner");
            if (PathToScanner == String.Empty)
                PathToScanner = "%VBA32%";

            Remove = pars.GetValue("Remove");
            SaveInfectedToReport = pars.GetValue("SaveInfectedToReport");
            ArchiveSize = pars.GetValue("ArchiveSize");
            AddArch = pars.GetValue("AddArch");
            Exclude = pars.GetValue("Exclude");
            Set = pars.GetValue("Set");

            Int32.TryParse(pars.GetValue("IfCureChecked"), out _IfCureChecked);
            Int32.TryParse(pars.GetValue("SuspiciousMode"), out _SuspiciousMode);
            Int32.TryParse(pars.GetValue("HeuristicAnalysis"), out _HeuristicAnalysis);
            Int32.TryParse(pars.GetValue("Mode"), out _Mode);
            Int32.TryParse(pars.GetValue("MultyThreading"), out _MultyThreading);

            IsRemoteServerEnabled = pars.GetValue("IsRemoteServerEnabled") == "1";
            IsRemoteClientEnabled = pars.GetValue("IsRemoteClientEnabled") == "1";
            RemoteClientAddress = pars.GetValue("RemoteClientAddress");
        }

        #endregion

        struct RemoteScanSettings
        {
            public String Address;
            public Boolean Enable;
            public Int32 Port;

            public RemoteScanSettings(Int32 port)
            {
                Port = port;
                Address = String.Empty;
                Enable = false;
            }
        }
    }
}
