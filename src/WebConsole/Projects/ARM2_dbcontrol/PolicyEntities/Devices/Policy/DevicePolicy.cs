using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;

using ARM2_dbcontrol.DataBase;

namespace VirusBlokAda.Vba32CC.Policies.Devices.Policy
{

    public struct DevicePolicy
    {

        #region Property

        private int _id;

        public int ID
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

        private DevicePolicyState _state;
        /// <summary>
        /// Type actions
        /// </summary>
        public DevicePolicyState State
        {
            get { return _state; }
            set { _state = value; }
        }

        #endregion

        private DateTime? _latestInsert;
        public DateTime? LatestInsert
        {
            get { return _latestInsert; }
            set { _latestInsert = value;}
        }

        public DevicePolicy(Device device, ComputersEntity computer,
            DevicePolicyState type, int id)
        {
            _device = device;
            _computer = computer;
            _state = type;
            _id = id;
            _latestInsert = null;
        }

        public DevicePolicy(Device device, ComputersEntity computer,
           DevicePolicyState type):this(device,computer,type,0)
        {

        }

        public DevicePolicy(Device device, ComputersEntity computer)
            : this(device, computer,DevicePolicyState.Undefined)
        {

        }


        

    }
}
