using System;
using System.Collections.Generic;
using System.Text;

namespace ARM2_dbcontrol.Tasks.ProactiveProtection
{
    /// <summary>
    /// Перечисление типов правил проактивной защиты
    /// </summary>
    public enum RuleTypeEnum
    {
        Trusted,
        Neutral,
        Distrusted,
        Private
    }
}
