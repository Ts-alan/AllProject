using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace VirusBlokAda.RemoteOperations.RemoteScan.RemoteInfo
{
    class RemoteInfoEntityScan:RemoteInfoEntityWrap
    {
        public RemoteInfoEntityScan(RemoteInfoEntity _remoteInfoEntity)
            : base(_remoteInfoEntity)
        { }
        private int _completionState = 0;
        public int IncreaseCompletionState()
        {
            return Interlocked.Increment(ref _completionState);
        }
    }
}
