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
            LogMessage("AgsXMPPClient.AgsXMPPClient started", 15);
            this.Server = server;
            this.FromJID = fromJID;
            this.Password = password;
        }

        public override void OpenConnection()
        {
            LogMessage(String.Format("AgsXMPPClient.OpenConnection():: server={0}, jid={1}, pass={2}",
                Server,FromJID,Password),10);
            lock (this)
            {
                OpenConnection(this.Server, this.FromJID, this.Password);
            }
        }

        public override void OpenConnection(string server, string fromJID, string password)
        {

            LogMessage("AgsXMPPClient.OpenConnection started",15);

            ManualResetEvent onEvent = new ManualResetEvent(false);
            Exception exception = null;
            try
            {
                Jid jidSender = new Jid(fromJID);
                xmpp = new XmppClientConnection(server);

                xmpp.OnAuthError += delegate(object sender, Element e)
                {
                    Vba32NS.LogMessage("AgsXMPPClient.OpenConnection:: OnAuthError: Ошибка при аутентификации. CloseConnection and throw exception ",15);
                    CloseConnection();
                    throw new Exception("AgsXMPPClient.OpenConnection::Ошибка при аутентификации ");

                };

                xmpp.OnError += delegate(object sender, Exception ex)
                {
                    LogMessage("AgsXMPPClient.OpenConnection:: OnError:" + ex.Message,10);
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
                    LogMessage("AgsXMPPClient.OnSocketError:: " + ex.Message,10);
                    CloseConnection();
                };

                xmpp.OnXmppError += delegate(object sender, Element e)
                {
                    LogMessage("AgsXMPPClient.OnXmppError",10);
                    CloseConnection();
                };

                //

                xmpp.OnLogin += delegate(object o)
                {
                    onEvent.Set();
                    LogMessage("Logged In",20);
                };
                xmpp.Open(jidSender.User, password);

                LogMessage("Wait for Login...",20);
                onEvent.WaitOne(TimeSpan.FromSeconds(20), false);

                if (exception != null)
                {
                    LogMessage("AgsXMPPClient.OpenConnection:: Exception variably is not null",20);
                    return;
                }
                onEvent.Reset();
            }
            catch (Exception ex)
            {
                LogMessage("AgsXMPPClient.OpenConnection:: " + ex.Message,20);
                return;
            }
        }

        public override void CloseConnection()
        {
            LogMessage("AgsXMPPClient.CloseConnection started", 15);
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
                LogMessage("AgsXMPPClient.CloseConnection:: " + ex.Message,10);
            }
        }

        public override void Send(string toJID, string message)
        {
            LogMessage("AgsXMPPClient.Send started", 15);
            try
            {
                xmpp.Send(new Message(new Jid(toJID), MessageType.chat, message));
            }
            catch (Exception ex)
            {
                LogMessage("AgsXMPPClient.Send():: " + ex.Message,10);
                return ;
            }
            return ;
        }

        public override bool CheckConnectionState()
        {
            LogMessage("AgsXMPPClient.CheckConnectionState started", 15);
            if ((xmpp.XmppConnectionState == XmppConnectionState.SessionStarted) ||
                    (xmpp.XmppConnectionState == XmppConnectionState.Connected))
                return true;

            return false;
        }

        protected void LogMessage(string body, int level)
        {
            Vba32NS.LogMessage(body, level);
        }
    }
}
