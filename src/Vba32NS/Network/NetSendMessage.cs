using System;
using System.Collections.Generic;
using System.Text;

using System.Diagnostics;

namespace Vba32.ControlCenter.NotificationService.Network
{
    public static class NetSendMessage
    {
        public static bool Send(string toAddr, string message)
        {
            LoggerNS.log.Info("NetSendMessage.Send():: Started ");
            try
            {
                ProcessStartInfo info = new ProcessStartInfo();
                info.WindowStyle = ProcessWindowStyle.Hidden;
                info.FileName = "net";
                info.Arguments = "send " + toAddr + " " + message;
                Process.Start(info);
            }
            catch (Exception ex)
            {
                LoggerNS.log.Info("NetSendMessage.Send():: " + ex.Message);
                return false;
            }
            return true;
        }
    }
}
