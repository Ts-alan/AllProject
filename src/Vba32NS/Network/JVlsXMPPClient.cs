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

        public JVlsXMPPClient(String server, String fromJID, String password)
        {
            LoggerNS.log.Info("JVlsXMPPClient.JVlsXMPPClient started");
            this.Server = server;
            this.FromJID = fromJID.Split('@')[0]; 
            this.Password = password;
        }

        public override void OpenConnection()
        {
            LoggerNS.log.Info("JVlsXMPPClient.OpenConnection started. Object locked");
            lock (this)
            {
                OpenConnection(Server, FromJID, Password);
            }
            LoggerNS.log.Info("JVlsXMPPClient.OpenConnection finished. Object unlocked");
        }

        public override void OpenConnection(String server, String fromJID, String password)
        {
            String jid = fromJID.Split('@')[0];

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
                    LoggerNS.log.Info("Exception in StartSession: "+ex);
                }

            });
            thread.Start();
            LoggerNS.log.Info("Wait 40 seconds for connection..");
            if (thread.Join(TimeSpan.FromSeconds(40)))
            {
                LoggerNS.log.Info("Connection thread is successfully completed");
            }
            else
            {
                LoggerNS.log.Info("Timeout expired. Aborting thread");
                thread.Abort();
            }

            
        }

        public override bool CheckConnectionState()
        {
            LoggerNS.log.Info("JVlsXMPPClient.CheckConnectionState started");
            LoggerNS.log.Info("Current state is"+
                xmpp.State.ToString());
            return ((xmpp != null) && (xmpp.State == JabberClientState.SessionStarted)); ;
        }

        public override void CloseConnection()
        {
            LoggerNS.log.Info("JVlsXMPPClient.CloseConnection started. Object locked");
            lock (this)
            {
                if (xmpp != null)
                    xmpp.Close();
            }
            LoggerNS.log.Info("JVlsXMPPClient.CloseConnection finished. Object unlocked");
        }

        public override void Send(String toJID, String message)
        {
            LoggerNS.log.Info("JVlsXMPPClient.Send started");
            if (CheckConnectionState())
                xmpp.Send(toJID, message);
            else
            {
                LoggerNS.log.Info("JVlsXMPPClient.Send():: Connection state is "+
                    xmpp.State.ToString());
            }
        }

        #region Events handlers

        void xmpp_ReceiveMessage(MessageEventArgs e)
        {
            LoggerNS.log.Info(String.Format("Ooops! I receive message='{1}' from '{0}' ",
                e.From,e.Body));

        }

        void xmpp_Error(JVlsEventArgs e)
        {
            LoggerNS.log.Info("Error occured");
        }

        void xmpp_Authenticate(object sender, EventArgs e)
        {
            LoggerNS.log.Info("Authenticate success");
        }

        #endregion

    }
}
