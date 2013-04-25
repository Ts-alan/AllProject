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

using Vba32.ControlCenter.SettingsService.Xml;

namespace Vba32.ControlCenter.SettingsService
{
    public partial class Vba32SettingsService : ServiceBase
    {

        private string path;                            //путь
        private static Logger log;                      //Класс, позволяющий записывать в файл на диске строки-сообщения

        public Vba32SettingsService()
        {
            LogMessage("Vba32SettingsService.Vba32SettingsService():: Вызван");
            InitializeComponent();
            try
            {
                path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\";
                log = new Logger(path + AppDomain.CurrentDomain.FriendlyName + ".log", Encoding.Default, true);
            }
            catch (Exception ex)
            {
                LogError("Vba32SettingsService::OnStart():" + ex.Message,
                    EventLogEntryType.Error);
            }
        }


        #region OnStart&OnStop

        protected override void OnStart(string[] args)
        {
            LogMessage("Vba32SettingsService::OnStart() Вызван");
            try
            {
                //Регистрируем тип для .NET Remoting
                RemotingConfiguration.RegisterWellKnownServiceType(typeof(Vba32SettingsImplementation), 
                    "Vba32SS.rem", WellKnownObjectMode.SingleCall);
                ConfigureIPC();
            }
            catch (Exception ex)
            {
                LogError("Vba32SettingsService::OnStart():" + ex.Message,
                    EventLogEntryType.Error);
            }
        }

        protected override void OnStop()
        {
           LogMessage("Vba32SettingsService::OnStop() Вызван");
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
                LogError("Vba32SettingsService::ConfigureIPC():" + ex.Message,
                  EventLogEntryType.Error);
                return false;
            }
            return true;
        }

        #region Loging
        public static void LogError(string errorMessage, EventLogEntryType eventLogType)
        {
            try
            {
                Debug.WriteLine(errorMessage);
                log.Write(errorMessage);
                EventLog.WriteEntry(AppDomain.CurrentDomain.FriendlyName, errorMessage, eventLogType);
            }
            catch
            {
            }

        }

        public static void LogMessage(string errorMessage)
        {
            try
            {
                Debug.WriteLine(errorMessage);
                log.Write(errorMessage);
            }
            catch
            {
            }
        }
        #endregion



    }
}
