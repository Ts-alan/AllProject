using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace VirusBlokAda.CC.DataBase
{
    public struct Device
    {
        private Int32 _id;

        public Int32 ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private String _serialNo;

        public String SerialNo
        {
            get { return _serialNo; }
            set { _serialNo = value; }
        }

        private DeviceType _type;

        public DeviceType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private String _comment;

        public String Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        private String _lastComputer;

        public String LastComputer
        {
            get { return _lastComputer; }
            set { _lastComputer = value; }
        }

        private DateTime? _lastInserted;

        public DateTime? LastInserted
        {
            get { return _lastInserted; }
            set { _lastInserted = value; }
        }

        #region Constructions

        public Device(String serialNo)
            :this(0,serialNo,DeviceType.USB)
        { 
        }

        public Device(String serialNo, DeviceType type)
            : this(0, serialNo, type)
        {
        }

        public Device(Int32 id, String serialNo, DeviceType type):
            this(id, serialNo, type,"")
        {
        }

        public Device(Int32 id, String serialNo, DeviceType type, String comment):
            this(id, serialNo, type, comment, DateTime.MinValue, "")
        {
            _id = id;
            _serialNo = serialNo;
            _type = type;
            _comment = comment;
        }

        public Device(Int32 id, String serialNo, DeviceType type, String comment, DateTime? lastInserted, String computerName)
        {
            _id = id;
            _serialNo = serialNo;
            _type = type;
            _comment = comment;
            _lastInserted = lastInserted;
            _lastComputer = computerName;
        }

        #endregion
    }
}
