using System;
using System.Collections.Generic;
using System.Text;

using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.Collections;
using agsXMPP.protocol.iq.roster;
using agsXMPP.Xml.Dom;

using System.Threading;
using System.Diagnostics;

namespace Vba32.ControlCenter.NotificationService.Network
{
    /// <summary>
    /// Фабрика над сторонней библиотекой
    /// </summary>
    public class AgsXMPPClient:JabberClient
    {
        private XmppClientConnection xmpp;



        public AgsXMPPClient(string server, string fromJID, string password)
        {
            LoggerNS.log.Info("AgsXMPPClient.AgsXMPPClient started");
            this.Server = server;
            this.FromJID = fromJID;
            this.Password = password;
        }

        public override void OpenConnection()
        {
            LoggerNS.log.Info(String.Format("AgsXMPPClient.OpenConnection():: server={0}, jid={1}, pass={2}",
                Server,FromJID,Password));
            lock (this)
            {
                OpenConnection(this.Server, this.FromJID, this.Password);
            }
        }

        public override void OpenConnection(string server, string fromJID, string password)
        {

            LoggerNS.log.Info("AgsXMPPClient.OpenConnection started");

            ManualResetEvent onEvent = new ManualResetEvent(false);
            Exception exception = null;
            try
            {
                Jid jidSender = new Jid(fromJID);
                xmpp = new XmppClientConnection(server);

                xmpp.OnAuthError += delegate(object sender, Element e)
                {
                    LoggerNS.log.Info("AgsXMPPClient.OpenConnection:: OnAuthError: Ошибка при аутентификации. CloseConnection and throw exception ");
                    CloseConnection();
                    throw new Exception("AgsXMPPClient.OpenConnection::Ошибка при аутентификации ");

                };

                xmpp.OnError += delegate(object sender, Exception ex)
                {
                    LoggerNS.log.Info("AgsXMPPClient.OpenConnection:: OnError:" + ex.Message);
                    onEvent.Set();
                    exception = ex;
                    CloseConnection();
                };

                xmpp.OnSocketError += delegate(object sender, Exception ex)
                {
                    //Данная ошибка проявляется, если сервер не доступен
                    //Но объект xmpp не будет равен null и вне этого метода
                    // будет считаться, что все ок и не будет пересоединяться.
                    //Поэтому присвоим тут явно null
                    LoggerNS.log.Info("AgsXMPPClient.OnSocketError:: " + ex.Message);
                    CloseConnection();
                };

                xmpp.OnXmppError += delegate(object sender, Element e)
                {
                    LoggerNS.log.Info("AgsXMPPClient.OnXmppError");
                    CloseConnection();
                };

                //

                xmpp.OnLogin += delegate(object o)
                {
                    onEvent.Set();
                    LoggerNS.log.Info("Logged In");
                };
                xmpp.Open(jidSender.User, password);

                LoggerNS.log.Info("Wait for Login...");
                onEvent.WaitOne(TimeSpan.FromSeconds(20), false);

                if (exception != null)
                {
                    LoggerNS.log.Info("AgsXMPPClient.OpenConnection:: Exception variably is not null");
                    return;
                }
                onEvent.Reset();
            }
            catch (Exception ex)
            {
                LoggerNS.log.Info("AgsXMPPClient.OpenConnection:: " + ex.Message);
                return;
            }
        }

        public override void CloseConnection()
        {
            LoggerNS.log.Info("AgsXMPPClient.CloseConnection started");
            try
            {
                if ((xmpp != null) ||
                    (xmpp.XmppConnectionState == XmppConnectionState.SessionStarted) ||
                    (xmpp.XmppConnectionState == XmppConnectionState.Connected)
                    )
                {
                    lock (this)
                    {
                        xmpp.Close();
                        xmpp.SocketDisconnect();
                    }
                    //xmpp = null;
                }
            }
            catch (Exception ex)
            {
                LoggerNS.log.Info("AgsXMPPClient.CloseConnection:: " + ex.Message);
            }
        }

        public override void Send(string toJID, string message)
        {
            LoggerNS.log.Info("AgsXMPPClient.Send started");
            try
            {
                xmpp.Send(new Message(new Jid(toJID), MessageType.chat, message));
            }
            catch (Exception ex)
            {
                LoggerNS.log.Info("AgsXMPPClient.Send():: " + ex.Message);
                return ;
            }
            return ;
        }

        public override bool CheckConnectionState()
        {
            LoggerNS.log.Info("AgsXMPPClient.CheckConnectionState started");
            if ((xmpp.XmppConnectionState == XmppConnectionState.SessionStarted) ||
                    (xmpp.XmppConnectionState == XmppConnectionState.Connected))
                return true;

            return false;
        }
    }
}
