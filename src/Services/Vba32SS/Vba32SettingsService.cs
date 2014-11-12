using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceProcess;
using System.Runtime.Remoting;
using System.Collections;
using System.Security.Principal;
using System.Security.AccessControl;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Remoting.Channels;

namespace Vba32.ControlCenter.SettingsService
{
    public partial class Vba32SettingsService : ServiceBase
    {
        public Vba32SettingsService()
        {
            InitializeComponent();
        }


        #region OnStart&OnStop

        protected override void OnStart(String[] args)
        {
            LoggerSS.Level = VirusBlokAda.CC.Settings.SettingsProvider.GetLogLevel();
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
        private Boolean ConfigureIPC()
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
