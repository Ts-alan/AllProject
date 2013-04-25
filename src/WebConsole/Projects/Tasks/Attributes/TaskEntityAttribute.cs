using System;
using System.Collections.Generic;
using System.Text;

namespace Tasks.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TaskEntityAttribute: Attribute
    {
        string _root;
        public string root
        {
            get { return _root; }
        }
        public TaskEntityAttribute(string root)
        { 
            if (String.IsNullOrEmpty(root))
            {
                throw new ArgumentException("TaskEntityAttribute does not accept null or empty root parameter");
            }
            _root = root;
        }
    }
}
