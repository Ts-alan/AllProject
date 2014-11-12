using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.CC.Tasks.Attributes;

namespace VirusBlokAda.CC.Tasks.Entities
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

        [Obsolete("Необходимо переопределить")]
        public override string ToTaskXml()
        {
            StringBuilder content = new StringBuilder();
            content.Append("<InstallProduct>");
            content.AppendFormat(@"<ServerFile>{0}</ServerFile>", SelectedIndex);
            content.AppendFormat(@"<AdditionalArgs>{0}</AdditionalArgs>", CommandLine);
            content.Append(@"</InstallProduct>");
            return content.ToString();
        }

    }
}
