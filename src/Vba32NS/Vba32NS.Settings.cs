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
using Vba32.ControlCenter.NotificationService.Network;
using Vba32.ControlCenter.NotificationService.Notification;
using VirusBlokAda.CC.Common.Xml;

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
            LoggerNS.log.Info("Vba32NS.GetNotifyEventList():: Started");
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
                LoggerNS.log.Error("Vba32NS.GetNotifyEventList()::" + ex.Message);
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
            LoggerNS.log.Info("Vba32NS.IsReRead():: Started");
            int isReRead = 0;
            try
            {
                RegistryKey key =
                        Registry.LocalMachine.OpenSubKey(registryControlCenterKeyName + "\\Notification");

                object tmp = key.GetValue("ReRead");
                if (tmp == null)
                {
                    LoggerNS.log.Info("Vba32NS.IsReRead()::Cannot get ReRead value");
                    return false;
                }
                else
                    isReRead = (int)tmp;

                key.Close();
            }
            catch (Exception ex)
            {
                LoggerNS.log.Error("Vba32NS.IsReRead()::" + ex.Message);
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
            LoggerNS.log.Info("Vba32NS.SkipReRead():: Started");
            try
            {
                RegistryKey key =
                           Registry.LocalMachine.CreateSubKey(registryControlCenterKeyName + "\\Notification");

                key.DeleteValue("ReRead");
            }
            catch (Exception ex)
            {
                LoggerNS.log.Error("Vba32NS.SkipReRead()::" + ex.Message);
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
            LoggerNS.log.Info("Vba32NS.ReadSettingsFromRegistry():: Started");
            try
            {
                RegistryKey key =
                          Registry.LocalMachine.OpenSubKey(registryControlCenterKeyName + "\\Notification");

                lock (synch)
                {
                    //Logging
                    int? tmp = (int?)key.GetValue("AllowLog");
                    logLevel = tmp.HasValue ? tmp.Value : 0;

                    LoggerNS.log.Info("AllowLog=" + logLevel);
                    
                    //xmpp library type
                    tmp = (int?)key.GetValue("XMPPLibrary");
                    xmppLibrary = tmp.HasValue ? tmp.Value : 0;

                    LoggerNS.log.Info("XMPPLibrary=" + xmppLibrary);


                    //Jabber
                    jabberServer = (string)key.GetValue("JabberServer");
                    if (String.IsNullOrEmpty(jabberServer))
                    {
                        LoggerNS.log.Error("Vba32NS.ReadSettingsFromRegistry()::Cannot get JabberServer value");

                       // return false;
                    }
                    else
                    {
                        LoggerNS.log.Info("JabberServer=" + jabberServer);

                        jabberFromJID = (string)key.GetValue("JabberFromJID");
                        if (jabberFromJID == null)
                        {
                            LoggerNS.log.Error("Vba32NS.ReadSettingsFromRegistry():: Cannot get JabberFromJID value");

                            return false;
                        }
                        LoggerNS.log.Info("JabberFromJID=" + jabberFromJID);

                        /*byte[] passBytes = (byte[])key.GetValue("JabberPassword");
                        if (passBytes == null)
                        {
                            LoggerNS.log.Error("ReadSettingsFromRegistry()::Не удалось получить ключ JabberPassword",
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
                            LoggerNS.log.Error("Vba32NS.ReadSettingsFromRegistry():: Cannot get JabberPassword value");

                            return false;
                        }
                        LoggerNS.log.Info("JabberPassword=" + jabberPassword);
                    }

                    //Mail
                    mailServer = (string)key.GetValue("MailServer");
                    if (String.IsNullOrEmpty(mailServer))
                    {
                        LoggerNS.log.Error("Vba32NS.ReadSettingsFromRegistry()::Cannot get MailServer value");

                        //return false;
                    }
                    else
                    {
                        LoggerNS.log.Info("MailServer=" + mailServer);

                        mailFrom = (string)key.GetValue("MailFrom");
                        if (mailFrom == null)
                        {
                            LoggerNS.log.Error("Vba32NS.ReadSettingsFromRegistry()::Cannot get MailFrom value");

                            return false;
                        }
                        LoggerNS.log.Info("MailFrom=" + mailFrom);

                        mailDisplayName = (string)key.GetValue("MailDisplayName");
                        if (mailDisplayName == null)
                        {
                            LoggerNS.log.Error("Vba32NS.ReadSettingsFromRegistry()::Cannot get MailDisplayName value");

                            return false;
                        }
                        LoggerNS.log.Info("MailDisplayName=" + mailDisplayName);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerNS.log.Error("Vba32NS.ReadSettingsFromRegistry():: Key: " + registryControlCenterKeyName +
                    " Error:"+ ex.Message);
                return false;
            }

            return true;
        }

    }
}