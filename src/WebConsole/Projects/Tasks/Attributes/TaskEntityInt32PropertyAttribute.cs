using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.Tasks.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TaskEntityInt32PropertyAttribute:TaskEntityPropertyAttribute
    {
        public TaskEntityInt32PropertyAttribute(string root) : base(root) { }
    }
}