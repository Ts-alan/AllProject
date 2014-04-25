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

namespace Vba32.ControlCenter.NotificationService
{
    public partial class Vba32NS : ServiceBase
    {
        #region variables

        internal static List<NotifyEvent> evList = new List<NotifyEvent>();

        internal static string path;                            //путь
        internal static string machineName;                     //Имя компа. Под этим именем присылаются на родительский центр событияS
        
        //jabber
        internal static string jabberServer = String.Empty;
        internal static string jabberFromJID = String.Empty;
        internal static string jabberPassword = String.Empty;
        //mail
        internal static string mailServer = String.Empty;
        internal static string mailFrom = String.Empty;
        internal static string mailDisplayName = String.Empty;
        //registry
        internal static string registryControlCenterKeyName;
        //loging level
        internal static int logLevel = 0;
        //notify event list
        internal static List<NotifyEvent> list = null;

        private static int xmppLibrary = 0;

        private static JabberClient jclient = null;
        /// <summary>
        /// XMPP library
        /// </summary>
        internal static JabberClient Jclient
        {
            get { return Vba32NS.jclient; }
            set { Vba32NS.jclient = value; }
        }

        internal static object synch = new object();

        #endregion

        #region Constructor
        public Vba32NS()
        {
            LoggerNS.log.Info("Vba32NS.Vba32NS():: Started");
            InitializeComponent();
            try
            {
                //Проверка на битность ОС
                if (Marshal.SizeOf(typeof(IntPtr)) == 8)
                    registryControlCenterKeyName = "SOFTWARE\\Wow6432Node\\Vba32\\ControlCenter\\";
                else
                    registryControlCenterKeyName = "SOFTWARE\\Vba32\\ControlCenter\\";

                machineName = Environment.MachineName;
                path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\";

                list = GetNotifyEventList();
                if (!ReadSettingsFromRegistry())
                    LoggerNS.log.Error("Vba32NS.OnStop()::ReadSettingsFromRegistry returned false");

        
                LoggerNS.log.Info("Vba32NS.Vba32NS()::Finished");
            }
            catch (Exception ex)
            {
                string errorMessage = "" + ex.Message;
                LoggerNS.log.Error(errorMessage);
                EventLog.WriteEntry(AppDomain.CurrentDomain.FriendlyName, errorMessage, EventLogEntryType.Error);
            }
        }
        #endregion

        #region OnStart, OnStop
        protected override void OnStart(string[] args)
        {
            LoggerNS.log.Info("Vba32NS.OnStart():: Started");
            
            try
            {
                if (!String.IsNullOrEmpty(jabberServer))
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
                    jclient = new JVlsXMPPClient(jabberServer, jabberFromJID, jabberPassword);
                    break;
                default:
                    LoggerNS.log.Info("AgsXMPPClient");
                    jclient = new AgsXMPPClient(jabberServer, jabberFromJID, jabberPassword);
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
        private bool ConfigureIPC()
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
