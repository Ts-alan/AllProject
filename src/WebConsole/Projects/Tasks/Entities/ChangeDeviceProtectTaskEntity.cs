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

        [Obsolete("Необходимо переопределить")]
        public override string ToTaskXml()
        {
            StringBuilder result = new StringBuilder(256);

            result.Append("<SetRegistrySettings>");
            result.AppendFormat(@"<Common><RegisterPath>{0}</RegisterPath><IsDeleteOld>0</IsDeleteOld></Common>",
                        @"HKLM\SOFTWARE\Vba32\Loader\Devices");
            result.AppendFormat(@"<Settings><DEVICE_PROTECT>reg_dword:{0}</DEVICE_PROTECT></Settings>", Index);
            result.Append("</SetRegistrySettings>");
            return result.ToString();
        }
    }
}
