﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceProcess;
using Microsoft.Win32;

namespace Vba32ControlCenterUpdate
{
    internal static class UpdateActions
    {
        private static SortedDictionary<String, String> services;
        private const String gc_database_regkey = "SOFTWARE\\Vba32\\ControlCenter\\DataBase";
        
        static UpdateActions()
        {
            services = new SortedDictionary<String, String>();
            services.Add("Vba32CCService.exe", "VbaControlCenter");
            services.Add("packet_parser.dll", "VbaControlCenter");
            services.Add("Vba32PMS.exe", "Vba32PMS");
            services.Add("Vba32SS.exe", "Vba32SS");
            services.Add("Vba32SS.exe.config", "Vba32SS");
            services.Add("Vba32NS.exe", "Vba32NS");
            services.Add("JVlsXMPPClient.dll", "Vba32NS");
            services.Add("agsXMPP.dll", "Vba32NS");
            services.Add("VbaTaskAssignment.exe", "VbaTaskAssignment");
            services.Add("VbaTaskAssignmentPS.dll", "VbaTaskAssignment");
        }

        #region Methods

        internal static Boolean ActionBeforeReplaceFiles(String[] files)
        {
            Logger.Debug(String.Format("ActionBeforeReplaceFiles() :: enter (files count: {0})", files.Length));
            foreach (String serviceName in GetServices(files))
            {
                StopService(serviceName);
            }

            return true;
        }

        internal static Boolean ActionAfterReplaceFiles(String[] files, String currentVersion, String newVersion)
        {
            Logger.Debug(String.Format("ActionAfterReplaceFiles() :: enter (files count: {0}, currentVersion: {1}, newVersion: {2})", files.Length, currentVersion, newVersion));
            //Start the services
            foreach (String serviceName in GetServices(files))
            {
                StartService(serviceName);
            }

            //Update DB
            IPatchUpdate updater = DBPatchFactory.GetPatch(newVersion);
            if (updater == null)
            {
                Logger.Debug("Can't get Updater.");
                return false;
            }

            String errorVersion;
            if (!updater.Update(currentVersion, GetConnectionString(), out errorVersion))
                return false;

            return true;
        }
        
        #region Helper Methods

        /// <summary>
        /// Get service names by file names
        /// </summary>
        /// <param name="files">file names</param>
        /// <returns>list of service names</returns>
        private static List<String> GetServices(String[] files)
        {
            List<String> list = new List<String>();
            foreach (String file in files)
            {
                if (services.ContainsKey(file))
                {
                    if (!list.Contains(services[file]))
                        list.Add(services[file]);
                }
            }
            return list;
        }

        /// <summary>
        /// Start the service
        /// </summary>
        /// <param name="serviceName">service name</param>
        /// <returns></returns>
        private static Boolean StartService(String serviceName)
        {
            ServiceController sc = new ServiceController(serviceName);

            if (sc.Status == ServiceControllerStatus.Stopped)
            {
                try
                {
                    sc.Start();
                    sc.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 5));
                }
                catch (InvalidOperationException e)
                {
                    Logger.Error(String.Format("Could not start the \"{0}\" service ({1}).", serviceName, e.Message));
                    return false;
                }
            }

            Logger.Debug(String.Format("Started the \"{0}\" service.", serviceName));
            return true;
        }

        /// <summary>
        /// Stop the service
        /// </summary>
        /// <param name="serviceName">service name</param>
        /// <returns></returns>
        private static Boolean StopService(String serviceName)
        {
            ServiceController sc = new ServiceController(serviceName);

            if (sc.Status == ServiceControllerStatus.Running)
            {
                try
                {
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 5));
                }
                catch (InvalidOperationException e)
                {
                    Logger.Error(String.Format("Could not stop the \"{0}\" service ({1}).", serviceName, e.Message));
                    return false;
                }
            }

            Logger.Debug(String.Format("Stopped the \"{0}\" service.", serviceName));
            return true;
        }

        /// <summary>
        /// Get connectionString
        /// </summary>
        /// <returns></returns>
        private static String GetConnectionString()
        {
            String connection_string = "SERVER=";
            connection_string += ReadServerName();
            connection_string += ";DATABASE=vbaControlCenterDB;UID=";
            connection_string += ReadUserName();
            connection_string += ";PWD=";
            connection_string += ReadPassword();
            return connection_string;
        }

        private static String ReadServerName()
        {
            return ReadRegistryValue(Registry.LocalMachine, gc_database_regkey, "DataSource");
        }

        private static String ReadUserName()
        {
            return ReadRegistryValue(Registry.LocalMachine, gc_database_regkey, "UserName");
        }

        private static String ReadPassword()
        {
            return DecryptBinaryToString(Registry.LocalMachine, gc_database_regkey, "Password");
        }

        private static String ReadRegistryValue(RegistryKey registry_key, String sub_key, String var_name)
        {
            String result = "";
            RegistryKey reg_key = registry_key;
            try
            {
                reg_key = reg_key.OpenSubKey(sub_key);
                result = reg_key.GetValue(var_name).ToString();
            }
            catch (Exception e)
            {
                Logger.Error("ReadRegistryValue() :: " + e.Message);
            }
            return result;
        }

        private static String DecryptBinaryToString(RegistryKey registry_key, String sub_key, String var_name)
        {
            RegistryKey reg_key = registry_key;
            String password = "";
            try
            {
                reg_key = reg_key.OpenSubKey(sub_key);
                Byte[] buffer = (Byte[])reg_key.GetValue(var_name);
                Int32 buffer_length = buffer.Length;
                for (Int32 i = 0; i < buffer_length; ++i)
                {
                    buffer[i] ^= 0x17;
                }
                password = System.Text.Encoding.UTF8.GetString(buffer);
            }
            catch (Exception e)
            {
                Logger.Error("DecryptBinaryToString() :: " + e.Message);
            }
            return password;
        }

        #endregion

        #endregion
    }
}
