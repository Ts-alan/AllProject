using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace VirusBlokAda.Vba32CC.Service.VSIS
{
    internal class UpdateService
    {
        #region Properties

        private vsisLib.Update _update;        

        #endregion

        #region Constructors

        internal UpdateService(vsisLib.Update update)
        {
            _update = update;
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
        }

        internal void Update()
        {
            new Thread(UpdateStart).Start();
        }

        private void UpdateStart()
        {
            try
            {
                vsisLib.UpdateParameters param = new vsisLib.UpdateParameters();
                param.complectation = "VBA32AAW";
                param.program_folder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                _update.UpdateNow(ref param);
            }
            catch { }
        }

        #endregion
    }
}
