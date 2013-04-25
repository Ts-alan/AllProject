using System;
using System.Collections.Generic;
using System.Text;

namespace ARM2_dbcontrol.Tasks
{
      /// <summary>
      /// ������������ ����� �����
      /// </summary>
      public enum TaskType { 
            SystemInfo,
            CreateProcess,
            SendFile,
            ComponentState,
            CancelTask,
            ListProcesses,
            ConfigureLoader,
            ConfigureMonitor,
            RunScanner,
            ConfigurePassword,
            ConfigureQuarantine,
            RestoreFileFromQtn,
            ProactiveProtection,
            Firewall,
            ChangeDeviceProtect,
            RequestPolicy,
            ConfigureSheduler,
            Uninstall
        }
}