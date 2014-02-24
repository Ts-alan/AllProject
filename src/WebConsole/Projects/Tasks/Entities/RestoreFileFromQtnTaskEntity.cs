using System;
using System.Collections.Generic;
using System.Text;
using Tasks.Attributes;

namespace Tasks.Entities
{
    [TaskEntity("task")]
    public class RestoreFileFromQtnTaskEntity : TaskEntity
    {
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
            throw new NotImplementedException();
        }
    }
}
