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

        private String server;

        public String Server
        {
            get { return server; }
            set { server = value; }
        }
        private String fromJID;

        public String FromJID
        {
            get { return fromJID; }
            set { fromJID = value; }
        }
        private String password;

        public String Password
        {
            get { return password; }
            set { password = value; }
        }

        #endregion

        public abstract void OpenConnection();
        public abstract void OpenConnection(String server, String fromJID, String password);
        public abstract Boolean CheckConnectionState();
        public abstract void CloseConnection();
        public abstract void Send(String toJID, String message);

        public Boolean IsNeedReconnect(String server, String jid, String password)
        {
            return !((Server == server) && (FromJID == jid) && (Password == password));
        }

    }
}
