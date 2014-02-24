using System;
using System.Collections.Generic;
using System.Text;
using Tasks.Attributes;

namespace Tasks.Entities
{
    [TaskEntity("task")]
    public class UninstallTaskEntity : TaskEntity
    { 
        public UninstallTaskEntity() : base("Uninstall")
        {
        
        }

        public UninstallTaskEntity(string domain,string login,string password )
            : this()
        {
            _domain = domain;
            _login = login;
            _password = password;
        }

        private bool _rebootAfterInstall;
        [TaskEntityBooleanProperty("RebootAfterInstall", format = "reg_dword:{0}")]
        public bool RebootAfterInstall
        {
            get { return _rebootAfterInstall; }
            set { _rebootAfterInstall = value; }
        }

        private string _domain;
        [TaskEntityStringProperty("Domain")]
        public string Domain
        {
            get { return _domain; }
            set { _domain = value; }
        }
        private string _login;
        [TaskEntityStringProperty("Login")]
        public string Login
        {
            get { return _login; }
            set { _login = value; }
        }
        private string _password;        
        [TaskEntityStringProperty("Password")]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        private int _selectedIndex;
        [TaskEntityInt32Property("SelectedIndex")]
        public Int32 SelectedIndex
        {
            get { return _selectedIndex; }
            set { _selectedIndex = value; }
        }

        [Obsolete("Необходимо переопределить")]
        public override string ToTaskXml()
        {
            throw new NotImplementedException();
        }
    }
}
