using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.CC.Settings.Entities;
using Microsoft.Win32;

namespace VirusBlokAda.CC.Settings
{
    public static class SettingsProvider
    {
        private static readonly Object lockToken = new Object();

        private static readonly String RegistryControlCenterKeyName;
        private static readonly String PMSKeyName;
        private static readonly String NSKeyName;
        private static readonly String DBKeyName;

        static SettingsProvider()
        {
            if (System.Runtime.InteropServices.Marshal.SizeOf(typeof(IntPtr)) == 8)
                RegistryControlCenterKeyName = "SOFTWARE\\Wow6432Node\\Vba32\\ControlCenter\\";
            else
                RegistryControlCenterKeyName = "SOFTWARE\\Vba32\\ControlCenter\\";

            PMSKeyName = RegistryControlCenterKeyName + "PeriodicalMaintenance";
            NSKeyName = RegistryControlCenterKeyName + "Notification";
            DBKeyName = RegistryControlCenterKeyName + "DataBase";
        }

        public static PMSSettingsEntity GetPMSSettings()
        {
            lock (lockToken)
            {
                PMSSettingsEntity ent = new PMSSettingsEntity();

                RegistryKey key = Registry.LocalMachine.OpenSubKey(PMSKeyName);

                ent.LastSelectDate = GetDateTime(key, "LastSelectDate");
                ent.LastSendDate = GetDateTime(key, "LastSendDate");
                ent.NextSendDate = GetDateTime(key, "NextSendDate");

                ent.Server = GetString(key, "Server");
                ent.Port = GetNumber(key, "Port");

                ent.DeliveryTimeoutCheck = GetNumber(key, "DeliveryTimeoutCheck");
                ent.MaintenanceEnabled = GetBoolean(key, "MaintenanceEnabled");
                ent.DaysToDelete = GetNumber(key, "DaysToDelete");
                ent.TaskDaysToDelete = GetNumber(key, "TaskDaysToDelete");
                ent.ComputerDaysToDelete = GetNumber(key, "ComputerDaysToDelete");

                ent.DataSendInterval = GetNumber(key, "DataSendInterval");
                ent.HourIntervalToSend = GetNumber(key, "HourIntervalToSend");

                ent.MaxFileLength = GetNumber(key, "MaxFileLength");
                ent.ReRead = GetBoolean(key, "ReRead");

                key.Close();

                return ent;
            }
        }

        private static String GetString(RegistryKey key, String name)
        {
            return (String)key.GetValue(name);
        }

        private static DateTime? GetDateTime(RegistryKey key, String name)
        {
            Object tmp = key.GetValue(name);
            DateTime? dt = null;
            if (tmp != null)
            {
                IFormatProvider culture = new System.Globalization.CultureInfo("ru-RU");
                try
                {
                    dt = Convert.ToDateTime(tmp, culture);
                }
                catch { }

                if (dt == DateTime.MinValue)
                    dt = null;
            }
            return dt;
        }

        private static Int32? GetNumber(RegistryKey key, String name)
        {
            Object tmp = key.GetValue(name);
            Int32? n = null;
            if (tmp != null)
            {
                try
                {
                    n = Convert.ToInt32(tmp);
                }
                catch { }
            }
            return n;
        }

        private static Boolean GetBoolean(RegistryKey key, String name)
        {
            Boolean result;
            Int32? n = GetNumber(key, name);
            if (n == null)
            {
                result = false;
            }
            else
            {
                result = Convert.ToBoolean(n);
            }

            return result;
        }
    }
}
