using System;
using System.Collections.Generic;
using System.Text;

using ARM2_dbcontrol.Generation;

namespace ARM2_dbcontrol.Tasks
{
    /// <summary>
    /// Пользовательская задача
    /// </summary>
    public class TaskUserEntity
    {
        private string name = String.Empty;     //task name
        private TaskType type;                  //task type
        private string param = String.Empty;    //xml with params

        public TaskUserEntity()
        {

        }

        public TaskUserEntity(string name, TaskType type, string param)
        {
            this.name = name;
            this.type = type;
            this.param = param;
        }

        #region Property

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public TaskType Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        public string Param
        {
            get { return this.param; }
            set { this.param = value; }
        }

        #endregion

    }
}
