using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.CC.Tasks.Attributes;
using VirusBlokAda.CC.Tasks.Service;

namespace VirusBlokAda.CC.Tasks.Entities
{
    [Serializable]
    [TaskEntity("loader")]
    public class ConfigureLoaderTaskEntity : TaskEntity
    {
        private bool _launchLoaderAtStart;
        [TaskEntityBooleanProperty("AUTO_START", format = "reg_dword:{0}")]
        public bool LaunchLoaderAtStart
        {
            get { return _launchLoaderAtStart; }
            set { _launchLoaderAtStart = value; }
        }

        private bool _enableMonitorAtStart;
        [TaskEntityBooleanProperty("MONITOR_AUTO_START", format = "reg_dword:{0}")]
        public bool EnableMonitorAtStart
        {
            get { return _enableMonitorAtStart; }
            set { _enableMonitorAtStart = value; }
        }

        private bool _protectProcess;
        [TaskEntityBooleanProperty("PROTECT_LOADER", format = "reg_dword:{0}")]
        public bool ProtectProcess
        {
            get { return _protectProcess; }
            set { _protectProcess = value; }
        }

        private bool _displayLoadingProgress;
        [TaskEntityBooleanProperty("SHOW_WINDOW", format = "reg_dword:{0}")]
        public bool DisplayLoadingProgress
        {
            get { return _displayLoadingProgress; }
            set { _displayLoadingProgress = value; }
        }

        private bool _scanMemory;
        [TaskEntityBooleanProperty("AUTO_CHECK_MEMORY", format = "reg_dword:{0}")]
        public bool ScanMemory
        {
            get { return _scanMemory; }
            set { _scanMemory = value; }
        }

        private int _scanMemoryMode;
        [TaskEntityInt32Property("CHECK_MEMORY_MODE", format = "reg_dword:{0}", dependOnTrueProperty = "ScanMemory")]
        public int ScanMemoryMode
        {
            get { return _scanMemoryMode; }
            set { _scanMemoryMode = value; }
        }

        private bool _scanBoot;
        [TaskEntityBooleanProperty("AUTO_CHECK_BOOT", format = "reg_dword:{0}")]
        public bool ScanBoot
        {
            get { return _scanBoot; }
            set { _scanBoot = value; }
        }

        private bool _scanBootFloppy;
        [TaskEntityBooleanProperty("AUTO_CHECK_BOOT_FLOPPY", format = "reg_dword:{0}")]
        public bool ScanBootFloppy
        {
            get { return _scanBootFloppy; }
            set { _scanBootFloppy = value; }
        }

        private bool _maximumSizeLog;
        [TaskEntityBooleanProperty("LOG_LIMIT", format = "reg_dword:{0}")]
        public bool MaximumSizeLog
        {
            get { return _maximumSizeLog; }
            set { _maximumSizeLog = value; }
        }

        private bool _soundWarning;
        [TaskEntityBooleanProperty("SOUND", format = "reg_dword:{0}")]
        public bool SoundWarning
        {
            get { return _soundWarning; }
            set { _soundWarning = value; }
        }

        private bool _trayIcon;
        [TaskEntityBooleanProperty("ANIMATION", format = "reg_dword:{0}")]
        public bool TrayIcon
        {
            get { return _trayIcon; }
            set { _trayIcon = value; }
        }

        private bool _timeIntervals;
        [TaskEntityBooleanProperty("UPDATE_TIME", format = "reg_dword:{0}")]
        public bool TimeIntervals
        {
            get { return _timeIntervals; }
            set { _timeIntervals = value; }
        }

        private bool _interactive;
        [TaskEntityBooleanProperty("UPDATE_INTERACTIVE", format = "reg_dword:{0}")]
        public bool Interactive
        {
            get { return _interactive; }
            set { _interactive = value; }
        }

        private bool _useProxyServer;
        [TaskEntityBooleanProperty("PROXY_USAGE", format = "reg_dword:{0}", replaceTrue = "2")]
        public bool UseProxyServer
        {
            get { return _useProxyServer; }
            set { _useProxyServer = value; }
        }

        private bool _useAccount;
        [TaskEntityBooleanProperty("PROXY_AUTHORIZE", format = "reg_dword:{0}", replaceTrue = "2")]
        public bool UseAccount
        {
            get { return _useAccount; }
            set { _useAccount = value; }
        }

        private string _logFile;
        [TaskEntityStringProperty("LOG_NAME", format = "reg_sz:{0}")]
        public string LogFile
        {
            get { return _logFile; }
            set { _logFile = value; }
        }

        private int _maximumSizeLogValue;
        [TaskEntityInt32Property("LOG_LIMIT_VALUE", format = "reg_dword:{0}", dependOnTrueProperty = "MaximumSizeLog")]
        public int MaximumSizeLogValue
        {
            get { return _maximumSizeLogValue; }
            set { _maximumSizeLogValue = value; }
        }

        private string _timeIntervalsValue;
        [TaskEntityStringProperty("UPDATE_TIME_VALUE", format = "reg_sz:{0}", allowNullOrEmpty = false,
            dependOnTrueProperty = "TimeIntervals")]
        public string TimeIntervalsValue
        {
            get { return _timeIntervalsValue; }
            set { _timeIntervalsValue = value; }
        }

        private string _updateFolder;
        [TaskEntityStringProperty("UPDATE_FOLDER", format = "reg_sz:{0}", allowNullOrEmpty = false)]
        public string UpdateFolder
        {
            get { return _updateFolder; }
            set { _updateFolder = value; }
        }

        private string _updateFolderList;
        [TaskEntityStringProperty("UPDATE_FOLDER_LIST", format = "reg_multi_sz:{0}", allowNullOrEmpty = false)]
        public string UpdateFolderList
        {
            get { return _updateFolderList; }
            set { _updateFolderList = value; }
        }

        private string _proxyAddress;
        [TaskEntityStringProperty("PROXY_ADDRESS", format = "reg_sz:{0}",
            dependOnTrueProperty = "UseProxyServer")]
        public string ProxyAddress
        {
            get { return _proxyAddress; }
            set { _proxyAddress = value; }
        }


        private int _proxyPort;
        [TaskEntityInt32Property("PROXY_PORT", format = "reg_dword:{0}",
            dependOnTrueProperty = "UseProxyServer")]
        public int ProxyPort
        {
            get { return _proxyPort; }
            set { _proxyPort = value; }
        }

        private string _proxyUser;
        [TaskEntityStringProperty("PROXY_USER", format = "reg_sz:{0}",
            dependOnTrueProperty = "UseAccount")]
        public string ProxyUser
        {
            get { return _proxyUser; }
            set { _proxyUser = value; }
        }

        private string _proxyPassword;
        [TaskEntityStringProperty("PROXY_PASSWORD", format = "reg_binary:{0}",
            dependOnTrueProperty = "UseAccount")]
        public string ProxyPassword
        {
            get { return _proxyPassword; }
            set { _proxyPassword = value; }
        }
        private bool _scanUSB;
        [TaskEntityBooleanProperty("SCAN_USB", format = "reg_dword:{0}")]
        public bool ScanUsb
        {
            get { return _scanUSB; }
            set { _scanUSB = value; }
        }



        public ConfigureLoaderTaskEntity() : base("ConfigureLoader") { }

        [Obsolete("Необходимо переопределить")]
        public override string ToTaskXml()
        {



            String PassPrefix = "HER!%&$";
            StringBuilder result = new StringBuilder(1024);

            result.Append("<TaskConfigureSettings>");
            result.Append("<loader>");
            result.AppendFormat("<AUTO_START>reg_dword:{0}</AUTO_START>", (LaunchLoaderAtStart == true) ? "1" : "0");
            result.AppendFormat("<MONITOR_AUTO_START>reg_dword:{0}</MONITOR_AUTO_START>", (EnableMonitorAtStart == true) ? "1" : "0");
            result.AppendFormat("<PROTECT_LOADER>reg_dword:{0}</PROTECT_LOADER>", (ProtectProcess == true) ? "1" : "0");
            result.AppendFormat("<SHOW_WINDOW>reg_dword:{0}</SHOW_WINDOW>", (DisplayLoadingProgress == true) ? "1" : "0");
            result.AppendFormat("<AUTO_CHECK_MEMORY>reg_dword:{0}</AUTO_CHECK_MEMORY>", (ScanMemory == true) ? "1" : "0");
            if (ScanMemoryMode > -1)
            {
                result.AppendFormat("<CHECK_MEMORY_MODE>reg_dword:{0}</CHECK_MEMORY_MODE>", ScanMemoryMode.ToString());
            }
            result.AppendFormat("<AUTO_CHECK_BOOT>reg_dword:{0}</AUTO_CHECK_BOOT>", (ScanBoot == true) ? "1" : "0");
            result.AppendFormat("<AUTO_CHECK_BOOT_FLOPPY>reg_dword:{0}</AUTO_CHECK_BOOT_FLOPPY>", (ScanBootFloppy == true) ? "1" : "0");
            result.AppendFormat("<LOG_LIMIT>reg_dword:{0}</LOG_LIMIT>", (MaximumSizeLog == true) ? "1" : "0");
            result.AppendFormat("<SOUND>reg_dword:{0}</SOUND>", (SoundWarning == true) ? "1" : "0");
            result.AppendFormat("<ANIMATION>reg_dword:{0}</ANIMATION>", (TrayIcon == true) ? "1" : "0");
            result.AppendFormat("<UPDATE_TIME>reg_dword:{0}</UPDATE_TIME>", (TimeIntervals == true) ? "1" : "0");
            result.AppendFormat("<UPDATE_INTERACTIVE>reg_dword:{0}</UPDATE_INTERACTIVE>", (Interactive == true) ? "1" : "0");
            result.AppendFormat("<PROXY_USAGE>reg_dword:{0}</PROXY_USAGE>", (UseProxyServer == true) ? "1" : "0");
            result.AppendFormat("<PROXY_AUTHORIZE>reg_dword:{0}</PROXY_AUTHORIZE>", (UseAccount == true) ? "1" : "0");


            result.AppendFormat("<LOG_NAME>reg_sz:{0}</LOG_NAME>", String.IsNullOrEmpty(LogFile) ? "Vba32Ldr.log" : LogFile);
            if (MaximumSizeLog == true && MaximumSizeLogValue > -1)
            {
                result.AppendFormat("<LOG_LIMIT_VALUE>reg_dword:{0}</LOG_LIMIT_VALUE>", MaximumSizeLogValue.ToString());
            }
            if (TimeIntervalsValue != String.Empty)
            {
                result.AppendFormat("<UPDATE_TIME_VALUE>reg_sz:{0}</UPDATE_TIME_VALUE>", TimeIntervalsValue);
            }
            if (UpdateFolder != String.Empty)
            {
                result.AppendFormat("<UPDATE_FOLDER>reg_sz:{0}</UPDATE_FOLDER>", UpdateFolder);
            }
            if (UpdateFolderList != String.Empty)
            {
                result.AppendFormat("<UPDATE_FOLDER_LIST>reg_multi_sz:{0}</UPDATE_FOLDER_LIST>", UpdateFolderList);
            }
            if (!String.IsNullOrEmpty(ProxyAddress))
            {
                result.AppendFormat("<PROXY_ADDRESS>reg_sz:{0}</PROXY_ADDRESS>", ProxyAddress);
                result.AppendFormat("<PROXY_PORT>reg_sz:{0}</PROXY_PORT>", ProxyPort.ToString());
            }
            if (!String.IsNullOrEmpty(ProxyUser))
            {
                result.AppendFormat("<PROXY_USER>reg_sz:{0}</PROXY_USER>", ProxyUser);

                if (!ProxyPassword.Contains(PassPrefix))
                {
                    Byte[] bytes = Encoding.Unicode.GetBytes(ProxyPassword);

                    Byte xorValue = 0xAA;
                    Byte delta = 0x1;

                    for (Int32 i = 0; i < bytes.Length; i++)
                    {
                        bytes[i] ^= xorValue;
                        delta = Convert.ToByte(delta % 3 + 1);
                        xorValue += delta;
                    }
                    result.AppendFormat("<PROXY_PASSWORD>reg_sz:{0}</PROXY_PASSWORD>", VirusBlokAda.CC.Common.Anchor.ConvertToDumpString(bytes));
                }
                else
                {
                    result.AppendFormat("<PROXY_PASSWORD>reg_sz:{0}</PROXY_PASSWORD>", ProxyPassword.Replace(PassPrefix, ""));
                }
            }
            result.AppendFormat("<SCAN_USB>reg_dword:{0}</SCAN_USB>", (ScanUsb == true) ? "1" : "0");




            result.Append("</loader>");
            result.AppendFormat("</TaskConfigureSettings>");

            return result.ToString();

        }
    }
}
