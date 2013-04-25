using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;

namespace VirusBlokAda.Vba32CC.Policies.Devices
{
    public struct Device
    {
        private int _id;

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _serialNo;

        public string SerialNo
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

        private string _comment;

        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        private string _lastComputer;

        public string LastComputer
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

        public Device(string serialNo)
            :this(0,serialNo,DeviceType.USB)
        { 
        }

        public Device(string serialNo, DeviceType type)
            : this(0, serialNo, type)
        {
        }

        public Device(int id, string serialNo, DeviceType type):
            this(id, serialNo, type,"")
        {
        }

        public Device(int id, string serialNo, DeviceType type, string comment):
            this(id, serialNo, type, comment, DateTime.MinValue, "")
        {
            _id = id;
            _serialNo = serialNo;
            _type = type;
            _comment = comment;
        }

        public Device(int id, string serialNo, DeviceType type, string comment, DateTime? lastInserted, string computerName)
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
