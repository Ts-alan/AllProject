using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;

using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Security.Principal;
using System.Security;
using System.Security.AccessControl;
using System.Collections;
using Microsoft.Win32;
using VirusBlokAda.CC.Common;

namespace Vba32.ControlCenter.SettingsService
{
    public partial class Vba32SettingsService : ServiceBase
    {
        private String path;                            //путь
        private String registryControlCenterKeyName;     //путь к настройкам центра управления 

        public Vba32SettingsService()
        {
            LoggerSS.log.Info("Vba32SettingsService.Vba32SettingsService():: Enter.");
            InitializeComponent();
            try
            {
                //Проверка на битность ОС
                if (System.Runtime.InteropServices.Marshal.SizeOf(typeof(IntPtr)) == 8)
                    registryControlCenterKeyName = "SOFTWARE\\Wow6432Node\\Vba32\\ControlCenter\\";
                else
                    registryControlCenterKeyName = "SOFTWARE\\Vba32\\ControlCenter\\";

                path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\";
            }
            catch (Exception ex)
            {
                LoggerSS.log.Error("Vba32SettingsService::OnStart():" + ex.Message);
            }
        }


        #region OnStart&OnStop

        protected override void OnStart(string[] args)
        {
            LoggerSS.log.Info("Vba32SettingsService::OnStart() Enter.");
            try
            {
                ReadSettingsFromRegistry();
                //Регистрируем тип для .NET Remoting
                RemotingConfiguration.RegisterWellKnownServiceType(typeof(Vba32SettingsImplementation), 
                    "Vba32SS.rem", WellKnownObjectMode.SingleCall);
                ConfigureIPC();
            }
            catch (Exception ex)
            {
                LoggerSS.log.Error("Vba32SettingsService::OnStart():" + ex.Message);
            }
        }

        protected override void OnStop()
        {
           LoggerSS.log.Info("Vba32SettingsService::OnStop() Enter.");
       }

        #endregion

       /// <summary>
        /// Настраивает .NET Remoting на использование IPC-канала
        /// </summary>
        /// <returns></returns>
        private bool ConfigureIPC()
        {
            try
            {
                IDictionary props = new Hashtable();
                props["portName"] = "Vba32SS";

                // Local administrators sid
                SecurityIdentifier localAdminSid = new SecurityIdentifier(
                    WellKnownSidType.BuiltinAdministratorsSid, null);

                SecurityIdentifier allSid = new SecurityIdentifier(
                    WellKnownSidType.WorldSid, null);

                DiscretionaryAcl dacl = new DiscretionaryAcl(false, false, 1);

                // Allow acces only from local administrators and power users
                dacl.AddAccess(AccessControlType.Allow, localAdminSid, -1,
                    InheritanceFlags.None, PropagationFlags.None);
                dacl.AddAccess(AccessControlType.Allow, allSid, -1,
                    InheritanceFlags.None, PropagationFlags.None);

                CommonSecurityDescriptor securityDescriptor =
                    new CommonSecurityDescriptor(false, false,
                            ControlFlags.GroupDefaulted |
                            ControlFlags.OwnerDefaulted |
                            ControlFlags.DiscretionaryAclPresent,
                            null, null, null, dacl);

                IpcServerChannel channel = new IpcServerChannel(
                                                        props,
                                                        null,
                                                        securityDescriptor);
                if(ChannelServices.GetChannel(channel.ChannelName)==null)
                    ChannelServices.RegisterChannel(channel, false);

            }
            catch(Exception ex)
            {
                LoggerSS.log.Error("Vba32SettingsService::ConfigureIPC():" + ex.Message);
                return false;
            }
            return true;
        }

        #region Чтение запись настроек из реестра
        /// <summary>
        /// Считывает необходимые настройки из реестра
        /// </summary>
        private Boolean ReadSettingsFromRegistry()
        {
            LoggerSS.log.Info("Vba32SS.ReadSettingsFromRegistry():: Try read settings from registry.");
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(registryControlCenterKeyName);
                if (key == null)
                {
                    LoggerSS.log.Error("ReadSettingsFromRegistry()::Can't get key 'ControlCenter'.");
                    return false;
                }

                Object t_allowLog = key.GetValue("AllowLog");
                if (t_allowLog == null)
                {
                    LoggerSS.Level = LogLevel.Debug;
                    LoggerSS.log.Warning("Log level isn't set.");
                }
                else
                {
                    try
                    {
                        LoggerSS.Level = (LogLevel)((Int32)t_allowLog);
                        LoggerSS.log.Info("Log level: " + LoggerSS.Level.ToString());
                    }
                    catch
                    {
                        LoggerSS.Level = LogLevel.Debug;
                        LoggerSS.log.Warning("Inadmissible log level.");
                    }
                }

                LoggerSS.log.LoggingLevel = LoggerSS.Level;

                key.Close();
            }
            catch (Exception ex)
            {
                LoggerSS.log.Error("Vba32SS.ReadSettingsFromRegistry()::" + ex.Message);
                return false;
            }
            return true;
        }

        #endregion
    }
}
