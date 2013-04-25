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
    /// ������ � ������ �������� �� �������
    /// </summary>
    partial class Vba32PMS
    {

        #region ������ ������ �������� �� �������
        /// <summary>
        /// ��������� ����������� ��������� �� �������
        /// </summary>
        private bool ReadSettingsFromRegistry()
        {
            LogMessage("Vba32PMS.ReadSettingsFromRegistry()::������� ������� ��������� �� �������");
            try
            {
              
                LogMessage("1. ��������� ��������� �� PeriodicalMaintenance");

                RegistryKey key =
                    Registry.LocalMachine.OpenSubKey(registryControlCenterKeyName + "\\PeriodicalMaintenance");
                if (key == null)
                {
                    LogError("ReadSettingsFromRegistry()::�� ������� �������� ���� PeriodicalMaintenance",
                        EventLogEntryType.Error);

                    return false;
                }

                object t_allowLog = key.GetValue("AllowLog");
                if (t_allowLog == null)
                {
                    this.allowLog = 0;
                    Debug.Write("������� ������������ �� ����������");
                }
                else
                {
                    this.allowLog = (int)t_allowLog;
                    LogMessage("������� ������������: " + this.allowLog.ToString());
                }


                server = (string)key.GetValue("Server");
                if (server == null)
                {
                    LogError("Vba32PMS.ReadSettingsFromRegistry()::�� ������� �������� ���� Server",
                        EventLogEntryType.Error);

                    return false;
                }
                LogMessage("���������� �������� Server: " + server);

                object tmp = key.GetValue("Port");
                if (tmp == null)
                {
                    LogError("ReadSettingsFromRegistry()::�� ������� �������� ���� Port",
                         EventLogEntryType.Error);

                    return false;
                }
                else
                    port = (int)tmp;
                LogMessage("���������� �������� Port: " + port);

                tmp = key.GetValue("MaxFileLength");
                if (tmp == null)
                {
                    LogError("ReadSettingsFromRegistry()::�� ������� �������� ���� MaxFileLength",
                        EventLogEntryType.Error);
                    return false;
                }
                else
                    maxFileLength = (int)tmp;
                LogMessage("���������� �������� MaxFileLength: " + maxFileLength);

                IFormatProvider culture = new CultureInfo("ru-RU");

                lastSelectDate = Convert.ToDateTime(key.GetValue("LastSelectDate"), culture);
                if ((lastSelectDate == null) || (lastSelectDate == DateTime.MinValue))
                {
                    LogMessage("��� �������� �� ������� ������� ���������. �������������� ��������� �� ���������");
                    lastSelectDate = DateTime.Now;
                }
                LogMessage("���������� �������� LastSelectDate: " + lastSelectDate);


                lastSendDate = Convert.ToDateTime(key.GetValue("LastSendDate"),culture);
                if ((lastSendDate == null) || (lastSendDate == DateTime.MinValue))
                {
                    LogMessage("��� �������� �� ������� ������� ���������. �������������� ��������� �� ���������");
                    lastSendDate = DateTime.Now;
                }
                LogMessage("���������� �������� LastSendDate: " + lastSendDate);

                nextSendDate = Convert.ToDateTime(key.GetValue("NextSendDate"), culture);
                if ((nextSendDate == null) || (nextSendDate == DateTime.MinValue))
                {
                    LogMessage("��� �������� �� ���� ����������� ������� ���������. �������������� ��������� �� ���������");
                    nextSendDate = DateTime.Now;
                }
                LogMessage("���������� �������� NextSendDate: " + nextSendDate);


                tmp = key.GetValue("DeliveryTimeoutCheck");
                if (tmp == null)
                {
                    LogError("ReadSettingsFromRegistry()::�� ������� �������� ���� DeliveryTimeoutCheck",
                        EventLogEntryType.Error);
                    return false;
                }
                else
                    deliveryTimeoutCheck = (int)tmp;
                LogMessage("���������� �������� DeliveryTimeoutCheck: " + deliveryTimeoutCheck);

                tmp = key.GetValue("DataSendInterval");
                if (tmp == null)
                {
                    LogError("ReadSettingsFromRegistry()::�� ������� �������� ���� DataSendInterval",
                            EventLogEntryType.Error);
                    return false;
                }
                else
                    dataSendInterval = (int)tmp;
                LogMessage("���������� �������� DataSendInterval: " + dataSendInterval);


                tmp = key.GetValue("DaysToDelete");
                if (tmp == null)
                {
                    LogError("ReadSettingsFromRegistry()::�� ������� �������� ���� DaysToDelete",
                            EventLogEntryType.Error);
                    return false;
                }
                else
                    daysToDelete = (int)tmp;
                LogMessage("���������� �������� DaysToDelete: " + daysToDelete);


                tmp = key.GetValue("TaskDaysToDelete");
                if (tmp == null)
                {
                    LogError("ReadSettingsFromRegistry()::�� ������� �������� ���� TaskDaysToDelete. �������������� �� ���������",
                            EventLogEntryType.Error);
                    taskDaysToDelete = 180;
                }
                else
                    taskDaysToDelete = (int)tmp;
                LogMessage("���������� �������� TaskDaysToDelete: " + taskDaysToDelete);



                tmp = key.GetValue("HourIntervalToSend");
                if (tmp == null)
                {
                    LogError("ReadSettingsFromRegistry()::�� ������� �������� ���� HourIntervalToSend. �������������� �� ���������",
                            EventLogEntryType.Error);
                    hourIntervalToSend = 4;
                }
                else
                    hourIntervalToSend = (int)tmp;
                LogMessage("���������� �������� HourIntervalToSend: " + hourIntervalToSend);



                tmp = key.GetValue("MaintenanceEnabled");
                if (tmp == null)
                {
                    LogError("ReadSettingsFromRegistry()::�� ������� �������� ���� MaintenanceEnabled. �������������� �� ���������.",
                        EventLogEntryType.Error);

                    maintenanceEnabled = true;
                }
                else
                    maintenanceEnabled = Convert.ToBoolean(tmp);
                LogMessage("��������� �������������� ����������: " + (maintenanceEnabled?"���������":"���������"));

                key.Close();

                LogMessage("2. ��������� ��������� �� DataBase");
                key =
                   Registry.LocalMachine.OpenSubKey(registryControlCenterKeyName + "\\DataBase");

                if (key == null)
                {
                    LogError("ReadSettingsFromRegistry():: �� ������� ������� ���� ������� DataBase",
                        EventLogEntryType.Error);

                    return false;
                }


                string dataSource = (string)key.GetValue("DataSource");
                if (dataSource == null)
                {
                    LogError("ReadSettingsFromRegistry()::�� ������� �������� ���� DataSource",
                        EventLogEntryType.Error);
                    return false;
                }
                LogMessage("���������� �������� DataSource: " + dataSource);

                string userName = (string)key.GetValue("UserName");
                if (userName == null)
                {
                    LogError("ReadSettingsFromRegistry()::�� ������� �������� ���� UserName",
                        EventLogEntryType.Error);

                    return false;
                }
                LogMessage("���������� �������� UserName: " + userName);

                byte[] passBytes = (byte[])key.GetValue("Password");
                if (passBytes == null)
                {
                    LogError("ReadSettingsFromRegistry()::�� ������� �������� ���� Password",
                       EventLogEntryType.Error);

                    return false;
                }
                //��������� ������
                int length = passBytes.Length;
                for (int i = 0; i < length; ++i)
                {
                    passBytes[i] ^= 0x17;
                }
                string password = System.Text.Encoding.UTF8.GetString(passBytes);

                LogMessage("��������� ������ ������������� � ��.");
                connectionString = GenerateConnectionString(dataSource, userName, password);
                //LogMessage("���������� ������ ����������: " + connectionString);

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
        /// ���������� ����������� ��������� � ������
        /// </summary>
        private bool WriteSettingsToRegistry()
        {
            Debug.Write("Vba32PMS.WriteSettingsToRegistry()::������� �������� ��������� � ������");
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
        /// ������������� � ������������� ������� ������ ���������
        /// ���������� ���� ������� IsReRead
        /// </summary>
        /// <returns></returns>
        private bool IsReRead()
        {
            LogMessage("Vba32PMS.IsReRead():: ������");
            int isReRead = 0;
            try
            {
                RegistryKey key =
                        Registry.LocalMachine.OpenSubKey(registryControlCenterKeyName + "\\PeriodicalMaintenance");

                object tmp = key.GetValue("ReRead");
                if (tmp == null)
                {
                    //LogError("Vba32NS.IsReRead()::�� ������� �������� ���� ReRead",
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
        /// ������� ReRead ����
        /// </summary>
        /// <returns></returns>
        private bool SkipReRead()
        {
            LogMessage("Vba32PMS.SkipReRead():: ������");
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