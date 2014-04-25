using System;
using System.Collections.Generic;
using System.Text;
using Vba32CC.TaskAssignment.Tasks.AuxiliaryClasses;
using VirusBlokAda.CC.Common.Xml;

namespace Vba32CC.TaskAssignment.Tasks
{
    public class TaskRunScanner
    {
        #region Properties
        protected Boolean _isCheckArchives = true;
        public Boolean IsCheckArchives
        {
            get { return _isCheckArchives; }
            set { _isCheckArchives = value; }
        }

        protected Boolean _isCheckMacros = true;
        public Boolean IsCheckMacros
        {
            get { return _isCheckMacros; }
            set { _isCheckMacros = value; }
        }

        protected Boolean _isCheckMail = true;
        public Boolean IsCheckMail
        {
            get { return _isCheckMail; }
            set { _isCheckMail = value; }
        }

        protected Boolean _isCheckMemory = true;
        public Boolean IsCheckMemory
        {
            get { return _isCheckMemory; }
            set { _isCheckMemory = value; }
        }

        protected Boolean _isCleanFiles = true;
        public Boolean IsCleanFiles
        {
            get { return _isCleanFiles; }
            set { _isCleanFiles = value; }
        }

        protected Boolean _isCheckCure = true;
        public Boolean IsCheckCure
        {
            get { return _isCheckCure; }
            set { _isCheckCure = value; }
        }

        protected Boolean _isCheckCureBoot = true;
        public Boolean IsCheckCureBoot
        {
            get { return _isCheckCureBoot; }
            set { _isCheckCureBoot = value; }
        }

        protected Boolean _isDeleteArchives = false;
        public Boolean IsDeleteArchives
        {
            get { return _isDeleteArchives; }
            set { _isDeleteArchives = value; }
        }

        protected Boolean _isDeleteMail = false;
        public Boolean IsDeleteMail
        {
            get { return _isDeleteMail; }
            set { _isDeleteMail = value; }
        }

        protected Boolean _isDetectAdware = true;
        public Boolean IsDetectAdware
        {
            get { return _isDetectAdware; }
            set { _isDetectAdware = value; }
        }

        protected Boolean _isEnableCach = true;
        public Boolean IsEnableCach
        {
            get { return _isEnableCach; }
            set { _isEnableCach = value; }
        }

        protected Boolean _isExclude = true;
        public Boolean IsExclude
        {
            get { return _isExclude; }
            set { _isExclude = value; }
        }

        protected Boolean _isSaveInfectedToQuarantine = true;
        public Boolean IsSaveInfectedToQuarantine
        {
            get { return _isSaveInfectedToQuarantine; }
            set { _isSaveInfectedToQuarantine = value; }
        }

        protected Boolean _isSaveInfectedToReport = true;
        public Boolean IsSaveInfectedToReport
        {
            get { return _isSaveInfectedToReport; }
            set { _isSaveInfectedToReport = value; }
        }

        protected Boolean _isSaveSusToQuarantine = true;
        public Boolean IsSaveSusToQuarantine
        {
            get { return _isSaveSusToQuarantine; }
            set { _isSaveSusToQuarantine = value; }
        }

        protected Boolean _isScanBootSectors = true;
        public Boolean IsScanBootSectors
        {
            get { return _isScanBootSectors; }
            set { _isScanBootSectors = value; }
        }

        protected Boolean _isSFX = true;
        public Boolean IsSFX
        {
            get { return _isSFX; }
            set { _isSFX = value; }
        }

        protected Boolean _isScanStartup = true;
        public Boolean IsScanStartup
        {
            get { return _isScanStartup; }
            set { _isScanStartup = value; }
        }

        protected Boolean _isSet = true;
        public Boolean IsSet
        {
            get { return _isSet; }
            set { _isSet = value; }
        }

        protected Boolean _isKeep = true;
        public Boolean IsKeep
        {
            get { return _isKeep; }
            set { _isKeep = value; }
        }

        protected Boolean _isAdd = true;
        public Boolean IsAdd
        {
            get { return _isAdd; }
            set { _isAdd = value; }
        }

        protected Boolean _isAddArch = true;
        public Boolean IsAddArch
        {
            get { return _isAddArch; }
            set { _isAddArch = value; }
        }

        protected Boolean _isAddInf = true;
        public Boolean IsAddInf
        {
            get { return _isAddInf; }
            set { _isAddInf = value; }
        }

        protected Boolean _isUpdateBase = true;
        public Boolean IsUpdateBase
        {
            get { return _isUpdateBase; }
            set { _isUpdateBase = value; }
        }

        protected String _checkObjects = String.Empty;
        public String CheckObjects
        {
            get { return _checkObjects; }
            set { _checkObjects = value; }
        }

        protected String _keep = String.Empty;
        public String Keep
        {
            get { return _keep; }
            set { _keep = value; }
        }

        protected String _pathToScanner = String.Empty;
        public String PathToScanner
        {
            get { return _pathToScanner; }
            set { _pathToScanner = value; }
        }

        protected String _saveInfectedToReport = String.Empty;
        public String SaveInfectedToReport
        {
            get { return _saveInfectedToReport; }
            set { _saveInfectedToReport = value; }
        }

        protected String _archiveSize = String.Empty;
        public String ArchiveSize
        {
            get { return _archiveSize; }
            set { _archiveSize = value; }
        }

        protected String _addArch = String.Empty;
        public String AddArch
        {
            get { return _addArch; }
            set { _addArch = value; }
        }

        protected String _exclude = String.Empty;
        public String Exclude
        {
            get { return _exclude; }
            set { _exclude = value; }
        }

        protected String _set = String.Empty;
        public String Set
        {
            get { return _set; }
            set { _set = value; }
        }

        protected ScanningModeEnum _mode = ScanningModeEnum.Fast;
        public ScanningModeEnum Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        protected HeuristicAnalysisEnum _heuristicAnalysis = HeuristicAnalysisEnum.Optimal;
        public HeuristicAnalysisEnum HeuristicAnalysis
        {
            get { return _heuristicAnalysis; }
            set { _heuristicAnalysis = value; }
        }

        protected Int32 _multyThreading = 0;
        public Int32 MultyThreading
        {
            get { return _multyThreading; }
            set
            {
                if(value >= 0 && value < 16)
                _multyThreading = value; 
            }
        }

        protected Boolean _isShowScanProgress = true;
        public Boolean IsShowScanProgress
        {
            get { return _isShowScanProgress; }
            set { _isShowScanProgress = value; }
        }

        protected String _remove = String.Empty;
        public String Remove
        {
            get { return _remove; }
            set { _remove = value; }
        }

        protected Int32 _ifCureChecked = 0;
        public Int32 IfCureChecked
        {
            get { return _ifCureChecked; }
            set { _ifCureChecked = value; }
        }

        public readonly String TaskType = "RunScanner";

        #endregion

        #region Contructors

        public TaskRunScanner() { }
        public TaskRunScanner(String xml)
        {
            LoadState(xml);
        }

        #endregion

        #region Methods

        public String BuildXml()
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
            
            xml.AddNode("CheckObjects", CheckObjects);

            if (IsKeep)
                xml.AddNode("Keep", Keep);

            xml.AddNode("PathToScanner", PathToScanner);
            
            if (IsSaveInfectedToReport)
                xml.AddNode("SaveInfectedToReport", SaveInfectedToReport);

            if (!String.IsNullOrEmpty(ArchiveSize))
                xml.AddNode("ArchiveSize", ArchiveSize);

            if (IsAddArch)
                xml.AddNode("AddArch", AddArch);
            
            if (IsExclude)
                xml.AddNode("Exclude", Exclude);
            
            if (IsSet)
                xml.AddNode("Set", Set);

            xml.AddNode("Mode", Convert.ToString((Int32)Mode));
            xml.AddNode("HeuristicAnalysis", Convert.ToString((Int32)HeuristicAnalysis));

            xml.AddNode("MultyThreading", MultyThreading == 0 ? "Auto" : MultyThreading.ToString());
            
            xml.AddNode("IsShowScanProgress", IsShowScanProgress ? "1" : "0");

            xml.AddNode("Remove", Remove);

            xml.AddNode("IfCureChecked", IfCureChecked.ToString());
            xml.AddNode("Vba32CCUser", AutomaticallyTasks.UserName);
            xml.AddNode("Type", TaskType);

            xml.Generate();

            return xml.Result;
        }

        public void LoadState(String xml)
        {
            if (String.IsNullOrEmpty(xml)) return;
            XmlTaskParser pars = new XmlTaskParser(xml);

            try
            {
                if (pars.GetValue("Type") != TaskType)
                    throw new ArgumentException("Error invalid task type.");
            }
            catch
            {
                return;
            }

            IsCheckArchives = pars.GetValue("IsCheckArchives") == "1"; ;
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

            IfCureChecked = Convert.ToInt32(pars.GetValue("IfCureChecked"));

            HeuristicAnalysis = (HeuristicAnalysisEnum)(Convert.ToInt32(pars.GetValue("HeuristicAnalysis")));
            Mode = (ScanningModeEnum)(Convert.ToInt32(pars.GetValue("Mode")));

            String value = pars.GetValue("MultyThreading");
            if (value == "Auto" || String.IsNullOrEmpty(value)) value = "0";
            MultyThreading = Convert.ToInt32(value);

            IsShowScanProgress = pars.GetValue("IsShowScanProgress") == "1";
        }

        public String GenerateCommandLine()
        {
            String fileName = IsUpdateBase ? "VbaControlAgent.exe\" LCSU" : "vba32w.exe\"";
            String commandLine = '"' + PathToScanner + fileName + " " + '"' + CheckObjects + '"';

            commandLine += IsCheckArchives ? " /AR+" : " /AR-";
            commandLine += IsCheckMacros ? " /VM+" : " /VM-";
            commandLine += IsCheckMail ? " /ML+" : " /ML-";
            commandLine += IsCheckMemory ? " /MR+" : " /MR-";
            commandLine += IsCleanFiles ? " /OK+" : " /OK-";
            commandLine += IsCheckCure ? " /FC+" : " /FC-";
            commandLine += IsCheckCureBoot ? " /BC+" : " /BC-";
            commandLine += IsDeleteArchives ? " /AD+" : " /AD-";
            commandLine += IsDeleteMail ? " /MD+" : " /MD-";
            commandLine += IsDetectAdware ? " /RW+" : " /RW-";
            commandLine += IsEnableCach ? " /CH+" : " /CH-";
            if (IsExclude)
                commandLine += " /EXT-" + Exclude;

            if (IsSaveInfectedToQuarantine)
                commandLine += " /QI+[" + Remove + "]";
            else
                commandLine += " /QI-";

            if (IsSaveSusToQuarantine)
                commandLine += " /QS+[" + Remove + "]";
            else
                commandLine += " /QS-";

            commandLine += IsScanBootSectors ? " /BT+" : " /BT-";
            commandLine += IsSFX ? " /SFX+" : " /SFX-";
            commandLine += IsScanStartup ? " /AS+" : " /AS-";

            if (IsSet)
                commandLine += " /EXT=" + Set;

            if (IsKeep)
            {
                if (IsAdd)
                    commandLine += " /R+" + '"' + Keep + '"';
                else
                    commandLine += " /R=" + '"' + Keep + '"';
            }

            if (IsAddArch)
                commandLine += " /EXT+" + AddArch;

            if (IsSaveInfectedToReport)
            {
                if (IsAddInf)
                    commandLine += " /L+" + SaveInfectedToReport;
                else
                    commandLine += " /L=" + SaveInfectedToReport;
            }

            switch (IfCureChecked)
            {
                case 2:
                    commandLine += " /FD+";
                    break;
                case 3:
                    commandLine += " /FR+";
                    break;
                case 4:
                    commandLine += " /FM+" + '"' + Remove + '"';
                    break;
            }

            commandLine += " /HA=" + ((Int32)HeuristicAnalysis).ToString();

            Int32 index = (Int32)Mode;
            index++;
            commandLine += " /M=" + index.ToString();

            if (ArchiveSize != String.Empty)
                commandLine += " /AL=" + ArchiveSize;

            if (MultyThreading == 0)
                commandLine += " /J=" + "Auto";
            else
                commandLine += " /J=" + MultyThreading.ToString();

            commandLine += IsShowScanProgress ? " /SP+" : " /SP-";

            return commandLine;
        }

        #endregion
    }
}
