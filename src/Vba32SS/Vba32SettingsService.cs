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

namespace Vba32.ControlCenter.SettingsService
{
    public partial class Vba32SettingsService : ServiceBase
    {

        private string path;                            //путь

        public Vba32SettingsService()
        {
            LoggerSS.log.Info("Vba32SettingsService.Vba32SettingsService():: Enter.");
            InitializeComponent();
            try
            {
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
    }
}
