using System;
using System.Collections.Generic;
using System.Text;
using Tasks.Attributes;

namespace Tasks.Entities
{
    [Serializable]
    [TaskEntity("task")]
    public class ChangeDeviceProtectTaskEntity : TaskEntity
    { 
        public ChangeDeviceProtectTaskEntity() : base("ChangeDeviceProtect")
        {
        
        }

        public ChangeDeviceProtectTaskEntity(int index)
            : this()
        {
            _index = index;
        }

        private int _index;
        [TaskEntityStringProperty("Index")]
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
    }
}
