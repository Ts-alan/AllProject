#define DEBUGMESSAGE
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Threading;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Security.Principal;
using System.Security;
using System.Security.AccessControl;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using agsXMPP;
using Vba32.ControlCenter.NotificationService.Network;
using Vba32.ControlCenter.NotificationService.Notification;
using VirusBlokAda.CC.Settings.Entities;
using ARM2_dbcontrol.Service.Vba32NS;

namespace Vba32.ControlCenter.NotificationService
{
    public partial class Vba32NS : ServiceBase
    {
        #region variables

        internal static List<NotifyEvent> evList = new List<NotifyEvent>();

        internal static String path;                            //путь
        internal static String machineName;                     //Имя компа. Под этим именем присылаются на родительский центр событияS

        internal static NSSettingsEntity settingsNS;
        //notify event list
        internal static List<NotifyEvent> list = null;

        private static Int32 xmppLibrary = 0;

        private static JabberClient jclient = null;
        /// <summary>
        /// XMPP library
        /// </summary>
        internal static JabberClient Jclient
        {
            get { return Vba32NS.jclient; }
            set { Vba32NS.jclient = value; }
        }

        #endregion

        #region Constructor
        public Vba32NS()
        {
            InitializeComponent();
            try
            {
                path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\";
                
                list = GetNotifyEventList();
        
                LoggerNS.log.Info("Vba32NS.Vba32NS()::Finished");
            }
            catch (Exception ex)
            {
                LoggerNS.log.Error(ex.Message);
                EventLog.WriteEntry(AppDomain.CurrentDomain.FriendlyName, ex.Message, EventLogEntryType.Error);
            }
        }
        #endregion

        #region OnStart, OnStop
        protected override void OnStart(String[] args)
        {
            LoggerNS.log.Info("Vba32NS.OnStart():: Started");
            
            try
            {
                machineName = Environment.MachineName;

                if (!ReadSettingsFromRegistry())
                    LoggerNS.log.Error("Vba32NS.OnStart()::ReadSettingsFromRegistry returned false");

                if (!String.IsNullOrEmpty(settingsNS.JabberServer))
                    SelectXMPPLibrary();

                //Регистрируем тип для .NET Remoting
                RemotingConfiguration.RegisterWellKnownServiceType(typeof(Vba32NSImplementation),
                    "Vba32NS.rem", WellKnownObjectMode.SingleCall);
                ConfigureIPC();
            }
            catch (Exception ex)
            {
                LoggerNS.log.Error("Vba32NS::OnStart():" + ex.Message);
            }


        }

        protected override void OnStop()
        {
            LoggerNS.log.Info("Vba32NS.OnStop():: Started");
            try
            {
                jclient.CloseConnection();
            }
            catch (Exception ex)
            {
                LoggerNS.log.Error("Vba32NS.OnStop():: " + ex.Message);
            }
            finally
            {
            }
        }
        #endregion

        /// <summary>
        /// Выбирает библиотеку для взаимодействия по протоколу XMPP
        /// </summary>
        public static void SelectXMPPLibrary()
        {
            LoggerNS.log.Info("Vba32NS.SelectXMPPLibrary started");
            LoggerNS.log.Info("xmppLibrary=" + xmppLibrary);
            switch (xmppLibrary)
            {
                case 1:
                    LoggerNS.log.Info("JVlsXMPPClient");
                    jclient = new JVlsXMPPClient(settingsNS.JabberServer, settingsNS.JabberFromJID, settingsNS.JabberPassword);
                    break;
                default:
                    LoggerNS.log.Info("AgsXMPPClient");
                    jclient = new AgsXMPPClient(settingsNS.JabberServer, settingsNS.JabberFromJID, settingsNS.JabberPassword);
                    break;
            }
            
            jclient.OpenConnection();
            LoggerNS.log.Info("Jabber connection state: " + jclient.CheckConnectionState());
        }


        #region IPC

        /// <summary>
        /// Настраивает .NET Remoting на использование IPC-канала
        /// </summary>
        /// <returns></returns>
        private Boolean ConfigureIPC()
        {
            try
            {
                IDictionary props = new Hashtable();
                props["portName"] = "Vba32NS";

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

                if (ChannelServices.GetChannel(channel.ChannelName) == null)
                    ChannelServices.RegisterChannel(channel, false);
            }
            catch (Exception ex)
            {
                LoggerNS.log.Error("Vba32NS::ConfigureIPC():" + ex.Message);
                return false;
            }
            return true;
        }

        #endregion
    }
}
