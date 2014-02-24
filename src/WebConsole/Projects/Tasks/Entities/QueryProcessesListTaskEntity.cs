﻿using System;
using System.Collections.Generic;
using System.Text;
using Tasks.Attributes;

namespace Tasks.Entities
{
    [TaskEntity("task")]
    public class QueryProcessesListTaskEntity : TaskEntity
    {
        public QueryProcessesListTaskEntity() : base("ListProcesses") { }

        [Obsolete("Необходимо переопределить")]
        public override string ToTaskXml()
        {
            throw new NotImplementedException();
        }
    }
}
