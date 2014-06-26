using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using vsisLib;
using VirusBlokAda.CC.DataBase;

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

        internal UpdateEntity LastProcessing
        {
            get
            {
                lock (syncObject)
                {
                    return provider.GetLast(UpdateStateEnum.Processing);
                }
            }
        }

        internal UpdateEntity LastFail
        {
            get
            {
                lock (syncObject)
                {
                    return provider.GetLast(UpdateStateEnum.Fail);
                }
            }
        }

        internal UpdateEntity LastSuccess
        {
            get
            {
                lock (syncObject)
                {
                    return provider.GetLast(UpdateStateEnum.Success);
                }
            }
        }

        private UpdateProvider provider = null;

        private String _ConnectionString = String.Empty;
        internal String ConnectionString
        {
            get { return _ConnectionString; }
            set
            {
                _ConnectionString = value;
                provider = new UpdateProvider(_ConnectionString);
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
            ThreadPool.QueueUserWorkItem(UpdateStart);
            //new Thread(UpdateStart).Start();
        }

        private void UpdateStart(Object obj)
        {
            provider.InsertUpdate();
            UpdateEntity ent = LastProcessing;
            try
            {
                if (IsAlive && IsAbort)
                {
                    lock (syncObject)
                    {
                        ent.State = UpdateStateEnum.Fail;
                        ent.Description = "Update already running...";
                        provider.Update(ent);
                    }
                    return;
                }
                String[] param = new String[] { "VBA32CCK" };
                lock (syncObject)
                {
                    _isAlive = true;
                }
                _update.UpdateNow(param);
            }
            catch (Exception e)
            {
                lock (syncObject)
                {
                    ent.State = UpdateStateEnum.Fail;
                    ent.Description = "Error update: " + e.Message;
                    provider.Update(ent);
                }
            }
            finally
            {
                lock (syncObject)
                {
                    _isAlive = false;
                    if (LastProcessing != null)
                    {
                        ent.State = UpdateStateEnum.Fail;
                        provider.Update(ent);
                    }
                }
                IsAbort = false;
            }
        }

        private void OnNeedStop(out Int32 stop)
        {
            stop = IsAbort ? 1 : 0;
            
            if (IsAbort)
            {
                lock (syncObject)
                {
                    UpdateEntity ent = LastProcessing;
                    ent.State = UpdateStateEnum.Fail;
                    provider.Update(ent);
                }
            }
        }

        private void OnNeedStopReason(String reason, out Int32 stop)
        {
            UpdateEntity ent = LastProcessing;

            lock (syncObject)
            {
                ent.State = UpdateStateEnum.Processing;
                ent.Description = reason;
                provider.Update(ent);
            }
            stop = IsAbort ? 1 : 0;

            if (IsAbort)
            {
                lock (syncObject)
                {
                    ent.State = UpdateStateEnum.Fail;
                    ent.Description = reason;
                    provider.Update(ent);
                }
            }
        }

        private void OnApplyFinish(Int32 success)
        {
            UpdateEntity ent = LastProcessing;

            lock (syncObject)
            {
                if (success != 0)
                {
                    ent.State = UpdateStateEnum.Success;
                    provider.Update(ent);
                }
                else
                {
                    ent.State = UpdateStateEnum.Fail;
                    provider.Update(ent);
                }
            }
        }

        #endregion
    }
}
