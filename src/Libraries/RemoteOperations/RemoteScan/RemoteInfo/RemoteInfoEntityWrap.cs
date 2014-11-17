using System;
using System.Configuration;
using System.Web;
using System.Reflection;
using System.Net;

namespace VirusBlokAda.CC.RemoteOperations.RemoteScan.RemoteInfo
{
    /// <summary>
    /// Summary description for RemoteInfoEntityExt
    /// </summary>
    public class RemoteInfoEntityWrap
    {
        public RemoteInfoEntityWrap(RemoteInfoEntity _remoteInfoEntity)
        {
            remoteInfoEntity = _remoteInfoEntity;
        }
        RemoteInfoEntity remoteInfoEntity;

        public RemoteInfoEntity RemoteInfoEntity
        {
            get { return remoteInfoEntity; }
        }

        public int Id
        {
            get { return remoteInfoEntity.Id; }
        }
        public string Name
        {
            get { return remoteInfoEntity.Name; }
            set { remoteInfoEntity.Name = value; }
        }
        public IPAddress IPAddress
        {
            get { return remoteInfoEntity.IPAddress; }
            set { remoteInfoEntity.IPAddress = value; }
        }
        public bool IsOnline
        {
            get { return remoteInfoEntity.IsOnline; }
            set { remoteInfoEntity.IsOnline = value; }
        }

        public bool IsAgentPortOpen
        {
            get { return remoteInfoEntity.IsAgentPortOpen; }
            set { remoteInfoEntity.IsAgentPortOpen = value; }
        }

        public bool IsLoaderPortOpen
        {
            get { return remoteInfoEntity.IsLoaderPortOpen; }
            set { remoteInfoEntity.IsLoaderPortOpen = value; }
        }

        public string OSVersion
        {
            get { return remoteInfoEntity.OSVersion; }
            set { remoteInfoEntity.OSVersion = value; }
        }

        public bool IsWorkstation
        {
            get { return remoteInfoEntity.IsWorkstation; }
            set { remoteInfoEntity.IsWorkstation = value; }
        }

        public string ErrorInfo
        {
            get { return remoteInfoEntity.ErrorInfo; }
            set { remoteInfoEntity.ErrorInfo = value; }
        }
    }

}