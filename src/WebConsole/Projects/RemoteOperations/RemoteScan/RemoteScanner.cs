using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.RemoteOperations.Common;
using System.Net;
using VirusBlokAda.RemoteOperations.Net;
using System.Threading;
using VirusBlokAda.RemoteOperations.Threading;
using VirusBlokAda.RemoteOperations.RemoteScan.RemoteInfo;

namespace VirusBlokAda.RemoteOperations.RemoteScan
{
    public class RemoteScanner
    {
        private CappedPrioritizedThreadPool cappedPrioritizedThreadPool;

        #region Constructor
        public RemoteScanner(Credentials credentials, int maxThreads)
        {
            _credentials = credentials;
            if (maxThreads <= 0)
            {
                throw new ArgumentOutOfRangeException("Number of threads must be positive number");
            }
            _maxThreads = maxThreads;
        }
        #endregion

        #region Properties
        #region Essential
        private Credentials _credentials;
        public Credentials Credentials
        {
            get { return _credentials; }
            set { _credentials = value; }
        }

        private int _maxThreads;
        public int MaxThreads
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
        #region Include in Results
        private bool _includeOfflineComputers = false;
        public bool IncludeOfflineComputers
        {
            get { return _includeOfflineComputers; }
            set { _includeOfflineComputers = value; }
        }

        private bool _includeNoLoaderComputers = true;
        public bool IncludeNoLoaderComputers
        {
            get { return _includeNoLoaderComputers; }
            set { _includeNoLoaderComputers = value; }
        }

        private bool _includeNoAgentComputers = true;
        public bool IncludeNoAgentComputers
        {
            get { return _includeNoAgentComputers; }
            set { _includeNoAgentComputers = value; }
        }

        private bool _includeNoOsComputers = true;
        public bool IncludeNoOsComputers
        {
            get { return _includeNoOsComputers; }
            set { _includeNoOsComputers = value; }
        }
        #endregion
        #region Scanning Progress
        private int _progressOnPaused;
        private int _progress;
        public int Progress
        {
            get { return _progress; }
        }
        private int _progressPercentage;
        public int ProgressPercentage
        {
            get { return _progressPercentage; }
        }
        private bool _isCompleted = true;
        public bool IsCompleted
        {
            get { return _isCompleted; }
        }
        private bool isRunning = false;
        public bool IsRunning
        {
            get { return isRunning; }
        }
        #endregion
        #region Execution
        private bool _testAgentPort = true;
        public bool TestAgentPort
        {
            get { return this._testAgentPort; }
            set { this._testAgentPort = value; }
        }

        private bool _testLoaderPort = true;
        public bool TestLoaderPort
        {
            get { return this._testLoaderPort; }
            set { this._testLoaderPort = value; }
        }

        private bool _testOs = true;
        public bool TestOs
        {
            get { return _testOs; }
            set { _testOs = value; }
        }
        private RemoteMethodsEnum _methodType = RemoteMethodsEnum.RemoteService;
        public RemoteMethodsEnum MethodType
        {
            get { return _methodType; }
            set { _methodType = value; }
        }
        private TimeSpan _pingTimeout = new TimeSpan(0, 0, 10);
        public TimeSpan PingTimeout
        {
            get { return _pingTimeout; }
            set { _pingTimeout = value; }
        }

        private Int32 _pingCount = 1;
        public Int32 PingCount
        {
            get { return _pingCount; }
            set { _pingCount = value; }
        }
        #endregion
        #region IP Addresses
        private bool _excludeIPAddress = true;
        public bool ExcludeIPAddress
        {
            get { return _excludeIPAddress; }
            set { _excludeIPAddress = value; }
        }

        private List<IPAddress> _excludeIPAddressList = new List<IPAddress>();
        public List<IPAddress> ExcludeIPAddressList
        {
            get { return _excludeIPAddressList; }
            set
            {
                _excludeIPAddress = true;
                _excludeIPAddressList = value;
            }
        }

        private List<IPRange> _scanIPRangeList = new List<IPRange>();
        public List<IPRange> ScanIPRangeList
        {
            get { return _scanIPRangeList; }
            set { _scanIPRangeList = value; }
        }

        private List<IPAddress> _scanIPAddressSet = null;
        protected List<IPAddress> ScanIPAddressSet
        {
            get
            {
                if (_scanIPAddressSet == null)
                {
                    _scanIPAddressSet = IPAddressHelper.GetIPAddressList(_scanIPRangeList);
                    if (_excludeIPAddress)
                    {
                        foreach (IPAddress nextIp in _excludeIPAddressList)
                        {
                            _scanIPAddressSet.Remove(nextIp);
                        }
                    }
                }
                return _scanIPAddressSet;
            }
        }

        private int _count;
        public int Count
        {
            get { return _count; }
        }
        #endregion
        #region Time
        private long elapsedTime;
        public long ElapsedTime
        {
            get 
            {
                long time = elapsedTime;
                if (lastRun != DateTime.MinValue)
                {
                    time += (DateTime.Now.Ticks - lastRun.Ticks) / 10000000;
                }
                return time;
            }
        }
        private DateTime lastRun;
        #endregion
        #region Result
        private List<RemoteInfoEntity> _scanCompletedList;
        public List<RemoteInfoEntity> ScanCompletedList
        {
            get { return _scanCompletedList; }
        }
        private ReaderWriterLock _scanCompletedListLock;
        public void AcquireReaderLockOnCompletedList()
        {
            _scanCompletedListLock.AcquireReaderLock(Timeout.Infinite);
        }
        public void ReleaseReaderLockOnCompletedList()
        {
            _scanCompletedListLock.ReleaseReaderLock();
        }

        public ProgressStateEnum State
        {
            get
            {
                if (IsCompleted)
                {
                    return ProgressStateEnum.Stopped;
                }
                else if (isRunning)
                {
                    return ProgressStateEnum.Running;
                }
                else 
                {
                    return ProgressStateEnum.Paused;
                }
            }
        }
        #endregion
        #endregion

        #region Timer
        private void ClearTimer()
        {
            elapsedTime = 0;
        }

        private void StartTimer()
        {
            lastRun = DateTime.Now;
        }

        private void StopTimer()
        {
            if (lastRun != DateTime.MinValue)
            {
                elapsedTime += (DateTime.Now.Ticks - lastRun.Ticks) / 10000000;
            }
            lastRun = DateTime.MinValue;
        }
        #endregion

        #region Result Buffer
        private Dictionary<int, RemoteInfoEntityScan> remoteInfoScanQueuedDict;

        private object _buf_lock = new object();
        private AutoResetEvent bufferNotEmpty;
        
        private Thread copyingThread;
        private void CopyFromBuffer()
        {
            try
            {
                while (isRunning)
                {
                    bufferNotEmpty.WaitOne(Timeout.Infinite);
                    _scanCompletedListLock.AcquireWriterLock(Timeout.Infinite);
                    lock (_buf_lock)
                    {
                        foreach (RemoteInfoEntity next in remoteInfoScanBuffer)
                        {
                            _scanCompletedList.Add(next);
                        }
                        remoteInfoScanBuffer.Clear();
                    }
                    _scanCompletedListLock.ReleaseWriterLock();
                }
            }
            catch (ThreadInterruptedException)
            {
                if (_scanCompletedListLock.IsWriterLockHeld)
                {
                    _scanCompletedListLock.ReleaseWriterLock();
                }
                //Stop or Pause method where called
            }
        }

        private List<RemoteInfoEntity> remoteInfoScanBuffer;
        #endregion

        #region Progress Methods
        public void StartScan()
        {
            if (!IsCompleted)
            {
                throw new InvalidOperationException("Can't call start on running scan");
            }
            
            _count = ScanIPAddressSet.Count;
            if (_count == 0)
            {
                throw new ArgumentException("After exclusion ip address list is empty");
            }
            _scanCompletedListLock = new ReaderWriterLock();
            bufferNotEmpty = new AutoResetEvent(false);
            isRunning = true;
            _isCompleted = false;
            copyingThread = new Thread(CopyFromBuffer);
            copyingThread.IsBackground = true;
            copyingThread.Start();
            cappedPrioritizedThreadPool = new CappedPrioritizedThreadPool(_maxThreads, 2);
            _progress = 0;
            _progressPercentage = 0;
            remoteInfoScanBuffer = new List<RemoteInfoEntity>();
            _scanCompletedList = new List<RemoteInfoEntity>();

            remoteInfoScanQueuedDict = new Dictionary<int, RemoteInfoEntityScan>();
            foreach (IPAddress nextAddress in ScanIPAddressSet)
            {
                RemoteInfoEntityScan ries = new RemoteInfoEntityScan(new RemoteInfoEntity());
                ries.IPAddress = nextAddress;
                remoteInfoScanQueuedDict.Add(ries.Id, ries);
                cappedPrioritizedThreadPool.QueueUserWorkItem(CheckPing, ries.Id, 2);
            }
            ClearTimer();
            StartTimer();
        }

        public void StopScan(bool skipResults)
        {
            if (IsCompleted)
            {
                throw new InvalidOperationException("Can't call stop on completed scan");
            }
            _isCompleted = true;
            isRunning = false;
            if (copyingThread != null)
            {
                copyingThread.Interrupt();
            }
            copyingThread = null;
            bufferNotEmpty.Reset();
            lock (_buf_lock)
            {
                remoteInfoScanBuffer.Clear();
                remoteInfoScanBuffer = null;
            }
            cappedPrioritizedThreadPool.Stop();
            remoteInfoScanQueuedDict.Clear();
            remoteInfoScanQueuedDict = null;
            if (skipResults)
            {
                _scanCompletedListLock.AcquireWriterLock(Timeout.Infinite);
                _scanCompletedList = null;
                _scanCompletedListLock.ReleaseWriterLock();
                _progress = 0;
                _progressPercentage = 0;
                _count = 0;
            }
            StopTimer();
        }

        public void PauseScan()
        {
            if (_isCompleted)
            {
                throw new InvalidOperationException("Can't call pause on completed scan");
            }
            if (!isRunning)
            {
                throw new InvalidOperationException("Can't call pause on not running scan");
            }
            _progressOnPaused = 0;
            isRunning = false;
            copyingThread.Interrupt();
            copyingThread = null;
            cappedPrioritizedThreadPool.Pause();
            StopTimer();
        }

        public void ContinueScan()
        {
            if (_isCompleted)
            {
                throw new InvalidOperationException("Can't call continue on completed scan");
            }
            isRunning = true;
            if (Interlocked.Add(ref _progress, _progressOnPaused) == _count)
            {
                _isCompleted = true;
                isRunning = false;
                StopTimer();
            }
            _progressPercentage = (int)Math.Round((_progress + 0.0) / _count * 100);
            if (!_isCompleted && (_progressPercentage == 100))
            {
                _progressPercentage = 99;
            }
            copyingThread = new Thread(CopyFromBuffer);
            copyingThread.IsBackground = true;
            copyingThread.Start();
            cappedPrioritizedThreadPool.Continue();
            StartTimer();
        }
        #endregion        

        #region Scanning Methods
        private void CheckPing(Object state)
        {
            try
            {
                int id = (int)state;
                RemoteInfoEntityScan rie;
                if (!remoteInfoScanQueuedDict.TryGetValue(id, out rie))
                {
                    IncreaseScanningProgress();
                    return;
                }
                IPAddress address = rie.IPAddress;
                if (RemoteScanHelper.IsComputerOnline(address, _pingTimeout, _pingCount))
                {
                    rie.IsOnline = true;
                    if (_testOs)
                    {
                        cappedPrioritizedThreadPool.QueueUserWorkItem(GetHostnameAndOs, rie.Id, 1);
                    }
                    else
                    {
                        rie.IncreaseCompletionState();
                    }
                    if (_testAgentPort)
                    {
                        cappedPrioritizedThreadPool.QueueUserWorkItem(CheckAgentPort, rie.Id, 1);
                    }
                    else
                    {
                        rie.IncreaseCompletionState();
                    }
                    if (_testLoaderPort)
                    {
                        cappedPrioritizedThreadPool.QueueUserWorkItem(CheckLoaderPort, rie.Id, 1);
                    }
                    else
                    {
                        if (rie.IncreaseCompletionState() == 3)
                        {
                            cappedPrioritizedThreadPool.QueueUserWorkItem(Complete, rie.Id, 0);
                        }
                    }
                }
                else
                {
                    if (_includeOfflineComputers)
                    {
                        lock (_buf_lock)
                        {
                            remoteInfoScanBuffer.Add(rie.RemoteInfoEntity);
                        }
                        bufferNotEmpty.Set();
                    }
                    IncreaseScanningProgress();
                }
            }
            catch (NullReferenceException)
            {
                //scan was stopped
            }
            catch (InvalidOperationException)
            {
                //scan was stopped
            }
        }
        private void IncreaseScanningProgress()
        {
            if (isRunning)
            {
                if (Interlocked.Increment(ref _progress) == _count)
                {
                    _isCompleted = true;
                    isRunning = false;
                    StopTimer();
                }
                _progressPercentage = (int)Math.Round((_progress + 0.0) / _count * 100);
                if (!_isCompleted && (_progressPercentage == 100))
                {
                    _progressPercentage = 99;
                }
            }
            else if (!_isCompleted)
            {
                Interlocked.Increment(ref _progressOnPaused);
            }
        }
        private void CheckLoaderPort(Object state)
        {
            try
            {
                int id = (int)state;
                RemoteInfoEntityScan rie;
                if (remoteInfoScanQueuedDict.TryGetValue(id, out rie))
                {
                    IPAddress address = rie.IPAddress;
                    rie.IsLoaderPortOpen = RemoteScanHelper.IsLoaderPortOpen(address);
                }
                if (rie.IncreaseCompletionState() == 3)
                {
                    cappedPrioritizedThreadPool.QueueUserWorkItem(Complete, rie.Id, 0);
                }
            }
            catch (NullReferenceException)
            {
                //scan was stopped
            }
            catch (InvalidOperationException)
            {
                //scan was stopped
            }
        }
        private void CheckAgentPort(Object state)
        {
            try
            {
                int id = (int)state;
                RemoteInfoEntityScan rie;
                if (remoteInfoScanQueuedDict.TryGetValue(id, out rie))
                {
                    IPAddress address = rie.IPAddress;
                    rie.IsAgentPortOpen = RemoteScanHelper.IsAgentPortOpen(address);
                }
                if (rie.IncreaseCompletionState() == 3)
                {
                    cappedPrioritizedThreadPool.QueueUserWorkItem(Complete, rie.Id, 0);
                }
            }
            catch (NullReferenceException)
            {
                //scan was stopped
            }
            catch (InvalidOperationException)
            {
                //scan was stopped
            }
        }
        private void GetHostnameAndOs(Object state)
        {
            try
            {
                int id = (int)state;
                RemoteInfoEntityScan rie;
                if (remoteInfoScanQueuedDict.TryGetValue(id, out rie))
                {
                    string hostname;
                    if (RemoteScanHelper.GetHostname(rie.IPAddress, out hostname))
                    {
                        rie.Name = hostname;
                    }
                    else
                    {
                        hostname = rie.IPAddress.ToString();
                        rie.Name = "-";
                    }
                    string osVersion;
                    bool isWorkstation;
                    string errorInfo;
                    RemoteScanHelper.GetOS(hostname, _credentials, out osVersion, out isWorkstation,
                        out errorInfo, _methodType);
                    rie.OSVersion = osVersion;
                    rie.IsWorkstation = isWorkstation;
                    rie.ErrorInfo = errorInfo;
                }
                if (rie.IncreaseCompletionState() == 3)
                {
                    cappedPrioritizedThreadPool.QueueUserWorkItem(Complete, rie.Id, 0);
                }
            }
            catch (NullReferenceException)
            {
                //scan was stopped
            }
            catch (InvalidOperationException)
            {
                //scan was stopped
            }
        }
        private void Complete(Object state)
        {
            try
            {
                int id = (int)state;
                RemoteInfoEntityScan rie;
                if (remoteInfoScanQueuedDict.TryGetValue(id, out rie))
                {
                    if (!((_testOs && !_includeNoOsComputers && (rie.OSVersion == String.Empty)) ||
                    (_testAgentPort && !_includeNoAgentComputers && (rie.IsAgentPortOpen = false)) ||
                    (_testLoaderPort && !_includeNoLoaderComputers && (rie.IsLoaderPortOpen = false))))
                    {
                        lock (_buf_lock)
                        {
                            remoteInfoScanBuffer.Add(rie.RemoteInfoEntity);
                        }
                        bufferNotEmpty.Set();
                    }
                }
                IncreaseScanningProgress();
            }
            catch (NullReferenceException)
            {
                //scan was stopped
            }
            catch (InvalidOperationException)
            {
                //scan was stopped
            }
        }
        #endregion
    }
}
