using System;
using System.Collections.Generic;
using System.Text;

namespace Tasks.Common
{
    public class TaskEventArgs: EventArgs
    {
        public TaskEventArgs(string _xml) : this (_xml, false)
        {
        }

        public TaskEventArgs(string _xml, bool _assignToAll)
        {
            xml = _xml;
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
