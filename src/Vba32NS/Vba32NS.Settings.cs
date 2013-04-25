using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;

using System.IO;
using System.Threading;

using Microsoft.Win32;

using Vba32.ControlCenter.NotificationService.Xml;
using Vba32.ControlCenter.NotificationService.Network;
using Vba32.ControlCenter.NotificationService.Notification;

namespace Vba32.ControlCenter.NotificationService
{
    /// <summary>
    /// Методы, для получения стартовых настроек сервиса
    /// </summary>
    public partial class Vba32NS : ServiceBase
    {
        /// <summary>
        /// Возвращает список событий, их уведомлений, действий
        /// Надо сделать данные операции потокобезопасными
        /// </summary>
        /// <returns></returns>
        internal static  List<NotifyEvent> GetNotifyEventList()
        {
            LogMessage("Vba32NS.GetNotifyEventList():: Started",5);
            List<NotifyEvent> tlist = new List<NotifyEvent>();
            try
            {
                string settingsFileName = path + "webconsole\\settings\\Vba32NS.xml";
               
                tlist = ObjectSerializer.XmlStrToObj<List<NotifyEvent>>(settingsFileName);

                if (tlist == null)
                {
                    if (!File.Exists(settingsFileName))
                        throw new IOException(String.Format("File {0} not been found", settingsFileName));
                    else
                        throw new IOException(String.Format("File {0} exists, but cannot deserialize data from him.", settingsFileName));
                }
            }
            catch (Exception ex)
            {
                LogError("Vba32NS.GetNotifyEventList()::" + ex.Message,EventLogEntryType.Error);
            }
            return tlist;
        }

        /// <summary>
        /// Сигнализирует о необходимости считать заново настройки
        /// Использует ключ реестра IsReRead
        /// </summary>
        /// <returns></returns>
        internal static bool IsReRead()
        {
            LogMessage("Vba32NS.IsReRead():: Started",5);
            int isReRead = 0;
            try
            {
                RegistryKey key =
                        Registry.LocalMachine.OpenSubKey(registryControlCenterKeyName + "\\Notification");

                object tmp = key.GetValue("ReRead");
                if (tmp == null)
                {
                    LogMessage("Vba32NS.IsReRead()::Cannot get ReRead value",
                         3);
                    return false;
                }
                else
                    isReRead = (int)tmp;

                key.Close();
            }
            catch(Exception ex)
            {
                LogError("Vba32NS.IsReRead()::"+ex.Message,EventLogEntryType.Error);
                return false;
            }
            return (isReRead > 0 ? true : false);
        }

        /// <summary>
        /// Удаляет ReRead ключ
        /// </summary>
        /// <returns></returns>
        internal static bool SkipReRead()
        {
            LogMessage("Vba32NS.SkipReRead():: Started",5);
            try
            {
                RegistryKey key =
                           Registry.LocalMachine.CreateSubKey(registryControlCenterKeyName + "\\Notification");

                key.DeleteValue("ReRead");
            }
            catch (Exception ex)
            {
                LogError("Vba32NS.SkipReRead()::" + ex.Message, EventLogEntryType.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Считывает необходимые настройки для уведомлений по почте, jabber.
        /// </summary>
        /// <returns></returns>
        internal static bool ReadSettingsFromRegistry()
        {
            LogMessage("Vba32NS.ReadSettingsFromRegistry():: Started",5);
            try
            {
                RegistryKey key =
                          Registry.LocalMachine.OpenSubKey(registryControlCenterKeyName + "\\Notification");

                lock (synch)
                {
                    //Logging
                    int? tmp = (int?)key.GetValue("AllowLog");
                    logLevel = tmp.HasValue ? tmp.Value : 0;

                    LogMessage("AllowLog=" + logLevel, 1);
                    
                    //xmpp library type
                    tmp = (int?)key.GetValue("XMPPLibrary");
                    xmppLibrary = tmp.HasValue ? tmp.Value : 0;

                    LogMessage("XMPPLibrary=" + xmppLibrary, 1);


                    //Jabber
                    jabberServer = (string)key.GetValue("JabberServer");
                    if (String.IsNullOrEmpty(jabberServer))
                    {
                        LogError("Vba32NS.ReadSettingsFromRegistry()::Cannot get JabberServer value",
                            EventLogEntryType.Warning);

                       // return false;
                    }
                    else
                    {
                        LogMessage("JabberServer=" + jabberServer,3);

                        jabberFromJID = (string)key.GetValue("JabberFromJID");
                        if (jabberFromJID == null)
                        {
                            LogError("Vba32NS.ReadSettingsFromRegistry():: Cannot get JabberFromJID value",
                                EventLogEntryType.Error);

                            return false;
                        }
                        LogMessage("JabberFromJID=" + jabberFromJID,3);

                        /*byte[] passBytes = (byte[])key.GetValue("JabberPassword");
                        if (passBytes == null)
                        {
                            LogError("ReadSettingsFromRegistry()::Не удалось получить ключ JabberPassword",
                               EventLogEntryType.Error);

                            return false;
                        }
                        //Дешифруем пароль
                        int length = passBytes.Length;
                        for (int i = 0; i < length; ++i)
                        {
                            passBytes[i] ^= 0x0D;
                        }
                        jabberPassword = System.Text.Encoding.UTF8.GetString(passBytes);
                        */

                        jabberPassword = (string)key.GetValue("JabberPassword");
                        if (jabberPassword == null)
                        {
                            LogError("Vba32NS.ReadSettingsFromRegistry():: Cannot get JabberPassword value",
                                EventLogEntryType.Error);

                            return false;
                        }
                        LogMessage("JabberPassword=" + jabberPassword,3);
                    }

                    //Mail
                    mailServer = (string)key.GetValue("MailServer");
                    if (String.IsNullOrEmpty(mailServer))
                    {
                        LogError("Vba32NS.ReadSettingsFromRegistry()::Cannot get MailServer value",
                            EventLogEntryType.Warning);

                        //return false;
                    }
                    else
                    {
                        LogMessage("MailServer=" + mailServer,3);

                        mailFrom = (string)key.GetValue("MailFrom");
                        if (mailFrom == null)
                        {
                            LogError("Vba32NS.ReadSettingsFromRegistry()::Cannot get MailFrom value",
                                EventLogEntryType.Error);

                            return false;
                        }
                        LogMessage("MailFrom=" + mailFrom,3);

                        mailDisplayName = (string)key.GetValue("MailDisplayName");
                        if (mailDisplayName == null)
                        {
                            LogError("Vba32NS.ReadSettingsFromRegistry()::Cannot get MailDisplayName value",
                                EventLogEntryType.Error);

                            return false;
                        }
                        LogMessage("MailDisplayName=" + mailDisplayName,3);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("Vba32NS.ReadSettingsFromRegistry():: Key: " + registryControlCenterKeyName +
                    " Error:"+ ex.Message, EventLogEntryType.Error);
                return false;
            }

            return true;
        }

    }
}