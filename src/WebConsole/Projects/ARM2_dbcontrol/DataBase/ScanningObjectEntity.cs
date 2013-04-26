using System;
using System.Collections.Generic;
using System.Text;

namespace ARM2_dbcontrol.DataBase
{
    public class ScanningObjectEntity : ICloneable
    {
        protected String ipAddress = String.Empty;
        protected String comment = String.Empty;
        protected Int32 id;

        //Default constructor
        public ScanningObjectEntity() { }

        //Constructor
        public ScanningObjectEntity(
            Int32 id, String ipAddress, String comment)
            : this(ipAddress, comment)
        {
            this.id = id;
        }

        public ScanningObjectEntity(String ipAddress, String comment)
        {
            this.ipAddress = ipAddress;
            this.comment = comment;
        }

        #region Public Properties

        public Int32 ID
        {
            get { return id; }
            set { id = value; }
        }
    
        public String IPAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        public String Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        #endregion

        /// <summary>
        /// Create and return clone object
        /// </summary>
        /// <returns>Clone object</returns>
        public virtual object Clone()
        {
            return new ScanningObjectEntity(
                    this.id,
                    this.ipAddress,
                    this.comment);
        }
    }
}
