using System;
using System.Collections.Specialized;

namespace VirusBlokAda.CC.DataBase
{
    /// <summary>
    /// IMPORTANT: This class should never be manually edited.
    /// This entity class represents the properties and methods of a Computers.
    /// </summary>
    public class ComputersEntity : ICloneable
    {   
        #region Properties

        protected Int16 iD = Int16.MinValue;
        public Int16 ID
        {
            get { return iD; }
            set { iD = value; }
        }

        protected String computerName = String.Empty;
        public String ComputerName
        {
            get { return computerName; }
            set { computerName = value; }
        }

        protected String macAddress = String.Empty;
        public String MacAddress
        {
            get { return macAddress; }
            set { macAddress = value; }
        }

        protected String iPAddress = String.Empty;
        public String IPAddress
        {
            get { return iPAddress; }
            set { iPAddress = value; }
        }

        protected Boolean controlCenter;
        public Boolean ControlCenter
        {
            get { return controlCenter; }
            set { controlCenter = value; }
        }

        protected String domainName = String.Empty;
        public String DomainName
        {
            get { return domainName; }
            set { domainName = value; }
        }

        protected String userLogin = String.Empty;
        public String UserLogin
        {
            get { return userLogin; }
            set { userLogin = value; }
        }

        protected String oSName = String.Empty;
        public String OSName
        {
            get { return oSName; }
            set { oSName = value; }
        }

        protected Int16 oSTypeID = Int16.MinValue;        
        public Int16 OSTypeID
        {
            get { return oSTypeID; }
            set { oSTypeID = value; }
        }

        protected Int16 rAM = Int16.MinValue;        
        public Int16 RAM
        {
            get { return rAM; }
            set { rAM = value; }
        }

        protected Int16 cPUClock = Int16.MinValue;        
        public Int16 CPUClock
        {
            get { return cPUClock; }
            set { cPUClock = value; }
        }

        protected DateTime recentActive = DateTime.MinValue;        
        public DateTime RecentActive
        {
            get { return recentActive; }
            set { recentActive = value; }
        }

        protected DateTime latestUpdate = DateTime.MinValue;        
        public DateTime LatestUpdate
        {
            get { return latestUpdate; }
            set { latestUpdate = value; }
        }

        protected String vba32Version = String.Empty;        
        public String Vba32Version
        {
            get { return vba32Version; }
            set { vba32Version = value; }
        }

        protected DateTime latestInfected = DateTime.MinValue;        
        public DateTime LatestInfected
        {
            get { return latestInfected; }
            set { latestInfected = value; }
        }

        protected String latestMalware = String.Empty;        
        public String LatestMalware
        {
            get { return latestMalware; }
            set { latestMalware = value; }
        }

        protected Boolean vba32Integrity;
        public Boolean Vba32Integrity
        {
            get { return vba32Integrity; }
            set { vba32Integrity = value; }
        }

        protected Boolean vba32KeyValid;
        public Boolean Vba32KeyValid
        {
            get { return vba32KeyValid; }
            set { vba32KeyValid = value; }
        }

        protected String description = String.Empty;
        public String Description
        {
            get { return description; }
            set { description = value; }
        }

        protected ComputerAdditionalEntity _additionalInfo;
        public ComputerAdditionalEntity AdditionalInfo
        {
            get { return _additionalInfo; }
            set { _additionalInfo = value; }
        }

        #endregion

        #region Constructors

        public ComputersEntity()
            : this(String.Empty, String.Empty, String.Empty, false, String.Empty, String.Empty,
            String.Empty, 0, 0, 0, DateTime.MinValue, DateTime.MinValue, String.Empty,
                DateTime.MinValue, String.Empty, false, false, String.Empty, null)
        { }

        public ComputersEntity(String computerName, String macAddress, String iPAddress, Boolean controlCenter,
            String domainName, String userLogin, String osName, Int16 oSTypeID, Int16 rAM, Int16 cPUClock,
            DateTime recentActive, DateTime latestUpdate, String vba32Version, DateTime latestInfected, String latestMalware,
            Boolean vba32Integrity, Boolean vba32KeyValid, String description, ComputerAdditionalEntity additionalInfo)
            : this(0, computerName, macAddress, iPAddress, controlCenter,
            domainName, userLogin, osName, oSTypeID, rAM, cPUClock,
            recentActive, latestUpdate, vba32Version, latestInfected,
            latestMalware, vba32Integrity, vba32KeyValid, description, additionalInfo)
        {
        }

        public ComputersEntity(Int16 iD, String computerName, String macAddress, String iPAddress, Boolean controlCenter,
            String domainName, String userLogin, String osName, Int16 oSTypeID, Int16 rAM, Int16 cPUClock,
            DateTime recentActive, DateTime latestUpdate, String vba32Version, DateTime latestInfected, String latestMalware,
            Boolean vba32Integrity, Boolean vba32KeyValid, String description, ComputerAdditionalEntity additionalInfo)
        {
            this.iD = iD;
            this.computerName = computerName;
            this.macAddress = macAddress;
            this.iPAddress = iPAddress;
            this.controlCenter = controlCenter;
            this.domainName = domainName;
            this.userLogin = userLogin;
            this.oSName = osName;
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
            this._additionalInfo = (additionalInfo != null) ? additionalInfo : new ComputerAdditionalEntity();
        }

        public ComputersEntity(ComputersEntity comp)
            : this(comp.ID, comp.ComputerName, comp.MacAddress, comp.IPAddress, comp.ControlCenter,
            comp.DomainName, comp.UserLogin, comp.OSName, comp.OSTypeID, comp.RAM, comp.CPUClock,
            comp.RecentActive, comp.LatestUpdate, comp.Vba32Version, comp.LatestInfected,
            comp.LatestMalware, comp.Vba32Integrity, comp.Vba32KeyValid, comp.Description, comp.AdditionalInfo)
        {
        }

        public ComputersEntity(StringDictionary name_value_map)
        {
            this.computerName = name_value_map["ComputerName"];
            this.iPAddress = name_value_map["IPAddress"];
            this.domainName = name_value_map["DomainName"];
            this.userLogin = name_value_map["UserLogin"];
            this.oSName = name_value_map["OSName"];
            this.rAM = Convert.ToInt16(name_value_map["RAM"]);
            this.cPUClock = Convert.ToInt16(name_value_map["CPUClock"]);
            this.vba32Version = name_value_map["Vba32Version"];
            this.vba32Integrity = Convert.ToBoolean(name_value_map["Vba32Integrity"]);
            this.vba32KeyValid = Convert.ToBoolean(name_value_map["Vba32KeyValid"]);
            this.controlCenter = Convert.ToBoolean(name_value_map["ControlCenter"]);            
            this.macAddress = name_value_map["MACAddress"];
            this.AdditionalInfo=new ComputerAdditionalEntity();
            this.AdditionalInfo.ControlDeviceType= ControlDeviceTypeEnumExtensions.Get(name_value_map["Observable"]);
        }

        #endregion
        
        /// <summary>
        /// Create and return clone object
        /// </summary>
        /// <returns>Clone object</returns>
        public virtual object Clone()
        {
            return new ComputersEntity(this);
        }

    }
}

