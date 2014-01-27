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
            Logger.Info("Vba32PMS.ReadSettingsFromRegistry()::Пробуем считать настройки из реестра");
            try
            {
              
                Logger.Info("1. Считываем настройки из PeriodicalMaintenance");

                RegistryKey key =
                    Registry.LocalMachine.OpenSubKey(registryControlCenterKeyName + "\\PeriodicalMaintenance");
                if (key == null)
                {
                    Logger.Error("ReadSettingsFromRegistry()::Не удалось получить ключ PeriodicalMaintenance");
                    return false;
                }

                Object t_allowLog = key.GetValue("AllowLog");
                if (t_allowLog == null)
                {
                    this.allowLog = LogLevel.Debug;
                    Logger.Warning("Уровень логгирования не установлен");
                }
                else
                {
                    try
                    {
                        this.allowLog = (LogLevel)((Int32)t_allowLog);
                        Logger.Info("Уровень логгирования: " + this.allowLog.ToString());
                    }
                    catch
                    {
                        this.allowLog = LogLevel.Debug;
                        Logger.Warning("Недопустимый уровень логгирования");
                    }
                }

                Logger.LoggingLevel = this.allowLog;

                server = (String)key.GetValue("Server");
                if (server == null)
                {
                    Logger.Error("Vba32PMS.ReadSettingsFromRegistry()::Не удалось получить ключ Server");
                    return false;
                }
                Logger.Info("Полученное значение Server: " + server);

                Object tmp = key.GetValue("Port");
                if (tmp == null)
                {
                    Logger.Error("ReadSettingsFromRegistry()::Не удалось получить ключ Port");
                    return false;
                }
                else
                    port = (Int32)tmp;
                Logger.Info("Полученное значение Port: " + port);

                tmp = key.GetValue("MaxFileLength");
                if (tmp == null)
                {
                    Logger.Error("ReadSettingsFromRegistry()::Не удалось получить ключ MaxFileLength");
                    return false;
                }
                else
                    maxFileLength = (Int32)tmp;
                Logger.Info("Полученное значение MaxFileLength: " + maxFileLength);

                IFormatProvider culture = new CultureInfo("ru-RU");

                lastSelectDate = Convert.ToDateTime(key.GetValue("LastSelectDate"), culture);
                if ((lastSelectDate == null) || (lastSelectDate == DateTime.MinValue))
                {
                    Logger.Warning("Нет сведений об прошлой выборке сообщений. Инициализируем значением по умолчанию");
                    lastSelectDate = DateTime.Now;
                }
                Logger.Info("Полученное значение LastSelectDate: " + lastSelectDate);


                lastSendDate = Convert.ToDateTime(key.GetValue("LastSendDate"),culture);
                if ((lastSendDate == null) || (lastSendDate == DateTime.MinValue))
                {
                    Logger.Warning("Нет сведений об прошлой отсылке сообщений. Инициализируем значением по умолчанию");
                    lastSendDate = DateTime.Now;
                }
                Logger.Info("Полученное значение LastSendDate: " + lastSendDate);

                nextSendDate = Convert.ToDateTime(key.GetValue("NextSendDate"), culture);
                if ((nextSendDate == null) || (nextSendDate == DateTime.MinValue))
                {
                    Logger.Warning("Нет сведений об дате необходимой отсылки сообщений. Инициализируем значением по умолчанию");
                    nextSendDate = DateTime.Now;
                }
                Logger.Info("Полученное значение NextSendDate: " + nextSendDate);


                tmp = key.GetValue("DeliveryTimeoutCheck");
                if (tmp == null)
                {
                    Logger.Error("ReadSettingsFromRegistry()::Не удалось получить ключ DeliveryTimeoutCheck");
                    return false;
                }
                else
                    deliveryTimeoutCheck = (Int32)tmp;
                Logger.Info("Полученное значение DeliveryTimeoutCheck: " + deliveryTimeoutCheck);

                tmp = key.GetValue("DataSendInterval");
                if (tmp == null)
                {
                    Logger.Error("ReadSettingsFromRegistry()::Не удалось получить ключ DataSendInterval");
                    return false;
                }
                else
                    dataSendInterval = (Int32)tmp;
                Logger.Info("Полученное значение DataSendInterval: " + dataSendInterval);


                tmp = key.GetValue("DaysToDelete");
                if (tmp == null)
                {
                    Logger.Error("ReadSettingsFromRegistry()::Не удалось получить ключ DaysToDelete");
                    return false;
                }
                else
                    daysToDelete = (Int32)tmp;
                Logger.Info("Полученное значение DaysToDelete: " + daysToDelete);


                tmp = key.GetValue("TaskDaysToDelete");
                if (tmp == null)
                {
                    Logger.Warning("ReadSettingsFromRegistry()::Не удалось получить ключ TaskDaysToDelete. Инициализируем по умолчанию");
                    taskDaysToDelete = 180;
                }
                else
                    taskDaysToDelete = (Int32)tmp;
                Logger.Info("Полученное значение TaskDaysToDelete: " + taskDaysToDelete);

                tmp = key.GetValue("ComputerDaysToDelete");
                if (tmp == null)
                {
                    Logger.Warning("ReadSettingsFromRegistry()::Не удалось получить ключ ComputerDaysToDelete. Инициализируем по умолчанию");
                    compDaysToDelete = 0;
                }
                else
                    compDaysToDelete = (Int32)tmp;
                Logger.Info("Полученное значение ComputerDaysToDelete: " + compDaysToDelete);

                tmp = key.GetValue("HourIntervalToSend");
                if (tmp == null)
                {
                    Logger.Warning("ReadSettingsFromRegistry()::Не удалось получить ключ HourIntervalToSend. Инициализируем по умолчанию");
                    hourIntervalToSend = 4;
                }
                else
                    hourIntervalToSend = (Int32)tmp;
                Logger.Info("Полученное значение HourIntervalToSend: " + hourIntervalToSend);

                tmp = key.GetValue("MaintenanceEnabled");
                if (tmp == null)
                {
                    Logger.Warning("ReadSettingsFromRegistry()::Не удалось получить ключ MaintenanceEnabled. Инициализируем по умолчанию.");
                    maintenanceEnabled = true;
                }
                else
                    maintenanceEnabled = Convert.ToBoolean(tmp);
                Logger.Info("Состояние периодического обновления: " + (maintenanceEnabled?"Разрешено":"Запрещено"));

                key.Close();

                Logger.Info("2. Считываем настройки из DataBase");
                key =
                   Registry.LocalMachine.OpenSubKey(registryControlCenterKeyName + "\\DataBase");

                if (key == null)
                {
                    Logger.Error("ReadSettingsFromRegistry():: Не удалось открыть ключ реестра DataBase");
                    return false;
                }

                String dataSource = (String)key.GetValue("DataSource");
                if (dataSource == null)
                {
                    Logger.Error("ReadSettingsFromRegistry()::Не удалось получить ключ DataSource");
                    return false;
                }
                Logger.Info("Полученное значение DataSource: " + dataSource);

                String userName = (String)key.GetValue("UserName");
                if (userName == null)
                {
                    Logger.Error("ReadSettingsFromRegistry()::Не удалось получить ключ UserName");
                    return false;
                }
                Logger.Info("Полученное значение UserName: " + userName);

                Byte[] passBytes = (Byte[])key.GetValue("Password");
                if (passBytes == null)
                {
                    Logger.Error("ReadSettingsFromRegistry()::Не удалось получить ключ Password");
                    return false;
                }
                //Дешифруем пароль
                Int32 length = passBytes.Length;
                for (Int32 i = 0; i < length; ++i)
                {
                    passBytes[i] ^= 0x17;
                }
                String password = System.Text.Encoding.UTF8.GetString(passBytes);

                Logger.Info("Формируем строку подсоединения к БД.");
                connectionString = GenerateConnectionString(dataSource, userName, password);
            }
            catch (Exception ex)
            {
                Logger.Error("Vba32PMS.ReadSettingsFromRegistry()::" + ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Записывает необходимые настройки в реестр
        /// </summary>
        private Boolean WriteSettingsToRegistry()
        {
            Logger.Debug("Vba32PMS.WriteSettingsToRegistry()::Пробуем записать настройки в реестр");
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
                Logger.Error("Vba32PMS.WriteSettingsToRegistry()::" + ex.Message);
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
            Logger.Debug("Vba32PMS.IsReRead():: Вызван");
            Int32 isReRead = 0;
            try
            {
                RegistryKey key =
                        Registry.LocalMachine.OpenSubKey(registryControlCenterKeyName + "\\PeriodicalMaintenance");

                Object tmp = key.GetValue("ReRead");
                if (tmp == null)
                {
                    Logger.Warning("Vba32NS.IsReRead()::Не удалось получить ключ ReRead");
                    return false;
                }
                else
                    isReRead = (int)tmp;

                key.Close();
            }
            catch (Exception ex)
            {
                Logger.Error("Vba32PMS.IsReRead()::" + ex.Message);
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
            Logger.Debug("Vba32PMS.SkipReRead():: Вызван");
            try
            {
                RegistryKey key =
                           Registry.LocalMachine.CreateSubKey(registryControlCenterKeyName + "\\PeriodicalMaintenance");

                key.DeleteValue("Reread");
            }
            catch (Exception ex)
            {
                Logger.Error("Vba32PMS.SkipReRead()::" + ex.Message);
                return false;
            }

            return true;
        }
        #endregion
    }
}