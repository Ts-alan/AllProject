using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Diagnostics;

namespace VirusBlokAda.CC.RemoteOperations.Threading
{
    class CappedPrioritizedThreadPool
    {
        private Object ctpLock = new Object();
        private int threadCount = 0;             

        private Thread schedulerThread;
        private AutoResetEvent continueRunning;
        private AutoResetEvent poolThreadAvailable;
        private bool isRunning = true;
        private bool isStopped = false;
        private Queue[] waitCallbackPrioritizedQueues;

        #region Properties
        private bool _doCheckAvailablePoolThreads = true;
        public bool DoCheckAvailablePoolThreads
        {
            get { return _doCheckAvailablePoolThreads; }
            set { _doCheckAvailablePoolThreads = value; }
        }
        
        private int _maxPriority;
        public int MaxPriority
        {
            get { return _maxPriority; }
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
                if (_maxThreads < value)
                {
                    _maxThreads = value;
                    continueRunning.Set();
                }
                else
                {
                    _maxThreads = value;
                }
            }
        }
        #endregion

        public CappedPrioritizedThreadPool(int maxThreads, int maxPriority)
        {
            Initialize(maxThreads, maxPriority);
        }

        private void Initialize(int maxThreads, int maxPriority)
        {
            if (maxThreads <= 0)
            {
                throw new ArgumentOutOfRangeException("Number of threads must be positive number");
            }
            _maxThreads = maxThreads;

            if (maxPriority < 0 || maxPriority > 10)
            {
                throw new ArgumentOutOfRangeException("Maximum priority must be between 0 and 9");
            }
            _maxPriority = maxPriority;

            waitCallbackPrioritizedQueues = new Queue[_maxPriority + 1];
            for (int i = 0; i <= _maxPriority; i++)
            {
                waitCallbackPrioritizedQueues[i] = new Queue();
            }

            poolThreadAvailable = new AutoResetEvent(false);

            continueRunning = new AutoResetEvent(false);
            schedulerThread = new Thread(RunSchedulerThread);
            schedulerThread.IsBackground = true;
            schedulerThread.Start();
        }

        public CappedPrioritizedThreadPool(int maxThreads)
        {
            Initialize(maxThreads, 0);
        }

        public bool QueueUserWorkItem(WaitCallback waitCallback, Object state, int priority)
        {
            if (isStopped)
            {
                throw new InvalidOperationException(
                    "Can't call QueueUserWorkItem on stopped CappedPrioritizedPool");
            }
            if (priority < 0 || priority > _maxPriority)
            {
                throw new ArgumentOutOfRangeException("Priority must be between 0 and Maxpriority");
            }
            if (waitCallback == null)
            {
                throw new ArgumentNullException("WaitCallback passed to ScanThreadPool is null");
            }
            WaitCallbackContainer container = new WaitCallbackContainer(waitCallback, state);
            try
            {
                lock (ctpLock)
                {
                    waitCallbackPrioritizedQueues[priority].Enqueue(container);
                }
                continueRunning.Set();
            }
            catch (Exception ex)
            {
                throw new NotSupportedException("ScanThreadPool failed to QueueUserWorkItem", ex);
            }
            return true;
        }

        public bool QueueUserWorkItem(WaitCallback waitCallback)
        {
            return QueueUserWorkItem(waitCallback, null, _maxPriority);
        }

        public bool QueueUserWorkItem(WaitCallback waitCallback, int priority)
        {
            return QueueUserWorkItem(waitCallback, null, priority);
        }

        public bool QueueUserWorkItem(WaitCallback waitCallback, Object state)
        {
            return QueueUserWorkItem(waitCallback, state, _maxPriority);
        }

        private void RunSchedulerThread()
        {
            try
            {
                while (isRunning)
                {
                    continueRunning.WaitOne(Timeout.Infinite, false);
                    while (Next()) ;                    
                }
            }
            catch (ThreadInterruptedException)
            {
                //Someone called the pause method 
            }
        }

        public void Pause()
        {
            if (isStopped)
            {
                throw new InvalidOperationException(
                    "Can't call Pause on stopped CappedPrioritizedPool");
            }
            isRunning = false;
            schedulerThread.Interrupt();
        }

        public bool Stop()
        {
            isStopped = true;
            isRunning = false;
            schedulerThread.Interrupt();
            bool isEmpty = true;
            lock (ctpLock)
            {
                for (int i = 0; i <= MaxPriority; i++)
                {
                    if (waitCallbackPrioritizedQueues[i].Count > 0)
                    {
                        isEmpty = false;
                        waitCallbackPrioritizedQueues[i].Clear();
                    }
                    waitCallbackPrioritizedQueues[i] = null;
                }
            }
            return isEmpty;
        }

        public void Continue()
        {
            if (isStopped)
            {
                throw new InvalidOperationException(
                    "Can't call Continue on stopped CappedPrioritizedPool");
            }
            if (isRunning)
            {
                return;
            }
            while (schedulerThread.IsAlive) 
            {
                //in unlikely event when someone calls Pause and than immediately Continue
                //we need the sheduler thread to stop first
                Thread.Sleep(50);
            }
            isRunning = true;
            schedulerThread = new Thread(RunSchedulerThread);
            schedulerThread.IsBackground = true;
            schedulerThread.Start();
            continueRunning.Set();
        }        

        private void AvailablePoolThread()
        {
            ThreadPool.QueueUserWorkItem(delegate(Object state)
            {
                poolThreadAvailable.Set();
            });
            poolThreadAvailable.WaitOne();
        }

        private bool Next()
        {
            lock (ctpLock)
            {
                if (threadCount < MaxThreads)
                {
                    if (_doCheckAvailablePoolThreads)
                    {
                        AvailablePoolThread();
                    }
                    for (int i = 0; i <= MaxPriority; i++)
                    {
                        if (waitCallbackPrioritizedQueues[i].Count > 0)
                        {
                            WaitCallbackContainer container = (WaitCallbackContainer)waitCallbackPrioritizedQueues[i].Dequeue();
                            threadCount++;
                            container.WaitCallback.BeginInvoke(container.State, WaitCallbackEnded, container.WaitCallback);
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        private void WaitCallbackEnded(IAsyncResult ar)
        {
            WaitCallback waitCallback = (WaitCallback)ar.AsyncState;
            try
            {
                waitCallback.EndInvoke(ar);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            NextFinished();
        }

        private void NextFinished()
        {
            lock (ctpLock)
            {
                threadCount--;
            }
            continueRunning.Set();
        }
    }
}