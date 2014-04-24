using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using VirusBlokAda.Vba32CC.DataBase;

namespace VirusBlokAda.Vba32CC.JSON.Entities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CompAdditionalInfo
    {
        #region Properties
        private String _compName = "-";
        [JsonProperty("computerName")]
        public String ComputerName
        {
            get { return _compName; }
            set { _compName = value; }
        }

        private String _userLogin = "-";
        [JsonProperty("userLogin")]
        public String UserLogin
        {
            get { return _userLogin; }
            set { _userLogin = value; }
        }

        private String _ipAddress = "-";
        [JsonProperty("ipAddress")]
        public String IPAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }

        private Boolean _controlCenter = false;
        [JsonProperty("controlCenter")]
        public Boolean ControlCenter
        {
            get { return _controlCenter; }
            set { _controlCenter = value; }
        }

        private String _domainName = "-";
        [JsonProperty("domainName")]
        public String DomainName
        {
            get { return _domainName; }
            set { _domainName = value; }
        }

        private String _osName = "-";
        [JsonProperty("osName")]
        public String OSName
        {
            get { return _osName; }
            set { _osName = value; }
        }

        private String _ram = "-";
        [JsonProperty("ram")]
        public String RAM
        {
            get { return _ram; }
            set { _ram = value; }
        }

        private String _cpu = "-";
        [JsonProperty("cpu")]
        public String CPU
        {
            get { return _cpu; }
            set { _cpu = value; }
        }

        private String _recentActive = "-";
        [JsonProperty("recentActive")]
        public String RecentActive
        {
            get { return _recentActive; }
            set { _recentActive = value; }
        }

        private String _latestUpdate = "-";
        [JsonProperty("latestUpdate")]
        public String LatestUpdate
        {
            get { return _latestUpdate; }
            set { _latestUpdate = value; }
        }

        private String _vba32Version = "-";
        [JsonProperty("vba32Version")]
        public string Vba32Version
        {
            get { return _vba32Version; }
            set { _vba32Version = value; }
        }

        private String _latestInfected = "-";
        [JsonProperty("latestInfected")]
        public String LatestInfected
        {
            get { return _latestInfected; }
            set { _latestInfected = value; }
        }

        private String _latestMalware = "-";
        [JsonProperty("latestMalware")]
        public String LatestMalware
        {
            get { return _latestMalware; }
            set { _latestMalware = value; }
        }

        private Boolean _vba32Integrity = false;
        [JsonProperty("vba32Integrity")]
        public Boolean Vba32Integrity
        {
            get { return _vba32Integrity; }
            set { _vba32Integrity = value; }
        }

        private Boolean _vba32KeyValid = false;
        [JsonProperty("vba32KeyValid")]
        public Boolean Vba32KeyValid
        {
            get { return _vba32KeyValid; }
            set { _vba32KeyValid = value; }
        }

        private String _description = "-";
        [JsonProperty("description")]
        public String Description
        {
            get { return _description; }
            set { _description = value; }
        }
        
        private String _policyName = "-";
        [JsonProperty("policyName")]
        public String PolicyName
        {
            get { return _policyName; }
            set { _policyName = value; }
        }

        private List<ComponentJSONEntity> _components = null;
        [JsonProperty("components")]
        public List<ComponentJSONEntity> Components
        {
            get { return _components; }
            set { _components = value; }
        }

        #endregion

        #region Constructors
        public CompAdditionalInfo(){}

        public CompAdditionalInfo(ComputersEntity comp)
        {
            this._compName = comp.ComputerName;
            this._userLogin = comp.UserLogin;
            this._ipAddress = comp.IPAddress;
            this._controlCenter = comp.ControlCenter;
            this._domainName = comp.DomainName;
            this._osName = comp.OSName;
            this._ram = comp.RAM.ToString();
            this._cpu = comp.CPUClock.ToString();
            this._recentActive = comp.RecentActive.ToString();
            this._latestUpdate = comp.LatestUpdate.ToString();
            this._vba32Version = comp.Vba32Version;
            this._latestInfected = comp.LatestInfected.ToString();
            this._latestMalware = comp.LatestMalware;
            this._vba32Integrity = comp.Vba32Integrity;
            this._vba32KeyValid = comp.Vba32KeyValid;
            this._description = comp.Description;
        }

        public CompAdditionalInfo(ComputersEntityEx comp)
            : this(comp as ComputersEntity)
        {
            this._policyName = comp.PolicyName;

            if (comp.Components == null) return;
            this._components = new List<ComponentJSONEntity>();
            foreach(ComponentsEntity ent in comp.Components)
            {
                this._components.Add(new ComponentJSONEntity(ent.ComponentName, ent.ComponentState, ent.Version));
            }
        }

        #endregion
    }
}
