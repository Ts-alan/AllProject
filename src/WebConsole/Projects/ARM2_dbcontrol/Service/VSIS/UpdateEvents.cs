using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.Vba32CC.Service.VSIS
{
    public struct UpdateEvents
    {
        public OnDownloadStartDelegate OnDownloadStart;
        public OnDownloadProgressDelegate OnDownloadProgress;
        public OnDownloadFinishDelegate OnDownloadFinish;
        public OnApplyStartDelegate OnApplyStart;
        public OnApplyProgressDelegate OnApplyProgress;
        public OnApplyFinishDelegate OnApplyFinish;
        //public OnErrorDelegate OnError;
        public OnInfoMessageDelegate OnInfoMessage;
        public OnNeedStopDelegate OnNeedStop;
        public OnNeedStopReasonDelegate OnNeedStopReason;
        //public OnReactionOnUpdateDelegate OnReactionOnUpdate;
        //public OnReserveProgressDelegate OnReserveProgress;
        //public OnTryRollbackDelegate OnTryRollback;
    }

    public delegate void OnDownloadStartDelegate();
    public delegate void OnDownloadProgressDelegate(String file_name, Int32 curent_size, Int32 total_size);
    public delegate void OnDownloadFinishDelegate(Int32 success);
    public delegate void OnApplyStartDelegate();
    public delegate void OnApplyProgressDelegate(String file_name, UInt32 curent_size, UInt32 total_size);
    public delegate void OnApplyFinishDelegate(Int32 success);
    public delegate void OnErrorDelegate(String error_message, UInt32 error_id);
    public delegate void OnInfoMessageDelegate(String info_message);
    public delegate void OnNeedStopDelegate(out Int32 stop);
    public delegate void OnNeedStopReasonDelegate(String info_message, out Int32 stop);
    public delegate void OnReactionOnUpdateDelegate(Array reactions);
    public delegate void OnReserveProgressDelegate(String file_name, UInt32 curent_size, UInt32 total_size);
    public delegate void OnTryRollbackDelegate(out Int32 try_roll_back);
}
