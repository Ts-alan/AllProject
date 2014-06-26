using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.DataBase
{
    public enum UpdateStateEnum
    {
        None,
        Processing,
        Success,
        Fail
    }

    public static class UpdateStateEnumExtensions
    {
        /// <summary>
        /// Get DeviceType by name
        /// </summary>
        /// <param name="name">Name of item</param>
        /// <returns></returns>
        public static UpdateStateEnum Get(String name)
        {
            foreach (UpdateStateEnum en in Enum.GetValues(typeof(UpdateStateEnum)))
            {
                if (en.ToString() == name)
                    return en;
            }

            return UpdateStateEnum.None;
        }
    }
}
