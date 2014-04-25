using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.CC.Tasks.Entities;

namespace VirusBlokAda.CC.Tasks.Common
{
    public interface ITaskOptions
    {
        void LoadTaskEntity(TaskEntity entity);
        TaskEntity SaveTaskEntity(TaskEntity oldEntity, out bool changed);
        string DivOptionsClientID { get; }
        Type TaskType { get; }
    }
}
