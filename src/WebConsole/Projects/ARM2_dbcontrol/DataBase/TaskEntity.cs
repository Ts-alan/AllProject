using System;

namespace ARM2_dbcontrol.DataBase
{
    /// <summary>
    /// IMPORTANT: This class should never be manually edited.
    /// This entity class represents the properties and methods of a Task.
    /// </summary>
    public class TaskEntity : ICloneable
    {

        protected Int64 iD = Int64.MinValue;
        protected String taskName = String.Empty;
        protected String computerName = String.Empty;
        protected String taskState = String.Empty;
        protected DateTime dateIssued = DateTime.MinValue;
        protected DateTime dateComplete = DateTime.MinValue;
        protected DateTime dateUpdated = DateTime.MinValue;
        protected String taskParams = String.Empty;
        protected String taskUser = String.Empty;

        //Default constructor
        public TaskEntity() { }

        //Constructor
        public TaskEntity(
            Int64 iD,
            String taskName,
            String computerName,
            String taskState,
            DateTime dateIssued,
            DateTime dateComplete,
            DateTime dateUpdated,
            String taskParams,
            String taskUser)
        {
            this.iD = iD;
            this.taskName = taskName;
            this.computerName = computerName;
            this.taskState = taskState;
            this.dateIssued = dateIssued;
            this.dateComplete = dateComplete;
            this.dateUpdated = dateUpdated;
            this.taskParams = taskParams;
            this.taskUser = taskUser;
        }

        #region Public Properties

        public Int64 ID
        {
            get { return iD; }
            set { iD = value; }
        }

        public String TaskName
        {
            get { return taskName; }
            set { taskName = value; }
        }

        public String ComputerName
        {
            get { return computerName; }
            set { computerName = value; }
        }

        public String TaskState
        {
            get { return taskState; }
            set { taskState = value; }
        }

        public DateTime DateIssued
        {
            get { return dateIssued; }
            set { dateIssued = value; }
        }

        public DateTime DateComplete
        {
            get { return dateComplete; }
            set { dateComplete = value; }
        }

        public DateTime DateUpdated
        {
            get { return dateUpdated; }
            set { dateUpdated = value; }
        }

        public String TaskParams
        {
            get { return taskParams; }
            set { taskParams = value; }
        }

        public String TaskUser
        {
            get { return taskUser; }
            set { taskUser = value; }
        }
        #endregion

        /// <summary>
        /// Create and return clone object
        /// </summary>
        /// <returns>Clone object</returns>
        public virtual object Clone()
        {
            return new TaskEntity(
                    this.iD,
                    this.taskName,
                    this.computerName,
                    this.taskState,
                    this.dateIssued,
                    this.dateComplete,
                    this.dateUpdated,
                    this.taskParams,
                    this.taskUser);
        }

    }
}

