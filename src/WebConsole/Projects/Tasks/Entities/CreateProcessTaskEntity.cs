using System;
using System.Collections.Generic;
using System.Text;
using Tasks.Attributes;

namespace Tasks.Entities
{
    [TaskEntity("task")]
    public class CreateProcessTaskEntity : TaskEntity
    { 
        public CreateProcessTaskEntity() : base("CreateProcess")
        {
        
        }

        public CreateProcessTaskEntity(string commandLine)
            : this()
        {
            _commandLine = commandLine;
        }

        private bool _commandSpec;
        [TaskEntityBooleanProperty("ComSpec", format = "reg_dword:{0}")]
        public bool CommandSpec
        {
            get { return _commandSpec; }
            set { _commandSpec = value; }
        }

        private string _commandLine;
        [TaskEntityStringProperty("CommandLine")]
        public string CommandLine
        {
            get { return _commandLine; }
            set { _commandLine = value; }
        }

        [Obsolete("Необходимо переопределить")]
        public override string ToTaskXml()
        {
            throw new NotImplementedException();
        }
    }
}
