using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;
using VirusBlokAda.CC.RemoteOperations.Common;

namespace VirusBlokAda.CC.RemoteOperations.Net
{

    class NetworkUtility
    {
        #region PInvoke
        private const UInt32 RESOURCETYPE_ANY = 0x00000000;
        private const UInt32 CONNECT_TEMPORARY = 0x00000004;
        private const UInt32 ERROR_NOT_CONNECTED = 0x000008CA;
        private const UInt32 ERROR_SUCCESS = 0x00000000;

        [StructLayout(LayoutKind.Sequential)]
        private struct NETRESOURCE
        {
            public UInt32 dwScope;
            public UInt32 dwType;
            public UInt32 dwDisplayType;
            public UInt32 dwUsage;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpLocalName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpRemoteName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpComment;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpProvider;
        }
        
        [DllImport("mpr.dll", EntryPoint = "WNetAddConnection2", CharSet = CharSet.Auto)]
        private static extern UInt32 WNetAddConnection2(
        ref NETRESOURCE lpNetResource, string lpPassword, string lpUsername, UInt32 dwFlags);

        [DllImport("mpr.dll", EntryPoint = "WNetCancelConnection2", CharSet = CharSet.Auto)]
        private static extern UInt32 WNetCancelConnection2(
        string lpNmae, UInt32 dwFlags, bool fForce);
        #endregion

        #region Service Methods
        /// <summary>
        /// Establish connection to resouce on remote machine
        /// </summary>
        /// <param name="credentials">Credentials (domain, username and password) that will be used
        /// to establish connection
        /// </param>
        /// <param name="remoteMachineName">Name of the remote machine without preceding backslashes</param>
        /// <param name="resource">Name of the shared resouce on remote machine</param>
        private static void EstablishConnection(Credentials credentials, string remoteMachineName,
            string resource)
        {
            string remoteResource = "\\\\" + remoteMachineName + "\\" + resource;
            NETRESOURCE nr = new NETRESOURCE();
            nr.dwType = RESOURCETYPE_ANY;
            nr.lpLocalName = null;
            nr.lpRemoteName = remoteResource;
            nr.lpProvider = null;
            string login = String.IsNullOrEmpty(credentials.Domain) ?
                (credentials.Username) : (credentials.Domain + "\\" + credentials.Username);
            UInt32 dwFlags = CONNECT_TEMPORARY;
            int result = (int)WNetAddConnection2(ref nr, credentials.Password,
                login, dwFlags);
            if (result != 0)
            {
                throw new Win32Exception(result);
            }
        }

        /// <summary>
        /// Abolish connection to resouce on remote machine
        /// </summary>
        /// <param name="credentials">Credentials (domain, username and password) that will be used
        /// to establish connection
        /// </param>
        /// <param name="remoteMachineName">Name of the remote machine without preceding backslashes</param>
        /// <param name="resource">Name of the shared resouce on remote machine</param>
        /// <param name="force">Specifies whether the disconnection should occur if there are open 
        /// files or jobs on the connection.
        /// </param>
        private static void AbolishConnection(Credentials credentials, string remoteMachineName,
            string resource, bool force)
        {
            string remoteResource = "\\\\" + remoteMachineName + "\\" + resource;
            int result = (int) WNetCancelConnection2(remoteResource, 0, force);
            if (result != ERROR_SUCCESS && result != ERROR_NOT_CONNECTED)
            {
                throw new Win32Exception(result);
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Establish connection to admin (Admin$) share on remote machine
        /// </summary>
        /// <param name="credentials">Credentials (domain, username and password) that will be used
        /// to establish connection
        /// </param>
        /// <param name="remoteMachineName">Name of the remote machine without preceding backslashes</param>
        public static void EstablishConnectionToAdminShare(Credentials credentials, string remoteMachineName)
        {
            EstablishConnection(credentials, remoteMachineName, "Admin$");
        }
        /// <summary>
        /// Establish connection to ipc (IPC$) share on remote machine
        /// </summary>
        /// <param name="credentials">Credentials (domain, username and password) that will be used
        /// to establish connection
        /// </param>
        /// <param name="remoteMachineName">Name of the remote machine without preceding backslashes</param>
        public static void EstablishConnectionToIPCShare(Credentials credentials, string remoteMachineName)
        {
            EstablishConnection(credentials, remoteMachineName, "IPC$");
        }
        /// <summary>
        /// Abolish connection to admin (Admin$) share on remote machine
        /// </summary>
        /// <param name="credentials">Credentials (domain, username and password) that will be used
        /// to establish connection
        /// </param>
        /// <param name="remoteMachineName">Name of the remote machine without preceding backslashes</param>
        public static void AbolishConnectionToAdminShare(Credentials credentials, string remoteMachineName)
        {
            AbolishConnection(credentials, remoteMachineName, "Admin$", true);
        }
        /// <summary>
        /// Abolish connection to ipc (IPC$) share on remote machine
        /// </summary>
        /// <param name="credentials">Credentials (domain, username and password) that will be used
        /// to establish connection
        /// </param>
        /// <param name="remoteMachineName">Name of the remote machine without preceding backslashes</param>
        public static void AbolishConnectionToIPCShare(Credentials credentials, string remoteMachineName)
        {
            AbolishConnection(credentials, remoteMachineName, "IPC$", true);
        }
        #endregion
    }
}
