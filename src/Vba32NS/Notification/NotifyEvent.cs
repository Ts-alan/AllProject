using System;
using System.Collections.Generic;
using System.Text;

namespace Vba32.ControlCenter.NotificationService.Notification
{
    public class NotifyEvent
    {
        #region Variables
        
        private String eventName = String.Empty;
        private Boolean isNotify = false;
        private NotifyTypeAddrList netSend = new NotifyTypeAddrList(NotifyType.NetSend, false, new List<String>(), String.Empty, "net send message");
        private NotifyTypeAddrList jabber = new NotifyTypeAddrList(NotifyType.Jabber, false, new List<String>(), String.Empty,"jabber message");
        private NotifyTypeAddrList mail = new NotifyTypeAddrList(NotifyType.Mail, false, new List<String>(), "Event", "mail message");
        
        #endregion

        public NotifyEvent()
        {

        }

        public NotifyEvent(String eventName, Boolean isNotify, NotifyTypeAddrList netSend,
            NotifyTypeAddrList jabber, NotifyTypeAddrList mail)
        {
            this.eventName = eventName;
            this.isNotify = isNotify;
            this.netSend = netSend;
            this.jabber = jabber;
            this.mail = mail;
        }

        #region Property
        public String EventName
        {
            get { return this.eventName; }
            set { this.eventName = value; }
        }

        public Boolean IsNotify
        {
            get { return this.isNotify; }
            set { this.isNotify = value; }
        }

        public NotifyTypeAddrList NetSend
        {
            get { return this.netSend; }
            set { this.netSend = value; }
        }

        public NotifyTypeAddrList Jabber
        {
            get { return this.jabber; }
            set { this.jabber = value; }
        }

        public NotifyTypeAddrList Mail
        {
            get { return this.mail; }
            set { this.mail = value; }
        }
        #endregion

    }
}
