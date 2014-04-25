﻿using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.CC.Tasks.Attributes;

namespace VirusBlokAda.CC.Tasks.Entities
{
    [TaskEntity("task")]
    public class QueryComponentsStateTaskEntity : TaskEntity
    {
        public QueryComponentsStateTaskEntity() : base("ComponentState") { }

        [Obsolete("Необходимо переопределить")]
        public override string ToTaskXml()
        {
            StringBuilder result = new StringBuilder(256);

            result.Append("<TaskRequestComponentState></TaskRequestComponentState>");
            return result.ToString();
        }
    }


}
