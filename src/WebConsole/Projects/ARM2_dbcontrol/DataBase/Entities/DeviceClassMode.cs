﻿using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.DataBase
{
    public enum DeviceClassMode
    {
        Undefined = 0,
        Enabled,
        Disabled,
        BlockWrite
    }

    public static class DeviceClassModeExtensions
    {
        /// <summary>
        /// Get DeviceClassMode by name
        /// </summary>
        /// <param name="name">Name of item</param>
        /// <returns></returns>
        public static DeviceClassMode Get(String name)
        {
            foreach (DeviceClassMode en in Enum.GetValues(typeof(DeviceClassMode)))
            {
                if (en.ToString() == name)
                    return en;
            }

            return DeviceClassMode.Undefined;
        }
    }
}