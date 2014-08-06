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

        #region ConnectionString

        /// <summary>
        /// Get connectionString
        /// </summary>
        /// <returns></returns>
        public static String GetConnectionString()
        {
            lock (lockToken)
            {
                RegistryKey key = null;
                try
                {
                    key = Registry.LocalMachine.OpenSubKey(DBKeyName);
                    return GenerateConnectionString(ReadServerName(key), ReadUserName(key), ReadPassword(key));
                }
                finally
                {
                    if (key != null)
                        key.Close();
                }
            }
        }

        /// <summary>
        /// Генерирует строку подключения к базе данных
        /// </summary>
        /// <param name="server">Сервер БД</param>
        /// <param name="user">Пользователь БД</param>
        /// <param name="password">Пароль пользователя</param>
        /// <returns>Строка подключения</returns>
        private static String GenerateConnectionString(String server, String user, String password)
        {
            System.Data.SqlClient.SqlConnectionStringBuilder connStr = new System.Data.SqlClient.SqlConnectionStringBuilder();
            connStr.UserID = user;
            connStr.Password = password;
            connStr.DataSource = server;
            connStr.PersistSecurityInfo = false;
            connStr.InitialCatalog = "vbaControlCenterDB";
            return connStr.ConnectionString;
        }

        private static String DecryptBinaryToString(Byte[] data)
        {
            Int32 buffer_length = data.Length;
            for (Int32 i = 0; i < buffer_length; ++i)
            {
                data[i] ^= 0x17;
            }
            return System.Text.Encoding.UTF8.GetString(data);
        }

        private static String ReadServerName(RegistryKey key)
        {
            return GetString(key, "DataSource");
        }

        private static String ReadUserName(RegistryKey key)
        {
            return GetString(key, "UserName");
        }

        private static String ReadPassword(RegistryKey key)
        {
            return DecryptBinaryToString(GetBinary(key, "Password"));
        }

        #endregion

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

        public static String GetDefaultPolicy()
        {
            lock (lockToken)
            {
                RegistryKey key = null;
                try
                {
                    key = Registry.LocalMachine.OpenSubKey(RegistryControlCenterKeyName);
                    return GetString(key, "DefaultPolicy");
                }
                catch
                {
                    return String.Empty;
                }
                finally
                {
                    if (key != null)
                        key.Close();
                }
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

        public static Boolean GetReRead(SettingTypes type)
        {
            switch (type)
            {
                case SettingTypes.PMS:
                    return GetReReadKey(PMSKeyName);
                case SettingTypes.NS:
                    return GetReReadKey(NSKeyName);
                default:
                    return false;
            }
        }

        public static void SkipReRead(SettingTypes type)
        {
            switch (type)
            {
                case SettingTypes.PMS:
                    SkipReReadKey(PMSKeyName);
                    break;
                case SettingTypes.NS:
                    SkipReReadKey(NSKeyName);
                    break;
            }
        }

        public static NSSettingsEntity GetNSSettings()
        {
            lock (lockToken)
            {
                NSSettingsEntity ent = new NSSettingsEntity();

                RegistryKey key = Registry.LocalMachine.OpenSubKey(NSKeyName);

                ent.XMPPLibrary = GetBoolean(key, "XMPPLibrary");
                ent.JabberServer = GetString(key, "JabberServer");
                ent.JabberFromJID = GetString(key, "JabberFromJID");
                ent.JabberPassword = GetString(key, "JabberPassword");

                ent.MailServer = GetString(key, "MailServer");
                ent.MailFrom = GetString(key, "MailFrom");
                ent.MailDisplayName = GetString(key, "MailDisplayName");
                ent.MailUsername = GetString(key, "MailUsername");
                ent.UseMailAuthorization = !String.IsNullOrEmpty(ent.MailUsername);
                ent.MailPassword = GetString(key, "MailPassword");                    

                ent.GlobalEpidemyLimit=GetNumber(key,"GlobalEpidemyLimit");
                ent.GlobalEpidemyTimeLimit=GetNumber(key,"GlobalEpidemyTimeLimit");
                ent.GlobalEpidemyCompCount=GetNumber(key,"GlobalEpidemyCompCount");
                ent.LocalHearthLimit=GetNumber(key,"LocalHearthLimit");
                ent.LocalHearthTimeLimit=GetNumber(key,"LocalHearthTimeLimit");
                ent.Limit=GetNumber(key,"Limit");
                ent.TimeLimit=GetNumber(key,"TimeLimit");
                ent.UseFlowAnalysis=GetNumber(key,"UseFlowAnalysis");
                
                ent.ReRead = GetBoolean(key, "ReRead");

                key.Close();

                return ent;
            }
        }

        private static Boolean GetReReadKey(String keyName)
        {
            lock (lockToken)
            {
                RegistryKey key = null;
                try
                {
                    key = Registry.LocalMachine.OpenSubKey(keyName);
                    return GetBoolean(key, "ReRead");
                }
                finally
                {
                    if (key != null)
                        key.Close();
                }
            }
        }

        private static void SkipReReadKey(String keyName)
        {
            lock (lockToken)
            {
                RegistryKey key = null;
                try
                {
                    key = Registry.LocalMachine.OpenSubKey(keyName);
                    key.DeleteValue("ReRead");
                }
                finally
                {
                    if (key != null)
                        key.Close();
                }
            }
        }

        #region Reading registry data

        private static String GetString(RegistryKey key, String name)
        {
            return (String)key.GetValue(name);
        }

        private static Byte[] GetBinary(RegistryKey key, String name)
        {
            return (Byte[])key.GetValue(name);
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
            if (root.Name != "VbaSettings" && root.Name != "ControlCenter")
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