using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.Vba32CC.DataBase
{
    public class DataBaseEntity: ICloneable
    {
        protected string name = String.Empty;        
        protected string size = String.Empty;
        protected string maxSize = String.Empty;
        protected string path = String.Empty;

        //Default constructor
        public DataBaseEntity() { }

        //Constructor
        public DataBaseEntity(
            string _name,
            string _size,
            string _maxSize,
            string _path)
        {
            this.name = _name;
            this.size = _size;
            this.maxSize = _maxSize;
            this.path = _path;
        }

        #region Public Properties

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Size
        {
            get { return size; }
            set { size = value; }
        }

        public string MaxSize
        {
            get { return maxSize; }
            set { maxSize = value; }
        }

        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        #endregion

        /// <summary>
        /// Create and return clone object
        /// </summary>
        /// <returns>Clone object</returns>
        public virtual object Clone()
        {
            return new DataBaseEntity(
                    this.name,
                    this.size,
                    this.maxSize,
                    this.path);

        }
    }
}
