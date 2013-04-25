using System;
using System.Collections.Generic;
using System.Text;

namespace ARM2_dbcontrol.Filters
{
    /// <summary>
    /// Фильтры устройств
    /// </summary>
    public class DeviceFilterEntity: FilterEntity
    {
        private string serialNumber = String.Empty;
        private string nameDevice = String.Empty;
        private string computerName = String.Empty;

        private DateTime dateInsertedFrom = DateTime.MinValue;
        private DateTime dateInsertedTo = DateTime.MinValue;
        private int dateInsertedIntervalIndex = Int32.MinValue;
        private int dateInsertedIntervalModeIndex = Int32.MinValue;

        public string SerialNumber
        {
            get { return serialNumber; }
            set { serialNumber = value; }
        }

        public string NameDevice
        {
            get { return nameDevice; }
            set { nameDevice = value; }
        }

        public string ComputerName
        {
            get { return computerName; }
            set { computerName = value; }
        }

        public DateTime DateInsertedFrom
        {
            get { return dateInsertedFrom; }
            set { dateInsertedFrom = value; }
        }
        public DateTime DateInsertedTo
        {
            get { return dateInsertedTo; }
            set { dateInsertedTo = value; }
        }
        public int DateInsertedIntervalIndex
        {
            get { return dateInsertedIntervalIndex; }
            set { dateInsertedIntervalIndex = value; }
        }
        public int DateInsertedIntervalModeIndex
        {
            get { return dateInsertedIntervalModeIndex; }
            set { dateInsertedIntervalModeIndex = value; }
        }

        private string termSerialNumber = "AND";
        private string termNameDevice = "AND";
        private string termComputerName = "AND";
        private string termDateInserted = "AND";

        public string TermSerialNumber
        {
            get { return termSerialNumber; }
            set { termSerialNumber = value; }
        }
        public string TermNameDevice
        {
            get { return termNameDevice; }
            set { termNameDevice = value; }
        }
        public string TermComputerName
        {
            get { return termComputerName; }
            set { termComputerName = value; }
        }
        public string TermDateInserted
        {
            get { return termDateInserted; }
            set { termDateInserted = value; }
        }

        #region constructors

        public DeviceFilterEntity() { }

        public DeviceFilterEntity(string _serialNumber, string _nameDevice, string _computerName, DateTime _dateInsertedFrom, DateTime _dateInsertedTo, string _termSerialNumber, string _termNameDevice, string _termComputerName, string _termDateInserted)
        {
            this.serialNumber = _serialNumber;
            this.termSerialNumber = _termSerialNumber;
            this.nameDevice = _nameDevice;
            this.termNameDevice = _termNameDevice;
            this.computerName = _computerName;
            this.termComputerName = _termComputerName;
            this.dateInsertedFrom = _dateInsertedFrom;
            this.dateInsertedTo = _dateInsertedTo;
            this.termDateInserted = _termDateInserted;
        }

        #endregion

        public override bool CheckFilters()
        {
            return true;
        }

        private void BuildQuery(string keyword)
        {
            if (termSerialNumber == keyword)
            {
                string[] array = serialNumber.Split('&');
                if (array.Length > 1)
                {
                    BuildDifferentQuery("SerialNo", termSerialNumber, array);
                }
                else
                    sqlWhereStatement += StringValue("SerialNo", serialNumber, termSerialNumber);
            }

            if (termNameDevice == keyword)
            {
                string[] array = nameDevice.Split('&');
                if (array.Length > 1)
                {
                    BuildDifferentQuery("Comment", termNameDevice, array);
                }
                else
                    sqlWhereStatement += StringValue("Comment", nameDevice, termNameDevice);
            }

            if (termComputerName == keyword)
            {
                string[] array = computerName.Split('&');
                if (array.Length > 1)
                {
                    BuildDifferentQuery("ComputerName", termComputerName, array);
                }
                else
                    sqlWhereStatement += StringValue("ComputerName", computerName, termComputerName);

            }

            if (termDateInserted == keyword)
            {
                if (dateInsertedIntervalIndex == Int32.MinValue)
                {
                    sqlWhereStatement += DateValue("LatestInsert", dateInsertedFrom, dateInsertedTo, termDateInserted);
                }
                else
                {
                    if (dateInsertedIntervalModeIndex == 0)
                    {
                        sqlWhereStatement += DateValue("LatestInsert", DateTime.Now.ToLocalTime().AddYears(-10),
                          base.GetDateInterval(dateInsertedIntervalIndex), termDateInserted);
                    }
                    else
                    {
                        sqlWhereStatement += DateValue("LatestInsert", base.GetDateInterval(dateInsertedIntervalIndex),
                        DateTime.Now.ToLocalTime(), termDateInserted);
                    }
                }
            }
           /*
            if (termNameDevice == keyword)
            {
                string[] array = nameDevice.Split('&');
                if (array.Length > 1)
                {
                    BuildDifferentQuery("Comment", termNameDevice, array);
                }
                else
                    sqlWhereStatement += StringValue("Comment", nameDevice, termNameDevice);
            }
           */ 
        }

        public override bool GenerateSQLWhereStatement()
        {
            base.sqlWhereStatement = null;
            base.dirtybit = false;            

            BuildQuery("AND");
            BuildQuery("NOT");
            BuildQuery("OR");

            return true;
        }

    }
}
