using System;
using System.Collections.Generic;

using System.Text;
using VirusBlokAda.CC.RemoteOperations.RemoteInstall;

namespace VirusBlokAda.CC.RemoteOperations.RemoteInstall
{
    public class RemoteInstallEntity
    {
        private String computerName;
        private String ip;
        private String guid;
        private String sourceFullPath;
        private Nullable<Int32> exitCode;
        private String errorInfo;
        private InstallationStatusEnum status;
        private Int64 id;
        private String vbaVersion;
        private Boolean isInstalled;

        public Int64 ID
        {
            get { return id; }
            set { id = value; }
        }

        public String VbaVersion
        {
            get { return vbaVersion; }
            set { vbaVersion = value; }
        }


        public String ComputerName 
        {
            get { return computerName; }
            set { computerName = value; }
        }
        public String Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        public String IP
        {
            get { return ip; }
            set { ip = value; }
        }
        public String SourceFullPath 
        {
            get { return sourceFullPath; }
            set { sourceFullPath = value; }
        }
        public Nullable<Int32> ExitCode 
        {
            get { return exitCode; }
            set { exitCode = value; }
        }
        public String ErrorInfo
        {
            get { return errorInfo; }
            set { errorInfo = value; }
        }
        public InstallationStatusEnum Status 
        {
            get { return status; }
            set { status = value; }
        }

        public Boolean IsInstalled
        {
            get { return isInstalled; }
            set { isInstalled = value; }
        }
    }
}
