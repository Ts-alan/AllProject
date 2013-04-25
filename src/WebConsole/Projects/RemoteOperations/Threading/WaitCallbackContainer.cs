using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace VirusBlokAda.RemoteOperations.Threading
{
    class WaitCallbackContainer
    {
        private WaitCallback _waitCallback;
        public WaitCallback WaitCallback
        {
            get { return _waitCallback; }
            set { _waitCallback = value; }
        }

        private Object _state;
        public Object State
        {
            get { return _state; }
            set { _state = value; }
        }

        public WaitCallbackContainer(WaitCallback _waitCallBack,
            Object _state)
        {
            WaitCallback = _waitCallBack;
            State = _state;
        }
    }
}
