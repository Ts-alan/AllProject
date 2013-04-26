using System;

namespace Vba32CC.DataBase
{
    /// <summary>
    /// IMPORTANT: This class should never be manually edited.
    /// This entity class represents the properties and methods of a Computers.
    /// </summary>
    public class ComputersEntity : ICloneable
    {

        protected Int16 iD = Int16.MinValue;
        protected String computerName = String.Empty;
        protected String iPAddress = String.Empty;
        protected Boolean controlCenter;
        protected String domainName = String.Empty;
        protected String userLogin = String.Empty;
        protected String oSName = String.Empty;
        protected Int16 oSTypeID = Int16.MinValue;
        protected Int16 rAM = Int16.MinValue;
        protected Int16 cPUClock = Int16.MinValue;
        protected DateTime recentActive = DateTime.MinValue;
        protected DateTime latestUpdate = DateTime.MinValue;
        protected String vba32Version = String.Empty;
        protected DateTime latestInfected = DateTime.MinValue;
        protected String latestMalware = String.Empty;
        protected Boolean vba32Integrity;
        protected Boolean vba32KeyValid;
        protected String description = String.Empty;

        //Default constructor
        public ComputersEntity() { }

        //Constructor: table
        public ComputersEntity(
            Int16 iD,
            String computerName,
            String iPAddress,
            Boolean controlCenter,
            String domainName,
            String userLogin,
            Int16 oSTypeID,
            Int16 rAM,
            Int16 cPUClock,
            DateTime recentActive,
            DateTime latestUpdate,
            String vba32Version,
            DateTime latestInfected,
            String latestMalware,
            Boolean vba32Integrity,
            Boolean vba32KeyValid,
            String description)
        {
            this.iD = iD;
            this.computerName = computerName;
            this.iPAddress = iPAddress;
            this.controlCenter = controlCenter;
            this.domainName = domainName;
            this.userLogin = userLogin;
            this.oSTypeID = oSTypeID;
            this.rAM = rAM;
            this.cPUClock = cPUClock;
            this.recentActive = recentActive;
            this.latestUpdate = latestUpdate;
            this.vba32Version = vba32Version;
            this.latestInfected = latestInfected;
            this.latestMalware = latestMalware;
            this.vba32Integrity = vba32Integrity;
            this.vba32KeyValid = vba32KeyValid;
            this.description = description;
        }

        #region Public Properties

        public Int16 ID
        {
            get { return iD; }
            set { iD = value; }
        }

        public String ComputerName
        {
            get { return computerName; }
            set { computerName = value; }
        }

        public String IPAddress
        {
            get { return iPAddress; }
            set { iPAddress = value; }
        }

        public Boolean ControlCenter
        {
            get { return controlCenter; }
            set { controlCenter = value; }
        }

        public String DomainName
        {
            get { return domainName; }
            set { domainName = value; }
        }

        public String UserLogin
        {
            get { return userLogin; }
            set { userLogin = value; }
        }

        public String OSName
        {
            get { return oSName; }
            set { oSName = value; }
        }

        public Int16 OSTypeID
        {
            get { return oSTypeID; }
            set { oSTypeID = value; }
        }

        public Int16 RAM
        {
            get { return rAM; }
            set { rAM = value; }
        }

        public Int16 CPUClock
        {
            get { return cPUClock; }
            set { cPUClock = value; }
        }

        public DateTime RecentActive
        {
            get { return recentActive; }
            set { recentActive = value; }
        }

        public DateTime LatestUpdate
        {
            get { return latestUpdate; }
            set { latestUpdate = value; }
        }

        public String Vba32Version
        {
            get { return vba32Version; }
            set { vba32Version = value; }
        }

        public DateTime LatestInfected
        {
            get { return latestInfected; }
            set { latestInfected = value; }
        }

        public String LatestMalware
        {
            get { return latestMalware; }
            set { latestMalware = value; }
        }

        public Boolean Vba32Integrity
        {
            get { return vba32Integrity; }
            set { vba32Integrity = value; }
        }

        public Boolean Vba32KeyValid
        {
            get { return vba32KeyValid; }
            set { vba32KeyValid = value; }
        }

        public String Description
        {
            get { return description; }
            set { description = value; }
        }

        #endregion

        /// <summary>
        /// Create and return clone object
        /// </summary>
        /// <returns>Clone object</returns>
        public virtual Object Clone()
        {
            return new ComputersEntity(
                    this.iD,
                    this.computerName,
                    this.iPAddress,
                    this.controlCenter,
                    this.domainName,
                    this.userLogin,
                    this.oSTypeID,
                    this.rAM,
                    this.cPUClock,
                    this.recentActive,
                    this.latestUpdate,
                    this.vba32Version,
                    this.latestInfected,
                    this.latestMalware,
                    this.vba32Integrity,
                    this.vba32KeyValid,
                    this.description);
        }

    }
}

