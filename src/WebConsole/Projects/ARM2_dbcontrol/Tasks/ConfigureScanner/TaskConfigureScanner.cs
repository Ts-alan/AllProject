using System;
using System.Collections.Generic;
using System.Text;

namespace ARM2_dbcontrol.Tasks.ConfigureScanner
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
               
        #endregion

        #region Properties

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
        { }

        #endregion

        #region Methods

        public String SaveToXml()
        {
            ARM2_dbcontrol.Generation.XmlBuilder xml = new ARM2_dbcontrol.Generation.XmlBuilder("task");

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

            ///HA=[0|1|2|3]
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
            Int32.TryParse(pars.GetValue("HeuristicAnalysis"), out _HeuristicAnalysis);
            Int32.TryParse(pars.GetValue("Mode"), out _Mode);
            Int32.TryParse(pars.GetValue("MultyThreading"), out _MultyThreading);
        }

        #endregion
    }
}
