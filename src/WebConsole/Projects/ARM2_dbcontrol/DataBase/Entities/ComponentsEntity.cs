using System;

namespace VirusBlokAda.CC.DataBase
{
    /// <summary>
    /// IMPORTANT: This class should never be manually edited.
    /// This entity class represents the properties and methods of a Components.
    /// </summary>
    public class ComponentsEntity : ICloneable
    {

        protected string computerName = String.Empty;
        protected string componentName = String.Empty;
        protected string componentState = String.Empty;
        protected string version = String.Empty;
        protected string name = String.Empty;

        //Default constructor
        public ComponentsEntity() { }

        //Constructor
        public ComponentsEntity(
            string computerName,
            string componentName,
            string componentState,
            string version,
            string name)
        {
            this.computerName = computerName;
            this.componentName = componentName;
            this.componentState = componentState;
            this.version = version;
            this.name = name;
        }

        #region Public Properties

        public string ComputerName
        {
            get { return computerName; }
            set { computerName = value; }
        }

        public string ComponentName
        {
            get { return componentName; }
            set { componentName = value; }
        }

        public string ComponentState
        {
            get { return componentState; }
            set { componentState = value; }
        }

        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        #endregion

        /// <summary>
        /// Create and return clone object
        /// </summary>
        /// <returns>Clone object</returns>
        public virtual object Clone()
        {
            return new ComponentsEntity(
                    this.computerName,
                    this.componentName,
                    this.ComponentState,
                    this.version,
                    this.name);

        }

    }
}

