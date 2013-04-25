using System;
using System.Collections.Generic;
using System.Text;

namespace Vba32.ControlCenter.NotificationService.Network
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class JabberClient
    {
        #region Property

        private string server;

        public string Server
        {
            get { return server; }
            set { server = value; }
        }
        private string fromJID;

        public string FromJID
        {
            get { return fromJID; }
            set { fromJID = value; }
        }
        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        #endregion

        public abstract void OpenConnection();
        public abstract void OpenConnection(string server, string fromJID, string password);
        public abstract bool CheckConnectionState();
        public abstract void CloseConnection();
        public abstract void Send(string toJID, string message);

        public bool IsNeedReconnect(string server, string jid, string password)
        {
            return !((Server == server) && (FromJID == jid) && (Password == password));
        }

    }
}
