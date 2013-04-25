using System;

namespace ARM2_dbcontrol.DataBase
{
    /// <summary>
    /// IMPORTANT: This class should never be manually edited.
    /// This entity class represents the properties and methods of a Computers.
    /// </summary>
    public class ComputersEntity : ICloneable
    {

        protected short iD = Int16.MinValue;
        protected string computerName = String.Empty;
        protected string iPAddress = String.Empty;
        protected bool controlCenter;
        protected string domainName = String.Empty;
        protected string userLogin = String.Empty;
        protected string oSName = String.Empty;
        protected short oSTypeID = Int16.MinValue;
        protected short rAM = Int16.MinValue;
        protected short cPUClock = Int16.MinValue;
        protected DateTime recentActive = DateTime.MinValue;
        protected DateTime latestUpdate = DateTime.MinValue;
        protected string vba32Version = String.Empty;
        protected DateTime latestInfected = DateTime.MinValue;
        protected string latestMalware = String.Empty;
        protected bool vba32Integrity;
        protected bool vba32KeyValid;
        protected string description = String.Empty;

        //Default constructor
        public ComputersEntity() { }

     /*   //Constructor: from inner-join
        public ComputersEntity(
            string computerName,
            string iPAddress,
            bool controlCenter,
            string domainName,
            string userLogin,
            string oSName,
            short rAM,
            short cPUClock,
            DateTime recentActive,
            DateTime latestUpdate,
            string vba32Version,
            DateTime latestInfected,
            string latestMalware,
            bool vba32Integrity,
            bool vba32KeyValid,
            string description)
        {
            this.computerName = computerName;
            this.iPAddress = iPAddress;
            this.controlCenter = controlCenter;
            this.domainName = domainName;
            this.userLogin = userLogin;
            this.oSName = oSName;
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
        }*/

        //Constructor: table
        public ComputersEntity(
            short iD,
            string computerName,
            string iPAddress,
            bool controlCenter,
            string domainName,
            string userLogin,
            short oSTypeID,
            short rAM,
            short cPUClock,
            DateTime recentActive,
            DateTime latestUpdate,
            string vba32Version,
            DateTime latestInfected,
            string latestMalware,
            bool vba32Integrity,
            bool vba32KeyValid,
            string description)
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

        public short ID
        {
            get { return iD; }
            set { iD = value; }
        }

        public string ComputerName
        {
            get { return computerName; }
            set { computerName = value; }
        }

        public string IPAddress
        {
            get { return iPAddress; }
            set { iPAddress = value; }
        }

        public bool ControlCenter
        {
            get { return controlCenter; }
            set { controlCenter = value; }
        }

        public string DomainName
        {
            get { return domainName; }
            set { domainName = value; }
        }

        public string UserLogin
        {
            get { return userLogin; }
            set { userLogin = value; }
        }

        public string OSName
        {
            get { return oSName; }
            set { oSName = value; }
        }

        public short OSTypeID
        {
            get { return oSTypeID; }
            set { oSTypeID = value; }
        }

        public short RAM
        {
            get { return rAM; }
            set { rAM = value; }
        }

        public short CPUClock
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

        public string Vba32Version
        {
            get { return vba32Version; }
            set { vba32Version = value; }
        }

        public DateTime LatestInfected
        {
            get { return latestInfected; }
            set { latestInfected = value; }
        }

        public string LatestMalware
        {
            get { return latestMalware; }
            set { latestMalware = value; }
        }

        public bool Vba32Integrity
        {
            get { return vba32Integrity; }
            set { vba32Integrity = value; }
        }

        public bool Vba32KeyValid
        {
            get { return vba32KeyValid; }
            set { vba32KeyValid = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        #endregion

        /// <summary>
        /// Create and return clone object
        /// </summary>
        /// <returns>Clone object</returns>
        public virtual object Clone()
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

