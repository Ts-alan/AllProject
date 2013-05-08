using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using VirusBlokAda.RemoteOperations.Common;

namespace VirusBlokAda.RemoteOperations.RemoteInstall.MsiexecHelper
{
    /// <summary>
    /// Helper class to generate specific msiexec command line options
    /// using given parameters
    /// </summary>
    class CommandLine
    {
        /// <summary>
        /// Generates install msiexec command line
        /// </summary>
        /// <param name="installDirectory">Directory of msi file</param>
        /// <param name="msiName">Name of msi file</param>
        /// <param name="doRestart">If true target pc will be forced to reboot after installation</param>
        /// <param name="logLevel">Logging level</param>
        /// <param name="logName">Name of log file</param>
        /// <returns>Generated command line</returns>
        public static String Install(String installDirectory, String msiName, String configName, Boolean doRestart, LogLevel logLevel, String logName)
        {
            StringBuilder sb = new StringBuilder("msiexec.exe ");
            sb.Append("/i \"");
            sb.Append(FileUtility.AppendTerminalBackslash(installDirectory));
            sb.Append(msiName);
            sb.Append("\" /q");
            if (!String.IsNullOrEmpty(configName))
            {
                sb.Append(" CC_CFG_FILEPATH=\"");
                sb.Append(FileUtility.AppendTerminalBackslash(installDirectory));
                sb.Append(configName);
                sb.Append("\"");
            }
            if (doRestart)
            {
                sb.Append(" /forcerestart ");
            }
            else
            {
                sb.Append(" /norestart ");
            }
            sb.Append(FormLogFileParameters(logLevel, FileUtility.AppendTerminalBackslash(installDirectory) + logName));
                        
            return sb.ToString();
        }
        /// <summary>
        /// Generates uninstall msiexec command line
        /// </summary>
        /// <param name="installDirectory">Directory of msi file</param>
        /// <param name="guid">Product guid</param>
        /// <param name="doRestart">If true target pc will be forced to reboot after installation</param>
        /// <param name="logLevel">Logging level</param>
        /// <param name="logName">Name of log file</param>
        /// <returns>Generated command line</returns>
        public static String Uninstall(String installDirectory, String guid, Boolean doRestart, LogLevel logLevel, String logName)
        {
            StringBuilder sb = new StringBuilder("msiexec.exe ");
            sb.Append("/x \"");
            sb.Append(guid);
            sb.Append("\" /q");
            if (doRestart)
            {
                sb.Append(" /forcerestart ");
            }
            else
            {
                sb.Append(" /norestart ");
            }
            sb.Append(FormLogFileParameters(logLevel, FileUtility.AppendTerminalBackslash(installDirectory) + logName));
            return sb.ToString();
        }

        /// <summary>
        /// Generates log part of msiexec command line options
        /// </summary>
        /// <param name="logLevel">Logging level</param>
        /// <param name="logPath">Path to log file</param>
        /// <returns>Generated part of command line</returns>
        private static String FormLogFileParameters(LogLevel logLevel, String logPath)
        {
            if (String.IsNullOrEmpty(logPath))
            {
                throw new ArgumentException("logPath can't be null or empty");
            }

            if (logLevel == LogLevel.None)
            {
                return String.Empty;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("/l");
            if ((logLevel & LogLevel.AllExceptVerbose) != 0)
            {
                sb.Append('*');
            }
            else if ((logLevel & LogLevel.Status) != 0)
            {
                sb.Append('i');
            }
            if ((logLevel & LogLevel.Verbose) != 0)
            {
                sb.Append('v');
            }
            sb.Append(" \"" + logPath + "\"");
            return sb.ToString();
        }
    }
}

