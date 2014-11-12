using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;

using System.Globalization;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Microsoft.Win32;

namespace Vba32.ControlCenter.PeriodicalMaintenanceService
{
    [RunInstaller(true)]
    public partial class Vba32PMSInstaller : Installer
    {
        private System.ServiceProcess.ServiceInstaller servInstaller;
        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller;


        public Vba32PMSInstaller()
        {
            InitializeComponent();


            this.servInstaller = new System.ServiceProcess.ServiceInstaller();
            this.serviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            //
            // serviceInstaller1
            //
            this.servInstaller.DisplayName = "Vba32 Periodical Maintenance Service";
            this.servInstaller.ServiceName = "Vba32PMS";
            this.servInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.servInstaller.Description = "Provides periodical DB maintenance. Is a part of Vba32 Control Center server. Cleans outdated events from DB, also implements CC hierarchy feature. If this service is stopped, DB cleanup and CC hierarchy will be unavaliable.";
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

        private Boolean StartUpCreateRegistryKey()
        {
            Debug.WriteLine("Vba32PMS.StartUpCreateRegistryKey()::Создаем в реестре настройки по умолчанию");
            try
            {
                String registryControlCenterKeyName;
                if (Marshal.SizeOf(typeof(IntPtr)) == 8)
                    registryControlCenterKeyName = "SOFTWARE\\Wow6432Node\\Vba32\\ControlCenter\\PeriodicalMaintenance";
                else
                    registryControlCenterKeyName = "SOFTWARE\\Vba32\\ControlCenter\\PeriodicalMaintenance";

                RegistryKey key =
                      Registry.LocalMachine.CreateSubKey(registryControlCenterKeyName);

                IFormatProvider culture = new CultureInfo("ru-RU");

                key.SetValue("Server", "127.0.0.1", RegistryValueKind.String);
                key.SetValue("MaxFileLength", 512 * 1024, RegistryValueKind.DWord);
                key.SetValue("Port", 17001, RegistryValueKind.DWord);
                key.SetValue("LastSelectDate", DateTime.Now.ToString(culture), RegistryValueKind.String);
                key.SetValue("LastSendDate", DateTime.MinValue.ToString(culture), RegistryValueKind.String);
                key.SetValue("NextSendDate", DateTime.Now.AddDays(1).ToString(culture), RegistryValueKind.String);
                key.SetValue("DeliveryTimeoutCheck", 60, RegistryValueKind.DWord);
                key.SetValue("DataSendInterval", 0, RegistryValueKind.DWord);
                key.SetValue("DaysToDelete", 90, RegistryValueKind.DWord);
                key.SetValue("TaskDaysToDelete", 90, RegistryValueKind.DWord);
                key.SetValue("MaintenanceEnabled", 0, RegistryValueKind.DWord);
                key.SetValue("TaskDaysToDelete", 180, RegistryValueKind.DWord);
                key.SetValue("HourIntervalToSend", 4, RegistryValueKind.DWord);
                key.SetValue("AllowLog", 3, RegistryValueKind.DWord);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Vba32PMS.StartUpCreateRegistryKey()::Ошибка: " + ex.Message);
                EventLog.WriteEntry(AppDomain.CurrentDomain.FriendlyName,
                    "Vba32PMS.StartUpCreateRegistryKey()::Ошибка: " + ex.Message, EventLogEntryType.Error);
            }
            return true;
        }
    }
}