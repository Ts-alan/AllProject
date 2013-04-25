using System;
using System.Collections.Generic;
using System.Text;

namespace ARM2_dbcontrol.DataBase
{
    public class ComputersEntityEx: ComputersEntity
    {
        #region Properties
        
        private String policyName;
        public String PolicyName
        {
            get { return policyName; }
            set { policyName = value; }
        }

        private List<ComponentsEntity> components;
        public List<ComponentsEntity> Components
        {
            get { return components; }
            set { components = value; }
        }
        #endregion

        #region Constructors
        public ComputersEntityEx() { }

        public ComputersEntityEx(
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
            String description,
            List<ComponentsEntity> components
            ) : base(iD, computerName, iPAddress, controlCenter, domainName, userLogin, oSTypeID, rAM, cPUClock, recentActive,
                     latestUpdate, vba32Version, latestInfected, latestMalware, vba32Integrity, vba32KeyValid, description)
        {
            this.components = components;
        }

        public ComputersEntityEx(ComputersEntity comp, List<ComponentsEntity> components)
            : base(comp.ID, comp.ComputerName, comp.IPAddress, comp.ControlCenter, comp.DomainName, comp.UserLogin, comp.OSTypeID,
                   comp.RAM, comp.CPUClock, comp.RecentActive, comp.LatestUpdate, comp.Vba32Version, comp.LatestInfected,
                   comp.LatestMalware, comp.Vba32Integrity, comp.Vba32KeyValid, comp.Description)
        {
            this.OSName = comp.OSName;
            this.components = components;
        }

        public ComputersEntityEx(ComputersEntity comp, String policyName, List<ComponentsEntity> components)
            : base(comp.ID, comp.ComputerName, comp.IPAddress, comp.ControlCenter, comp.DomainName, comp.UserLogin, comp.OSTypeID,
                   comp.RAM, comp.CPUClock, comp.RecentActive, comp.LatestUpdate, comp.Vba32Version, comp.LatestInfected,
                   comp.LatestMalware, comp.Vba32Integrity, comp.Vba32KeyValid, comp.Description)
        {
            this.OSName = comp.OSName;
            this.policyName = policyName;
            this.components = components;
        }
        #endregion

        public override object  Clone()
        {
            return new ComputersEntityEx(
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
                    this.description,
                    this.components);
        }
    }
}
