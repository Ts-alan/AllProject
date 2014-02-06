using System;
using System.Collections.Generic;
using System.Text;
using Tasks.Attributes;

namespace Tasks.Entities
{
    [TaskEntity("task")]
    public class InstallProductTaskEntity : TaskEntity
    { 
        public InstallProductTaskEntity() : base("InstallProduct")
        {
        
        }

        public InstallProductTaskEntity(string commandLine)
            : this()
        {
            _commandLine = commandLine;
        }

        private string _commandLine;
        [TaskEntityStringProperty("CommandLine")]
        public string CommandLine
        {
            get { return _commandLine; }
            set { _commandLine = value; }
        }

        private int _selectedIndex;
        [TaskEntityInt32Property("SelectedIndex")]
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { _selectedIndex = value; }
        }

    }
}
