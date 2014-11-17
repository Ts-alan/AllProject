using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;

using System.IO;
using System.Threading;
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
        internal static Boolean SendJabber(String to, String message)
        {
            LoggerNS.log.Info("Vba32NS.SendJabber():: Started");
            Boolean retValue = false;
            Thread thread = new Thread(delegate()
            {
                jclient.Send(to, message);

            });
            thread.Start();
            LoggerNS.log.Info("Wait 10 seconds..");
            if (thread.Join(TimeSpan.FromSeconds(10)))
            {
                LoggerNS.log.Info("thread.Join returned true");
            }
            else
            {
                LoggerNS.log.Info("thread.Join returned false");
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
        internal static Boolean SendMail(String server, Int32 port, Boolean useSSL, String from, String displayName,
            String subject, String to, String body, Int32 priority, System.Net.NetworkCredential credential)
        {
            LoggerNS.log.Info("Vba32NS.SendMail():: Started");
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
            return ob.Send(server, port, useSSL, from, displayName, to, subject, body, mpriority, credential);
        }

        /// <summary>
        /// �������� ��������� � ������� net send
        /// </summary>
        /// <param name="to">����</param>
        /// <param name="message">���������</param>
        /// <returns></returns>
        internal static Boolean SendNetSend(String to, String message)
        {
            LoggerNS.log.Info("Vba32NS.SendNetSend():: Started");
            Boolean retValue = false;
            Thread thread = new Thread(delegate()
            {
                retValue = NetSendMessage.Send(to, message);
            });

            thread.Start();
            LoggerNS.log.Info("Wait 10 seconds...");
            if (thread.Join(TimeSpan.FromSeconds(10)))
            {
                LoggerNS.log.Info("thread.Join returned true");
            }
            else
            {
                LoggerNS.log.Info("thread.Join returned false");
                thread.Abort();
                return false;
            }

            return retValue;
        }

        #endregion
    }
}