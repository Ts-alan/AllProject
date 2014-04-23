using System;
using System.Collections.Generic;
using System.Text;

namespace Vba32ControlCenterUpdate
{
    internal enum EventEnum
    {
        Unknown,
        ActionBeforeReplaceFiles,
        ActionAfterReplaceFiles
    }

    internal static class EventEnumExtensions
    {
        /// <summary>
        /// Get EventEnum by name
        /// </summary>
        /// <param name="name">Name of item</param>
        /// <returns></returns>
        public static EventEnum Get(String name)
        {
            foreach (EventEnum en in Enum.GetValues(typeof(EventEnum)))
            {
                if (en.ToString() == name)
                    return en;
            }

            return EventEnum.Unknown;
        }
    }
}
