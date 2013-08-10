using System;
using System.Collections.Generic;
using System.Text;

namespace ARM2_dbcontrol.DataBase
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

        #endregion

        #region Constructors

        public ComputerAdditionalEntity()
            : this(true, false, String.Empty)
        { }

        public ComputerAdditionalEntity(Boolean isControllable, Boolean isRenamed, String previousComputerName)
        {
            this._isControllable = isControllable;
            this._isRenamed = isRenamed;
            this._previousComputerName = previousComputerName;
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            return new ComputerAdditionalEntity(this._isControllable, this._isRenamed, this._previousComputerName);
        }

        #endregion
    }
}
