using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;
using ListOfProcess.Services;

namespace ListOfProcess.ConnectionLogic
{
    public class ConnectionLogic : IConnection
    {
        ObjectCache cache = MemoryCache.Default;
     
        public List<object> GetProcessOnLocalMachine()
        {
            ManagementScope scope = new ManagementScope("\\\\localhost\\root\\cimv2");
            scope.Connect();
            WqlObjectQuery query = new WqlObjectQuery("Select * from Win32_Process");
            ManagementObjectSearcher find = new ManagementObjectSearcher(query);
            List<object> list = new List<object>();
            cache.Set("ManagementScope", scope, null, null);
            foreach (ManagementObject service in find.Get())
            {
                list.Add(service["Name"]);
            }
     
            return list;
            
        }

        public IEnumerable<object> GetProcessOnRemoteMachine(string username, string password, string ip, out Exception ex)
        {
            List<object> list = new List<object>();
            try
            {

                ConnectionOptions options = new ConnectionOptions();
                options.Username = username;
                options.Password = password;
                ManagementScope scope = new ManagementScope("\\\\" + ip + "\\root\\cimv2", options);
                scope.Connect();
                WqlObjectQuery query = new WqlObjectQuery("Select * from Win32_Process");
                ManagementObjectSearcher find = new ManagementObjectSearcher(query);
                cache.Set("ManagementScope", scope, null, null); 
                foreach (ManagementObject service in find.Get())
                {
                    list.Add(service["Name"]);
                }
                ex = null;
            }
            catch(Exception e){

                ex=e;
            }

            return list;
        }

        public void StartProcess(string process)
        {
            ManagementScope scope = (ManagementScope)cache.Get("ManagementScope");
            ManagementPath managementPath = new ManagementPath("Win32_Process");
            object[] theProcessToRun = {process};
            ManagementClass theClass = new ManagementClass(scope, managementPath, null);
            theClass.InvokeMethod("Create", theProcessToRun);
        }
       
    }
}