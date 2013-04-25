using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.IO;
using System.Diagnostics;

namespace VirusBlokAda.RemoteOperations.RemoteService
{
    class RemoteClient
    {
        //IMPORTANT
        //constant and struct layouts must match here and in c++ service code
        //if they don't errors will occur upon communication throw pipes
        #region Constants
        private const int OUTPUT_SIZE = 1024;
        private const int COMMAND_SIZE = 128;
        private const int ARGUMENTS_SIZE = 512;
        private const string pipeName = "vba32helperservice";
        #endregion

        #region Structs
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
        public struct Request
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = COMMAND_SIZE)]
            public string szCommand;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = ARGUMENTS_SIZE)]
            public string szArguments;
            [MarshalAs(UnmanagedType.Bool)]
            public bool bNoWait;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
        public struct Response
        {
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dwErrorCode;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dwReturnCode;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = OUTPUT_SIZE)]
            public string szOut;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Sends request through connected pipe
        /// </summary>
        /// <param name="request">Request struct to send</param>
        /// <returns>Returns true if no errors occured</returns>
        private bool SubmitRequest(Request request)
        {
            if (pipeStream == null)
            {
                return false;
            }
            int size = Marshal.SizeOf(request);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(request, ptr, false);
                byte[] buf = new byte[size];
                Marshal.Copy(ptr, buf, 0, size);
                pipeStream.Write(buf, 0, size);
                pipeStream.Flush();
                return true;
            }
            catch (Exception ex)
            {
                Marshal.FreeHGlobal(ptr);
                Debug.WriteLine(ex.Message);
                return false;
            }
            
        }
        /// <summary>
        /// Get response through connected pipe
        /// </summary>
        /// <param name="response">Response struct that was sent thorugh pipe </param>
        /// <returns>Returns true if no errors occured</returns>
        private bool GetResponse(out Response response)
        {
            response = new Response();
            if (pipeStream == null)
            {
                return false;
            }
            int size = Marshal.SizeOf(response);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            try
            {
                
                byte[] buf = new byte[size];
                pipeStream.Read(buf, 0, size);                
                Marshal.Copy(buf, 0, ptr, size);
                response = (Response)Marshal.PtrToStructure(ptr, response.GetType());
                return true;
            }
            catch (Exception ex)
            {
                Marshal.FreeHGlobal(ptr);
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Executes command on remote machine by sending command request
        /// and receiving response
        /// </summary>
        /// <param name="request">Command request</param>
        /// <param name="response">Response from command</param>
        /// <returns>Returns true if no errors occured</returns>
        private bool ExecuteCommand(Request request, out Response response)
        {
            if (!SubmitRequest(request))
            {
                response = new Response();
                response.dwErrorCode = 1;
                return false;
            }
            return GetResponse(out response);
        }
        #endregion

        #region Private Fields
        private SafeFileHandle hPipe = null;
        private FileStream pipeStream = null;
        private string remoteMachineName;
        #endregion

        #region Timeout Settings
        private const int dwRetry = 7;
        private static TimeSpan retryTimeOut = new TimeSpan(0, 0, 3);
        #endregion

        #region Public Methods
        /// <summary>
        /// Connect to remote client
        /// </summary>
        /// <param name="_remoteMachineName">Remote machine name</param>
        /// <returns>Returns true if no errors occured</returns>
        public bool ConnectToRemoteClientPipe(string _remoteMachineName)
        {
            try
            {
                remoteMachineName = _remoteMachineName;
                bool success = PipeUtility.ConnectToPipe(remoteMachineName, pipeName, dwRetry, retryTimeOut,
                    out hPipe);
                if (success)
                {
                    pipeStream = new FileStream(hPipe, FileAccess.ReadWrite);
                }
                return success;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Closes current connection
        /// </summary>
        /// <returns>Returns true if no errors occured</returns>
        public bool CloseConnectionToPipe()
        {
            if (hPipe.IsClosed || hPipe.IsInvalid)
            {
                return false;
            }
            try
            {
                hPipe.Close();
                hPipe.SetHandleAsInvalid();
                pipeStream = null;
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Get windows directory of remote client
        /// </summary>
        /// <param name="windir">Windows directory</param>
        /// <returns>Returns true if no errors occured</returns>
        public bool GetWindowsDirectory(out string windir)
        {
            Request request = new Request();
            request.szCommand = "getwindir";
            Response response;
            windir = "";
            bool success = ExecuteCommand(request, out response);
            windir = response.szOut;
            return success;
        }


        /// <summary>
        /// Execute reboot command on remote client
        /// </summary>
        /// <returns>Returns true if no errors occured</returns>
        public bool RebootCommand()
        {
            Request request = new Request();
            request.szCommand = "reboot";
            Response response;
            return ExecuteCommand(request, out response);
        }

        /// <summary>
        /// Get windows os name of remote client
        /// </summary>
        /// <param name="windowsOs">Windows os name</param>
        /// <returns>Returns true if no errors occured</returns>
        public bool GetWindowsOsCommand(out string windowsOs)
        {
            Request request = new Request();
            request.szCommand = "getos";
            Response response;
            windowsOs = "";
            bool success = ExecuteCommand(request, out response);
            windowsOs = response.szOut;
            return success;
        }

        /// <summary>
        /// Executes process on remote client
        /// </summary>
        /// <param name="commandLine">Process command line</param>
        /// <param name="errorCode">Error code upon process start</param>
        /// <param name="exitCode">Exit code of completed process</param>
        /// <returns>Returns true if no errors occured</returns>
        public bool RunProcessCommand(string commandLine, out uint errorCode, out uint exitCode)
        {
            Request request = new Request();
            request.szCommand = "runprocess";
            request.szArguments = commandLine;
            Response response;
            exitCode = uint.MaxValue;
            bool success = ExecuteCommand(request, out response);
            exitCode = response.dwReturnCode;
            errorCode = response.dwErrorCode;
            return success;
        }
        #endregion


    }
}
