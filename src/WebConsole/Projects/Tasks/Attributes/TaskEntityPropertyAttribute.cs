using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.Tasks.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class TaskEntityPropertyAttribute:Attribute
    {
        private string _name;
        public string name
        {
            get { return _name; }
        }

        private string _format;
        public string format
        {
            get { return _format; }
            set { _format = value; }
        }

        private string _dependOnTrueProperty = null;
        public string dependOnTrueProperty 
        {
            get { return _dependOnTrueProperty; }
            set { _dependOnTrueProperty = value; }
        }

        public TaskEntityPropertyAttribute(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentException("TaskEntityPropertyAttribute does not accept null or empty name parameter");
            }
            _name = name;
        }
    }
}
