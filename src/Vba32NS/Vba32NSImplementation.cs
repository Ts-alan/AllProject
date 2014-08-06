using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Collections.Specialized;
using Vba32.ControlCenter.NotificationService.Network;
using Vba32.ControlCenter.NotificationService.Notification;
using ARM2_dbcontrol.Service.Vba32NS;

namespace Vba32.ControlCenter.NotificationService
{
    internal class Vba32NSImplementation : MarshalByRefObject, INotification
    {
        /// <summary>
        /// ������ ������������������ �������
        /// </summary>
        /// <param name="message">�������</param>
        public void OnRegisteredMessage(StringDictionary message)
        {
            try
            {
                //���� ������ ��������, ���� ������ �����-�� �����
                //����������� ����������, ���� ��������� null � �� �������� ���
                //������� �� ��������� ��� ��������� ���� �� null
                try
                {
                    LoggerNS.log.Info("Vba32NS.OnRegisteredMessage():: " + message["EventName"]);
                }
                catch (Exception iexp)
                {
                    LoggerNS.log.Error("Vba32NS.OnRegisteredMessage():: Input parameter is wrong. Throw exception.. ");
                    throw iexp;
                }

                //��������� ������������� ���������� ��������..
                if (Vba32NS.IsReRead())
                    if (Vba32NS.ReadSettingsFromRegistry())
                    {
                        Vba32NS.list = Vba32NS.GetNotifyEventList();
                        Vba32NS.SkipReRead();
                        if (Vba32NS.Jclient != null)
                        {
                            if (Vba32NS.Jclient.IsNeedReconnect(Vba32NS.settingsNS.JabberServer,
                                            Vba32NS.settingsNS.JabberFromJID,
                                            Vba32NS.settingsNS.JabberPassword))
                            {
                                LoggerNS.log.Info("Need to reconnect to server jabber");
                                Vba32NS.Jclient.CloseConnection();
                            }
                            else
                            {
                                LoggerNS.log.Info("Needn't to reconnect to server jabber");
                            }
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(Vba32NS.settingsNS.JabberServer))
                            {
                                Vba32NS.SelectXMPPLibrary();
                            }
                        }
                    }

                if (Vba32NS.list == null)
                {
                    Vba32NS.list = Vba32NS.GetNotifyEventList();
                    if (Vba32NS.list == null)
                    {
                        LoggerNS.log.Error("Vba32NS.OnRegisteredMessage():: Cannot get list of events");
                        //���� �� � ����� �� ��� �� ��������, �� ������ ����������
                        //��������. ��� ������������ ���������� ������������� ������ 
                        //��������, ����� ���� � ������� ����������� �� ����������� �� �����
                        return;
                    }
                    if (!Vba32NS.ReadSettingsFromRegistry())
                        LoggerNS.log.Error("Vba32NS.OnRegisteredMessage()::ReadSettingsFromRegistry returned false ");
                    else
                        Vba32NS.SkipReRead();
                }

                Vba32NS.list.Find(delegate(NotifyEvent ev)
                {
                    try
                    {
                        //������� �������� isNotify ��������
                        //�� ��� ��������� ������� ���� ������� ����
                        //if ((ev.IsNotify) && (ev.EventName == message["EventName"]))
                        if (ev.EventName == message["EventName"])
                        {
                            LoggerNS.log.Info("Vba32NS.OnRegisteredMessage():: Event " + message["EventName"] + " has been registred");
                            if (ev.NetSend.IsUse)
                            {
                                foreach (String addr in ev.NetSend.AddrList)
                                {
                                    LoggerNS.log.Info(String.Format("NetSend to {0}, message: {1}",
                                        addr, NotifyMessageBuilder.BuildBody(message, ev.NetSend.Message)));

                                    Vba32NS.SendNetSend(addr, NotifyMessageBuilder.BuildBody(message, ev.NetSend.Message));

                                }
                            }

                            if (ev.Jabber.IsUse)
                            {
                                if (!String.IsNullOrEmpty(Vba32NS.settingsNS.JabberServer))
                                {
                                    //Credentials could change
                                    if (!Vba32NS.Jclient.CheckConnectionState())
                                        Vba32NS.Jclient.OpenConnection(
                                            Vba32NS.settingsNS.JabberServer,
                                            Vba32NS.settingsNS.JabberFromJID,
                                            Vba32NS.settingsNS.JabberPassword);


                                    foreach (String addr in ev.Jabber.AddrList)
                                    {

                                        LoggerNS.log.Info(String.Format("Jabber to {0}, message: {1}",
                                             addr, NotifyMessageBuilder.BuildBody(message, ev.Jabber.Message)));

                                        Vba32NS.SendJabber(addr, NotifyMessageBuilder.BuildBody(message, ev.Jabber.Message));
                                    }
                                }
                                else
                                    LoggerNS.log.Error("Vba32NS.OnRegisteredMessage():: The jabber server hasn't been defined, but the event will be sent");

                            }

                            if (ev.Mail.IsUse)
                            {
                                if (!String.IsNullOrEmpty(Vba32NS.settingsNS.MailServer))
                                    foreach (String addr in ev.Mail.AddrList)
                                    {
                                        LoggerNS.log.Info(String.Format("Mail to {0}, message: {1}",
                                             addr, NotifyMessageBuilder.BuildBody(message, ev.Mail.Message)));

                                        System.Net.NetworkCredential credential = null;
                                        if(Vba32NS.settingsNS.UseMailAuthorization)
                                        {
                                            credential = new System.Net.NetworkCredential(Vba32NS.settingsNS.MailUsername, Vba32NS.settingsNS.MailPassword);
                                        }

                                        Vba32NS.SendMail(Vba32NS.settingsNS.MailServer, Vba32NS.settingsNS.MailFrom, Vba32NS.settingsNS.MailDisplayName,
                                            NotifyMessageBuilder.BuildSubject(message, ev.Mail.Subject),
                                            addr, NotifyMessageBuilder.BuildBody(message, ev.Mail.Message),
                                            ev.Mail.Priority, credential);
                                    }
                                else
                                    LoggerNS.log.Error("Vba32NS.OnRegisteredMessage():: The mail server hasn't been defined, but the event will be sent");
                            }
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggerNS.log.Error("Vba32NS.OnRegisteredMessage():: Find error: " +
                            String.Format("{0}, {1}, {2}", ex, ex.Source, ex.StackTrace));
                    }
                    return false;
                }
                );

            }
            catch (Exception ex)
            {
                LoggerNS.log.Error("Vba32NS.OnRegisteredMessage():: " + ex.Message);
            }
        }
    }
}