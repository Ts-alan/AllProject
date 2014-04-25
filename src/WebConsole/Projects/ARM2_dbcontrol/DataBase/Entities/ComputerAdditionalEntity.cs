using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.DataBase
{
    public class ComputerAdditionalEntity: ICloneable
    {
        #region Properties

        protected Boolean _isControllable;
        public Boolean IsControllable
        {
            get { return _isControllable; }
            set { _isControllable = value; }
        }

        protected Boolean _isRenamed;
        public Boolean IsRenamed
        {
            get { return _isRenamed; }
            set { _isRenamed = value; }
        }

        protected String _previousComputerName;
        public String PreviousComputerName
        {
            get { return _previousComputerName; }
            set { _previousComputerName = value; }
        }

        protected ControlDeviceTypeEnum _controlDeviceType;
        public ControlDeviceTypeEnum ControlDeviceType
        {
            get { return _controlDeviceType; }
            set { _controlDeviceType = value; }
        }

        #endregion

        #region Constructors

        public ComputerAdditionalEntity()
            : this(true, false, String.Empty, ControlDeviceTypeEnum.Unknown)
        { }

        public ComputerAdditionalEntity(Boolean isControllable, Boolean isRenamed, String previousComputerName, ControlDeviceTypeEnum controlDeviceType)
        {
            this._isControllable = isControllable;
            this._isRenamed = isRenamed;
            this._previousComputerName = previousComputerName;
            this._controlDeviceType = controlDeviceType;
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            return new ComputerAdditionalEntity(this._isControllable, this._isRenamed, this._previousComputerName, this._controlDeviceType);
        }

        #endregion
    }    
}
