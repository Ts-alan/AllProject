using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.RemoteOperations.Common;
using System.Threading;
using VirusBlokAda.RemoteOperations.Threading;
using VirusBlokAda.RemoteOperations.Net;
using System.Net;
using VirusBlokAda.RemoteOperations.RemoteScan.RemoteInfo;
using VirusBlokAda.RemoteOperations.RemoteScan;

namespace VirusBlokAda.RemoteOperations.RemoteInstall
{
    public class RemoteInstaller
    {
        #region Constructor
        public RemoteInstaller(Credentials credentials, Int32 maxThreads)
        {
            _credentials = credentials;
            if (maxThreads <= 0)
            {
                throw new ArgumentOutOfRangeException("Number of threads must be positive number");
            }
            _maxThreads = maxThreads;
            cappedPrioritizedThreadPool = new CappedPrioritizedThreadPool(_maxThreads, 0);
        }

        public RemoteInstaller(Credentials credentials, Int32 maxThreads, String connectionString)
        {
            _credentials = credentials;
            if (maxThreads <= 0)
            {
                throw new ArgumentOutOfRangeException("Number of threads must be positive number");
            }
            _maxThreads = maxThreads;
            cappedPrioritizedThreadPool = new CappedPrioritizedThreadPool(_maxThreads, 0);

            RemoteInstallHelper.ConnectionString = connectionString;
        }
        #endregion
        #region Properties   
        #region Essential
        private Credentials _credentials;
        public Credentials Credentials
        {
            get { return _credentials; }
            set { _credentials = value;}
        }

        private Int32 _maxThreads;
        public Int32 MaxThreads
        {
            get
            {
                return _maxThreads;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("Number of threads must be positive number");
                }
                _maxThreads = value;
                if (cappedPrioritizedThreadPool != null)
                {
                    cappedPrioritizedThreadPool.MaxThreads = _maxThreads;
                }
            }
        }
        #endregion
        private RemoteMethodsEnum _methodType = RemoteMethodsEnum.RemoteService;
        public RemoteMethodsEnum MethodType
        {
            get
            {
                return _methodType;
            }
            set
            {
                _methodType = value;
            }
        }
        #endregion

        private CappedPrioritizedThreadPool cappedPrioritizedThreadPool;
        public void InstallAll(List<RemoteInstallEntity> computers, Boolean doRestart)
        {
            foreach (RemoteInstallEntity rie in computers)
            {
                cappedPrioritizedThreadPool.QueueUserWorkItem(delegate (Object state)
                {
                    RemoteInstallEntity r = (RemoteInstallEntity)state;
                    RemoteInstallHelper.Install(r, _credentials, doRestart,_methodType);
                }, rie);
            }
        }

        public void UninstallAll(List<RemoteInstallEntity> computers, Boolean doRestart)
        {
            foreach (RemoteInstallEntity rie in computers)
            {
                cappedPrioritizedThreadPool.QueueUserWorkItem(delegate(Object state)
                {
                    RemoteInstallEntity r = (RemoteInstallEntity)state;
                    RemoteInstallHelper.Uninstall(r, _credentials, doRestart, _methodType);
                }, rie);
            }
        }
    }

}
