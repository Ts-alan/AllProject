using System;
using System.Collections.Generic;
using System.Text;

namespace Vba32.ControlCenter.NotificationService.Notification
{
    /// <summary>
    /// Представляет собой тип уведомления со списком адресов
    /// </summary>
    public class NotifyTypeAddrList
    {
        private NotifyType type = NotifyType.NetSend;          //тип уведомления
        private bool isUse = false;
        private List<string> addrList = new List<string>();    //список адресов

        private int priority = 0;                               //приоритет уведомления

        private string subject = String.Empty;
        private string message = String.Empty;

        #region Constructors    
        public NotifyTypeAddrList()
        {
        }

        public NotifyTypeAddrList(NotifyType type, bool isUse, List<string> addrList, string message)
        {
            this.type = type;
            this.isUse = isUse;
            this.addrList = addrList;
            this.message = message;
        }

        public NotifyTypeAddrList(NotifyType type, bool isUse, List<string> addrList, string subject, string message)
        {
            this.type = type;
            this.isUse = isUse;
            this.addrList = addrList;
            this.subject = subject;
            this.message = message;
        }

        public NotifyTypeAddrList(NotifyType type, bool isUse, List<string> addrList, string subject, string message, int priority)
        {
            this.type = type;
            this.isUse = isUse;
            this.addrList = addrList;
            this.subject = subject;
            this.message = message;
            this.priority = priority;
        }

        #endregion

        #region Property

        public NotifyType Type
        {
            get { return this.type; }
            //set { this.type = value; }
        }

        public bool IsUse
        {
            get { return this.isUse; }
            set { this.isUse = value; }
        }

        public List<string> AddrList
        {
            get { return this.addrList; }
            set { this.addrList = value; }
        }

        public string Subject
        {
            get { return this.subject; }
            set { this.subject = value; }
        }

        public string Message
        {
            get { return this.message; }
            set { this.message = value; }
        }
        public int Priority
        {
            get { return this.priority; }
            set { this.priority = value; }
        }


        #endregion

    }
}
