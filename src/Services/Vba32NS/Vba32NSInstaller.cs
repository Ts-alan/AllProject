using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;

using System.Runtime.InteropServices;
using System.Diagnostics;
using Microsoft.Win32;

namespace Vba32.ControlCenter.NotificationService
{
    [RunInstaller(true)]
    public partial class Vba32NSInstaller : Installer
    {
        private System.ServiceProcess.ServiceInstaller servInstaller;
        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller;

        public Vba32NSInstaller()
        {
            InitializeComponent();

            this.servInstaller = new System.ServiceProcess.ServiceInstaller();
            this.serviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            //
            // serviceInstaller1
            //
            this.servInstaller.DisplayName = "Vba32 Notification Service";
            this.servInstaller.ServiceName = "Vba32NS";
            this.servInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.servInstaller.Description = "Provides administrator notification. Is a part of Vba32 Control Center server. Notifies administrators of specified events that triggered on workstations. If this service is stopped, no notifications from CC will be sent.";
            //
            // serviceProcessInstaller1
            //
            this.serviceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.serviceProcessInstaller.Password = null;
            this.serviceProcessInstaller.Username = null;

            //
            // ProjectInstaller
            //
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.servInstaller,this.serviceProcessInstaller});

            StartUpCreateRegistryKey();

        }

        private bool StartUpCreateRegistryKey()
        {
            Debug.WriteLine("Vba32NS.StartUpCreateRegistryKey()::Создаем в реестре настройки по умолчанию");
            try
            {
                string registryControlCenterKeyName;
                if (Marshal.SizeOf(typeof(IntPtr)) == 8)
                    registryControlCenterKeyName = "SOFTWARE\\Wow6432Node\\Vba32\\ControlCenter\\Notification";
                else
                    registryControlCenterKeyName = "SOFTWARE\\Vba32\\ControlCenter\\Notification";

                RegistryKey key =
                      Registry.LocalMachine.CreateSubKey(registryControlCenterKeyName);

                key.SetValue("JabberFromJID", "", RegistryValueKind.String);
                key.SetValue("JabberPassword", "", RegistryValueKind.String);
                key.SetValue("JabberServer", "", RegistryValueKind.String);
                key.SetValue("MailDisplayName", "Vba32 Control Center Notification", RegistryValueKind.String);
                key.SetValue("MailFrom", "", RegistryValueKind.String);
                key.SetValue("MailServer", "(Enter IP)", RegistryValueKind.String);

                key.SetValue("UseFlowAnalysis", 0, RegistryValueKind.DWord);
                key.SetValue("GlobalEpidemyCompCount", 10, RegistryValueKind.DWord);
                key.SetValue("GlobalEpidemyLimit", 10, RegistryValueKind.DWord);
                key.SetValue("GlobalEpidemyTimeLimit", 10, RegistryValueKind.DWord);
                key.SetValue("LocalHearthLimit", 10, RegistryValueKind.DWord);
                key.SetValue("LocalHearthTimeLimit", 10, RegistryValueKind.DWord);
                key.SetValue("Limit", 10, RegistryValueKind.DWord);
                key.SetValue("TimeLimit", 10, RegistryValueKind.DWord);
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Vba32NS.StartUpCreateRegistryKey()::Error: " + ex.Message);
                EventLog.WriteEntry(AppDomain.CurrentDomain.FriendlyName,
                    "Vba32NS.StartUpCreateRegistryKey()::Error: " + ex.Message, EventLogEntryType.Error);
            }
            return true;
        }
    }
}