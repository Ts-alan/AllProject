using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.ComponentModel;

namespace VirusBlokAda.RemoteOperations.RemoteService
{
    /// <summary>
    /// Most of the used api function are wrapped in C# 3.5
    /// </summary>
    class ServiceUtility
    {
        #region PInvoke
        private const UInt32 SC_MANAGER_ALL_ACCESS = 0xF003F;
        private const UInt32 SERVICE_ALL_ACCESS = 0xF01FF;
        private const UInt32 SERVICE_WIN32_OWN_PROCESS = 0x00000010;
        private const UInt32 SERVICE_DEMAND_START = 0x00000003;
        private const UInt32 SERVICE_ERROR_NORMAL = 0x00000001;
        private const UInt32 SERVICE_STOPPED = 0x00000001;
        private const UInt32 SERVICE_RUNNING = 0x00000004;
        private const UInt32 SERVICE_CONTROL_STOP = 0x00000001;

        [StructLayout(LayoutKind.Sequential)]
        private struct SERVICE_STATUS
        {
            public UInt32 dwServiceType;
            public UInt32 dwCurrentState;
            public UInt32 dwControlsAccepted;
            public UInt32 dwWin32ExitCode;
            public UInt32 dwServiceSpecificExitCode;
            public UInt32 dwCheckPoint;
            public UInt32 dwWaitHint;
        }

        [DllImport("advapi32.dll", EntryPoint = "OpenSCManager",
            CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr OpenSCManager(string lpMachineName, string lpDatabaseName, UInt32 dwDesiredAccess);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr OpenService(IntPtr hSCManager, string lpServiceName, UInt32 dwDesiredAccess);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr CreateService(IntPtr hSCManager, string lpServiceName, string lpDisplayName,
            UInt32 dwDesiredAccess, UInt32 dwServiceType, UInt32 dwStartType, UInt32 dwErrorControl,
            string lpBinaryPathName, string lpLoadOrderGroup, string lpdwTagId, string lpDependencies,
            string lpServiceStartName, string lpPassword);

        [DllImport("advapi32.dll", EntryPoint = "CloseServiceHandle", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseServiceHandleA(IntPtr hSCObject);

        [DllImport("advapi32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool StartService(IntPtr hService, UInt32 dwNumServiceArgs, string[] lpServiceArgVectors);

        [DllImport("advapi32.dll", EntryPoint = "DeleteService", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteServiceA(IntPtr hService);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ControlService(IntPtr hService, UInt32 dwControl, ref SERVICE_STATUS lpServiceStatus);

        [DllImport("advapi32.dll", EntryPoint = "QueryServiceStatus", CharSet = CharSet.Auto)]
        private static extern bool QueryServiceStatusA(IntPtr hService, ref SERVICE_STATUS dwServiceStatus);
        #endregion

        #region Service Methods
        /// <summary>
        /// Queries service status
        /// </summary>
        /// <param name="hService">Handle of service</param>
        /// <param name="dwServiceStatus">Service status struct of specified service</param>
        private static void QueryServiceStatus(IntPtr hService, ref SERVICE_STATUS dwServiceStatus)
        { 
            if (!QueryServiceStatusA(hService, ref dwServiceStatus))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        /// <summary>
        /// Waiting for service to reach specified state
        /// </summary>
        /// <param name="hService">Pointer to service</param>
        /// <param name="dwDesiredState">Desired state</param>
        /// <param name="sServiceStatus">Service status returned</param>
        /// <param name="timeout">Timeout</param>
        private static void WaitForServiceToReachState(IntPtr hService, UInt32 dwDesiredState, 
            ref SERVICE_STATUS sServiceStatus, TimeSpan timeout)
        {
            TimeSpan timeWaiting = new TimeSpan();

            // Loop until the service reaches the desired state,
            // an error occurs, or we timeout 
            while (true)
            {
                if (!QueryServiceStatusA(hService, ref sServiceStatus))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
                if (sServiceStatus.dwCurrentState == dwDesiredState)
                {
                    break;
                }
                if (timeWaiting > timeout)
                {
                    throw new TimeoutException();
                }
                // We're not done, wait the specified period of time
                timeWaiting.Add(new TimeSpan(sServiceStatus.dwWaitHint));
                Thread.Sleep(new TimeSpan(sServiceStatus.dwWaitHint));
            }
        }

        /// <summary>
        /// Opens service manager on remote machine
        /// </summary>
        /// <param name="remoteMachineName">Name of the remote machine</param>
        /// <returns></returns>
        private static IntPtr OpenServiceManager(string remoteMachineName)
        {
            IntPtr hSCManager = OpenSCManager(remoteMachineName, null, SC_MANAGER_ALL_ACCESS);
            if (hSCManager == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return hSCManager;
        }

        /// <summary>
        /// Closes service manager
        /// </summary>
        /// <param name="hSCManager">Handle of service manager</param>
        private static void CloseServiceManager(IntPtr hSCManager)
        {
            if (!CloseServiceHandleA(hSCManager))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        /// <summary>
        /// Creates service
        /// </summary>
        /// <param name="hSCManager">Handle of service manager</param>
        /// <param name="serviceName">Name of service</param>
        /// <param name="servicePath">Absolute path to service</param>
        /// <returns></returns>
        private static IntPtr CreateService(IntPtr hSCManager, string serviceName, string servicePath)
        {
            IntPtr hService = CreateService(hSCManager, serviceName, serviceName, SERVICE_ALL_ACCESS, 
                SERVICE_WIN32_OWN_PROCESS, SERVICE_DEMAND_START, SERVICE_ERROR_NORMAL,
                servicePath, null, null, "", null, null);
            if (hService == null)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return hService;            
        }

        /// <summary>
        /// Start service
        /// </summary>
        /// <param name="hService">Handle of service</param>
        private static void StartService(IntPtr hService)
        {
            if (!StartService(hService, 0, null))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        /// <summary>
        /// Opens service
        /// </summary>
        /// <param name="hSCManager">Handle of service manager</param>
        /// <param name="serviceName">Name of service</param>
        /// <returns>Returns handle of service</returns>
        private static IntPtr OpenService(IntPtr hSCManager, string serviceName)
        {
            IntPtr hService = OpenService(hSCManager, serviceName, SERVICE_ALL_ACCESS);
            if (hService == null)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return hService;
        }

        /// <summary>
        /// Closes service handle
        /// </summary>
        /// <param name="hService">Handle of service</param>
        private static void CloseServiceHandle(IntPtr hService)
        {
            if (!CloseServiceHandleA(hService))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        /// <summary>
        /// Sends a control code to service
        /// </summary>
        /// <param name="hService">Handle of service</param>
        /// <param name="dwControl">Control code</param>
        private static void ControlService(IntPtr hService, uint dwControl)
        {
            SERVICE_STATUS ss = new SERVICE_STATUS();
            if (!ControlService(hService, dwControl, ref ss))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        /// <summary>
        /// Delete service
        /// </summary>
        /// <param name="hService">Handle of service</param>
        private static void DeleteService(IntPtr hService)
        {
            if (!DeleteServiceA(hService))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Installs and starts service on remote machine
        /// </summary>
        /// <param name="remoteMachineName">Name of remote machine</param>
        /// <param name="serviceName">Name of service</param>
        /// <param name="servicePath">Absolute path to service</param>
        /// <param name="timeout">Time to wait for service to start</param>
        public static void InstallAndStartRemoteService(string remoteMachineName, string serviceName, string servicePath,
            TimeSpan timeout)
        {
            IntPtr hSCM = IntPtr.Zero;
            IntPtr hService = IntPtr.Zero;
            try
            {
                hSCM = OpenServiceManager(remoteMachineName);

                // Maybe it's already there and installed, let's try to open it
                hService = OpenService(hSCM, serviceName);

                // Creates service on remote machine, if it's not installed yet
                if (hService == IntPtr.Zero)
                {
                    hService = CreateService(hSCM, serviceName, servicePath);
                }
                if (hService != IntPtr.Zero) //Created service successfully, now try to start it
                {
                    SERVICE_STATUS ss = new SERVICE_STATUS();
                    QueryServiceStatus(hService, ref ss);
                    //Check if service is already running
                    if (ss.dwCurrentState != SERVICE_RUNNING)
                    {
                        StartService(hService);
                        // wait the service to reach the running status

                        WaitForServiceToReachState(hService, SERVICE_RUNNING, ref ss, timeout);
                    }                    
                }
            }
            finally
            {
                if (hService != null && hService != IntPtr.Zero)
                {
                    CloseServiceHandle(hService);
                }
                if (hSCM != null && hSCM != IntPtr.Zero)
                {
                    CloseServiceManager(hSCM);
                }
            }
        }

        /// <summary>
        /// Stops and deletes service on remote machine
        /// </summary>
        /// <param name="remoteMachineName">Name of remote machine</param>
        /// <param name="serviceName">Name of service</param>
        /// <param name="timeout">Time to wait for service to stop</param>
        [Obsolete("Service will be stoped and deleted by service itself on remote machine")]
        public static void StopAndDeleteRemoteService(string remoteMachineName, string serviceName, TimeSpan timeout)
        {
            IntPtr hSCM = IntPtr.Zero;
            IntPtr hService = IntPtr.Zero;
            try
            {
                hSCM = OpenServiceManager(remoteMachineName);
                hService = OpenService(hSCM, serviceName);

                if (hService == IntPtr.Zero) //no such service
                {
                    return;
                }
                SERVICE_STATUS ss = new SERVICE_STATUS();
                QueryServiceStatus(hService, ref ss);
                if (ss.dwCurrentState != SERVICE_STOPPED)
                {
                    ControlService(hService, SERVICE_CONTROL_STOP);
                }
                WaitForServiceToReachState(hService, SERVICE_CONTROL_STOP, ref ss, timeout);

                DeleteService(hService);
            }
            finally
            {
                if (hService != null && hService != IntPtr.Zero)
                {
                    CloseServiceHandle(hService);
                }
                if (hSCM != null && hSCM != IntPtr.Zero)
                {
                    CloseServiceManager(hSCM);
                }
            }
        }
        #endregion

    }
}
