using System;
using System.Collections.Generic;
using System.Text;

using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using System.ComponentModel;

using System.Diagnostics;

namespace Vba32.ControlCenter.NotificationService.Network
{
    public class SMTPMessage
    {
        private ManualResetEvent onEvent = new ManualResetEvent(false);
        static Boolean mailSent = true;

        public Boolean Send(String server, String fromMail,String displayName,
            String toMail, String  subject, String body, MailPriority priority, NetworkCredential credential)
        {
            LoggerNS.log.Info("SMTPMessage.Send():: Started ");

            // Command line argument must the the SMTP host.
            SmtpClient client = new SmtpClient(server);
            if (credential != null)
            {
                client.UseDefaultCredentials = false;
                client.Credentials = credential;
            }
            // Specify the e-mail sender.
            // Create a mailing address that includes a UTF8 character
            // in the display name.
            MailAddress from = new MailAddress(fromMail, displayName, System.Text.Encoding.UTF8);
            // Set destinations for the e-mail message.
            MailAddress to = new MailAddress(toMail);
            // Specify the message content.
            MailMessage message = new MailMessage(from, to);
            message.Body = body;
            message.Priority = priority;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Subject = subject;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            // Set the method that is called back when the send operation ends.
            client.SendCompleted += new  SendCompletedEventHandler(SendCompletedCallback);

            lock (this)
            {
                mailSent = true;
            }

            String userState = "Successfully";
            client.SendAsync(message, userState);

            if(!onEvent.WaitOne(TimeSpan.FromSeconds(6),false))
            {
                client.SendAsyncCancel();
            }
            message.Dispose();
            return mailSent;
        }

        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            LoggerNS.log.Info("SMTPMessage.SendCompletedCallback():: Started ");
            // Get the unique identifier for this asynchronous operation.
            String token = (String)e.UserState;

            if (e.Error != null)
            {
               LoggerNS.log.Info("Vba32NS, SMTPSend: " + e.Error.ToString());
                lock (this)
                {
                    mailSent = false;
                }
            }
            onEvent.Set();
        }        
    }
}
