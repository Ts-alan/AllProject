using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.Settings.Entities
{
    public class PMSSettingsEntity
    {
        #region Fields

        private String _Server = null;
        private Int32? _Port = null;
        private Int32? _TaskDaysToDelete = null;
        private Int32? _ComputerDaysToDelete = null;
        private Int32? _DaysToDelete = null;

        private Int32? _DataSendInterval = null;
        private Int32? _DeliveryTimeoutCheck = null;
        private Int32? _HourIntervalToSend = null;
        private Boolean _MaintenanceEnabled;
        private Int32? _MaxFileLength = null;

        private DateTime? _LastSelectDate = null;
        private DateTime? _LastSendDate = null;
        private DateTime? _NextSendDate = null;
        
        private Boolean _ReRead = false;
        
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

        #region Methods

        public String GenerateXML()
        {
            StringBuilder xml = new StringBuilder(1024);
            xml.Append("<VbaSettings><ControlCenter><PeriodicalMaintenance>");
            if (DeliveryTimeoutCheck.HasValue)
                xml.AppendFormat("<DeliveryTimeoutCheck type=" + "\"reg_dword\"" + ">{0}</DeliveryTimeoutCheck>", DeliveryTimeoutCheck);

            if (DaysToDelete.HasValue)
                xml.AppendFormat("<DaysToDelete type=" + "\"reg_dword\"" + ">{0}</DaysToDelete>", DaysToDelete);
            if (TaskDaysToDelete.HasValue)
                xml.AppendFormat("<TaskDaysToDelete type=" + "\"reg_dword\"" + ">{0}</TaskDaysToDelete>", TaskDaysToDelete);
            if (ComputerDaysToDelete.HasValue)
                xml.AppendFormat("<ComputerDaysToDelete type=" + "\"reg_dword\"" + ">{0}</ComputerDaysToDelete>", ComputerDaysToDelete);
            
            xml.AppendFormat("<MaintenanceEnabled type=" + "\"reg_dword\"" + ">{0}</MaintenanceEnabled>", MaintenanceEnabled ? "1" : "0");

            if (MaintenanceEnabled)
            {
                if (!String.IsNullOrEmpty(Server))
                    xml.AppendFormat("<Server type=" + "\"reg_sz\"" + ">{0}</Server>", Server);

                if (HourIntervalToSend.HasValue)
                    xml.AppendFormat("<HourIntervalToSend type=" + "\"reg_dword\"" + ">{0}</HourIntervalToSend>", HourIntervalToSend);
                if (DataSendInterval.HasValue)
                    xml.AppendFormat("<DataSendInterval type=" + "\"reg_dword\"" + ">{0}</DataSendInterval>", DataSendInterval);
                if (NextSendDate.HasValue)
                    xml.AppendFormat("<NextSendDate type=" + "\"reg_sz\"" + ">{0}</NextSendDate>", NextSendDate.Value.ToString(new System.Globalization.CultureInfo("ru-RU")));
                if (LastSelectDate.HasValue)
                    xml.AppendFormat("<LastSelectDate type=" + "\"reg_sz\"" + ">{0}</LastSelectDate>", LastSelectDate.Value.ToString(new System.Globalization.CultureInfo("ru-RU")));
                if (LastSendDate.HasValue)
                    xml.AppendFormat("<LastSendDate type=" + "\"reg_sz\"" + ">{0}</LastSendDate>", LastSendDate.Value.ToString(new System.Globalization.CultureInfo("ru-RU")));
            }

            xml.AppendFormat("<Reread type=" + "\"reg_dword\"" + ">{0}</Reread>", ReRead ? "1" : "0");

            xml.Append("</PeriodicalMaintenance></ControlCenter></VbaSettings>");

            return xml.ToString();
        }

        #endregion
    }
}
