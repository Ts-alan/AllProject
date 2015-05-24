using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ListOfProcess.Services
{
    public interface IConnection
    {
         List<object> GetProcessOnLocalMachine();
         IEnumerable<object> GetProcessOnRemoteMachine(string username, string password, string ip,out Exception ex);
         void StartProcess(string process);
    }
}
