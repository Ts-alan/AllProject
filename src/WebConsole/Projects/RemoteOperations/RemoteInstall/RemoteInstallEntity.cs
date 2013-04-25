using System;
using System.Collections.Generic;

using System.Text;
using VirusBlokAda.RemoteOperations.RemoteInstall;

namespace VirusBlokAda.RemoteOperations.RemoteInstall
{
    public class RemoteInstallEntity
    {
        private string computerName;
        private string ip;
        private string guid;
        private string sourceFullPath;
        private Nullable<int> exitCode;
        private string errorInfo;
        private InstallationStatusEnum status;
        private Int64 id;
        private string vbaVersion;

        public Int64 ID
        {
            get { return id; }
            set { id = value; }
        }

        public string VbaVersion
        {
            get { return vbaVersion; }
            set { vbaVersion = value; }
        }


        public string ComputerName 
        {
            get { return computerName; }
            set { computerName = value; }
        }
        public string Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        public string IP
        {
            get { return ip; }
            set { ip = value; }
        }
        public string SourceFullPath 
        {
            get { return sourceFullPath; }
            set { sourceFullPath = value; }
        }
        public Nullable<int> ExitCode 
        {
            get { return exitCode; }
            set { exitCode = value; }
        }
        public string ErrorInfo
        {
            get { return errorInfo; }
            set { errorInfo = value; }
        }
        public InstallationStatusEnum Status 
        {
            get { return status; }
            set { status = value; }
        }
    }
}
