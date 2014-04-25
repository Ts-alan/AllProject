using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.RemoteOperations.Common
{
    public struct Credentials
    {
        public string Username;
        public string Password;
        public string Domain;
        public Credentials(string domain, string username, string password)
        {
            Username = username;
            Domain = domain;
            Password = password;
        }
    }
}
