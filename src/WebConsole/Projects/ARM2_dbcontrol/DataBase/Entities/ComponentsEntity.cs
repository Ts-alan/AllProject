using System;

namespace VirusBlokAda.CC.DataBase
{
    /// <summary>
    /// IMPORTANT: This class should never be manually edited.
    /// This entity class represents the properties and methods of a Components.
    /// </summary>
    public class ComponentsEntity : ICloneable
    {
        protected String computerName;
        protected String componentName;
        protected String componentState;
        protected String version;
        protected String name;

        #region Public Properties

        public String ComputerName
        {
            get { return computerName; }
            set { computerName = value; }
        }

        public String ComponentName
        {
            get { return componentName; }
            set { componentName = value; }
        }

        public String ComponentState
        {
            get { return componentState; }
            set { componentState = value; }
        }

        public String Version
        {
            get { return version; }
            set { version = value; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        #endregion

        #region Constructors
        //Default constructor
        public ComponentsEntity()
            : this(String.Empty, String.Empty, String.Empty, String.Empty, String.Empty)
        {
        }

        //Constructor
        public ComponentsEntity(
            String computerName,
            String componentName,
            String componentState,
            String version,
            String name)
        {
            this.computerName = computerName;
            this.componentName = componentName;
            this.componentState = componentState;
            this.version = version;
            this.name = name;
        }

        public ComponentsEntity(ComponentsEntity cmpt)
            : this(cmpt.ComputerName, cmpt.ComponentName, cmpt.ComponentState, cmpt.Version, cmpt.Name)
        {
        }

        #endregion

        /// <summary>
        /// Create and return clone object
        /// </summary>
        /// <returns>Clone object</returns>
        public virtual object Clone()
        {
            return new ComponentsEntity(this);

        }

    }
}

