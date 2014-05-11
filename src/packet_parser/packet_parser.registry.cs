using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using VirusBlokAda.CC.Common;

namespace Vba32CC
{
    public partial class PacketParser
    {
        #region Registry Reading Functions

        private String gc_database_regkey;
        private String registryControlCenterKeyName;     //путь к настройкам центра управления 

        private String ReadRegistryValue(RegistryKey registry_key, String sub_key, String var_name)
        {
            String result = "";
            RegistryKey reg_key = registry_key;
            try
            {
                reg_key = reg_key.OpenSubKey(sub_key);
                result = reg_key.GetValue(var_name).ToString();
            }
            catch (Exception e)
            {
                m_error_info = e.Message;
            }
            return result;
        }

        private String DecryptBinaryToString(RegistryKey registry_key, String sub_key, String var_name)
        {
            RegistryKey reg_key = registry_key;
            String password = "";
            try
            {
                reg_key = reg_key.OpenSubKey(sub_key);
                Byte[] buffer = (Byte[])reg_key.GetValue(var_name);
                int buffer_length = buffer.Length;
                for (int i = 0; i < buffer_length; ++i)
                {
                    buffer[i] ^= 0x17;
                }
                password = System.Text.Encoding.UTF8.GetString(buffer);
            }
            catch (Exception e)
            {
                m_error_info = e.Message;
            }
            return password;
        }

        private String ReadServerName()
        {
            return ReadRegistryValue(Registry.LocalMachine, gc_database_regkey, "DataSource");
        }

        private String ReadUserName()
        {
            return ReadRegistryValue(Registry.LocalMachine, gc_database_regkey, "UserName");
        }

        private String ReadPassword()
        {
            return DecryptBinaryToString(Registry.LocalMachine, gc_database_regkey, "Password");
        }

        /// <summary>
        /// Считывает необходимые настройки из реестра
        /// </summary>
        private Boolean ReadSettingsFromRegistry()
        {
            LoggerPP.log.Info("Vba32SS.ReadSettingsFromRegistry():: Try read settings from registry.");
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(registryControlCenterKeyName);
                if (key == null)
                {
                    LoggerPP.log.Error("ReadSettingsFromRegistry()::Can't get key 'ControlCenter'.");
                    return false;
                }

                Object t_allowLog = key.GetValue("AllowLog");
                if (t_allowLog == null)
                {
                    LoggerPP.Level = LogLevel.Debug;
                    LoggerPP.log.Warning("Log level isn't set.");
                }
                else
                {
                    try
                    {
                        LoggerPP.Level = (LogLevel)((Int32)t_allowLog);
                        LoggerPP.log.Info("Log level: " + LoggerPP.Level.ToString());
                    }
                    catch
                    {
                        LoggerPP.Level = LogLevel.Debug;
                        LoggerPP.log.Warning("Inadmissible log level.");
                    }
                }

                LoggerPP.log.LoggingLevel = LoggerPP.Level;

                key.Close();
            }
            catch (Exception ex)
            {
                LoggerPP.log.Error("Vba32SS.ReadSettingsFromRegistry()::" + ex.Message);
                return false;
            }
            return true;
        }

        #endregion
    }
}
