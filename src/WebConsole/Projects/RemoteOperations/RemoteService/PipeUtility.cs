using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Threading;
using System.IO;

namespace VirusBlokAda.RemoteOperations.RemoteService
{
    class PipeUtility
    {
        #region PInvoke
        [DllImport("kernel32.dll", EntryPoint = "WaitNamedPipe", CharSet = CharSet.Auto)]
        private static extern bool WaitNamedPipe(string lpNamedPipeName, UInt32 nTimeOut);

        [DllImport("kernel32.dll", EntryPoint = "CreateFile", CharSet = CharSet.Auto, 
            CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern SafeFileHandle CreateFile(string lpFileName, UInt32 dwDesiredAccess,
              UInt32 dwShareMode, IntPtr SecurityAttributes, UInt32 dwCreationDisposition,
              UInt32 dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool InitializeSecurityDescriptor(IntPtr pSecurityDescriptor, UInt32 dwRevision);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetSecurityDescriptorDacl(ref SECURITY_DESCRIPTOR sd, bool daclPresent, 
            IntPtr dacl, bool daclDefaulted);


        [StructLayout(LayoutKind.Sequential)]
        private struct SECURITY_ATTRIBUTES
        {
            public int nLength;
            public IntPtr lpSecurityDescriptor;
            public int bInheritHandle;
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        struct SECURITY_DESCRIPTOR
        {
            public byte revision;
            public byte size;
            public short control;
            public IntPtr owner;
            public IntPtr group;
            public IntPtr sacl;
            public IntPtr dacl;
        }

        private const UInt32 SECURITY_DESCRIPTOR_REVISION = 1;
        private const UInt32 GENERIC_READ = 0x80000000;
        private const UInt32 GENERIC_WRITE = 0x40000000;
        private const UInt32 OPEN_EXISTING = 3;
        private const UInt32 FILE_ATTRIBUTE_NORMAL = 0x00000080;
        #endregion



        public static bool ConnectToPipe(string remoteMachineName, string pipeName, 
            int dwRetry, TimeSpan retryTimeOut, out SafeFileHandle hPipe)
        {
            hPipe = null;
            string szPipeName = "\\\\" + remoteMachineName + "\\pipe\\" + pipeName;
            SECURITY_ATTRIBUTES SecAttrib = new SECURITY_ATTRIBUTES();
           
            SECURITY_DESCRIPTOR SecDesc = new SECURITY_DESCRIPTOR();
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(SecDesc));
            try
            {
                Marshal.StructureToPtr(SecDesc, ptr, false);

                InitializeSecurityDescriptor(ptr, SECURITY_DESCRIPTOR_REVISION);
                SetSecurityDescriptorDacl(ref SecDesc, true, IntPtr.Zero, true);
                SecAttrib.nLength = Marshal.SizeOf(typeof(SECURITY_ATTRIBUTES));
                SecAttrib.bInheritHandle = 1; //true

                // Connects to the remote service's communication pipe
                while (dwRetry-- > 0)
                {
                    if (WaitNamedPipe(szPipeName, 5000))
                    {
                        IntPtr SecurityAttributes = Marshal.AllocHGlobal(Marshal.SizeOf(SecAttrib));
                        try
                        {
                            Marshal.StructureToPtr(SecAttrib, SecurityAttributes, true);
                            hPipe = CreateFile(szPipeName, GENERIC_WRITE | GENERIC_READ, 0,
                            SecurityAttributes, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);
                        }
                        finally
                        {
                            Marshal.FreeHGlobal(SecurityAttributes);
                        }
                        break;
                    }
                    else
                    {
                        // Let's try it again
                        Thread.Sleep(retryTimeOut);
                    }
                }
                if (hPipe == null)
                {
                    return false;
                }
                return (!hPipe.IsInvalid);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }       
    }
}
