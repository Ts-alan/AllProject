using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Reflection;
using System.Net;

namespace VirusBlokAda.RemoteOperations.RemoteScan.RemoteInfo
{
    /// <summary>
    /// Summary description for RemoteInfoEntityExt
    /// </summary>
    public class RemoteInfoEntityShow:RemoteInfoEntityWrap
    {
        public RemoteInfoEntityShow(RemoteInfoEntity _remoteInfoEntity)
            : base(_remoteInfoEntity)
        { }
        private bool _isSelected = false;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; }
        }

        private bool _isDisabled = false;

        public bool IsDisabled
        {
            get { return _isDisabled; }
            set { _isDisabled = value; }
        }

        private string _info;
        public string Info
        {
            get { return _info; }
            set { _info = value; }
        }
    }

}