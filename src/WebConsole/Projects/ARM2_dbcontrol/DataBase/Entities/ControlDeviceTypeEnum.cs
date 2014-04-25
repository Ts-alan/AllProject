using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.DataBase
{
    public enum ControlDeviceTypeEnum
    {
        Unknown = 1,
        Loader = 2,
        Vsis = 3,
        RCS = 4
    }

    public static class ControlDeviceTypeEnumExtensions
    {
        /// <summary>
        /// Get StateObject by name
        /// </summary>
        /// <param name="name">Name of item</param>
        /// <returns></returns>
        public static ControlDeviceTypeEnum Get(String name)
        {
            foreach (ControlDeviceTypeEnum en in Enum.GetValues(typeof(ControlDeviceTypeEnum)))
            {
                if (en.ToString() == name)
                    return en;
            }

            return ControlDeviceTypeEnum.Unknown;
        }
    }
}
