using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.DataBase
{
    public class DeviceClass: ICloneable
    {
        #region Properties

        private Int16 _id;
        public Int16 ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private String _uid;
        public String UID
        {
            get { return _uid; }
            set { _uid = value; }
        }

        private String _class;
        public String Class
        {
            get { return _class; }
            set { _class = value; }
        }

        private String _className;
        public String ClassName
        {
            get { return _className; }
            set { _className = value; }
        }

        #endregion

        #region Constructors

        public DeviceClass()
            : this(String.Empty)
        {
        }

        public DeviceClass(String _uid)
            : this(_uid, String.Empty, String.Empty)
        {
        }
        public DeviceClass(String _uid, String _class, String _className)
            : this(0, _uid, _class, _className)
        {
        }

        public DeviceClass(Int16 _id, String _uid, String _class, String _className)
        {
            this._id = _id;
            this._uid = _uid;
            this._class = _class;
            this._className = _className;
        }

        #endregion

        #region ICloneable Members

        public Object Clone()
        {
            return new DeviceClass(this._id, this._uid, this._class, this._className);
        }

        #endregion
    }
}
