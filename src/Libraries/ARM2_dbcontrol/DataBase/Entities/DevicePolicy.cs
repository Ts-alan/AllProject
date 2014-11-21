using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace VirusBlokAda.CC.DataBase
{
    public struct DevicePolicy
    {
        #region Property

        private Int32 _id;

        public Int32 ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private ComputersEntity _computer;

        public ComputersEntity Computer
        {
            get { return _computer; }
            set { _computer = value; }
        }

        private Device _device;
        /// <summary>
        /// device id
        /// </summary>
        public Device Device
        {
            get { return _device; }
            set { _device = value; }
        }

        private DeviceMode _state;
        /// <summary>
        /// Type actions
        /// </summary>
        public DeviceMode State
        {
            get { return _state; }
            set { _state = value; }
        }

        private DateTime? _latestInsert;
        public DateTime? LatestInsert
        {
            get { return _latestInsert; }
            set { _latestInsert = value; }
        }

        #endregion

        public DevicePolicy(Device device, ComputersEntity computer,
            DeviceMode type, Int32 id)
        {
            _device = device;
            _computer = computer;
            _state = type;
            _id = id;
            _latestInsert = null;
        }

        public DevicePolicy(Device device, ComputersEntity computer,
           DeviceMode type)
            : this(device, computer, type, 0)
        {
        }

        public DevicePolicy(Device device, ComputersEntity computer)
            : this(device, computer, DeviceMode.Undefined)
        {
        }
    }
}