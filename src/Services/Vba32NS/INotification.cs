using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;

using System.Collections.Specialized;

namespace Vba32.ControlCenter.NotificationService
{
    public interface INotification
    {
        void OnRegisteredMessage(StringDictionary message);
    }
}
