using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.Tasks.Common
{
    public abstract class TaskState: ITask
    {
        private bool _isSelected = false;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; }
        }

        public abstract bool IsActive();

        public abstract string GetXmlString();

        public abstract string GetTaskXml();

        public abstract string GetTaskName();
    }
}
