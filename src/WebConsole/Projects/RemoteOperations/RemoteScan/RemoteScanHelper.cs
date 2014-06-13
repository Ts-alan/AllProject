using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using VirusBlokAda.CC.RemoteOperations.Common;
using VirusBlokAda.CC.RemoteOperations.Wmi;
using System.Runtime.InteropServices;
using VirusBlokAda.CC.RemoteOperations.RemoteService;
using VirusBlokAda.CC.RemoteOperations.Net;
using VirusBlokAda.CC.RemoteOperations.RemoteScan.RemoteInfo;
using System.Security.Principal;

namespace VirusBlokAda.CC.RemoteOperations.RemoteScan
{
    class RemoteScanHelper
    {
        [DllImport("advapi32.DLL", SetLastError = true)]
        public static extern int LogonUser(string lpszUsername, string lpszDomain,
            string lpszPassword, int dwLogonType, int dwLogonProvider, out IntPtr phToken);

        [DllImport("advapi32.DLL")]
        public static extern bool ImpersonateLoggedOnUser(IntPtr hToken); //handle to token for logged-on user

        [DllImport("advapi32.DLL")]
        public static extern bool RevertToSelf();

        [DllImport("kernel32.dll")]
        public extern static bool CloseHandle(IntPtr hToken);

        #region Internal enums

        private enum LogonType
        {
            Interactive = 2,
            Network = 3,
            Batch = 4,
            Service = 5,
            Unlock = 7,
            NetworkClearText = 8,
            NewCredentials = 9
        }

        private enum LogonProvider
        {
            Default = 0,
            WinNT35 = 1,
            WinNT40 = 2,
            WinNT50 = 3
        }

        #endregion

        protected const int AgentPort = 17002;
        protected const int LoaderPort = 17003;
        public static bool IsComputerOnline(IPAddress address, TimeSpan timeout, Int32 pingCount)
        {
            int itimeout;
            if (timeout.TotalMilliseconds > int.MaxValue)
            {
                itimeout = int.MaxValue;
            }
            else 
            {
                itimeout = (int) timeout.TotalMilliseconds;
            }
            Ping pingSender = new Ping();
            try
            {
                PingReply reply;
                while (pingCount > 0)
                {
                    reply = pingSender.Send(address, itimeout);
                    if (reply.Status == IPStatus.Success)
                    {
                        return true;
                    }
                    pingCount--;
                }
            }
            catch
            { }
            return false;
        }

        private static bool IsPortOpen(IPEndPoint endpoint)
        {
            try
            {
                TcpClient t = new TcpClient();
                t.Connect(endpoint);
                t.Close();
                return true;
            }
            catch 
            {
                return false;
            }
        }
        public static bool IsAgentPortOpen(IPAddress address)
        {
            return IsPortOpen(new IPEndPoint(address, AgentPort));
        }

        public static bool IsLoaderPortOpen(IPAddress address)
        {
            return IsPortOpen(new IPEndPoint(address, LoaderPort));
        }

        public static bool GetHostname(IPAddress address, out string hostname)
        {
            try
            {
                //advocated method sometime fails with socket exception
                //hostname = Dns.GetHostEntry(address).HostName;
                #pragma warning disable 0618
                hostname = Dns.Resolve(address.ToString()).HostName;
                #pragma warning restore 0618
                return true;
            }
            catch (SocketException)
            {
                hostname = String.Empty;
                return false;
            }
        }

        public static bool GetOSWithWmi(RemoteInfoEntityScan rie, Credentials credentials,
            out string osVersion, out bool isWorkstation, out string errorInfo)
        {
            isWorkstation = true;
            errorInfo = String.Empty;
            osVersion = String.Empty;            
            try
            {                
                WmiProvider provider = new WmiProvider(rie.Name, credentials);
                provider.ConnectToRemoteComputer(TimeSpan.FromSeconds(10));
                string osType = String.Empty;
                provider.RemoteDetectWindowsOS(out osVersion, out osType);
                if (osVersion == String.Empty)
                {
                    osVersion = "Windows NT";
                }
                if (osType == "Server")
                {
                    isWorkstation = false;
                }
                return true;
            }
            catch (COMException)
            {
                errorInfo = "RPC error occured. Check running services and firewall exclusions";
            }
            catch (UnauthorizedAccessException)
            {
                errorInfo = "Cannot connect to remote computer with given credentials";
            }
            catch (Exception)
            {
                errorInfo = "-";
            }
            return false;        
        }

        public static bool GetOSWithRemoteService(RemoteInfoEntityScan rie, Credentials credentials,
            out string osVersion, out bool isWorkstation, out string errorInfo)
        {
            bool connectedToAdminShare = false;
            bool connectedToIpcShare = false;
            bool copiedFiles = false;
            bool installedAndStartedService = false;
            bool connectedToPipe = false;
            isWorkstation = true;
            errorInfo = String.Empty;
            osVersion = String.Empty;
            bool noError = true;

            IntPtr admin_token;
            Int32 valid = LogonUser(credentials.Username,
                            credentials.Domain,
                            credentials.Password,
                            (Int32)LogonType.Interactive,
                            (Int32)LogonProvider.WinNT50,
                            out admin_token);

            WindowsImpersonationContext context = null;
            if (valid != 0)
            {
                context = WindowsIdentity.Impersonate(admin_token);
                CloseHandle(admin_token);
            }

            String hostname = rie.Name;
            try
            {
                NetworkUtility.EstablishConnectionToAdminShare(credentials, rie.Name);
                connectedToAdminShare = true;
            }
            catch (Exception)
            {
                try
                {
                    NetworkUtility.EstablishConnectionToAdminShare(credentials, rie.IPAddress.ToString());
                    connectedToAdminShare = true;
                    hostname = rie.IPAddress.ToString();
                }
                catch (Exception)
                {
                    noError = false;
                    errorInfo = "Failed to connect to remote admin share";
                }
            }

            if (connectedToAdminShare)
            {
                try
                {
                    NetworkUtility.EstablishConnectionToIPCShare(credentials, hostname);
                    connectedToIpcShare = true;
                }
                catch (Exception)
                {
                    noError = false;
                    errorInfo = "Failed to connect to remote ipc share";
                }
            }
            if (connectedToIpcShare)
            {
                try
                {
                    FileUtility.CopyFileToRemoteComputerAdminTempFolder(hostname,
                        System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Settings\vba32helperservice.exe"), "vba32helperservice.exe");
                    copiedFiles = true;
                }
                catch (Exception)
                {
                    noError = false;
                    errorInfo = "Failed to copy service file to admin share";
                }
            }

            if (copiedFiles)
            {
                try
                {
                    ServiceUtility.InstallAndStartRemoteService(hostname, "vba32helperservice",
                        @"%systemroot%\Temp\vba32helperservice.exe", new TimeSpan(0, 0, 20));
                    installedAndStartedService = true;
                }
                catch (Exception)
                {
                    noError = false;
                    errorInfo = "Failed to install or start remote service";
                }
            }

            RemoteClient r = new RemoteClient();

            if (installedAndStartedService)
            {
                if (r.ConnectToRemoteClientPipe(hostname))
                {
                    connectedToPipe = true;
                }
                else
                {
                    noError = false;
                    errorInfo = "Failed to connect to remote pipe";
                }
            }

            if (connectedToPipe)
            {
                if (!r.GetWindowsOsCommand(out osVersion))
                {
                    noError = false;
                    errorInfo = "Failed to get os version of remote machine";
                }
                else
                {
                    if (osVersion.ToLower().Contains("server"))
                    {
                        isWorkstation = false;
                    }
                }
            }

            if (connectedToPipe)
            {
                if (!r.CloseConnectionToPipe())
                {
                    noError = false;
                    errorInfo = "Failed to close connection to remote pipe";
                }
            }


            if (connectedToIpcShare)
            {
                try
                {
                    NetworkUtility.AbolishConnectionToIPCShare(credentials, hostname);
                }
                catch (Exception)
                {
                    noError = false;
                    errorInfo = "Failed to disconnect from remote ipc share";
                }
            }
            if (connectedToAdminShare)
            {
                try
                {
                    NetworkUtility.AbolishConnectionToAdminShare(credentials, hostname);
                }
                catch (Exception)
                {
                    noError = false;
                    errorInfo = "Failed to disconnect from remote admin share";
                }
            }

            if (context != null)
            {
                //context.Undo();
                context.Dispose();
            }

            return noError;
        }

        public static bool GetOS(RemoteInfoEntityScan rie, Credentials credentials, out string osVersion, 
            out bool isWorkstation, out string errorInfo, RemoteMethodsEnum scanMethod)
        {
            if (scanMethod == RemoteMethodsEnum.Wmi)
            {
                return GetOSWithWmi(rie, credentials, out osVersion, out isWorkstation,
                    out errorInfo);
            }
            else //scanMethod == RemoteScanMethodsEnum.RemoteService
            {
                return GetOSWithRemoteService(rie, credentials, out osVersion, out isWorkstation,
                        out errorInfo);
            }
        }
    }
}
