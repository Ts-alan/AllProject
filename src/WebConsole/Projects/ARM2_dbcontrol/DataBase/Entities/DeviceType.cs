using System;

namespace VirusBlokAda.CC.DataBase
{
    public enum DeviceType
    {
        USB,
        NET
    }

    public static class DeviceTypeExtensions
    {
        /// <summary>
        /// Get DeviceType by name
        /// </summary>
        /// <param name="name">Name of item</param>
        /// <returns></returns>
        public static DeviceType Get(String name)
        {
            foreach (DeviceType en in Enum.GetValues(typeof(DeviceType)))
            {
                if (en.ToString() == name)
                    return en;
            }

            return DeviceType.USB;
        }
    }
}
