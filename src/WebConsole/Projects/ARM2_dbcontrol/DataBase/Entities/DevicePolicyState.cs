using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.DataBase
{
    /// <summary>
    /// Types of action are used to device device
    /// </summary>
    public enum DevicePolicyState
    {
        Undefined = 0,
        Enabled = 1,
        Disabled = 0
    }

    public static class DevicePolicyStateExtensions
    {
        /// <summary>
        /// Get DevicePolicyState by name
        /// </summary>
        /// <param name="name">Name of item</param>
        /// <returns></returns>
        public static DevicePolicyState Get(String name)
        {
            foreach (DevicePolicyState en in Enum.GetValues(typeof(DevicePolicyState)))
            {
                if (en.ToString() == name)
                    return en;
            }

            return DevicePolicyState.Undefined;
        }
    }
}
