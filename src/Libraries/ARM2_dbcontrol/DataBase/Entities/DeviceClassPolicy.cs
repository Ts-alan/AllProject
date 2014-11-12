using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.DataBase
{
    public class DeviceClassPolicy: ICloneable
    {
        #region Properties

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

        private DeviceClass _deviceClass;
        public DeviceClass ClassOfDevice
        {
            get { return _deviceClass; }
            set { _deviceClass = value; }
        }

        private DeviceClassMode _mode;
        public DeviceClassMode Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        #endregion

        #region Constructors

        public DeviceClassPolicy()
            : this(new ComputersEntity(), new DeviceClass())
        {
        }

        public DeviceClassPolicy(ComputersEntity _computer, DeviceClass _deviceClass)
            : this(_computer, _deviceClass, DeviceClassMode.Disabled)
        {
        }
        public DeviceClassPolicy(ComputersEntity _computer, DeviceClass _deviceClass, DeviceClassMode _mode)
            : this(0, _computer, _deviceClass, _mode)
        {
        }

        public DeviceClassPolicy(Int32 _id, ComputersEntity _computer, DeviceClass _deviceClass, DeviceClassMode _mode)
        {
            this._id = _id;
            this._computer = _computer;
            this._deviceClass = _deviceClass;
            this._mode = _mode;
        }

        #endregion

        #region ICloneable Members

        public Object Clone()
        {
            return new DeviceClassPolicy(this._id, this._computer, this._deviceClass, this._mode);
        }

        #endregion
    }
}
