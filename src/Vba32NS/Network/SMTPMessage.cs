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
        static bool mailSent = true;

        public bool Send(string server, string fromMail,string displayName,
            string toMail, string  subject, string body, MailPriority priority)
        {
            LogMessage("SMTPMessage.Send():: Started ", 15);

            // Command line argument must the the SMTP host.
            SmtpClient client = new SmtpClient(server);
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

            string userState = "Successfully";
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
            LogMessage("SMTPMessage.SendCompletedCallback():: Started ", 15);
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Error != null)
            {
               LogMessage("Vba32NS, SMTPSend: " + e.Error.ToString(),10);
                lock (this)
                {
                    mailSent = false;
                }
            }
            onEvent.Set();
        }

        protected void LogMessage(string body, int level)
        {
            Vba32NS.LogMessage(body, level);
        }


    }
}
