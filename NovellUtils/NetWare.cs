using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace NovellUtils
{
    public static class NetWare
    {
        public static string GetConnectionsLog()
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = @"z:\map.exe",
                    Arguments = "",//@"/c z:\map",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = false,
                    StandardOutputEncoding = Encoding.GetEncoding(866)
                }
            };
            

            proc.Start();
            
            proc.WaitForExit();
            var netUseLog = proc.StandardOutput.ReadToEnd();                                 
            proc.Close();
            return netUseLog;
        }

        public static bool ConnectToNovell()
        {
            const int defaultLoginTimeOut = 5000;
            const string loginProcessName = "loginw32";
            bool res;
            string novellConnectionScript = System.Configuration.ConfigurationManager.AppSettings["NovellConnection"];
            string connectionTimeOut = System.Configuration.ConfigurationManager.AppSettings["ConnectionTimeOutInMilliseconds"];
            int novellConnectionTimeOut;
            if (!int.TryParse(connectionTimeOut, out novellConnectionTimeOut))
                novellConnectionTimeOut = defaultLoginTimeOut;
            using (var proc = Process.Start(novellConnectionScript))
            {
                res = proc.WaitForExit(novellConnectionTimeOut);
                proc.Close();
            }

            // Убиваем все висящие процессы логина
            KillProcesses(loginProcessName);

            return res;
        }

        static void KillProcesses(string procName) {
            foreach (var proc in Process.GetProcessesByName(procName))
                proc.Kill();
        }
    }
}
