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
using VirusBlokAda.CC.Common;
using VirusBlokAda.CC.Settings;
using ARM2_dbcontrol.Service.Vba32NS;

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
                String settingsFileName = path + "webconsole\\settings\\Vba32NS.xml";
               
                tlist = ObjectSerializer.XmlFileToObj<List<NotifyEvent>>(settingsFileName);

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
        internal static Boolean IsReRead()
        {
            LoggerNS.log.Info("Vba32NS.IsReRead():: Started");
            try
            {
                return SettingsProvider.GetReRead(SettingTypes.NS);
            }
            catch (Exception ex)
            {
                LoggerNS.log.Error("Vba32NS.IsReRead()::" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Удаляет ReRead ключ
        /// </summary>
        /// <returns></returns>
        internal static Boolean SkipReRead()
        {
            LoggerNS.log.Info("Vba32NS.SkipReRead():: Started");
            try
            {
                SettingsProvider.SkipReRead(SettingTypes.NS);
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
        internal static Boolean ReadSettingsFromRegistry()
        {
            LoggerNS.log.Info("Vba32NS.ReadSettingsFromRegistry():: Started");
            try
            {
                LoggerNS.log.Info("1. Read LogLevel settings.");

                LoggerNS.Level = SettingsProvider.GetLogLevel();
                LoggerNS.log.Info("Log level: " + LoggerNS.Level.ToString());

                LoggerNS.log.Info("2. Read settings from 'Notification'.");

                settingsNS = SettingsProvider.GetNSSettings();

                    LoggerNS.log.Info("XMPPLibrary=" + settingsNS.XMPPLibrary);

                    //Jabber
                    if (String.IsNullOrEmpty(settingsNS.JabberServer))
                    {
                        LoggerNS.log.Error("Vba32NS.ReadSettingsFromRegistry()::Cannot get JabberServer value");
                    }
                    else
                    {
                        LoggerNS.log.Info("JabberServer=" + settingsNS.JabberServer);

                        if (String.IsNullOrEmpty(settingsNS.JabberFromJID))
                        {
                            LoggerNS.log.Error("Vba32NS.ReadSettingsFromRegistry():: Cannot get JabberFromJID value");
                            return false;
                        }
                        LoggerNS.log.Info("JabberFromJID=" + settingsNS.JabberFromJID);

                        if (String.IsNullOrEmpty(settingsNS.JabberPassword))
                        {
                            LoggerNS.log.Error("Vba32NS.ReadSettingsFromRegistry():: Cannot get JabberPassword value");
                            return false;
                        }
                        LoggerNS.log.Info("JabberPassword=" + settingsNS.JabberPassword);
                    }

                    //Mail
                    if (String.IsNullOrEmpty(settingsNS.MailServer))
                    {
                        LoggerNS.log.Error("Vba32NS.ReadSettingsFromRegistry()::Cannot get MailServer value");
                    }
                    else
                    {
                        LoggerNS.log.Info("MailServer=" + settingsNS.MailServer);

                        if (String.IsNullOrEmpty(settingsNS.MailFrom))
                        {
                            LoggerNS.log.Error("Vba32NS.ReadSettingsFromRegistry()::Cannot get MailFrom value");
                            return false;
                        }
                        LoggerNS.log.Info("MailFrom=" + settingsNS.MailFrom);

                        if (String.IsNullOrEmpty(settingsNS.MailDisplayName))
                        {
                            LoggerNS.log.Error("Vba32NS.ReadSettingsFromRegistry()::Cannot get MailDisplayName value");

                            return false;
                        }
                        LoggerNS.log.Info("MailDisplayName=" + settingsNS.MailDisplayName);
                    }
            }
            catch (Exception ex)
            {
                LoggerNS.log.Error("Vba32NS.ReadSettingsFromRegistry():: Error:" + ex.Message);
                return false;
            }

            return true;
        }

    }
}