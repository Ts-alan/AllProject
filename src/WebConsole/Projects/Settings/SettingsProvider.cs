using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.CC.Settings.Entities;
using Microsoft.Win32;
using VirusBlokAda.CC.Common;
using System.Xml;

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

        #region Methods

        /// <summary>
        /// Считывает настройки уровня логгирования
        /// </summary>
        public static LogLevel GetLogLevel()
        {
            lock (lockToken)
            {
                LogLevel level = LogLevel.Info;
                RegistryKey key = null;
                try
                {
                    key = Registry.LocalMachine.OpenSubKey(RegistryControlCenterKeyName);
                    if (key == null)
                    {
                        return level;
                    }

                    Object t_allowLog = key.GetValue("AllowLog");
                    if (t_allowLog != null)
                    {
                        level = (LogLevel)((Int32)t_allowLog);
                    }
                }
                catch { }
                finally
                {
                    if (key != null)
                        key.Close();
                }

                return level;
            }
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

        #region Reading registry data

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

        #endregion

        #region  Create keys to registry

        /// <summary>
        /// Создает или удаляет ключ реестра
        /// </summary>
        /// <param name="key">путь</param>
        /// <param name="name">имя ключа</param>
        /// <param name="mode">режим</param>
        /// <returns></returns>
        private static Boolean ChangeKey(String key, String name, String mode)
        {
            lock (lockToken)
            {
                RegistryKey regkey = null;
                try
                {
                    regkey = Registry.LocalMachine.CreateSubKey(key);
                    switch (mode)
                    {
                        case "create":
                            regkey.CreateSubKey(name);
                            break;
                        case "delete":
                            regkey.DeleteSubKey(name, false);
                            break;
                    }                 
                }
                catch
                {
                    return false;
                }
                finally
                {
                    if (regkey != null)
                        regkey.Close();
                }

                return true;
            }
        }

        /// <summary>
        /// Изменяет параметр реестра
        /// </summary>
        /// <param name="key">путь</param>
        /// <param name="type">тип параметра</param>
        /// <param name="name">имя</param>
        /// <param name="value">значение</param>
        /// <returns></returns>
        private static Boolean ChangeValue(String key, String type, String name, String value)
        {
            lock (lockToken)
            {
                RegistryKey regkey = null;
                try
                {
                    regkey = Registry.LocalMachine.CreateSubKey(key);

                    switch (type)
                    {
                        case "reg_sz":
                            regkey.SetValue(name, value, RegistryValueKind.String);
                            break;

                        case "reg_dword":
                            regkey.SetValue(name, Convert.ToUInt32(value), RegistryValueKind.DWord);
                            break;

                        case "reg_binary":
                            regkey.SetValue(name, Encoding.Default.GetBytes(value), RegistryValueKind.Binary);
                            break;

                        case "delete":
                            regkey.DeleteValue(name, false);
                            break;
                    }
                }
                catch
                {
                    return false;
                }
                finally
                {
                    if (regkey != null)
                        regkey.Close();
                }

                return true;
            }
        }

        #endregion

        #region Parse Xml

        public static void WriteSettings(String xml)
        {
            lock (lockToken)
            {
                if ((xml[0] != '<') && (xml[1] != '?'))
                    xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" + xml;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                XmlNode root = doc.SelectSingleNode("VbaSettings");

                ReverseNode(root, RegistryControlCenterKeyName);
            }
        }

        private static void ReverseNode(XmlNode root, String key)
        {
            if (root.Name != "VbaSettings")
                key += root.Name + "\\";

            foreach (XmlNode node in root.ChildNodes)
            {
                if ((node.HasChildNodes) && (node.ChildNodes[0].NodeType != XmlNodeType.Text))
                {
                    ReverseNode(node, key);
                }
                else
                {
                    String attrName = String.Empty;
                    String attrValue = String.Empty;
                    if (node.Attributes != null)
                        foreach (XmlAttribute attr in node.Attributes)
                        {
                            attrName = attr.Name.ToLower();
                            attrValue = attr.Value.ToLower();
                        }

                    switch (attrName)
                    {
                        case "type":
                            //Это value
                            ChangeValue(key, attrValue, node.Name, node.InnerText);
                            break;

                        case "delete":
                            //Это key для уделения
                            if (attrValue == "true")
                                ChangeKey(key, node.Name, "delete");
                            break;

                        case "":
                            //Это key для создания
                            ChangeKey(key, node.Name, "create");
                            break;
                    }
                }
            }
        }

        #endregion

        #endregion
    }
}