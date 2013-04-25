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
            LogMessage("NetSendMessage.Send():: Started ", 15);
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
                LogMessage("NetSendMessage.Send():: "+ex.Message,10);
                return false;
            }
            return true;
        }

        public static void LogMessage(string body, int level)
        {
            Vba32NS.LogMessage(body, level);
        }
    }
}
