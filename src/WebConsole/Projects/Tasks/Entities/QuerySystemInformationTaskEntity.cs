using System;
using System.Collections.Generic;
using Tasks.Attributes;

namespace Tasks.Entities
{
    [TaskEntity("task")]
    public class QuerySystemInformationTaskEntity : TaskEntity
    {
        public QuerySystemInformationTaskEntity() : base("SystemInfo") { }
    }
}
