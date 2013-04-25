using System;
using System.Collections.Generic;
using System.Text;

namespace Tasks.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TaskEntityBooleanPropertyAttribute:TaskEntityPropertyAttribute
    {
        public TaskEntityBooleanPropertyAttribute(string root) : base(root) { }
        string _replaceTrue = "1";
        string _replaceFalse = "0";
        public string replaceTrue
        {
            get
            {
                return _replaceTrue;
            }
            set
            {
                _replaceTrue = value;
            }
        }

        public string replaceFalse
        {
            get
            {
                return _replaceFalse;
            }
            set
            {
                _replaceFalse = value;
            }
        }


    }
}
