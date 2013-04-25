using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;


namespace VirusBlokAda.RemoteOperations.RemoteScan.RemoteInfo
{
    public class RemoteInfoEntity
    {        
        private static int nextId = 0;
        private int _id = Interlocked.Increment(ref nextId);
        public int Id
        {
            get { return _id; }
        }

        private string _name = String.Empty;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private IPAddress _ipAddress = null;

        public IPAddress IPAddress
        {
            get
            {
                return _ipAddress;
            }
            set 
            { 
                _ipAddress = value;
            }
        }

        private bool _isOnline = false;

        public bool IsOnline
        {
            get { return _isOnline; }
            set { _isOnline = value; }
        }


        private bool _isAgentPortOpen = false;

        public bool IsAgentPortOpen
        {
            get { return _isAgentPortOpen; }
            set { _isAgentPortOpen = value; }
        }

        private bool _isLoaderPortOpen = false;

        public bool IsLoaderPortOpen
        {
            get { return _isLoaderPortOpen; }
            set { _isLoaderPortOpen = value; }
        }

        private string _OSVersion = String.Empty;

        public string OSVersion
        {
            get { return _OSVersion; }
            set { _OSVersion = value; }
        }

        private string _errorInfo = String.Empty;

        public string ErrorInfo
        {
            get { return _errorInfo; }
            set { _errorInfo = value; }
        }

        private bool _isWorkstation = false;

        public bool IsWorkstation
        {
            get { return _isWorkstation; }
            set { _isWorkstation = value; }
        }
    }
}
