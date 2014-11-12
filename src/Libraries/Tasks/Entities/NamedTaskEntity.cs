using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.Tasks.Entities
{
    public class NamedTaskEntity
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private TaskEntity _entity;
        public TaskEntity Entity
        {
            get { return _entity; }
            set { _entity = value; }
        }
    }
}
