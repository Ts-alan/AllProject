using System;
using System.Collections.Generic;
using System.Text;

namespace Vba32.ControlCenter.NotificationService.Notification
{
    /// <summary>
    /// ������������ ����� ��� ����������� �� ������� �������
    /// </summary>
    public class NotifyTypeAddrList
    {
        private NotifyType type = NotifyType.NetSend;          //��� �����������
        private Boolean isUse = false;
        private List<String> addrList = new List<String>();    //������ �������

        private Int32 priority = 0;                               //��������� �����������

        private String subject = String.Empty;
        private String message = String.Empty;

        #region Constructors    
        public NotifyTypeAddrList()
        {
        }

        public NotifyTypeAddrList(NotifyType type, Boolean isUse, List<String> addrList, String message)
        {
            this.type = type;
            this.isUse = isUse;
            this.addrList = addrList;
            this.message = message;
        }

        public NotifyTypeAddrList(NotifyType type, Boolean isUse, List<String> addrList, String subject, String message)
        {
            this.type = type;
            this.isUse = isUse;
            this.addrList = addrList;
            this.subject = subject;
            this.message = message;
        }

        public NotifyTypeAddrList(NotifyType type, Boolean isUse, List<String> addrList, String subject, String message, Int32 priority)
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

        public Boolean IsUse
        {
            get { return this.isUse; }
            set { this.isUse = value; }
        }

        public List<String> AddrList
        {
            get { return this.addrList; }
            set { this.addrList = value; }
        }

        public String Subject
        {
            get { return this.subject; }
            set { this.subject = value; }
        }

        public String Message
        {
            get { return this.message; }
            set { this.message = value; }
        }
        public Int32 Priority
        {
            get { return this.priority; }
            set { this.priority = value; }
        }


        #endregion

    }
}
