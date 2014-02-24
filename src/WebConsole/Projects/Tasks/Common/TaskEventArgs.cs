using System;
using System.Collections.Generic;
using System.Text;

namespace Tasks.Common
{
    public class TaskEventArgs: EventArgs
    {
        public TaskEventArgs(string _xml,string _taskXml,string _taskName) : this (_xml,_taskXml,_taskName, false)
        {
        }

        public TaskEventArgs(string _xml,string _taskXml,string _taskName, bool _assignToAll)
        {
            xml = _xml;
            taskXml = _taskXml;
            taskName = _taskName;
            assignToAll = _assignToAll;
        }

        private readonly string xml;
        public string Xml
        {
            get
            {
                return xml;
            }
        }

        private readonly string taskXml;
        public string TaskXml
        {
            get
            {
                return taskXml;
            }
        }

        private readonly string taskName;
        public string TaskName
        {
            get
            {
                return taskName;
            }
        }

        private readonly bool assignToAll;
        public bool AssignToAll
        {
            get
            {
                return assignToAll;
            }
        }
    }
}
