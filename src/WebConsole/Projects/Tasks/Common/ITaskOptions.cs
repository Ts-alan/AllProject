using System;
using System.Collections.Generic;
using System.Text;
using Tasks.Entities;

namespace Tasks.Common
{
    public interface ITaskOptions
    {
        void LoadTaskEntity(TaskEntity entity);
        TaskEntity SaveTaskEntity(TaskEntity oldEntity, out bool changed);
        string DivOptionsClientID { get; }
        Type TaskType { get; }
    }
}
