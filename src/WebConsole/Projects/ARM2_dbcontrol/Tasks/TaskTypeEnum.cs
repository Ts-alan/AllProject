using System;
using System.Collections.Generic;
using System.Text;

namespace ARM2_dbcontrol.Tasks
{
      /// <summary>
      /// Перечисление типов задач
      /// </summary>
    public enum TaskType
    {
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
        RequestPolicy,
        ConfigureSheduler,
        Uninstall,
        ConfigureAgent,
        DetachAgent,
        Install,
        AgentSettings,
        ConfigureScanner,
        MonitorOn,
        MonitorOff,
        ConfigureIntegrityCheck,
        FileCleaner,
        JornalEvents
    }
}
