using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using vsisLib;

namespace VirusBlokAda.Vba32CC.Service.VSIS
{
    internal class UpdateService
    {
        #region Properties

        private vsisLib.Update _update;
        public vsisLib.Update UpdateClass
        {
            get { return _update; }
            set
            {
                _update = value;
                _update.OnNeedStop += new vsisLib._IUpdateEvents_OnNeedStopEventHandler(OnNeedStop);
                _update.OnNeedStopReason += new vsisLib._IUpdateEvents_OnNeedStopReasonEventHandler(OnNeedStopReason);
                _update.OnApplyFinish += new vsisLib._IUpdateEvents_OnApplyFinishEventHandler(OnApplyFinish);
            }
        }

        private Boolean _isAbort = false;
        internal Boolean IsAbort
        {
            get
            {
                lock (syncObject)
                {
                    return _isAbort;
                }
            }
            set
            {
                lock (syncObject)
                {
                    _isAbort = value;
                }
            }
        }

        private Boolean _isAlive = false;
        internal Boolean IsAlive
        {
            get
            {
                lock (syncObject)
                {
                    return _isAlive;
                }
            }
        }

        private String _stopReason = String.Empty;
        internal String StopReason
        {
            get
            {
                lock (syncObject)
                {
                    return _stopReason;
                }
            }
        }

        private DateTime _lastUpdate = DateTime.MinValue;
        internal DateTime LastUpdate
        {
            get
            {
                lock (syncObject)
                {
                    return _lastUpdate;
                }
            }
        }

        private Object syncObject;

        #endregion

        #region Constructors

        internal UpdateService()
        {
            syncObject = new Object();
        }

        #endregion

        #region Methods

        internal void SetEvents(UpdateEvents events)
        {
            if (events.OnDownloadStart != null)
                _update.OnDownloadStart += new vsisLib._IUpdateEvents_OnDownloadStartEventHandler(events.OnDownloadStart);
            if (events.OnDownloadProgress != null)
                _update.OnDownloadProgress += new vsisLib._IUpdateEvents_OnDownloadProgressEventHandler(events.OnDownloadProgress);
            if (events.OnDownloadFinish != null)
                _update.OnDownloadFinish += new vsisLib._IUpdateEvents_OnDownloadFinishEventHandler(events.OnDownloadFinish);

            if (events.OnApplyStart != null)
                _update.OnApplyStart += new vsisLib._IUpdateEvents_OnApplyStartEventHandler(events.OnApplyStart);
            if (events.OnApplyProgress != null)
                _update.OnApplyProgress += new vsisLib._IUpdateEvents_OnApplyProgressEventHandler(events.OnApplyProgress);
            if (events.OnApplyFinish != null)
                _update.OnApplyFinish += new vsisLib._IUpdateEvents_OnApplyFinishEventHandler(events.OnApplyFinish);

            if (events.OnInfoMessage != null)
                _update.OnInfoMessage += new vsisLib._IUpdateEvents_OnInfoMessageEventHandler(events.OnInfoMessage);

            if (events.OnNeedStop != null)
                _update.OnNeedStop += new vsisLib._IUpdateEvents_OnNeedStopEventHandler(events.OnNeedStop);
            if (events.OnNeedStopReason != null)
                _update.OnNeedStopReason += new vsisLib._IUpdateEvents_OnNeedStopReasonEventHandler(events.OnNeedStopReason);

            if (events.OnError != null)
                _update.OnError += new vsisLib._IUpdateEvents_OnErrorEventHandler(events.OnError);
            if (events.OnReactionOnUpdate != null)
                _update.OnReactionOnUpdate += new vsisLib._IUpdateEvents_OnReactionOnUpdateEventHandler(events.OnReactionOnUpdate);
            if (events.OnReserveProgress != null)
                _update.OnReserveProgress += new vsisLib._IUpdateEvents_OnReserveProgressEventHandler(events.OnReserveProgress);
        }

        internal void Update()
        {
            new Thread(UpdateStart).Start();
        }

        private void UpdateStart()
        {
            try
            {
                if (IsAlive && IsAbort)
                {
                    lock (syncObject)
                    {
                        _stopReason = "Update already running...";
                    }
                    return;
                }
                UpdateParameters param = new UpdateParameters();
                param.complectation = "VBA32AAW";
                String AppPath = System.Reflection.Assembly.GetExecutingAssembly().CodeBase.Replace(@"file:///", "").Replace(@"/", @"\");
                param.program_folder = System.IO.Directory.GetParent(AppPath).Parent.Parent.FullName;
                lock (syncObject)
                {
                    _stopReason = String.Empty;
                    _isAlive = true;
                }
                _update.UpdateNow(ref param);
            }
            catch (Exception e)
            {
                lock (syncObject)
                {
                    _stopReason = "Error update: " + e.Message;
                }
            }
            finally
            {
                lock (syncObject)
                {
                    _isAlive = false;
                }
                IsAbort = false;
            }
        }

        private void OnNeedStop(out Int32 stop)
        {
            stop = IsAbort ? 1 : 0;
        }

        private void OnNeedStopReason(String reason, out Int32 stop)
        {
            lock (syncObject)
            {
                _stopReason = reason;
            }
            stop = IsAbort ? 1 : 0;
        }

        private void OnApplyFinish(Int32 success)
        {
            lock (syncObject)
            {
                if (success != 0)
                {
                    _lastUpdate = DateTime.Now;
                    _stopReason = String.Empty;
                }
                else
                    _stopReason = "Error update: " + _stopReason;
            }
        }

        #endregion
    }
}
