using System;

namespace VirusBlokAda.CC.DataBase
{
    /// <summary>
    /// IMPORTANT: This class should never be manually edited.
    /// This entity class represents the properties and methods of a OSTypes.
    /// </summary>
    public class OSTypesEntity : ICloneable
    {

        protected short iD = Int16.MinValue;
        protected string oSName = String.Empty;

        //Default constructor
        public OSTypesEntity() { }

        //Constructor
        public OSTypesEntity(
            short iD,
            string oSName)
        {
            this.iD = iD;
            this.oSName = oSName;
        }

        #region Public Properties

        public short ID
        {
            get { return iD; }
            set { iD = value; }
        }

        public string OSName
        {
            get { return oSName; }
            set { oSName = value; }
        }
        #endregion

        /// <summary>
        /// Create and return clone object
        /// </summary>
        /// <returns>Clone object</returns>
        public virtual object Clone()
        {
            return new OSTypesEntity(
                    this.iD,
                    this.oSName);

        }

    }
}

