using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Diagnostics;
using System.Web;
using VirusBlokAda.RemoteOperations.MsiInfo;
using VirusBlokAda.RemoteOperations.RemoteInstall.MsiexecHelper;
using VirusBlokAda.RemoteOperations.RemoteInstall;
using System.Management;
using System.ComponentModel;
using System.Runtime.InteropServices;
using VirusBlokAda.RemoteOperations.Common;

namespace VirusBlokAda.RemoteOperations.Wmi
{
    /// <summary>
    /// Helper class that provides methods needed to install msi using wmi on remote computer.
    /// </summary>
    public class WmiProvider
    {
        #region Private Fields
        private ManagementScope scope;
        #endregion

        #region Property
        private Credentials _credentials;
        public Credentials Credentials
        {
            get { return _credentials; }
            set { _credentials = value; }
        }

        private string _computerName = String.Empty;
        /// <summary>
        /// Remote computer name
        /// </summary>
        public string ComputerName
        {
            get { return _computerName; }
            set { _computerName = value; }
        }

        private bool _isConnected = false;
        /// <summary>
        /// True if WMI connection is success
        /// </summary>
        public bool IsConnected
        {
            get { return _isConnected; }
        }
        #endregion

        #region Constructor
        public WmiProvider(string computerName, Credentials credentials)
        {
            ComputerName = computerName;
            _credentials = credentials;       
        }
        #endregion

        #region Service methods
        /// <summary>
        /// Gets product code guid from product name on remote computer
        /// </summary>
        /// <param name="productName">Product name</param>
        /// <param name="guid">String to save guid into</param>
        /// <returns>Returns true is product with given name exists</returns>
        private bool GetProductGuid(string productName, out string guid)
        {
            if (!IsConnected)
                throw new Exception("WMI connection is not open");
            ObjectQuery query = new ObjectQuery(
                String.Format("SELECT * FROM Win32_Product WHERE Name='{0}'", productName));

            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher(scope, query);

            foreach (ManagementObject nextObject in searcher.Get())
            {
                guid = nextObject.Properties["IdentifyingNumber"].Value.ToString();
                return true;
            }
            guid = String.Empty;
            return false;
        }

        public string[] GetSystemEnvironmentVariables()
        { 
            if (!IsConnected)
                throw new Exception("WMI connection is not open");
            ObjectQuery query = new ObjectQuery("Select * from Win32_Environment");

            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher(scope, query);

            ManagementObjectCollection col = searcher.Get();
            List<string> result = new List<string>();
            foreach (ManagementObject nextObject in col)
            {
                //if ((bool)nextObject.Properties["SystemVariable"].Value)
                //{
                result.Add(nextObject.Properties["Name"].Value.ToString() + ":" +
                    nextObject.Properties["VariableValue"].Value.ToString());
               // }
            }
            return result.ToArray();
        }

        private string GetWindowsDirectory()
        {
            if (!IsConnected)
                throw new Exception("WMI connection is not open");

            ObjectQuery query = new ObjectQuery(
                  "SELECT WindowsDirectory FROM Win32_OperatingSystem");

            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher(scope, query);

            foreach (ManagementObject queryObj in searcher.Get())
                return queryObj["WindowsDirectory"] as String;

            return String.Empty;
        }

        /// <summary>
        /// Creates process on remote computer
        /// </summary>
        /// <param name="commandLine">Process command line</param>
        /// <param name="processId">Process id will be saved here</param>
        /// <returns>Returns true if process was started successfully</returns>
        private bool CreateProcess(string commandLine, out uint processId)
        {
            if (!IsConnected)
                throw new Exception("WMI connection is not open");
            try
            {
                ManagementClass classInstance =
                    new ManagementClass(scope,
                    new ManagementPath("Win32_Process"), null);

                // Obtain in-parameters for the method
                ManagementBaseObject inParams =
                    classInstance.GetMethodParameters("Create");

                // Add the input parameters.
                inParams["CommandLine"] = commandLine;
                ManagementBaseObject outParams =
                    classInstance.InvokeMethod("Create", inParams, null);

                // Get output parameters.
                processId = (uint)outParams["ProcessId"];
                uint result = (uint)outParams["ReturnValue"];
                if (result == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (ManagementException err)
            {
                throw new Exception("An error occurred while trying to execute the WMI method: ", err);
            }
            catch (System.UnauthorizedAccessException unauthorizedErr)
            {
                throw new Exception("Connection error (user name or password might be incorrect): ", unauthorizedErr);
            }
        }

        /// <summary>
        /// Creates process on remote computer
        /// </summary>
        /// <param name="commandLine">Process command line</param>
        /// <returns>Returns true if process was started successfully</returns>
        private bool CreateProcess(string commandLine)
        {
            uint processId;
            return CreateProcess(commandLine, out processId);
        }

        protected void SynchronousWaitForProcessToStop(uint processId, string commandLine, TimeSpan pollingTime, TimeSpan timeout)
        {
            try
            {
                string processName = "msiexec.exe";
                TimeSpan timeRunning = new TimeSpan();
                bool isWaiting = true;
                int collectionNullCounter = 0;
                int collectionNullMax = 10;
                while (isWaiting && (timeRunning < timeout))
                {
                    Thread.Sleep(pollingTime);
                    timeRunning += pollingTime;
                     ObjectQuery query = new System.Management.ObjectQuery(
                        String.Format("select Name, CommandLine from Win32_Process where ProcessId={0}", processId));
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
                    ManagementObjectCollection returnCollection = searcher.Get();
                    bool isRunning = false;
                    if (returnCollection == null)
                    {
                        ++collectionNullCounter;
                        if (collectionNullCounter < collectionNullMax)
                        {
                            isRunning = true;
                        }
                    }
                    else
                    {
                        foreach (ManagementObject next in returnCollection)
                        {
                            if (next == null)
                            {
                                ++collectionNullCounter;                                
                                if (collectionNullCounter < collectionNullMax)
                                {
                                    isRunning = true;
                                }                 
                                break;
                            }
                            object nextName = next["Name"];
                            object nextCommandLine = next["CommandLine"];
                            if (nextName == null || nextCommandLine == null)
                            {
                                ++collectionNullCounter;
                                if (collectionNullCounter < collectionNullMax)
                                {
                                    isRunning = true;
                                }
                                break;
                            }
                            if (processName.Equals(nextName.ToString()) && commandLine.Equals(nextCommandLine.ToString()))
                            {
                                isRunning = true;
                                break;
                            }
                        }
                    }

                    if (!isRunning)
                    {
                        isWaiting = false;
                    }
                }
                if (isWaiting) //timeout
                {
                    throw new TimeoutException("Msiexec timeout");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }

        }
                
        protected void SemisynchronousWaitForProcessToStop(uint processId, TimeSpan pollingTime, TimeSpan timeout)
        {
            WqlEventQuery query = new WqlEventQuery(String.Format(
                "Select * From __InstanceDeletionEvent Within {0} Where TargetInstance ISA 'Win32_Process' AND TargetInstance.ProcessID='{1}'",
                pollingTime.Seconds, processId));
            EventWatcherOptions options = new EventWatcherOptions();
            options.Timeout = timeout;
            ManagementEventWatcher watcher = new ManagementEventWatcher(scope, query, options);
            try
            {
                ManagementBaseObject NewEvent = watcher.WaitForNextEvent();
            }
            catch (System.Management.ManagementException)
            {
                throw new TimeoutException("Msiexec timeout");
            }
            finally
            {
                watcher.Stop();
            }
        }
             
        protected void AsynchronousWaitForProcessToStop(uint processId, TimeSpan timeout)
        {
            ManualResetEvent isFinished = new ManualResetEvent(false);            
            if (!IsConnected)
                throw new Exception("WMI connection is not open");
            WqlEventQuery stopQuery = new WqlEventQuery("Win32_ProcessStopTrace",
                String.Format("ProcessId='{0}'", processId));
            ManagementEventWatcher processStopEvent = new ManagementEventWatcher(scope, stopQuery);
            processStopEvent.EventArrived += new EventArrivedEventHandler(
                delegate(object sender, EventArrivedEventArgs e)
                {
                    (sender as ManagementEventWatcher).Stop();
                    isFinished.Set();
                });
            try
            {
                processStopEvent.Start();
                if (!isFinished.WaitOne(timeout))
                {
                    this.scope = null;
                    throw new TimeoutException("Msiexec timeout");
                }
            }
            catch (COMException comException)
            {
                this.scope = null;
                throw new ArgumentException("Server does not exists or access denied. \nCheck host name and configure firewall options on local and remote computers.", comException);
            }

            catch (UnauthorizedAccessException authException)
            {
                this.scope = null;
                throw new ArgumentException("Access denied or timeout expired. \nCheck if username, password and domain are correct and if user is a member of domain Administrators group.", authException);
            }
            catch
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            finally
            {
                try
                {
                    processStopEvent.Stop();
                }
                catch
                { }
            }
        }        













        #endregion

        #region Public methods
        /// <summary>
        /// Connect to remote computer
        /// </summary>
        /// <param name="connectionTimeout">Connection timeout</param>
        public void ConnectToRemoteComputer(TimeSpan connectionTimeout)
        {
            ConnectionOptions connectionConfiguration = new ConnectionOptions();
            connectionConfiguration.Impersonation = ImpersonationLevel.Impersonate;
            connectionConfiguration.Authentication = AuthenticationLevel.Default;

            if (_credentials.Domain == null || _credentials.Domain == string.Empty)
            {
                connectionConfiguration.Username = this.ComputerName + "\\" + _credentials.Username;
                connectionConfiguration.Authority = null;
            }
            else
            {
                connectionConfiguration.Username = _credentials.Username;
                connectionConfiguration.Authority = "NTLMdomain:" + _credentials.Domain;
            }
            connectionConfiguration.Password = _credentials.Password;
            connectionConfiguration.Timeout = connectionTimeout;
            connectionConfiguration.EnablePrivileges = true;
            scope = new ManagementScope();
            scope.Path = new ManagementPath(@"\\" + this.ComputerName + @"\root\cimv2");
            scope.Options = connectionConfiguration;
            try
            {
                scope.Connect();
               _isConnected = true;
            }
            catch (COMException comException)
            {
                this.scope = null;
                throw new COMException("Server does not exists or access denied. \nCheck host name and configure firewall options.", comException);
            }
            catch (UnauthorizedAccessException authException)
            {
                this.scope = null;
                throw new UnauthorizedAccessException("Access denied or timeout expired. \nCheck if username, password and domain are correct and if user is a member of domain Administrators group.", authException);
            }
            catch
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        /// <summary>
        /// Force reboot remote computer
        /// </summary>
        public void RebootRemoteComputer()
        {
            ManagementClass classInstance = new ManagementClass(scope,
                                new ManagementPath("Win32_OperatingSystem"), null);
            ManagementBaseObject inParams, outParams;

            ManagementObjectCollection col = classInstance.GetInstances();
            foreach (ManagementObject nextObject in col)
            {
                inParams = nextObject.GetMethodParameters("Win32Shutdown");
                inParams["Flags"] = 6; //forced reboot
                inParams["Reserved"] = 0;
                outParams = nextObject.InvokeMethod("Win32Shutdown", inParams, null);

                int result = Convert.ToInt32(outParams["returnValue"]);
                if (result != 0) throw new Win32Exception(result);
            }

        }

        /// <summary>
        /// Retrives os caption and product type of remote computer using WMI
        /// </summary>
        /// <param name="caption">Caption of os</param>
        /// <param name="productType">Product type of os (workstation, server, domain controller)</param>
        /// <returns>True if no errors</returns>
        public void RemoteDetectWindowsOS(out string caption, out string productType)
        {
            ObjectQuery q = new ObjectQuery("SELECT Caption, ProductType FROM  Win32_OperatingSystem");
            ManagementObjectSearcher objMOS = new ManagementObjectSearcher(scope, q);
            caption = "";
            productType = "";
            foreach (ManagementObject objManagement in objMOS.Get())
            {
                object osCaption = objManagement.GetPropertyValue("Caption");
                if (osCaption != null)
                {
                    caption = (String)osCaption;
                }
                object osType = objManagement.GetPropertyValue("ProductType");
                if (osType != null)
                {
                    UInt32 t = (UInt32)osType;
                    switch (t)
                    {
                        case 1:
                            productType = "Work Station";
                            break;
                        case 2:
                            productType = "Domain Controller";
                            break;
                        case 3:
                            productType = "Server";
                            break;
                        default:
                            throw new ArgumentException("Invalid windows product type");
                    }
                }               
            }          
        }

        /// <summary>
        /// Uninstall product with given GUID
        /// </summary>
        /// <param name="guid">Product code guid</param>
        /// <param name="logFileName">File name to log to</param>
        /// <param name="pollingTime">Polling time for checking if msiexec completed</param>
        /// <param name="timeout">Timeout for checking if msiexec completed</param>
        public void UninstallProductWithMsiExec(WmiMethodsCallMode wmiMethodCallMode, string guid, string logFileName, 
            TimeSpan pollingTime, TimeSpan timeout)
        {
            string windir = GetWindowsDirectory();
            string tmp = FileUtility.AppendTerminalBackslash(windir) + "temp";
            string commandLine = CommandLine.Uninstall(tmp, guid, false,
                LogLevel.Verbose | LogLevel.AllExceptVerbose, logFileName);
            uint processId;
            bool result = CreateProcess(commandLine, out processId);
            switch (wmiMethodCallMode)
            {
                case WmiMethodsCallMode.Synchronous:
                    SynchronousWaitForProcessToStop(processId, commandLine, pollingTime, timeout);
                    break;
                case WmiMethodsCallMode.Semisynchronous:
                    SemisynchronousWaitForProcessToStop(processId, pollingTime, timeout);
                    break;
                case WmiMethodsCallMode.Asynchronous:
                    AsynchronousWaitForProcessToStop(processId, timeout);
                    break;
            }
        }

        /// <summary>
        /// Install software with create process
        /// </summary>
        /// <param name="msiFileName">Msi file name on remote computer</param>
        /// <param name="logFileName">File name to log to</param>
        /// <param name="pollingTime">Polling time for checking if msiexec completed</param>
        /// <param name="timeout">Timeout for checking if msiexec completed</param>
        public void InstallProductWithMsiExec(WmiMethodsCallMode wmiMethodCallMode, string msiFileName,
            string logFileName, TimeSpan pollingTime, TimeSpan timeout)
        {
            string windir = GetWindowsDirectory();
            string tmp = FileUtility.AppendTerminalBackslash(windir) + "temp";
            string commandLine = CommandLine.Install(tmp, msiFileName, false,
                LogLevel.Verbose | LogLevel.AllExceptVerbose, logFileName);
            uint processId;
            bool result = CreateProcess(commandLine, out processId);
            switch (wmiMethodCallMode)
            {
                case WmiMethodsCallMode.Synchronous:
                    SynchronousWaitForProcessToStop(processId, commandLine, pollingTime, timeout);
                    break;
                case WmiMethodsCallMode.Semisynchronous:
                    SemisynchronousWaitForProcessToStop(processId, pollingTime, timeout);
                    break;
                case WmiMethodsCallMode.Asynchronous:
                    AsynchronousWaitForProcessToStop(processId, timeout);
                    break;
            }
        }
        #endregion        
    }
}