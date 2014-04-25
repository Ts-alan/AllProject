using System;
using System.Collections.Generic;
using VirusBlokAda.CC.Tasks.Attributes;
using System.Text;

namespace VirusBlokAda.CC.Tasks.Entities
{
    [TaskEntity("task")]
    public class QuerySystemInformationTaskEntity : TaskEntity
    {
        public QuerySystemInformationTaskEntity() : base("SystemInfo") { }

        [Obsolete("Необходимо переопределить")]
        public override string ToTaskXml()
        {
            StringBuilder result = new StringBuilder(256);

            result.Append("<TaskRequestSystemInfo></TaskRequestSystemInfo>");
            return result.ToString();
        }
    }
}
