using System;
using System.Collections.Generic;
using System.Text;

namespace Tasks.Attributes
{
    public class TaskEntityStringPropertyAttribute : TaskEntityPropertyAttribute
    {
        public TaskEntityStringPropertyAttribute(string root) : base(root) { }
        private bool _allowNullOrEmpty = true;
        public bool allowNullOrEmpty
        {
            get { return _allowNullOrEmpty; }
            set { _allowNullOrEmpty = value; }
        }
    }
}
