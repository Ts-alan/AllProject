using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using VirusBlokAda.CC.Common.Xml;
using VirusBlokAda.CC.Common;

namespace ARM2_dbcontrol.Tasks
{
    public class TaskConfigureMonitor: IConfigureTask
    {
        #region Fields

        public const String TaskType = "ConfigureMonitor";
        public const String DefaultFilters = @"COM.EXE.DLL.DRV.SYS.OV?.VXD.SCR.CPL.OCX.BPL.AX.PIF.DO?.XL?.HLP.RTF.WI?.WZ?.MSI.MSC.HT*.VB*.JS.JSE.ASP*.CGI.PHP*.?HTML.BAT.CMD.EML.NWS.MSG.XML.MSO.WPS.PPT.PUB.JPG.JPEG.ANI.INF.SWF.PDF";

        private Int32 _CHECK_MODE;
        private String _FILTER_DEFINED;
        private String _FILTER_EXCLUDE;
        private Int32 _FAST_MODE;
        private Int32 _DETECT_RISKWARE;
        private Int32 _IDLE_CHECK;
        private Int32 _IDLE_PROCESSOR;
        private Int32 _IDLE_DISK;
        private Int32 _IDLE_MOUSE;
        private Int32 _IDLE_NOTEBOOK;
        private Int32 _IDLE_USERFILES;
        private Int32 _IDLE_AUTORUN;
        private Int32 _IDLE_PROCESSOR_VALUE = -1;
        private Int32 _IDLE_DISK_VALUE = -1;
        private Int32 _IDLE_MOUSE_VALUE = -1;
        private Int32 _IDLE_NOTEBOOK_VALUE = -1;
        private Int32 _NOTIFY;
        private String _REPORT_NAME;
        private Int32 _LIMIT_REPORT;
        private Int32 _LIMIT_REPORT_VALUE = -1;
        private Int32 _SHOW_OK;
        private Int32 _InfectedAction1;
        private Int32 _InfectedAction2;
        private Int32 _InfectedAction3;
        private Int32 _HEURISTIC;
        private Int32 _BlockAutorunInf;
        private Int32 _SuspiciousAction1;
        private Int32 _SuspiciousAction2;
        private List<String> _ExcludingFoldersAndFilesDelete;
        private List<String> _ListOfPathToScan;
        private Int32 _InfectedCopy1;
        private Int32 _InfectedCopy2;
        private Int32 _InfectedCopy3;
        private Int32 _SuspiciousCopy1;
        private Int32 _SuspiciousCopy2;
        private Boolean _IsScanInBackGround = false;
        private String _Vba32CCUser;

        #endregion

        #region Properties

        public Boolean IsScanInBackGround
        {
            get { return _IsScanInBackGround; }
            set { _IsScanInBackGround = value; }
        }

        public String Vba32CCUser
        {
            get { return _Vba32CCUser; }
            set { _Vba32CCUser = value; }
        }

        public Int32 SuspiciousCopy2
        {
            get { return _SuspiciousCopy2; }
            set { _SuspiciousCopy2 = value; }
        }

        public Int32 SuspiciousCopy1
        {
            get { return _SuspiciousCopy1; }
            set { _SuspiciousCopy1 = value; }
        }

        public Int32 InfectedCopy3
        {
            get { return _InfectedCopy3; }
            set { _InfectedCopy3 = value; }
        }

        public Int32 InfectedCopy2
        {
            get { return _InfectedCopy2; }
            set { _InfectedCopy2 = value; }
        }

        public Int32 InfectedCopy1
        {
            get { return _InfectedCopy1; }
            set { _InfectedCopy1 = value; }
        }
        

        public List<String> ListOfPathToScan
        {
            get { return _ListOfPathToScan; }
            set { _ListOfPathToScan = value; }
        }

        public List<String> ExcludingFoldersAndFilesDelete
        {
            get { return _ExcludingFoldersAndFilesDelete; }
            set { _ExcludingFoldersAndFilesDelete = value; }
        }

        public Int32 SuspiciousAction2
        {
            get { return _SuspiciousAction2; }
            set { _SuspiciousAction2 = value; }
        }

        public Int32 SuspiciousAction1
        {
            get { return _SuspiciousAction1; }
            set { _SuspiciousAction1 = value; }
        }

        public Int32 BlockAutorunInf
        {
            get { return _BlockAutorunInf; }
            set { _BlockAutorunInf = value; }
        }

        public Int32 HEURISTIC
        {
            get { return _HEURISTIC; }
            set { _HEURISTIC = value; }
        }

        public Int32 InfectedAction3
        {
            get { return _InfectedAction3; }
            set { _InfectedAction3 = value; }
        }

        public Int32 InfectedAction2
        {
            get { return _InfectedAction2; }
            set { _InfectedAction2 = value; }
        }

        public Int32 InfectedAction1
        {
            get { return _InfectedAction1; }
            set { _InfectedAction1 = value; }
        }


        public Int32 SHOW_OK
        {
            get { return _SHOW_OK; }
            set { _SHOW_OK = value; }
        }

        public Int32 LIMIT_REPORT_VALUE
        {
            get { return _LIMIT_REPORT_VALUE; }
            set { _LIMIT_REPORT_VALUE = value; }
        }

        public Int32 LIMIT_REPORT
        {
            get { return _LIMIT_REPORT; }
            set { _LIMIT_REPORT = value; }
        }

        public String REPORT_NAME
        {
            get { return _REPORT_NAME; }
            set { _REPORT_NAME = value; }
        }

        public Int32 NOTIFY
        {
            get { return _NOTIFY; }
            set { _NOTIFY = value; }
        }

        public Int32 IDLE_NOTEBOOK_VALUE
        {
            get { return _IDLE_NOTEBOOK_VALUE; }
            set { _IDLE_NOTEBOOK_VALUE = value; }
        }

        public Int32 IDLE_MOUSE_VALUE
        {
            get { return _IDLE_MOUSE_VALUE; }
            set { _IDLE_MOUSE_VALUE = value; }
        }

        public Int32 IDLE_DISK_VALUE
        {
            get { return _IDLE_DISK_VALUE; }
            set { _IDLE_DISK_VALUE = value; }
        }

        public Int32 IDLE_PROCESSOR_VALUE
        {
            get { return _IDLE_PROCESSOR_VALUE; }
            set { _IDLE_PROCESSOR_VALUE = value; }
        }

        public Int32 IDLE_AUTORUN
        {
            get { return _IDLE_AUTORUN; }
            set { _IDLE_AUTORUN = value; }
        }

        public Int32 IDLE_USERFILES
        {
            get { return _IDLE_USERFILES; }
            set { _IDLE_USERFILES = value; }
        }

        public Int32 IDLE_NOTEBOOK
        {
            get { return _IDLE_NOTEBOOK; }
            set { _IDLE_NOTEBOOK = value; }
        }


        public Int32 IDLE_MOUSE
        {
            get { return _IDLE_MOUSE; }
            set { _IDLE_MOUSE = value; }
        }

        public Int32 IDLE_DISK
        {
            get { return _IDLE_DISK; }
            set { _IDLE_DISK = value; }
        }

        public Int32 IDLE_PROCESSOR
        {
            get { return _IDLE_PROCESSOR; }
            set { _IDLE_PROCESSOR = value; }
        }

        public Int32 IDLE_CHECK
        {
            get { return _IDLE_CHECK; }
            set { _IDLE_CHECK = value; }
        } 

        public Int32 DETECT_RISKWARE
        {
            get { return _DETECT_RISKWARE; }
            set { _DETECT_RISKWARE = value; }
        }

        public Int32 FAST_MODE
        {
            get { return _FAST_MODE; }
            set { _FAST_MODE = value; }
        }

        public String FILTER_EXCLUDE
        {
            get { return _FILTER_EXCLUDE; }
            set { _FILTER_EXCLUDE = value; }
        }

        public String FILTER_DEFINED
        {
            get { return _FILTER_DEFINED; }
            set { _FILTER_DEFINED = value; }
        }
        
        public Int32 CHECK_MODE
        {
            get { return _CHECK_MODE; }
            set { _CHECK_MODE = value; }
        }

        #endregion

        #region Constructors

        public TaskConfigureMonitor()
        {
            _ExcludingFoldersAndFilesDelete = new List<String>();
            _ListOfPathToScan = new List<String>();
        }

        #endregion

        #region Methods

        public String GetTaskForVSIS()
        {
            StringBuilder result = new StringBuilder(512);

            result.Append("<VsisCommand>");
            result.Append("<Args>");

            result.Append(@"<command><arg><key>module-id</key><value>{7C62F84A-A362-4CAA-800C-DEA89110596C}</value></arg>");
            result.Append(@"<arg><key>command</key><value>apply_settings</value></arg>");
            result.Append(@"<arg><key>settings</key><value><config><id>Normal</id><module><id>{A3F5FCA0-46DC-4328-8568-5FDF961E87E6}</id>");
            
            #region Settings
            
            result.Append(@"<param><id>MntExcludedFolders</id><type>stringlist</type>");
            if (ExcludingFoldersAndFilesDelete.Count == 0)
                result.Append(@"<value />");
            else
            {
                result.Append(@"<value>");
                for (Int32 index = 0; index < ExcludingFoldersAndFilesDelete.Count; index++)
                {
                    result.AppendFormat(@"<string><id>{0}</id><val>{1}</val></string>", index.ToString(), ExcludingFoldersAndFilesDelete[index]);
                }
                result.Append(@"</value>");
            }
            result.Append(@"</param>");

            Int32 id = 0;
            result.Append(@"<param><id>ScanSettings</id><type>stringmap</type><value>");

            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", id++, "AddSpyRiskWareAnalyze", DETECT_RISKWARE == 1 ? "On" : "Off");
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", id++, "Cache", FAST_MODE == 1 ? "On" : "Off");
            switch (CHECK_MODE)
            {
                case 0:
                    result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val /></string>", id++, "ExcludedExtensions");
                    result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", id++, "ProcessedExtensions", DefaultFilters);
                    break;
                case 1:                    
                    result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val /></string>", id++, "ExcludedExtensions");
                    result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", id++, "ProcessedExtensions", String.IsNullOrEmpty(FILTER_DEFINED) ? DefaultFilters : FILTER_DEFINED);
                    break;
                case 2:                    
                    if (String.IsNullOrEmpty(FILTER_EXCLUDE))
                        result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val /></string>", id++, "ExcludedExtensions");
                    else
                        result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", id++, "ExcludedExtensions", FILTER_EXCLUDE);

                    result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", id++, "ProcessedExtensions", "*");
                    break;
            }
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", id++, "Heuristic", HEURISTIC == 0 ? "Off" : "On");
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", id++, "InfectedQuarantine", (InfectedCopy1 == 1 || InfectedCopy2 == 1 || InfectedCopy3 == 1) ? "On" : "Off");
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", id++, "SuspectedQuarantine", (SuspiciousCopy1 == 1 || SuspiciousCopy2 == 1) ? "On" : "Off");

            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", id++, "FirstInfectedAction", ((MonitorActionsEnum)InfectedAction1).ToString());
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", id++, "SecondInfectedAction", ((MonitorActionsEnum)InfectedAction2).ToString());
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", id++, "ThirdInfectedAction", ((MonitorActionsEnum)InfectedAction3).ToString());
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", id++, "FirstSuspectedAction", ((MonitorActionsEnum)SuspiciousAction1).ToString());
            result.AppendFormat(@"<string><id>{0}</id><key>{1}</key><val>{2}</val></string>", id++, "SecondSuspectedAction", ((MonitorActionsEnum)SuspiciousAction2).ToString());

            result.Append(@"</value></param>");
            
            #endregion

            result.Append(@"</module></config></value></arg></command>");

            result.Append(@"</Args>");
            result.Append(@"<Async>0</Async>");
            result.Append(@"</VsisCommand>");

            return result.ToString();
        }

        #region IConfigureTask Members

        public String SaveToXml()
        {
            XmlBuilder xml = new XmlBuilder("monitor");
            XmlBuilder xmlExclusions = new XmlBuilder("exclusions");
            XmlBuilder xmlIdlecheck = new XmlBuilder("idlecheck");
            xml.Top = String.Empty;
            xmlExclusions.Top = String.Empty;
            xmlIdlecheck.Top = String.Empty;

            //Набор файлов
            xml.AddNode("CHECK_MODE", "reg_dword:" + CHECK_MODE.ToString());
            switch (CHECK_MODE)
            {
                case 1:
                    xml.AddNode("FILTER_DEFINED", "reg_sz:" + FILTER_DEFINED);
                    break;
                case 2:
                    xml.AddNode("FILTER_EXCLUDE", "reg_sz:" + FILTER_EXCLUDE);
                    break;
            }

            xml.AddNode("FAST_MODE", "reg_dword:" + FAST_MODE.ToString());
            xml.AddNode("DETECT_RISKWARE", "reg_dword:" + DETECT_RISKWARE.ToString());

            //фоновая проверка
            xml.AddNode("IDLE_CHECK", "reg_dword:" + IDLE_CHECK.ToString());
            if (IsScanInBackGround)
            {
                //cbox
                xml.AddNode("IDLE_PROCESSOR", "reg_dword:" + IDLE_PROCESSOR.ToString());
                xml.AddNode("IDLE_DISK", "reg_dword:" + IDLE_DISK.ToString());
                xml.AddNode("IDLE_MOUSE", "reg_dword:" + IDLE_MOUSE.ToString());
                xml.AddNode("IDLE_NOTEBOOK", "reg_dword:" + IDLE_NOTEBOOK.ToString());
                //xml.AddNode("IDLE_AUTORUN", "reg_dword:" + IDLE_AUTORUN.ToString());
                xml.AddNode("IDLE_USERFILES", "reg_dword:" + IDLE_USERFILES.ToString());

                //tbox
                if (IDLE_PROCESSOR_VALUE < 0)
                    IDLE_PROCESSOR_VALUE = 20;
                xml.AddNode("IDLE_PROCESSOR_VALUE", "reg_dword:" + IDLE_PROCESSOR_VALUE.ToString());
                if (IDLE_DISK_VALUE < 0)
                    IDLE_DISK_VALUE = 20;
                xml.AddNode("IDLE_DISK_VALUE", "reg_dword:" + IDLE_DISK_VALUE.ToString());
                if (IDLE_MOUSE_VALUE < 0)
                    IDLE_MOUSE_VALUE = 50;
                xml.AddNode("IDLE_MOUSE_VALUE", "reg_dword:" + IDLE_MOUSE_VALUE.ToString());
                if (IDLE_NOTEBOOK_VALUE < 0)
                    IDLE_NOTEBOOK_VALUE = 90;
                xml.AddNode("IDLE_NOTEBOOK_VALUE", "reg_dword:" + IDLE_NOTEBOOK_VALUE.ToString());

            }


            xml.AddNode("NOTIFY", "reg_dword:" + NOTIFY.ToString());

            //лог-файл
            //xml.AddNode("REPORT", cboxKeep.Checked ? "reg_dword:1" : "reg_dword:0");            
            xml.AddNode("REPORT_NAME", "reg_sz:" + (String.IsNullOrEmpty(REPORT_NAME) ? "Vba32mNt.log" : REPORT_NAME));
            // xml.AddNode("ADD_TO_REPORT", cboxAddLog.Checked ? "reg_dword:1" : "reg_dword:0");

            xml.AddNode("LIMIT_REPORT", "reg_dword:" + LIMIT_REPORT.ToString());
            if (LIMIT_REPORT_VALUE < 0)
                LIMIT_REPORT_VALUE = 256;
            xml.AddNode("LIMIT_REPORT_VALUE", "reg_dword:" + LIMIT_REPORT_VALUE.ToString());

            xml.AddNode("SHOW_OK", "reg_dword:" + SHOW_OK.ToString());

            // Действия над инфицированными
            xml.AddNode("InfectedAction1", "reg_dword:" + InfectedAction1.ToString());
            xml.AddNode("InfectedAction2", "reg_dword:" + InfectedAction2.ToString());
            xml.AddNode("InfectedAction3", "reg_dword:" + InfectedAction3.ToString());

            xml.AddNode("HEURISTIC", "reg_dword:" + HEURISTIC.ToString());

            if (HEURISTIC > 0)
            {
                xml.AddNode("BlockAutorunInf", "reg_dword:" + BlockAutorunInf.ToString());
            }

            //Действия над подозрительными
            xml.AddNode("SuspiciousAction1", "reg_dword:" + SuspiciousAction1.ToString());
            xml.AddNode("SuspiciousAction2", "reg_dword:" + SuspiciousAction2.ToString());

            //пути фоновой проверки и список необрабатываемых путей        
            if (ExcludingFoldersAndFilesDelete.Count != 0)
            {
                foreach (String item in ExcludingFoldersAndFilesDelete)
                    xmlExclusions.AddNode((item[0] == '-' ? "N" : "S") + Anchor.GetMd5Hash(item.Substring(1)),
                        "reg_sz:" + item.Substring(1));
            }
            xmlExclusions.Generate();

            if (ListOfPathToScan.Count != 0)
            {
                foreach (String item in ListOfPathToScan)
                    xmlIdlecheck.AddNode((item[0] == '-' ? "N" : "S") + Anchor.GetMd5Hash(item.Substring(1)),
                        "reg_sz:" + item.Substring(1));
            }
            xmlIdlecheck.Generate();

            //cbox to dll
            xml.AddNode("InfectedCopy1", "reg_dword:" + InfectedCopy1.ToString());
            xml.AddNode("InfectedCopy2", "reg_dword:" + InfectedCopy2.ToString());
            xml.AddNode("InfectedCopy3", "reg_dword:" + InfectedCopy3.ToString());
            xml.AddNode("SuspiciousCopy1", "reg_dword:" + SuspiciousCopy1.ToString());
            xml.AddNode("SuspiciousCopy2", "reg_dword:" + SuspiciousCopy2.ToString());

            xml.AddNode("Vba32CCUser", Vba32CCUser);
            xml.AddNode("Type", TaskType);

            xml.Generate();

            return xml.Result + xmlExclusions.Result + xmlIdlecheck.Result;
        }
        
        public void LoadFromXml(String xml)
        {
            XmlTaskParser pars = new XmlTaskParser("<TaskMonitor>" + xml + "</TaskMonitor>");

            Int32.TryParse(pars.GetValue("FAST_MODE", "reg_dword:"), out _FAST_MODE);
            Int32.TryParse(pars.GetValue("DETECT_RISKWARE", "reg_dword:"), out _DETECT_RISKWARE);

            Int32.TryParse(pars.GetValue("IDLE_CHECK", "reg_dword:"), out _IDLE_CHECK);
            Int32.TryParse(pars.GetValue("IDLE_PROCESSOR", "reg_dword:"), out _IDLE_PROCESSOR);
            Int32.TryParse(pars.GetValue("IDLE_DISK", "reg_dword:"), out _IDLE_DISK);
            Int32.TryParse(pars.GetValue("IDLE_MOUSE", "reg_dword:"), out _IDLE_MOUSE);
            Int32.TryParse(pars.GetValue("IDLE_NOTEBOOK", "reg_dword:"), out _IDLE_NOTEBOOK);

            Int32.TryParse(pars.GetValue("IDLE_USERFILES", "reg_dword:"), out _IDLE_USERFILES);
            Int32.TryParse(pars.GetValue("NOTIFY", "reg_dword:"), out _NOTIFY);
            Int32.TryParse(pars.GetValue("LIMIT_REPORT", "reg_dword:"), out _LIMIT_REPORT);
            Int32.TryParse(pars.GetValue("SHOW_OK", "reg_dword:"), out _SHOW_OK);
            Int32.TryParse(pars.GetValue("InfectedCopy1", "reg_dword:"), out _InfectedCopy1);
            Int32.TryParse(pars.GetValue("InfectedCopy2", "reg_dword:"), out _InfectedCopy2);
            Int32.TryParse(pars.GetValue("InfectedCopy3", "reg_dword:"), out _InfectedCopy3);
            Int32.TryParse(pars.GetValue("SuspiciousCopy1", "reg_dword:"), out _SuspiciousCopy1);
            Int32.TryParse(pars.GetValue("SuspiciousCopy2", "reg_dword:"), out _SuspiciousCopy2);
            Int32.TryParse(pars.GetValue("BlockAutorunInf", "reg_dword:"), out _BlockAutorunInf);

            if (pars.GetValue("IDLE_PROCESSOR_VALUE", "reg_dword:") != String.Empty)
                Int32.TryParse(pars.GetValue("IDLE_PROCESSOR_VALUE", "reg_dword:"), out _IDLE_PROCESSOR_VALUE);
            else
                _IDLE_PROCESSOR_VALUE = 20;
            if (pars.GetValue("IDLE_DISK_VALUE", "reg_dword:") != String.Empty)
                Int32.TryParse(pars.GetValue("IDLE_DISK_VALUE", "reg_dword:"), out _IDLE_DISK_VALUE);
            else 
                _IDLE_DISK_VALUE = 20;
            if (pars.GetValue("IDLE_MOUSE_VALUE", "reg_dword:") != String.Empty)
                Int32.TryParse(pars.GetValue("IDLE_MOUSE_VALUE", "reg_dword:"), out _IDLE_MOUSE_VALUE);
            else
                _IDLE_MOUSE_VALUE = 50;
            if (pars.GetValue("IDLE_NOTEBOOK_VALUE", "reg_dword:") != String.Empty)
                Int32.TryParse(pars.GetValue("IDLE_NOTEBOOK_VALUE", "reg_dword:"), out _IDLE_NOTEBOOK_VALUE);
            else
                _IDLE_NOTEBOOK_VALUE = 90;
            if (pars.GetValue("REPORT_NAME", "reg_sz:") != String.Empty)
                _REPORT_NAME = pars.GetValue("REPORT_NAME", "reg_sz:");
            else
                _REPORT_NAME = "Vba32mNt.log";
            if (pars.GetValue("LIMIT_REPORT_VALUE", "reg_dword:") != String.Empty)
                Int32.TryParse(pars.GetValue("LIMIT_REPORT_VALUE", "reg_dword:"), out _LIMIT_REPORT_VALUE);
            else
                _LIMIT_REPORT_VALUE = 256;
            if (pars.GetValue("FILTER_DEFINED", "reg_sz:") != String.Empty)
                _FILTER_DEFINED = pars.GetValue("FILTER_DEFINED", "reg_sz:");
            else _FILTER_DEFINED = DefaultFilters;
            _FILTER_EXCLUDE = pars.GetValue("FILTER_EXCLUDE", "reg_sz:");

            Int32 index = 0;
            if (pars.GetValue("InfectedAction1", "reg_dword:") != String.Empty)
            {
                index = Convert.ToInt32(pars.GetValue("InfectedAction1", "reg_dword:"));
                index--;
            }
            _InfectedAction1 = index;

            if (index < 2)
            {
                if (pars.GetValue("InfectedAction2", "reg_dword:") != String.Empty)
                {
                    index = Convert.ToInt32(pars.GetValue("InfectedAction2", "reg_dword:"));
                    _InfectedAction2 = index == 1 ? 0 : (index - 2);
                }
                else
                    _InfectedAction2 = 0;

                if (_InfectedAction2 == 0)
                {
                    if (pars.GetValue("InfectedAction3", "reg_dword:") != String.Empty)
                    {
                        index = Convert.ToInt32(pars.GetValue("InfectedAction3", "reg_dword:"));
                        _InfectedAction3 = index - 3;
                    }
                    else
                        _InfectedAction3 = 0;
                }
            }

            if (pars.GetValue("SuspiciousAction1", "reg_dword:") != String.Empty)
            {
                index = Convert.ToInt32(pars.GetValue("SuspiciousAction1", "reg_dword:"));
                _SuspiciousAction1 = index == 3 ? 2 : index;
            }
            else
                _SuspiciousAction1 = 0;

            switch (_SuspiciousAction1)
            {
                case 0:
                    _SuspiciousAction2 = 0;
                    break;
                case 1:
                    SuspiciousAction2 = 0;
                    break;
                case 2:
                    _SuspiciousAction2 = 1;
                    break;
            }

            if (pars.GetValue("SuspiciousAction2", "reg_dword:") != String.Empty)
            {
                index = Convert.ToInt32(pars.GetValue("SuspiciousAction2", "reg_dword:"));
                _SuspiciousAction2 = index == 3 ? 1 : 0;
            }
            else
                _SuspiciousAction2 = 0;

            Int32.TryParse(pars.GetValue("HEURISTIC", "reg_dword:"), out _HEURISTIC);

            String str = pars.GetXmlTagContent("exclusions");
            _ExcludingFoldersAndFilesDelete.Clear();
            foreach (String item in TaskHelper.ParseMD5Path(str))
            {
                _ExcludingFoldersAndFilesDelete.Add(item);
            }

            _ListOfPathToScan.Clear();
            str = pars.GetXmlTagContent("idlecheck");
            foreach (String item in TaskHelper.ParseMD5Path(str))
            {
                _ListOfPathToScan.Add(item);
            }

            Int32.TryParse(pars.GetValue("CHECK_MODE", "reg_dword:"), out _CHECK_MODE);
        }

        public void LoadFromRegistry(String reg)
        {
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
        
        public string GetTask()
        {
            throw new NotImplementedException();
        }

        #endregion
        #endregion
    }    
}
