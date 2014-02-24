using System;
using System.Collections.Generic;
using System.Text;
using Tasks.Attributes;

namespace Tasks.Entities
{
    [Serializable]
    [TaskEntity("task")]
    public class ConfigurePasswordTaskEntity : TaskEntity
    { 
        public ConfigurePasswordTaskEntity() : base("ConfigurePassword")
        {
        
        }

        public ConfigurePasswordTaskEntity(string password)
            : this()
        {
            _password = password;
        }

        private string _password;
        [TaskEntityStringProperty("Password")]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        [Obsolete("Необходимо переопределить")]
        public override string ToTaskXml()
        {
            throw new NotImplementedException();
        }
    }
}
