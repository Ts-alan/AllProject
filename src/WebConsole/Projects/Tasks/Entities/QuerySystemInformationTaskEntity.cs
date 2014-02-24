using System;
using System.Collections.Generic;
using Tasks.Attributes;

namespace Tasks.Entities
{
    [TaskEntity("task")]
    public class QuerySystemInformationTaskEntity : TaskEntity
    {
        public QuerySystemInformationTaskEntity() : base("SystemInfo") { }

        [Obsolete("Необходимо переопределить")]
        public override string ToTaskXml()
        {
            throw new NotImplementedException();
        }
    }
}
