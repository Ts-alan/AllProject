using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.Settings.Entities
{
    public class PMSSettingsEntity
    {
        #region Fields

        private String _Server;
        private Int32? _Port;
        private Int32? _TaskDaysToDelete;
        private Int32? _ComputerDaysToDelete;
        private Int32? _DaysToDelete;

        private Int32? _DataSendInterval;
        private Int32? _DeliveryTimeoutCheck;
        private Int32? _HourIntervalToSend;
        private Boolean _MaintenanceEnabled;
        private Int32? _MaxFileLength;

        private DateTime? _LastSelectDate;
        private DateTime? _LastSendDate;
        private DateTime? _NextSendDate;
        
        private Boolean _ReRead;
        
        #endregion

        #region Properties

        public String Server
        {
            get { return _Server; }
            set { _Server = value; }
        }

        public Int32? Port
        {
            get { return _Port; }
            set { _Port = value; }
        }

        public Int32? TaskDaysToDelete
        {
            get { return _TaskDaysToDelete; }
            set { _TaskDaysToDelete = value; }
        }

        public Int32? ComputerDaysToDelete
        {
            get { return _ComputerDaysToDelete; }
            set { _ComputerDaysToDelete = value; }
        }

        public Int32? DaysToDelete
        {
            get { return _DaysToDelete; }
            set { _DaysToDelete = value; }
        }

        public Int32? DataSendInterval
        {
            get { return _DataSendInterval; }
            set { _DataSendInterval = value; }
        }

        public Int32? DeliveryTimeoutCheck
        {
            get { return _DeliveryTimeoutCheck; }
            set { _DeliveryTimeoutCheck = value; }
        }

        public Int32? HourIntervalToSend
        {
            get { return _HourIntervalToSend; }
            set { _HourIntervalToSend = value; }
        }

        public Boolean MaintenanceEnabled
        {
            get { return _MaintenanceEnabled; }
            set { _MaintenanceEnabled = value; }
        }

        public Int32? MaxFileLength
        {
            get { return _MaxFileLength; }
            set { _MaxFileLength = value; }
        }

        public DateTime? LastSelectDate
        {
            get { return _LastSelectDate; }
            set { _LastSelectDate = value; }
        }

        public DateTime? LastSendDate
        {
            get { return _LastSendDate; }
            set { _LastSendDate = value; }
        }

        public DateTime? NextSendDate
        {
            get { return _NextSendDate; }
            set { _NextSendDate = value; }
        }

        public Boolean ReRead
        {
            get { return _ReRead; }
            set { _ReRead = value; }
        }

        #endregion
        
        #region Constructors

        public PMSSettingsEntity() { }

        #endregion
    }
}
