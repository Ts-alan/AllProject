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
    /// ������ � ������ �������� �� �������
    /// </summary>
    partial class Vba32PMS
    {

        #region ������ ������ �������� �� �������
        /// <summary>
        /// ��������� ����������� ��������� �� �������
        /// </summary>
        private Boolean ReadSettingsFromRegistry()
        {
            Logger.Info("Vba32PMS.ReadSettingsFromRegistry()::������� ������� ��������� �� �������");
            try
            {
              
                Logger.Info("1. ��������� ��������� �� PeriodicalMaintenance");

                RegistryKey key =
                    Registry.LocalMachine.OpenSubKey(registryControlCenterKeyName + "\\PeriodicalMaintenance");
                if (key == null)
                {
                    Logger.Error("ReadSettingsFromRegistry()::�� ������� �������� ���� PeriodicalMaintenance");
                    return false;
                }

                Object t_allowLog = key.GetValue("AllowLog");
                if (t_allowLog == null)
                {
                    this.allowLog = LogLevel.Debug;
                    Logger.Warning("������� ������������ �� ����������");
                }
                else
                {
                    try
                    {
                        this.allowLog = (LogLevel)((Int32)t_allowLog);
                        Logger.Info("������� ������������: " + this.allowLog.ToString());
                    }
                    catch
                    {
                        this.allowLog = LogLevel.Debug;
                        Logger.Warning("������������ ������� ������������");
                    }
                }

                Logger.LoggingLevel = this.allowLog;

                server = (String)key.GetValue("Server");
                if (server == null)
                {
                    Logger.Error("Vba32PMS.ReadSettingsFromRegistry()::�� ������� �������� ���� Server");
                    return false;
                }
                Logger.Info("���������� �������� Server: " + server);

                Object tmp = key.GetValue("Port");
                if (tmp == null)
                {
                    Logger.Error("ReadSettingsFromRegistry()::�� ������� �������� ���� Port");
                    return false;
                }
                else
                    port = (Int32)tmp;
                Logger.Info("���������� �������� Port: " + port);

                tmp = key.GetValue("MaxFileLength");
                if (tmp == null)
                {
                    Logger.Error("ReadSettingsFromRegistry()::�� ������� �������� ���� MaxFileLength");
                    return false;
                }
                else
                    maxFileLength = (Int32)tmp;
                Logger.Info("���������� �������� MaxFileLength: " + maxFileLength);

                IFormatProvider culture = new CultureInfo("ru-RU");

                lastSelectDate = Convert.ToDateTime(key.GetValue("LastSelectDate"), culture);
                if ((lastSelectDate == null) || (lastSelectDate == DateTime.MinValue))
                {
                    Logger.Warning("��� �������� �� ������� ������� ���������. �������������� ��������� �� ���������");
                    lastSelectDate = DateTime.Now;
                }
                Logger.Info("���������� �������� LastSelectDate: " + lastSelectDate);


                lastSendDate = Convert.ToDateTime(key.GetValue("LastSendDate"),culture);
                if ((lastSendDate == null) || (lastSendDate == DateTime.MinValue))
                {
                    Logger.Warning("��� �������� �� ������� ������� ���������. �������������� ��������� �� ���������");
                    lastSendDate = DateTime.Now;
                }
                Logger.Info("���������� �������� LastSendDate: " + lastSendDate);

                nextSendDate = Convert.ToDateTime(key.GetValue("NextSendDate"), culture);
                if ((nextSendDate == null) || (nextSendDate == DateTime.MinValue))
                {
                    Logger.Warning("��� �������� �� ���� ����������� ������� ���������. �������������� ��������� �� ���������");
                    nextSendDate = DateTime.Now;
                }
                Logger.Info("���������� �������� NextSendDate: " + nextSendDate);


                tmp = key.GetValue("DeliveryTimeoutCheck");
                if (tmp == null)
                {
                    Logger.Error("ReadSettingsFromRegistry()::�� ������� �������� ���� DeliveryTimeoutCheck");
                    return false;
                }
                else
                    deliveryTimeoutCheck = (Int32)tmp;
                Logger.Info("���������� �������� DeliveryTimeoutCheck: " + deliveryTimeoutCheck);

                tmp = key.GetValue("DataSendInterval");
                if (tmp == null)
                {
                    Logger.Error("ReadSettingsFromRegistry()::�� ������� �������� ���� DataSendInterval");
                    return false;
                }
                else
                    dataSendInterval = (Int32)tmp;
                Logger.Info("���������� �������� DataSendInterval: " + dataSendInterval);


                tmp = key.GetValue("DaysToDelete");
                if (tmp == null)
                {
                    Logger.Error("ReadSettingsFromRegistry()::�� ������� �������� ���� DaysToDelete");
                    return false;
                }
                else
                    daysToDelete = (Int32)tmp;
                Logger.Info("���������� �������� DaysToDelete: " + daysToDelete);


                tmp = key.GetValue("TaskDaysToDelete");
                if (tmp == null)
                {
                    Logger.Warning("ReadSettingsFromRegistry()::�� ������� �������� ���� TaskDaysToDelete. �������������� �� ���������");
                    taskDaysToDelete = 180;
                }
                else
                    taskDaysToDelete = (Int32)tmp;
                Logger.Info("���������� �������� TaskDaysToDelete: " + taskDaysToDelete);

                tmp = key.GetValue("ComputerDaysToDelete");
                if (tmp == null)
                {
                    Logger.Warning("ReadSettingsFromRegistry()::�� ������� �������� ���� ComputerDaysToDelete. �������������� �� ���������");
                    compDaysToDelete = 0;
                }
                else
                    compDaysToDelete = (Int32)tmp;
                Logger.Info("���������� �������� ComputerDaysToDelete: " + compDaysToDelete);

                tmp = key.GetValue("HourIntervalToSend");
                if (tmp == null)
                {
                    Logger.Warning("ReadSettingsFromRegistry()::�� ������� �������� ���� HourIntervalToSend. �������������� �� ���������");
                    hourIntervalToSend = 4;
                }
                else
                    hourIntervalToSend = (Int32)tmp;
                Logger.Info("���������� �������� HourIntervalToSend: " + hourIntervalToSend);

                tmp = key.GetValue("MaintenanceEnabled");
                if (tmp == null)
                {
                    Logger.Warning("ReadSettingsFromRegistry()::�� ������� �������� ���� MaintenanceEnabled. �������������� �� ���������.");
                    maintenanceEnabled = true;
                }
                else
                    maintenanceEnabled = Convert.ToBoolean(tmp);
                Logger.Info("��������� �������������� ����������: " + (maintenanceEnabled?"���������":"���������"));

                key.Close();

                Logger.Info("2. ��������� ��������� �� DataBase");
                key =
                   Registry.LocalMachine.OpenSubKey(registryControlCenterKeyName + "\\DataBase");

                if (key == null)
                {
                    Logger.Error("ReadSettingsFromRegistry():: �� ������� ������� ���� ������� DataBase");
                    return false;
                }

                String dataSource = (String)key.GetValue("DataSource");
                if (dataSource == null)
                {
                    Logger.Error("ReadSettingsFromRegistry()::�� ������� �������� ���� DataSource");
                    return false;
                }
                Logger.Info("���������� �������� DataSource: " + dataSource);

                String userName = (String)key.GetValue("UserName");
                if (userName == null)
                {
                    Logger.Error("ReadSettingsFromRegistry()::�� ������� �������� ���� UserName");
                    return false;
                }
                Logger.Info("���������� �������� UserName: " + userName);

                Byte[] passBytes = (Byte[])key.GetValue("Password");
                if (passBytes == null)
                {
                    Logger.Error("ReadSettingsFromRegistry()::�� ������� �������� ���� Password");
                    return false;
                }
                //��������� ������
                Int32 length = passBytes.Length;
                for (Int32 i = 0; i < length; ++i)
                {
                    passBytes[i] ^= 0x17;
                }
                String password = System.Text.Encoding.UTF8.GetString(passBytes);

                Logger.Info("��������� ������ ������������� � ��.");
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
        /// ���������� ����������� ��������� � ������
        /// </summary>
        private Boolean WriteSettingsToRegistry()
        {
            Logger.Debug("Vba32PMS.WriteSettingsToRegistry()::������� �������� ��������� � ������");
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
        /// ������������� � ������������� ������� ������ ���������
        /// ���������� ���� ������� IsReRead
        /// </summary>
        /// <returns></returns>
        private Boolean IsReRead()
        {
            Logger.Debug("Vba32PMS.IsReRead():: ������");
            Int32 isReRead = 0;
            try
            {
                RegistryKey key =
                        Registry.LocalMachine.OpenSubKey(registryControlCenterKeyName + "\\PeriodicalMaintenance");

                Object tmp = key.GetValue("ReRead");
                if (tmp == null)
                {
                    Logger.Warning("Vba32NS.IsReRead()::�� ������� �������� ���� ReRead");
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
        /// ������� ReRead ����
        /// </summary>
        /// <returns></returns>
        private Boolean SkipReRead()
        {
            Logger.Debug("Vba32PMS.SkipReRead():: ������");
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