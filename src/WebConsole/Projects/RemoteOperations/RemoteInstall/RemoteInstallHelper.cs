#define USE_CONSOLE
using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.CC.RemoteOperations.Common;
using VirusBlokAda.CC.RemoteOperations.Net;
using System.IO;
using VirusBlokAda.CC.RemoteOperations.Wmi;
using System.Runtime.InteropServices;
using VirusBlokAda.CC.RemoteOperations.RemoteInstall.MsiexecHelper;
using VirusBlokAda.CC.RemoteOperations.RemoteService;
using VirusBlokAda.CC.DataBase;
using System.Configuration;
using System.Diagnostics;

namespace VirusBlokAda.CC.RemoteOperations.RemoteInstall
{
    class RemoteInstallHelper
    {
        protected const String InstallLogFileName = "vba32install.log";
        protected const String UninstallLogFileName = "vba32uninstall.log";
        protected const WmiMethodsCallMode MethodsCallMode = WmiMethodsCallMode.Synchronous;
        protected static TimeSpan PollingTime = new TimeSpan(0, 0, 10);
        protected static TimeSpan Timeout = new TimeSpan(0, 10, 0);
        
        protected static InstallationTaskProvider taskProvider = null;
        protected static String connectionString = "";
        public static String ConnectionString
        {
            get { return connectionString; }
            set
            {
                connectionString = value;
                taskProvider = new InstallationTaskProvider(connectionString);
            }
        }
        
        
        protected static void SetError(RemoteInstallEntity rie, String errorInfo)
        {  
            try
            {
                rie.ErrorInfo = errorInfo;
                rie.Status = InstallationStatusEnum.Error;

                taskProvider.UpdateTask(new InstallationTaskEntity(rie.ID, "", "", rie.Status.ToString(), (Int16?)rie.ExitCode, rie.ErrorInfo));
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception setting error " + e);
            }
        }

        protected static void SetStatus(RemoteInstallEntity rie, InstallationStatusEnum status, Nullable<Int32> exitCode)
        {
            try
            {
                rie.Status = status;
                rie.ExitCode = exitCode;

                taskProvider.UpdateTask(new InstallationTaskEntity(rie.ID, "", "", rie.Status.ToString(), (Int16?)rie.ExitCode, ""));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception setting status " + ex);
            }            
        }
        protected static void SetStatus(RemoteInstallEntity rie, InstallationStatusEnum status)
        {
            SetStatus(rie, status, null);
        }
        protected static void InsertIntoBase(ref RemoteInstallEntity rie, String taskType)
        {
            rie.ID = taskProvider.InsertTask(new InstallationTaskEntity(rie.ComputerName, rie.IP, rie.Status.ToString(), null, ""));            
        }

        protected static Boolean InstallWithWmi(RemoteInstallEntity rie, Credentials credentials, Boolean doRestart)
        {
            InsertIntoBase(ref rie, "Install");
            SetStatus(rie, InstallationStatusEnum.Initializing);
            Boolean usedComputerName;
            if (!ConnectToAdminShare(rie, credentials, out usedComputerName))
            {
                return false;
            }

            WmiProvider provider = null;
            Boolean noError = CopyInstallFileToRemoteComputerAdminTempFolder(rie, credentials, usedComputerName);

            if (noError)
            {
                noError = ConnectToRemoteComputerUsingWmi(rie, credentials, ref provider, usedComputerName);
            }

            if (noError)
            {
                noError = InstallProductUsingWmi(rie, provider);
            }

            if (noError)
            {
                noError = ParseLogFile(rie, InstallLogFileName, usedComputerName);
            }

            DisconnectFromAdminShare(rie, credentials, usedComputerName);

            if (noError)
            {
                if (doRestart)
                {
                    noError = RebootRemoteComputerUsingWmi(rie, provider);
                }
            }
            return noError;
        }

        protected static Boolean UninstallWithWmi(RemoteInstallEntity rie, Credentials credentials, Boolean doRestart)
        {
            InsertIntoBase(ref rie, "Uninstall");
            SetStatus(rie, InstallationStatusEnum.Initializing);
            Boolean usedComputerName;
            if (!ConnectToAdminShare(rie, credentials, out usedComputerName))
            {
                return false;
            }

            WmiProvider provider = null;
            Boolean noError = ConnectToRemoteComputerUsingWmi(rie, credentials, ref provider, usedComputerName);

            if (noError)
            {
                noError = UninstallProductUsingWmi(rie, provider);
            }

            if (noError)
            {
                noError = ParseLogFile(rie, UninstallLogFileName, usedComputerName);
            }


            DisconnectFromAdminShare(rie, credentials, usedComputerName);

            if (noError)
            {
                if (doRestart)
                {
                    noError = RebootRemoteComputerUsingWmi(rie, provider);
                }
            }
            return noError;
        }

        protected static Boolean InstallWithRemoteService(RemoteInstallEntity rie, Credentials credentials, Boolean doRestart)
        {
            InsertIntoBase(ref rie, "Install");

            if (rie.IsInstalled)
            {
                SetStatus(rie, InstallationStatusEnum.Installed);
                return true;
            }

            SetStatus(rie, InstallationStatusEnum.Initializing);

            Boolean usedComputerName;
            Boolean connectedToAdminShare = false;
            Boolean connectedToIpcShare = false;
            Boolean copiedInstallFile = false;
            Boolean copiedServiceFile = false;
            Boolean installedAndStartedService = false;
            Boolean connectedToPipe = false;
            Boolean noError = true;
            Boolean gotWindir = false;
            Boolean installedSuccessfully = false;

            connectedToAdminShare = ConnectToAdminShare(rie, credentials, out usedComputerName);
            if (connectedToAdminShare)
            {
                connectedToIpcShare = ConnectToIPCShare(rie, credentials, usedComputerName);
            }            

            if (connectedToIpcShare)
            {
                copiedInstallFile = CopyInstallFileToRemoteComputerAdminTempFolder(rie, credentials,
                    usedComputerName);
            }
            
            if (copiedInstallFile)
            {
                copiedServiceFile = CopyServiceFileToRemoteComputerAdminTempFolder(rie, credentials, 
                    usedComputerName);
            }

            if (copiedServiceFile)
            {
                installedAndStartedService = InstallAndStartRemoteService(rie, credentials, usedComputerName);               
            }

            RemoteClient r = new RemoteClient();

            if (installedAndStartedService)
            {
                connectedToPipe = ConnectToRemoteClientPipe(rie, r, usedComputerName);
            }
            String windir = String.Empty;
            if (connectedToPipe)
            {
                gotWindir = GetWindowsDirectoryWirhRemoteClient(rie, r, out windir);
                
            }

            if (gotWindir)
            {
                installedSuccessfully = InstallProductUsingRemoteClient(rie, r, windir);
            }

            if (installedSuccessfully)
            {
                if (doRestart)
                {
                    noError = RebootRemoteComputerRemoteClient(rie, r);
                }
            }
            else 
            {
                noError = false;
            }

            if (connectedToPipe)
            {
                noError = CloseConnectionToRemoteClientPipe(rie, r);
            }

            if (connectedToIpcShare)
            {
                noError = DisconnectFromIPCShare(rie, credentials, usedComputerName);

            }
            if (connectedToAdminShare)
            {
                noError = DisconnectFromAdminShare(rie, credentials, usedComputerName);

            }
            return noError;
        }

        protected static Boolean UninstallWithRemoteService(RemoteInstallEntity rie, Credentials credentials, Boolean doRestart)
        {
            InsertIntoBase(ref rie, "Uninstall");
            SetStatus(rie, InstallationStatusEnum.Initializing);

            Boolean connectedToAdminShare = false;
            Boolean connectedToIpcShare = false;
            Boolean copiedServiceFile = false;
            Boolean installedAndStartedService = false;
            Boolean connectedToPipe = false;
            Boolean gotWindir = false;
            Boolean noError = true;
            Boolean uninstalledSuccessfully = false;
            Boolean usedComputerName;


            connectedToAdminShare = ConnectToAdminShare(rie, credentials, out usedComputerName);
            if (connectedToAdminShare)
            {
                connectedToIpcShare = ConnectToIPCShare(rie, credentials, usedComputerName);
            }

            if (connectedToIpcShare)
            {
                copiedServiceFile = CopyServiceFileToRemoteComputerAdminTempFolder(rie, credentials,
                    usedComputerName);
            }

            if (copiedServiceFile)
            {
                installedAndStartedService = InstallAndStartRemoteService(rie, credentials, usedComputerName);
            }

            RemoteClient r = new RemoteClient();

            if (installedAndStartedService)
            {
                connectedToPipe = ConnectToRemoteClientPipe(rie, r, usedComputerName);
            }

            String windir = String.Empty;
            if (connectedToPipe)
            {
                gotWindir = GetWindowsDirectoryWirhRemoteClient(rie, r, out windir);
                
            }

            if (gotWindir)
            {
                uninstalledSuccessfully = UninstallProductUsingRemoteClient(rie, r, windir);
            }



            if (uninstalledSuccessfully)
            {
                if (doRestart)
                {
                    noError = RebootRemoteComputerRemoteClient(rie, r);
                }
            }
            else
            {
                noError = false;
            }

            if (connectedToPipe)
            {
                noError = CloseConnectionToRemoteClientPipe(rie, r);
            }

            if (connectedToIpcShare)
            {
                noError = DisconnectFromIPCShare(rie, credentials, usedComputerName);

            }
            if (connectedToAdminShare)
            {
                noError = DisconnectFromAdminShare(rie, credentials, usedComputerName);

            }
            return noError;
        }


        protected static Boolean ConnectToAdminShare(RemoteInstallEntity rie, Credentials credentials, out Boolean usedComputerName)
        {
            try
            {
                NetworkUtility.EstablishConnectionToAdminShare(credentials, rie.ComputerName);
                usedComputerName = true;
                return true;
            }
            catch (Exception)
            {
                usedComputerName = false;
                try
                {
                    NetworkUtility.EstablishConnectionToAdminShare(credentials, rie.IP);
                    return true;
                }
                catch (Exception)
                {
                    SetError(rie, "Failed to connect to remote admin share");
                    return false;
                }
            }
        }

        protected static Boolean ConnectToIPCShare(RemoteInstallEntity rie, Credentials credentials, Boolean usedComputerName)
        {
            try
            {
                NetworkUtility.EstablishConnectionToIPCShare(credentials, usedComputerName ? 
                    rie.ComputerName : rie.IP);
                return true;
            }
            catch (Exception)
            {
                SetError(rie, "Failed to connect to remote ipc share");
                return false;
            }
        }

        protected static Boolean CopyInstallFileToRemoteComputerAdminTempFolder(RemoteInstallEntity rie,
            Credentials credentials, Boolean usedComputerName)
        {
            try
            {
                SetStatus(rie, InstallationStatusEnum.Copying);
                FileUtility.CopyFileToRemoteComputerAdminTempFolder(usedComputerName ? rie.ComputerName : rie.IP,
                    rie.SourceFullPath, Path.GetFileName(rie.SourceFullPath));
                return true;
            }
            catch (Exception)
            {
                SetError(rie, "Failed to copy installation msi file to admin share");
                return false;
            }
        }        

        protected static Boolean ConnectToRemoteComputerUsingWmi(RemoteInstallEntity rie, Credentials credentials,
            ref WmiProvider provider, Boolean usedComputerName)
        {
            try
            {
                SetStatus(rie, InstallationStatusEnum.Connecting);
                provider = new WmiProvider(usedComputerName ? rie.ComputerName : rie.IP, credentials);
                provider.ConnectToRemoteComputer(TimeSpan.FromSeconds(10));
                return true;
            }
            catch (COMException)
            {
                SetError(rie, "RPC error occured. Check running services and firewall exclusions");
            }
            catch (UnauthorizedAccessException)
            {
                SetError(rie, "Cannot connect to remote computer with given credentials");
            }
            catch (Exception)
            {
                SetError(rie, "-");
            }
            return false;
        }

        protected static Boolean InstallProductUsingWmi(RemoteInstallEntity rie, WmiProvider provider)
        {
            try
            {
                SetStatus(rie, InstallationStatusEnum.Processing);
                provider.InstallProductWithMsiExec(MethodsCallMode, Path.GetFileName(rie.SourceFullPath),
                    InstallLogFileName, PollingTime, Timeout);
                return true;
            }
            catch
            {
                SetError(rie, "Failed during installing with wmi");
                return false;
            }
        }

        protected static Boolean UninstallProductUsingWmi(RemoteInstallEntity rie, WmiProvider provider)
        {
            try
            {
                SetStatus(rie, InstallationStatusEnum.Processing);
                provider.UninstallProductWithMsiExec(MethodsCallMode, rie.Guid, UninstallLogFileName,
    PollingTime, Timeout);

                return true;
            }
            catch
            {
                SetError(rie, "Failed during uninstalling with wmi");
                return false;
            }
        }

        protected static Boolean ParseLogFile(RemoteInstallEntity rie, String logFileName, Boolean usedComputerName)
        {
            try
            {
                SetStatus(rie, InstallationStatusEnum.Parsing);
                Int32 exitCode;
                String path = String.Format(@"\\{0}\ADMIN$\temp\{1}",
                    usedComputerName ? rie.ComputerName : rie.IP, logFileName);
                if (LogParser.GetCompletionInfo(path, out exitCode))
                {
                    SetStatus(rie, InstallationStatusEnum.Success, exitCode);
                }
                else
                {
                    SetStatus(rie, InstallationStatusEnum.Fail, exitCode);
                }
                return true;
            }
            catch
            {
                SetError(rie, "Failed during parsing result");
                return false;
            }
        }

        protected static Boolean DisconnectFromAdminShare(RemoteInstallEntity rie, Credentials credentials,
            Boolean usedComputerName)
        {
            try
            {
                NetworkUtility.AbolishConnectionToAdminShare(credentials, 
                     usedComputerName ? rie.ComputerName : rie.IP);
                return true;
            }
            catch (Exception)
            {
                SetError(rie, "Failed to disconnect from remote admin share");
                return false;
            }
        }
        protected static Boolean DisconnectFromIPCShare(RemoteInstallEntity rie, Credentials credentials,
            Boolean usedComputerName)
        {
            try
            {
                NetworkUtility.AbolishConnectionToIPCShare(credentials,
                     usedComputerName ? rie.ComputerName : rie.IP);
                return true;
            }
            catch (Exception)
            {
                SetError(rie, "Failed to disconnect from remote admin share");
                return false;
            }
        }

        protected static Boolean RebootRemoteComputerUsingWmi(RemoteInstallEntity rie, WmiProvider provider)
        {
            try
            {
                provider.RebootRemoteComputer();
                return true;
            }
            catch
            {
                SetError(rie, "Failed to initiate reboot");
                return false;
            }
        }

        protected static Boolean CopyServiceFileToRemoteComputerAdminTempFolder(RemoteInstallEntity rie,
            Credentials credentials, Boolean usedComputerName)
        {

            try
            {
                FileUtility.CopyFileToRemoteComputerAdminTempFolder(usedComputerName ? rie.ComputerName : rie.IP,
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Settings\vba32helperservice.exe"), "vba32helperservice.exe");
                return true;
            }
            catch (Exception)
            {
                SetError(rie, "Failed to copy service file to admin share");
                return false;
            }
        }

        protected static Boolean InstallAndStartRemoteService(RemoteInstallEntity rie, Credentials credentials,
            Boolean usedComputerName)
        {
            try
            {
                SetStatus(rie, InstallationStatusEnum.Connecting);
                ServiceUtility.InstallAndStartRemoteService(usedComputerName ? rie.ComputerName : rie.IP, "vba32helperservice",
                    @"%systemroot%\Temp\vba32helperservice.exe", new TimeSpan(0, 0, 20));
                return true;
            }
            catch (Exception)
            {
                SetError(rie, "Failed to install or start remote service");
                return false;
            }

        }

        protected static Boolean ConnectToRemoteClientPipe(RemoteInstallEntity rie, RemoteClient remoteClient, Boolean usedComputerName)
        {
            if (remoteClient.ConnectToRemoteClientPipe(usedComputerName ? rie.ComputerName : rie.IP))
            {
                return true;
            }
            else
            {                
                SetError(rie, "Failed to connect to remote pipe");
                return false;
            }
        }

        protected static Boolean GetWindowsDirectoryWirhRemoteClient(RemoteInstallEntity rie, RemoteClient remoteClient, 
            out String windir)
        {
            if (!remoteClient.GetWindowsDirectory(out windir))
            {
                SetError(rie, "Failed to get windows directory");
                return false;
            }
            else if (windir == "GetWindowsDirectory call failed.")
            {
                SetError(rie, "Failed to get windows directory");
                return false;
            }
            else
            {
                return true;
            }
        }


        protected static Boolean InstallProductUsingRemoteClient(RemoteInstallEntity rie, RemoteClient remoteClient,
            String windir)
        {
            SetStatus(rie, InstallationStatusEnum.Processing);
            String tmp = FileUtility.AppendTerminalBackslash(windir) + "temp";
            String commandLine = CommandLine.Install(tmp,
                Path.GetFileName(rie.SourceFullPath), false,
                LogLevel.Verbose | LogLevel.AllExceptVerbose, InstallLogFileName);

            UInt32 errorCode, exitCode;
            if (!remoteClient.RunProcessCommand(commandLine, out errorCode, out exitCode))
            {
                SetError(rie, "Failed to initiate install by remote service");
                return false;
            }
            else
            {
                if (errorCode != 0)
                {
                    SetError(rie, String.Format("Remote service failed to run remote process with {0} windows "
                    + "error", errorCode));
                    return false;
                }
                else
                {
                    if (exitCode == 3010 || exitCode == 0)
                    {                        
                        SetStatus(rie, InstallationStatusEnum.Success, (Int32)exitCode);
                    }
                    else
                    {
                        SetStatus(rie, InstallationStatusEnum.Fail, (Int32)exitCode);
                    }
                    return true;
                }
            }
        }

        protected static Boolean UninstallProductUsingRemoteClient(RemoteInstallEntity rie, RemoteClient remoteClient,
            String windir)
        {
            SetStatus(rie, InstallationStatusEnum.Processing);
            String tmp = FileUtility.AppendTerminalBackslash(windir) + "temp";
            String commandLine = CommandLine.Uninstall(tmp, rie.Guid, false,
                LogLevel.Verbose | LogLevel.AllExceptVerbose, UninstallLogFileName);

            UInt32 errorCode, exitCode;
            if (!remoteClient.RunProcessCommand(commandLine, out errorCode, out exitCode))
            {
                SetError(rie, "Failed to initiate uninstall by remote service");
                return false;
            }
            else
            {
                if (errorCode != 0)
                {
                    SetError(rie, String.Format("Remote service failed wtih {0} windows error", errorCode));
                    return false;
                }
                else
                {
                    if (exitCode == 3010 || exitCode == 0)
                    {
                        SetStatus(rie, InstallationStatusEnum.Success, (Int32)exitCode);
                    }
                    else
                    {
                        SetStatus(rie, InstallationStatusEnum.Fail, (Int32)exitCode);
                    }
                    return true;
                }
            }
        }

        protected static Boolean RebootRemoteComputerRemoteClient(RemoteInstallEntity rie, RemoteClient remoteClient)
        {
            if (!remoteClient.RebootCommand())
            {
                SetError(rie, "Failed to initiate reboot");
                return false;
            }
            return true;
        }
        protected static Boolean CloseConnectionToRemoteClientPipe(RemoteInstallEntity rie, RemoteClient remoteClient)
        {
            if (!remoteClient.CloseConnectionToPipe())
            {
                SetError(rie, "Failed to close connection to remote pipe");
                return false;
            }
            return true;
        }

        public static Boolean Install(RemoteInstallEntity rie, Credentials credentials, Boolean doRestart,
            RemoteMethodsEnum installMethod)
        {
            if (installMethod == RemoteMethodsEnum.Wmi)
            {
                return InstallWithWmi(rie, credentials, doRestart);
            }
            else //scanMethod == RemoteScanMethodsEnum.RemoteService
            {
                return InstallWithRemoteService(rie, credentials, doRestart);
            }
        }

        public static Boolean Uninstall(RemoteInstallEntity rie, Credentials credentials, Boolean doRestart,
            RemoteMethodsEnum uninstallMethod)
        {
            if (uninstallMethod == RemoteMethodsEnum.Wmi)
            {
                return UninstallWithWmi(rie, credentials, doRestart);
            }
            else //scanMethod == RemoteScanMethodsEnum.RemoteService
            {
                return UninstallWithRemoteService(rie, credentials, doRestart);
            }
        }
    }
}
