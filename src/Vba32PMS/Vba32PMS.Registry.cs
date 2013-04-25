using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Text;

using System.Net;
using System.Globalization;

using Microsoft.Win32;

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
        private bool ReadSettingsFromRegistry()
        {
            LogMessage("Vba32PMS.ReadSettingsFromRegistry()::Пробуем считать настройки из реестра");
            try
            {
              
                LogMessage("1. Считываем настройки из PeriodicalMaintenance");

                RegistryKey key =
                    Registry.LocalMachine.OpenSubKey(registryControlCenterKeyName + "\\PeriodicalMaintenance");
                if (key == null)
                {
                    LogError("ReadSettingsFromRegistry()::Не удалось получить ключ PeriodicalMaintenance",
                        EventLogEntryType.Error);

                    return false;
                }

                object t_allowLog = key.GetValue("AllowLog");
                if (t_allowLog == null)
                {
                    this.allowLog = 0;
                    Debug.Write("Уровень логгирования не установлен");
                }
                else
                {
                    this.allowLog = (int)t_allowLog;
                    LogMessage("Уровень логгирования: " + this.allowLog.ToString());
                }


                server = (string)key.GetValue("Server");
                if (server == null)
                {
                    LogError("Vba32PMS.ReadSettingsFromRegistry()::Не удалось получить ключ Server",
                        EventLogEntryType.Error);

                    return false;
                }
                LogMessage("Полученное значение Server: " + server);

                object tmp = key.GetValue("Port");
                if (tmp == null)
                {
                    LogError("ReadSettingsFromRegistry()::Не удалось получить ключ Port",
                         EventLogEntryType.Error);

                    return false;
                }
                else
                    port = (int)tmp;
                LogMessage("Полученное значение Port: " + port);

                tmp = key.GetValue("MaxFileLength");
                if (tmp == null)
                {
                    LogError("ReadSettingsFromRegistry()::Не удалось получить ключ MaxFileLength",
                        EventLogEntryType.Error);
                    return false;
                }
                else
                    maxFileLength = (int)tmp;
                LogMessage("Полученное значение MaxFileLength: " + maxFileLength);

                IFormatProvider culture = new CultureInfo("ru-RU");

                lastSelectDate = Convert.ToDateTime(key.GetValue("LastSelectDate"), culture);
                if ((lastSelectDate == null) || (lastSelectDate == DateTime.MinValue))
                {
                    LogMessage("Нет сведений об прошлой выборке сообщений. Инициализируем значением по умолчанию");
                    lastSelectDate = DateTime.Now;
                }
                LogMessage("Полученное значение LastSelectDate: " + lastSelectDate);


                lastSendDate = Convert.ToDateTime(key.GetValue("LastSendDate"),culture);
                if ((lastSendDate == null) || (lastSendDate == DateTime.MinValue))
                {
                    LogMessage("Нет сведений об прошлой отсылке сообщений. Инициализируем значением по умолчанию");
                    lastSendDate = DateTime.Now;
                }
                LogMessage("Полученное значение LastSendDate: " + lastSendDate);

                nextSendDate = Convert.ToDateTime(key.GetValue("NextSendDate"), culture);
                if ((nextSendDate == null) || (nextSendDate == DateTime.MinValue))
                {
                    LogMessage("Нет сведений об дате необходимой отсылки сообщений. Инициализируем значением по умолчанию");
                    nextSendDate = DateTime.Now;
                }
                LogMessage("Полученное значение NextSendDate: " + nextSendDate);


                tmp = key.GetValue("DeliveryTimeoutCheck");
                if (tmp == null)
                {
                    LogError("ReadSettingsFromRegistry()::Не удалось получить ключ DeliveryTimeoutCheck",
                        EventLogEntryType.Error);
                    return false;
                }
                else
                    deliveryTimeoutCheck = (int)tmp;
                LogMessage("Полученное значение DeliveryTimeoutCheck: " + deliveryTimeoutCheck);

                tmp = key.GetValue("DataSendInterval");
                if (tmp == null)
                {
                    LogError("ReadSettingsFromRegistry()::Не удалось получить ключ DataSendInterval",
                            EventLogEntryType.Error);
                    return false;
                }
                else
                    dataSendInterval = (int)tmp;
                LogMessage("Полученное значение DataSendInterval: " + dataSendInterval);


                tmp = key.GetValue("DaysToDelete");
                if (tmp == null)
                {
                    LogError("ReadSettingsFromRegistry()::Не удалось получить ключ DaysToDelete",
                            EventLogEntryType.Error);
                    return false;
                }
                else
                    daysToDelete = (int)tmp;
                LogMessage("Полученное значение DaysToDelete: " + daysToDelete);


                tmp = key.GetValue("TaskDaysToDelete");
                if (tmp == null)
                {
                    LogError("ReadSettingsFromRegistry()::Не удалось получить ключ TaskDaysToDelete. Инициализируем по умолчанию",
                            EventLogEntryType.Error);
                    taskDaysToDelete = 180;
                }
                else
                    taskDaysToDelete = (int)tmp;
                LogMessage("Полученное значение TaskDaysToDelete: " + taskDaysToDelete);



                tmp = key.GetValue("HourIntervalToSend");
                if (tmp == null)
                {
                    LogError("ReadSettingsFromRegistry()::Не удалось получить ключ HourIntervalToSend. Инициализируем по умолчанию",
                            EventLogEntryType.Error);
                    hourIntervalToSend = 4;
                }
                else
                    hourIntervalToSend = (int)tmp;
                LogMessage("Полученное значение HourIntervalToSend: " + hourIntervalToSend);



                tmp = key.GetValue("MaintenanceEnabled");
                if (tmp == null)
                {
                    LogError("ReadSettingsFromRegistry()::Не удалось получить ключ MaintenanceEnabled. Инициализируем по умолчанию.",
                        EventLogEntryType.Error);

                    maintenanceEnabled = true;
                }
                else
                    maintenanceEnabled = Convert.ToBoolean(tmp);
                LogMessage("Состояние периодического обновления: " + (maintenanceEnabled?"Разрешено":"Запрещено"));

                key.Close();

                LogMessage("2. Считываем настройки из DataBase");
                key =
                   Registry.LocalMachine.OpenSubKey(registryControlCenterKeyName + "\\DataBase");

                if (key == null)
                {
                    LogError("ReadSettingsFromRegistry():: Не удалось открыть ключ реестра DataBase",
                        EventLogEntryType.Error);

                    return false;
                }


                string dataSource = (string)key.GetValue("DataSource");
                if (dataSource == null)
                {
                    LogError("ReadSettingsFromRegistry()::Не удалось получить ключ DataSource",
                        EventLogEntryType.Error);
                    return false;
                }
                LogMessage("Полученное значение DataSource: " + dataSource);

                string userName = (string)key.GetValue("UserName");
                if (userName == null)
                {
                    LogError("ReadSettingsFromRegistry()::Не удалось получить ключ UserName",
                        EventLogEntryType.Error);

                    return false;
                }
                LogMessage("Полученное значение UserName: " + userName);

                byte[] passBytes = (byte[])key.GetValue("Password");
                if (passBytes == null)
                {
                    LogError("ReadSettingsFromRegistry()::Не удалось получить ключ Password",
                       EventLogEntryType.Error);

                    return false;
                }
                //Дешифруем пароль
                int length = passBytes.Length;
                for (int i = 0; i < length; ++i)
                {
                    passBytes[i] ^= 0x17;
                }
                string password = System.Text.Encoding.UTF8.GetString(passBytes);

                LogMessage("Формируем строку подсоединения к БД.");
                connectionString = GenerateConnectionString(dataSource, userName, password);
                //LogMessage("Полученная строка соединения: " + connectionString);

            }

            catch (Exception ex)
            {
                LogError("Vba32PMS.ReadSettingsFromRegistry()::" + ex.Message,
                           EventLogEntryType.Error);
                
                return false;
            }
            return true;
        }

        /// <summary>
        /// Записывает необходимые настройки в реестр
        /// </summary>
        private bool WriteSettingsToRegistry()
        {
            Debug.Write("Vba32PMS.WriteSettingsToRegistry()::Пробуем записать настройки в реестр");
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
                LogError("Vba32PMS.WriteSettingsToRegistry()::" + ex.Message,
                           EventLogEntryType.Error);

                return false;
            }
            return true;
        }

        /// <summary>
        /// Сигнализирует о необходимости считать заново настройки
        /// Использует ключ реестра IsReRead
        /// </summary>
        /// <returns></returns>
        private bool IsReRead()
        {
            LogMessage("Vba32PMS.IsReRead():: Вызван");
            int isReRead = 0;
            try
            {
                RegistryKey key =
                        Registry.LocalMachine.OpenSubKey(registryControlCenterKeyName + "\\PeriodicalMaintenance");

                object tmp = key.GetValue("ReRead");
                if (tmp == null)
                {
                    //LogError("Vba32NS.IsReRead()::Не удалось получить ключ ReRead",
                    //     EventLogEntryType.Error);
                    return false;
                }
                else
                    isReRead = (int)tmp;

                key.Close();
            }
            catch (Exception ex)
            {
                LogError("Vba32PMS.IsReRead()::" + ex.Message, EventLogEntryType.Error);
                return false;
            }
            return (isReRead > 0 ? true : false);
        }

        /// <summary>
        /// Удаляет ReRead ключ
        /// </summary>
        /// <returns></returns>
        private bool SkipReRead()
        {
            LogMessage("Vba32PMS.SkipReRead():: Вызван");
            try
            {
                RegistryKey key =
                           Registry.LocalMachine.CreateSubKey(registryControlCenterKeyName + "\\PeriodicalMaintenance");

                key.DeleteValue("Reread");
            }
            catch (Exception ex)
            {
                LogError("Vba32PMS.SkipReRead()::" + ex.Message, EventLogEntryType.Error);
                return false;
            }

            return true;
        }
        #endregion
    }
}