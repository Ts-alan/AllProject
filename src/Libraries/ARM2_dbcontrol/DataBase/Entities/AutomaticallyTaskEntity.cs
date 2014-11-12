using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.DataBase
{
    public class AutomaticallyTaskEntity
    {
        protected Int32 iD = Int32.MinValue;
        protected Int16 taskID = Int16.MinValue;
        protected Int16 eventID = Int16.MinValue;
        protected String taskParams = String.Empty;
        protected Boolean isAllowed = false;

        //Default constructor
        public AutomaticallyTaskEntity() { }

        //Constructor
        public AutomaticallyTaskEntity(
                Int32 iD,
                Int16 taskID,
                Int16 eventID,
                String taskParams,
                Boolean isAllowed)
        {
            this.iD = iD;
            this.taskID = taskID;
            this.eventID = eventID;
            this.taskParams = taskParams;
            this.isAllowed = isAllowed;
        }

        #region Public Properties

        public Int32 ID
        {
            get { return iD; }
            set { iD = value; }
        }

        public Int16 TaskID
        {
            get { return taskID; }
            set { taskID = value; }
        }

        public Int16 EventID
        {
            get { return eventID; }
            set { eventID = value; }
        }

        public String TaskParams
        {
            get { return taskParams; }
            set { taskParams = value; }
        }

        public Boolean IsAllowed
        {
            get { return isAllowed; }
            set { isAllowed = value; }
        }
        #endregion
    }
}
