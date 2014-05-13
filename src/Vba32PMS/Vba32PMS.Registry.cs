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
using VirusBlokAda.CC.Settings;
using VirusBlokAda.CC.Settings.Entities;

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
                LoggerPMS.log.Info("1. Read LogLevel settings.");

                LoggerPMS.Level = SettingsProvider.GetLogLevel();
                LoggerPMS.log.Info("Log level: " + LoggerPMS.Level.ToString());

                LoggerPMS.log.Info("2. Read settings from 'PeriodicalMaintenance'.");

                settingsPMS = SettingsProvider.GetPMSSettings();
                
                LoggerPMS.log.Info("Get value 'Server': " + settingsPMS.Server);
                LoggerPMS.log.Info("Get value 'Port': " + settingsPMS.Port);
                LoggerPMS.log.Info("Get value 'MaxFileLength': " + settingsPMS.MaxFileLength);
                LoggerPMS.log.Info("Get value 'LastSelectDate': " + settingsPMS.LastSelectDate);
                LoggerPMS.log.Info("Get value 'LastSendDate': " + settingsPMS.LastSendDate);
                LoggerPMS.log.Info("Get value 'NextSendDate': " + settingsPMS.NextSendDate);
                LoggerPMS.log.Info("Get value 'DeliveryTimeoutCheck': " + settingsPMS.DeliveryTimeoutCheck);
                LoggerPMS.log.Info("Get value 'DataSendInterval': " + settingsPMS.DataSendInterval);
                LoggerPMS.log.Info("Get value 'DaysToDelete': " + settingsPMS.DaysToDelete);
                LoggerPMS.log.Info("Get value 'TaskDaysToDelete': " + settingsPMS.TaskDaysToDelete);
                LoggerPMS.log.Info("Get value 'ComputerDaysToDelete': " + settingsPMS.ComputerDaysToDelete);
                LoggerPMS.log.Info("Get value 'HourIntervalToSend': " + settingsPMS.HourIntervalToSend);
                LoggerPMS.log.Info("Periodically update state: " + (settingsPMS.MaintenanceEnabled ? "Allowed" : "Denied"));

                if (settingsPMS.MaintenanceEnabled)
                {
                    if (String.IsNullOrEmpty(settingsPMS.Server))
                        return false;
                }

                if (!settingsPMS.MaxFileLength.HasValue)
                    settingsPMS.MaxFileLength = 8000666;
                if (!settingsPMS.LastSelectDate.HasValue)
                    settingsPMS.LastSelectDate = DateTime.Now;
                if (!settingsPMS.LastSendDate.HasValue)
                    settingsPMS.LastSendDate = DateTime.Now;
                if (!settingsPMS.NextSendDate.HasValue)
                    settingsPMS.NextSendDate = DateTime.Now;
                if (!settingsPMS.HourIntervalToSend.HasValue)
                    settingsPMS.HourIntervalToSend = 4;
                if (!settingsPMS.ComputerDaysToDelete.HasValue)
                    settingsPMS.ComputerDaysToDelete = 0;
                if (!settingsPMS.TaskDaysToDelete.HasValue)
                    settingsPMS.TaskDaysToDelete = 180;
                if (!settingsPMS.DaysToDelete.HasValue)
                    settingsPMS.DaysToDelete = 180;
                if (!settingsPMS.DeliveryTimeoutCheck.HasValue)
                    settingsPMS.DeliveryTimeoutCheck = 60;
                if (!settingsPMS.Port.HasValue)
                    settingsPMS.Port = 17001;
                
                LoggerPMS.log.Info("3. Read settings from DataBase");
                connectionString = SettingsProvider.GetConnectionString();
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
                PMSSettingsEntity ent = new PMSSettingsEntity();
                ent.LastSelectDate = settingsPMS.LastSelectDate;
                ent.LastSendDate = settingsPMS.LastSendDate;
                ent.NextSendDate = settingsPMS.NextSendDate;
                ent.MaintenanceEnabled = settingsPMS.MaintenanceEnabled;

                SettingsProvider.WriteSettings(ent.GenerateXML());
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
            try
            {
                return SettingsProvider.GetReRead_PMS();
            }
            catch (Exception ex)
            {
                LoggerPMS.log.Error("Vba32PMS.IsReRead()::" + ex.Message);
                return false;
            }
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
                SettingsProvider.SkipReRead_PMS();                
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