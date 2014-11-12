using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.DataBase
{
    public enum DeviceMode
    {
        Undefined = 3,
        Disabled = 0,
        Enabled = 1,
        BlockWrite = 2
    }

    public static class DeviceModeExtensions
    {
        /// <summary>
        /// Get DeviceMode by name
        /// </summary>
        /// <param name="name">Name of item</param>
        /// <returns></returns>
        public static DeviceMode Get(String name)
        {
            foreach (DeviceMode en in Enum.GetValues(typeof(DeviceMode)))
            {
                if (en.ToString() == name)
                    return en;
            }

            return DeviceMode.Undefined;
        }
    }
}
