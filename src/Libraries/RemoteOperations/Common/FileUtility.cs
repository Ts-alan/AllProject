using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace VirusBlokAda.CC.RemoteOperations.Common
{
    /// <summary>
    /// Class containing helper functions
    /// </summary>
    public static class FileUtility
    {
        /// <summary>
        /// Appends terminal backslash to given string
        /// </summary>
        /// <param name="path">String representing path</param>
        /// <returns>String ending with backslash</returns>
        public static string AppendTerminalBackslash(string path)
        {
            if (path.IndexOf(Path.DirectorySeparatorChar, path.Length - 1) == -1)
            {
                return path + Path.DirectorySeparatorChar;
            }
            else
            {
                return path;
            }
        }

        /// <summary>
        /// Create directory, if it doesn't exist
        /// </summary>
        /// <param name="path">Path with directory name</param>
        /// <returns>True if directory was created</returns>
        public static bool CreateDirectory(string path)
        {
            if (Directory.Exists(path))
                return false;
            Directory.CreateDirectory(path);
            return true;
        }

        public static void CopyFileToRemoteComputerAdminTempFolder(string remoteMachineName, string sourceFullPath,
            string destFileName)
        {
            string RemoteAdminTempDirectory = String.Format(@"\\{0}\ADMIN$\temp", remoteMachineName);
            if (!Directory.Exists(RemoteAdminTempDirectory))
                Directory.CreateDirectory(RemoteAdminTempDirectory);

            string copyFile = RemoteAdminTempDirectory + "\\" + destFileName;

            if (File.Exists(copyFile))
            {
                try
                {
                    File.Delete(copyFile);
                    File.Copy(sourceFullPath, copyFile);
                }
                catch (UnauthorizedAccessException)
                {
                    //Service is in use
                }
            }
            else
            {
                File.Copy(sourceFullPath, copyFile);
            }
        }

        public static string RemoteSystemTempDirectory
        {
            get { return @"C:\Windows\Temp"; }
        }
    }

}