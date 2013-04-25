using System;
using System.Collections.Generic;
using System.Text;

using System.Diagnostics;
using System.Threading;
using System.Collections.Specialized;

using Vba32.ControlCenter.NotificationService.Xml;
using Vba32.ControlCenter.NotificationService.Network;
using Vba32.ControlCenter.NotificationService.Notification;



namespace Vba32.ControlCenter.NotificationService
{
    internal class Vba32NSImplementation: MarshalByRefObject, INotification
    {
        /// <summary>
        /// Пришло зарегистрированное событие
        /// </summary>
        /// <param name="message">Событие</param>
        public void OnRegisteredMessage(StringDictionary message)
        {
            try
            {
                //Дабы отсечь ситуацию, если придет какой-то мусор
                //Сгенерирует исключение, если передадим null и не валидное имя
                //неплохо бы проверить еще остальные поля на null
                try
                {
                    Vba32NS.LogMessage("Vba32NS.OnRegisteredMessage():: " + message["EventName"],1);
                }
                catch (Exception iexp)
                {
                    Vba32NS.LogError("Vba32NS.OnRegisteredMessage():: Input parameter is wrong. Throw exception.. ", 
                        EventLogEntryType.Error);
                    throw iexp;
                }

                //Проверяем необходимость считывания настроек..
                if (Vba32NS.IsReRead())
                    if (Vba32NS.ReadSettingsFromRegistry())
                    {
                        Vba32NS.list = Vba32NS.GetNotifyEventList();
                        Vba32NS.SkipReRead();
                        if (Vba32NS.Jclient != null)
                        {
                            if (Vba32NS.Jclient.IsNeedReconnect(Vba32NS.jabberServer,
                                            Vba32NS.jabberFromJID,
                                            Vba32NS.jabberPassword))
                            {
                                Vba32NS.LogMessage("Need to reconnect to server jabber", 6);
                                Vba32NS.Jclient.CloseConnection();
                            }
                            else
                            {
                                Vba32NS.LogMessage("Needn't to reconnect to server jabber", 6);
                            }
                        }
                        else 
                        {
                            if (!String.IsNullOrEmpty(Vba32NS.jabberServer))
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
                        Vba32NS.LogError("Vba32NS.OnRegisteredMessage():: Cannot get list of events",
                               EventLogEntryType.Error);
                        //Если уж и здесь мы его не получили, то дальше бесполезно
                        //рыпаться. При тестировании получилось воспроизвести данную 
                        //ситуацию, когда файл с данными настройками не существовал на диске
                        return;
                    }
                    if (!Vba32NS.ReadSettingsFromRegistry())
                        Vba32NS.LogError("Vba32NS.OnRegisteredMessage()::ReadSettingsFromRegistry returned false ",
                            EventLogEntryType.Error);
                    else
                        Vba32NS.SkipReRead();
                }

                Vba32NS.list.Find(delegate(NotifyEvent ev)
                {
                    try
                    {
                        //Полагаю проверку isNotify излишней
                        //Но для возможных будущих нужд оставлю поле
                        //if ((ev.IsNotify) && (ev.EventName == message["EventName"]))
                        if (ev.EventName == message["EventName"])
                        {
                            Vba32NS.LogMessage("Vba32NS.OnRegisteredMessage():: Event " + message["EventName"] + " has been registred",1);
                            if (ev.NetSend.IsUse)
                            {
                                foreach (string addr in ev.NetSend.AddrList)
                                {
                                    Vba32NS.LogMessage(String.Format("NetSend to {0}, message: {1}",
                                        addr, NotifyMessageBuilder.BuildBody(message, ev.NetSend.Message)),4);
                                        
                                    Vba32NS.SendNetSend(addr, NotifyMessageBuilder.BuildBody(message, ev.NetSend.Message));

                                }
                            }

                            if (ev.Jabber.IsUse)
                            {
                                if (!String.IsNullOrEmpty(Vba32NS.jabberServer))
                                {
                                    //Credentials could change
                                    if (!Vba32NS.Jclient.CheckConnectionState())
                                        Vba32NS.Jclient.OpenConnection(
                                            Vba32NS.jabberServer,
                                            Vba32NS.jabberFromJID,
                                            Vba32NS.jabberPassword);


                                    foreach (string addr in ev.Jabber.AddrList)
                                    {

                                        Vba32NS.LogMessage(String.Format("Jabber to {0}, message: {1}",
                                             addr, NotifyMessageBuilder.BuildBody(message, ev.Jabber.Message)),4);

                                        Vba32NS.SendJabber(addr, NotifyMessageBuilder.BuildBody(message, ev.Jabber.Message));
                                    }
                                }
                                else
                                    Vba32NS.LogError("Vba32NS.OnRegisteredMessage():: The jabber server hasn't been defined, but the event will be sent",
                                       EventLogEntryType.Error);

                            }

                            if (ev.Mail.IsUse)
                            {
                                if (!String.IsNullOrEmpty(Vba32NS.mailServer))
                                    foreach (string addr in ev.Mail.AddrList)
                                    {
                                        Vba32NS.LogMessage(String.Format("Mail to {0}, message: {1}",
                                             addr, NotifyMessageBuilder.BuildBody(message, ev.Mail.Message)),4);
                                             
                                        Vba32NS.SendMail(Vba32NS.mailServer, Vba32NS.mailFrom, Vba32NS.mailDisplayName,
                                            NotifyMessageBuilder.BuildSubject(message, ev.Mail.Subject),
                                            addr, NotifyMessageBuilder.BuildBody(message, ev.Mail.Message),
                                            ev.Mail.Priority);
                                    }
                                else
                                    Vba32NS.LogError("Vba32NS.OnRegisteredMessage():: The mail server hasn't been defined, but the event will be sent",
                                        EventLogEntryType.Error);
                            }
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Vba32NS.LogError("Vba32NS.OnRegisteredMessage():: Find error: " +
                            String.Format("{0}, {1}, {2}",ex,ex.Source,ex.StackTrace) , EventLogEntryType.Error);
                    }
                    return false;
                }
                );

            }
            catch (Exception ex)
            {
                Vba32NS.LogError("Vba32NS.OnRegisteredMessage():: " +
                    ex.Message, EventLogEntryType.Error);
            }
        }
    }
}
