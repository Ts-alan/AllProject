using System;

namespace VirusBlokAda.CC.DataBase
{
    /// <summary>
    /// IMPORTANT: This class should never be manually edited.
    /// This entity class represents the properties and methods of a Processes.
    /// </summary>
    public class ProcessesEntity : ICloneable
    {
        protected string computerName = String.Empty;
        protected string processName = String.Empty;
        protected int memorySize = Int32.MinValue;
        protected DateTime lastDate = DateTime.MinValue;

        //Default constructor
        public ProcessesEntity() { }

        //Constructor
        public ProcessesEntity(
            string computerName,
            string processName,
            int memorySize,
            DateTime lastDate)
        {
            this.computerName = computerName;
            this.processName = processName;
            this.memorySize = memorySize;
            this.lastDate = lastDate;
        }

        #region Public Properties


        public string ComputerName
        {
            get { return computerName; }
            set { computerName = value; }
        }

        public string ProcessName
        {
            get { return processName; }
            set { processName = value; }
        }

        public int MemorySize
        {
            get { return memorySize; }
            set { memorySize = value; }
        }

        public DateTime LastDate
        {
            get { return lastDate; }
            set { lastDate = value; }
        }

        #endregion

        /// <summary>
        /// Create and return clone object
        /// </summary>
        /// <returns>Clone object</returns>
        public virtual object Clone()
        {
            return new ProcessesEntity(
                    this.computerName,
                    this.processName,
                    this.memorySize,
                    this.lastDate);
        }

    }
}

