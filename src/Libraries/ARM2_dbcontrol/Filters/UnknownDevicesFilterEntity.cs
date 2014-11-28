using System;
using System.Collections.Generic;
using System.Text;

namespace ARM2_dbcontrol.Filters
{
    /// <summary>
    /// Фильтры неназначенных устройств
    /// </summary>
    public class UnknownDevicesFilterEntity: FilterEntity
    {
        private string serialNumber = String.Empty;
        private string comment = String.Empty;
        private DateTime dateInsertedFrom = DateTime.MinValue;
        private DateTime dateInsertedTo = DateTime.MinValue;
        private int dateInsertedIntervalIndex = Int32.MinValue;
        private int dateInsertedIntervalModeIndex = Int32.MinValue;

        public string SerialNumber
        {
            get { return serialNumber; }
            set { serialNumber = value; }
        }
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
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
        private string termComment = "AND";
        private string termDateInserted = "AND";

        public string TermSerialNumber
        {
            get { return termSerialNumber; }
            set { termSerialNumber = value; }
        }
        public string TermComment
        {
            get { return termComment; }
            set { termComment = value; }
        }
        public string TermDateInserted
        {
            get { return termDateInserted; }
            set { termDateInserted = value; }
        }

        #region constructors

        public UnknownDevicesFilterEntity() { }

        public UnknownDevicesFilterEntity(string _serialNumber, string _computerName, string _comment, DateTime _dateInsertedFrom, DateTime _dateInsertedTo, string _termSerialNumber, string _termComputerName, string _termComment, string _termDateInserted)
        {
            this.serialNumber = _serialNumber;
            this.computerName = _computerName;
            this.comment = _comment;
            this.dateInsertedFrom = _dateInsertedFrom;
            this.dateInsertedTo = _dateInsertedTo;
            this.termSerialNumber = _termSerialNumber;
            this.termComputerName = _termComputerName;
            this.termComment = _termComment;
            this.termDateInserted = _termDateInserted;
        }
        
        #endregion

        public override bool CheckFilters()
        {
            return true;
        }
        /// <summary>
        /// построение запроса
        /// </summary>
        /// <param name="keyword">ключевое слово</param>
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
            /*
            if (termComment == keyword)
            {
                string[] array = comment.Split('&');
                if (array.Length > 1)
                {
                    BuildDifferentQuery("Comment", termComment, array);
                }
                else
                    sqlWhereStatement += StringValue("Comment", comment, termComment);

            }
            */
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
        }
        /// <summary>
        /// генерация запроса
        /// </summary>
        /// <returns></returns>
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
