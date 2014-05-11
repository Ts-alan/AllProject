using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Net;
using System.Globalization;
using Microsoft.Win32;
using Vba32.ControlCenter.PeriodicalMaintenanceService.Xml;
using VirusBlokAda.CC.Common;

namespace Vba32.ControlCenter.PeriodicalMaintenanceService
{
    /// <summary>
    /// Чтение и запись настроек из реестра
    /// </summary>
    partial class Vba32PMS
    {

        #region Чтение запись настроек из реестра
        /// <summary>
        /// Считывает необходимые настройки из реестра
        /// </summary>
        private Boolean ReadSettingsFromRegistry()
        {
            LoggerPMS.log.Info("Vba32PMS.ReadSettingsFromRegistry():: Try read settings from registry.");
            try
            {

                LoggerPMS.log.Info("1. Read settings from 'PeriodicalMaintenance'.");

                RegistryKey key =
                    Registry.LocalMachine.OpenSubKey(registryControlCenterKeyName);
                if (key == null)
                {
                    LoggerPMS.log.Error("ReadSettingsFromRegistry()::Can't get key 'ControlCenter'.");
                    return false;
                }

                Object t_allowLog = key.GetValue("AllowLog");
                if (t_allowLog == null)
                {
                    LoggerPMS.Level = LogLevel.Debug;
                    LoggerPMS.log.Warning("Log level isn't set.");
                }
                else
                {
                    try
                    {
                        LoggerPMS.Level = (LogLevel)((Int32)t_allowLog);
                        LoggerPMS.log.Info("Log level: " + LoggerPMS.Level.ToString());
                    }
                    catch
                    {
                        LoggerPMS.Level = LogLevel.Debug;
                        LoggerPMS.log.Warning("Inadmissible log level.");
                    }
                }

                LoggerPMS.log.LoggingLevel = LoggerPMS.Level;
                key.Close();

                key = Registry.LocalMachine.OpenSubKey(registryControlCenterKeyName + "\\PeriodicalMaintenance");
                if (key == null)
                {
                    LoggerPMS.log.Error("ReadSettingsFromRegistry()::Can't get key 'PeriodicalMaintenance'.");
                    return false;
                }

                server = (String)key.GetValue("Server");
                if (server == null)
                {
                    LoggerPMS.log.Error("Vba32PMS.ReadSettingsFromRegistry()::Can't get key 'Server'.");
                    return false;
                }
                LoggerPMS.log.Info("Get value 'Server': " + server);

                Object tmp = key.GetValue("Port");
                if (tmp == null)
                {
                    LoggerPMS.log.Error("ReadSettingsFromRegistry():: Can't get key 'Port'.");
                    return false;
                }
                else
                    port = (Int32)tmp;
                LoggerPMS.log.Info("Get value 'Port': " + port);

                tmp = key.GetValue("MaxFileLength");
                if (tmp == null)
                {
                    LoggerPMS.log.Error("ReadSettingsFromRegistry()::Can't get key 'MaxFileLength'.");
                    return false;
                }
                else
                    maxFileLength = (Int32)tmp;
                LoggerPMS.log.Info("Get value 'MaxFileLength': " + maxFileLength);

                IFormatProvider culture = new CultureInfo("ru-RU");

                lastSelectDate = Convert.ToDateTime(key.GetValue("LastSelectDate"), culture);
                if ((lastSelectDate == null) || (lastSelectDate == DateTime.MinValue))
                {
                    LoggerPMS.log.Warning("No information about the last selection messages. Initialize default.");
                    lastSelectDate = DateTime.Now;
                }
                LoggerPMS.log.Info("Get value 'LastSelectDate': " + lastSelectDate);


                lastSendDate = Convert.ToDateTime(key.GetValue("LastSendDate"),culture);
                if ((lastSendDate == null) || (lastSendDate == DateTime.MinValue))
                {
                    LoggerPMS.log.Warning("No information about the last sending messages. Initialize default.");
                    lastSendDate = DateTime.Now;
                }
                LoggerPMS.log.Info("Get value 'LastSendDate': " + lastSendDate);

                nextSendDate = Convert.ToDateTime(key.GetValue("NextSendDate"), culture);
                if ((nextSendDate == null) || (nextSendDate == DateTime.MinValue))
                {
                    LoggerPMS.log.Warning("No information about the date of last sending messages. Initialize default.");
                    nextSendDate = DateTime.Now;
                }
                LoggerPMS.log.Info("Get value 'NextSendDate': " + nextSendDate);


                tmp = key.GetValue("DeliveryTimeoutCheck");
                if (tmp == null)
                {
                    LoggerPMS.log.Error("ReadSettingsFromRegistry():: Can't get key 'DeliveryTimeoutCheck'.");
                    return false;
                }
                else
                    deliveryTimeoutCheck = (Int32)tmp;
                LoggerPMS.log.Info("Get value 'DeliveryTimeoutCheck': " + deliveryTimeoutCheck);

                tmp = key.GetValue("DataSendInterval");
                if (tmp == null)
                {
                    LoggerPMS.log.Error("ReadSettingsFromRegistry():: Can't get key 'DataSendInterval'.");
                    return false;
                }
                else
                    dataSendInterval = (Int32)tmp;
                LoggerPMS.log.Info("Get value 'DataSendInterval': " + dataSendInterval);


                tmp = key.GetValue("DaysToDelete");
                if (tmp == null)
                {
                    LoggerPMS.log.Error("ReadSettingsFromRegistry():: Can't get key 'DaysToDelete'.");
                    return false;
                }
                else
                    daysToDelete = (Int32)tmp;
                LoggerPMS.log.Info("Get value 'DaysToDelete': " + daysToDelete);


                tmp = key.GetValue("TaskDaysToDelete");
                if (tmp == null)
                {
                    LoggerPMS.log.Warning("ReadSettingsFromRegistry():: Can't get key 'TaskDaysToDelete'. Initialize default.");
                    taskDaysToDelete = 180;
                }
                else
                    taskDaysToDelete = (Int32)tmp;
                LoggerPMS.log.Info("Get value 'TaskDaysToDelete': " + taskDaysToDelete);

                tmp = key.GetValue("ComputerDaysToDelete");
                if (tmp == null)
                {
                    LoggerPMS.log.Warning("ReadSettingsFromRegistry():: Can't get key 'ComputerDaysToDelete'. Initialize default.");
                    compDaysToDelete = 0;
                }
                else
                    compDaysToDelete = (Int32)tmp;
                LoggerPMS.log.Info("Get value 'ComputerDaysToDelete': " + compDaysToDelete);

                tmp = key.GetValue("HourIntervalToSend");
                if (tmp == null)
                {
                    LoggerPMS.log.Warning("ReadSettingsFromRegistry():: Can't get key 'HourIntervalToSend'. Initialize default.");
                    hourIntervalToSend = 4;
                }
                else
                    hourIntervalToSend = (Int32)tmp;
                LoggerPMS.log.Info("Get value 'HourIntervalToSend': " + hourIntervalToSend);

                tmp = key.GetValue("MaintenanceEnabled");
                if (tmp == null)
                {
                    LoggerPMS.log.Warning("ReadSettingsFromRegistry():: Can't get key 'MaintenanceEnabled'. Initialize default.");
                    maintenanceEnabled = true;
                }
                else
                    maintenanceEnabled = Convert.ToBoolean(tmp);
                LoggerPMS.log.Info("Periodically update state: " + (maintenanceEnabled ? "Allowed" : "Denied"));

                key.Close();

                LoggerPMS.log.Info("2. Read settings from DataBase");
                key =
                   Registry.LocalMachine.OpenSubKey(registryControlCenterKeyName + "\\DataBase");

                if (key == null)
                {
                    LoggerPMS.log.Error("ReadSettingsFromRegistry():: Can't open registry key 'DataBase'.");
                    return false;
                }

                String dataSource = (String)key.GetValue("DataSource");
                if (dataSource == null)
                {
                    LoggerPMS.log.Error("ReadSettingsFromRegistry():: Can't get key 'DataSource'.");
                    return false;
                }
                LoggerPMS.log.Info("Get value 'DataSource': " + dataSource);

                String userName = (String)key.GetValue("UserName");
                if (userName == null)
                {
                    LoggerPMS.log.Error("ReadSettingsFromRegistry():: Can't get key 'UserName'.");
                    return false;
                }
                LoggerPMS.log.Info("Get value 'UserName': " + userName);

                Byte[] passBytes = (Byte[])key.GetValue("Password");
                if (passBytes == null)
                {
                    LoggerPMS.log.Error("ReadSettingsFromRegistry():: Can't get key 'Password'.");
                    return false;
                }
                //Дешифруем пароль
                Int32 length = passBytes.Length;
                for (Int32 i = 0; i < length; ++i)
                {
                    passBytes[i] ^= 0x17;
                }
                String password = System.Text.Encoding.UTF8.GetString(passBytes);

                LoggerPMS.log.Info("Generate connection string.");
                connectionString = GenerateConnectionString(dataSource, userName, password);
            }
            catch (Exception ex)
            {
                LoggerPMS.log.Error("Vba32PMS.ReadSettingsFromRegistry()::" + ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Записывает необходимые настройки в реестр
        /// </summary>
        private Boolean WriteSettingsToRegistry()
        {
            LoggerPMS.log.Debug("Vba32PMS.WriteSettingsToRegistry():: Try write settings to registry.");
            try
            {
                RegistryKey key =
                   Registry.LocalMachine.CreateSubKey(registryControlCenterKeyName + "\\PeriodicalMaintenance");

                IFormatProvider culture = new CultureInfo("ru-RU");

                key.SetValue("LastSelectDate", lastSelectDate.ToString(culture), RegistryValueKind.String);
                key.SetValue("LastSendDate", lastSendDate.ToString(culture), RegistryValueKind.String);
                key.SetValue("NextSendDate", nextSendDate.ToString(culture), RegistryValueKind.String);
             
            }
            catch (Exception ex)
            {
                LoggerPMS.log.Error("Vba32PMS.WriteSettingsToRegistry()::" + ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Сигнализирует о необходимости считать заново настройки
        /// Использует ключ реестра IsReRead
        /// </summary>
        /// <returns></returns>
        private Boolean IsReRead()
        {
            LoggerPMS.log.Debug("Vba32PMS.IsReRead():: Enter");
            Int32 isReRead = 0;
            try
            {
                RegistryKey key =
                        Registry.LocalMachine.OpenSubKey(registryControlCenterKeyName + "\\PeriodicalMaintenance");

                Object tmp = key.GetValue("ReRead");
                if (tmp == null)
                {
                    LoggerPMS.log.Warning("Vba32NS.IsReRead():: Can't get key 'ReRead'.");
                    return false;
                }
                else
                    isReRead = (int)tmp;

                key.Close();
            }
            catch (Exception ex)
            {
                LoggerPMS.log.Error("Vba32PMS.IsReRead()::" + ex.Message);
                return false;
            }
            return (isReRead > 0 ? true : false);
        }

        /// <summary>
        /// Удаляет ReRead ключ
        /// </summary>
        /// <returns></returns>
        private Boolean SkipReRead()
        {
            LoggerPMS.log.Debug("Vba32PMS.SkipReRead():: Enter");
            try
            {
                RegistryKey key =
                           Registry.LocalMachine.CreateSubKey(registryControlCenterKeyName + "\\PeriodicalMaintenance");

                key.DeleteValue("Reread");
            }
            catch (Exception ex)
            {
                LoggerPMS.log.Error("Vba32PMS.SkipReRead()::" + ex.Message);
                return false;
            }

            return true;
        }
        #endregion
    }
}