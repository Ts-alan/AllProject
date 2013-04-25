using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;

using System.IO;
using System.Threading;

using Vba32.ControlCenter.NotificationService.Xml;
using Vba32.ControlCenter.NotificationService.Network;
using Vba32.ControlCenter.NotificationService.Notification;

namespace Vba32.ControlCenter.NotificationService
{
    /// <summary>
    /// ����� ����������� ������, ������� ���������� �������� ��������� �� ����������
    /// ���������
    /// </summary>
    public partial class Vba32NS : ServiceBase
    {
        #region sending methods

        /// <summary>
        /// �������� IM Jabber
        /// </summary>
        /// <param name="to">����</param>
        /// <param name="message">���������</param>
        /// <returns></returns>
        internal static bool SendJabber(string to, string message)
        {
            LogMessage("Vba32NS.SendJabber():: Started",5);
            bool retValue = false;
            Thread thread = new Thread(delegate()
            {
                jclient.Send(to, message);

            });
            thread.Start();
            LogMessage("Wait 10 seconds..",10);
            if (thread.Join(TimeSpan.FromSeconds(10)))
            {
                LogMessage("thread.Join returned true",6);
            }
            else
            {
                LogMessage("thread.Join returned false", 6);
                thread.Abort();
                return false;
            }

            return retValue;
        }

        /// <summary>
        /// �������� �����
        /// </summary>
        /// <param name="server">�������� ������</param>
        /// <param name="from">�� ����</param>
        /// <param name="displayName">������������ ���</param>
        /// <param name="subject">����</param>
        /// <param name="to">����</param>
        /// <param name="body">����� ������</param>
        /// <returns></returns>
        internal static bool SendMail(string server, string from, string displayName,
            string subject, string to, string body, int priority)
        {
            LogMessage("Vba32NS.SendMail():: Started",5);
            SMTPMessage ob = new SMTPMessage();
            System.Net.Mail.MailPriority mpriority = new System.Net.Mail.MailPriority();
            switch (priority)
            {
                case 0:
                    mpriority = System.Net.Mail.MailPriority.Low;
                    break;
                case 1:
                    mpriority = System.Net.Mail.MailPriority.Normal;
                    break;
                case 2:
                    mpriority = System.Net.Mail.MailPriority.High;
                    break;
                default:
                    mpriority = System.Net.Mail.MailPriority.Low;
                    break;
            }
            return ob.Send(server, from, displayName, to, subject, body,mpriority);
        }

        /// <summary>
        /// �������� ��������� � ������� net send
        /// </summary>
        /// <param name="to">����</param>
        /// <param name="message">���������</param>
        /// <returns></returns>
        internal static bool SendNetSend(string to, string message)
        {
            LogMessage("Vba32NS.SendNetSend():: Started",5);
            bool retValue = false;
            Thread thread = new Thread(delegate()
            {
                retValue = NetSendMessage.Send(to, message);
            });

            thread.Start();
            LogMessage("Wait 10 seconds...",10);
            if (thread.Join(TimeSpan.FromSeconds(10)))
            {
                LogMessage("thread.Join returned true",10);
            }
            else
            {
                LogMessage("thread.Join returned false", 10);
                thread.Abort();
                return false;
            }

            return retValue;
        }

        #endregion
    }
}