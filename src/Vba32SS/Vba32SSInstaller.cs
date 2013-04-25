using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;

namespace Vba32.ControlCenter.SettingsService
{
    [RunInstaller(true)]
    public partial class Vba32SSInstaller : Installer
    {
        private System.ServiceProcess.ServiceInstaller servInstaller;
        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller;
        public Vba32SSInstaller()
        {
            InitializeComponent();

            this.servInstaller = new System.ServiceProcess.ServiceInstaller();
            this.serviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            //
            // serviceInstaller1
            //
            this.servInstaller.DisplayName = "Vba32 Settings Service";
            this.servInstaller.ServiceName = "Vba32SS";
            this.servInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.servInstaller.Description = "Provides ability to change settings. Is a part of Vba32 Control Center server. Makes it possible to configure program using Web interface. If this service is stopped, applying settings from Web interface will be unavailable.";
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

        }
    }
}