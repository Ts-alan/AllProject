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

using Vba32.ControlCenter.NotificationService.Xml;
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
        internal static Logger log;                             //Класс, позволяющий записывать в файл на диске строки-сообщения
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
            LogMessage("Vba32NS.Vba32NS():: Started",5);
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
                log = new Logger(path + AppDomain.CurrentDomain.FriendlyName + ".log", Encoding.Default, true);
                
                list = GetNotifyEventList();
                if (!ReadSettingsFromRegistry())
                    LogError("Vba32NS.OnStop()::ReadSettingsFromRegistry returned false", EventLogEntryType.Error);

        
                LogMessage("Vba32NS.Vba32NS()::Finished",5);
            }
            catch (Exception ex)
            {
                string errorMessage = "" + ex.Message;
                LogError(errorMessage, EventLogEntryType.Error);
                EventLog.WriteEntry(AppDomain.CurrentDomain.FriendlyName, errorMessage, EventLogEntryType.Error);
            }
        }
        #endregion

        #region OnStart, OnStop
        protected override void OnStart(string[] args)
        {
            LogMessage("Vba32NS.OnStart():: Started",5);
            
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
                LogError("Vba32NS::OnStart():" + ex.Message,
                    EventLogEntryType.Error);
            }


        }

        protected override void OnStop()
        {
            LogMessage("Vba32NS.OnStop():: Started",5);
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
            LogMessage("Vba32NS.SelectXMPPLibrary started",5);
            LogMessage("xmppLibrary=" + xmppLibrary, 10);
            switch (xmppLibrary)
            {
                case 1:
                    LogMessage("JVlsXMPPClient", 10);
                    jclient = new JVlsXMPPClient(jabberServer, jabberFromJID, jabberPassword);
                    break;
                default:
                    LogMessage("AgsXMPPClient", 10);
                    jclient = new AgsXMPPClient(jabberServer, jabberFromJID, jabberPassword);
                    break;
            }
            
            jclient.OpenConnection();
            LogMessage("Jabber connection state: " + jclient.CheckConnectionState(), 6);
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
                LogError("Vba32NS::ConfigureIPC():" + ex.Message,
                  EventLogEntryType.Error);
                return false;
            }
            return true;
        }

        #endregion

        #region Loging
        internal static void LogError(string errorMessage, EventLogEntryType eventLogType)
        {
            try
            {
                Debug.WriteLine(errorMessage);
                log.Write(errorMessage);
                EventLog.WriteEntry(AppDomain.CurrentDomain.FriendlyName, errorMessage, eventLogType);
            }
            catch(Exception ex)
            {
                Debug.WriteLine("LogError ->"+ex);
            }

        }

        internal static void LogMessage(string errorMessage, int level)
        {
            try
            {
                Debug.WriteLine(errorMessage);
                if (logLevel >= level)
                {
                    log.Write(errorMessage);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("LogMessage ->" + ex);
            }
        }
        #endregion
    }
}
