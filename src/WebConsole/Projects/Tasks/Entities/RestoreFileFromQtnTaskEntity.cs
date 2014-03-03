using System;
using System.Collections.Generic;
using System.Text;
using Tasks.Attributes;

namespace Tasks.Entities
{
    [TaskEntity("task")]
    public class RestoreFileFromQtnTaskEntity : TaskEntity
    {
        private string comSpecOption = "\"%VBA32%Vba32Qtn.exe\" RF=";

        public RestoreFileFromQtnTaskEntity()
            : base("RestoreFileFromQtn")
        {        
        }

        public RestoreFileFromQtnTaskEntity(string fullpath)
            : this()
        {
            _fullPath = fullpath;
        }


        private string _fullPath;
        [TaskEntityStringProperty("FullPath")]
        public string FullPath
        {
            get { return _fullPath; }
            set { _fullPath = value; }
        }

        [Obsolete("Необходимо переопределить")]
        public override string ToTaskXml()
        {
           StringBuilder result = new StringBuilder(256);

            result.Append("<TaskCreateProcess>");
            result.AppendFormat(@"<CommandLine>{0}\{1}</CommandLine>", comSpecOption, FullPath);
            result.Append("</TaskCreateProcess>");

            return result.ToString();
        }
    }
}
