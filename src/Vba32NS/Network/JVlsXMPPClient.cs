using System;
using System.Collections.Generic;
using System.Text;

using Vls.JVC;

using System.Threading;

namespace Vba32.ControlCenter.NotificationService.Network
{
    /// <summary>
    /// 
    /// </summary>
    public class JVlsXMPPClient:JabberClient
    {
        private JVlsClient xmpp;

        public JVlsXMPPClient(string server, string fromJID, string password)
        {
            LogMessage("JVlsXMPPClient.JVlsXMPPClient started", 15);
            this.Server = server;
            this.FromJID = fromJID.Split('@')[0]; 
            this.Password = password;
        }

        public override void OpenConnection()
        {
            LogMessage("JVlsXMPPClient.OpenConnection started. Object locked", 15);
            lock (this)
            {
                OpenConnection(Server, FromJID, Password);
            }
            LogMessage("JVlsXMPPClient.OpenConnection finished. Object unlocked", 15);
        }

        public override void OpenConnection(string server, string fromJID, string password)
        {
            string jid = fromJID.Split('@')[0];

            xmpp = new JVlsClient(server, jid, password);
            xmpp.Resource = "";
            xmpp.Authenticate += new EventHandler(xmpp_Authenticate);
            xmpp.Error += new Vls.JVC.ErrorEventHandler(xmpp_Error);
            xmpp.ReceiveMessage += new MessageReceiveEventHandler(xmpp_ReceiveMessage);

            Thread thread = new Thread(delegate()
            {
                try
                {
                    xmpp.StartSession(UserStatus.Online, "Vba32 Notification service");
                }
                catch (Exception ex)
                {
                    LogMessage("Exception in StartSession: "+ex, 10);
                }

            });
            thread.Start();
            LogMessage("Wait 40 seconds for connection..", 20);
            if (thread.Join(TimeSpan.FromSeconds(40)))
            {
                LogMessage("Connection thread is successfully completed", 20);
            }
            else
            {
                LogMessage("Timeout expired. Aborting thread", 10);
                thread.Abort();
            }

            
        }

        public override bool CheckConnectionState()
        {
            LogMessage("JVlsXMPPClient.CheckConnectionState started", 15);
            LogMessage("Current state is"+
                xmpp.State.ToString(), 15);
            return ((xmpp != null) && (xmpp.State == JabberClientState.SessionStarted)); ;
        }

        public override void CloseConnection()
        {
            LogMessage("JVlsXMPPClient.CloseConnection started. Object locked", 15);
            lock (this)
            {
                if (xmpp != null)
                    xmpp.Close();
            }
            LogMessage("JVlsXMPPClient.CloseConnection finished. Object unlocked", 15);
        }

        public override void Send(string toJID, string message)
        {
            LogMessage("JVlsXMPPClient.Send started", 15);
            if (CheckConnectionState())
                xmpp.Send(toJID, message);
            else
            {
                LogMessage("JVlsXMPPClient.Send():: Connection state is "+
                    xmpp.State.ToString() , 10);
            }
        }

        #region Events handlers

        void xmpp_ReceiveMessage(MessageEventArgs e)
        {
            LogMessage(String.Format("Ooops! I receive message='{1}' from '{0}' ",
                e.From,e.Body), 66);

        }

        void xmpp_Error(JVlsEventArgs e)
        {
            LogMessage("Error occured", 10);
        }

        void xmpp_Authenticate(object sender, EventArgs e)
        {
            LogMessage("Authenticate success",20);
        }

        #endregion

        protected void LogMessage(string body, int level)
        {
            Vba32NS.LogMessage(body, level);
        }

    }
}
