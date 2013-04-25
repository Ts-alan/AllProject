using System;
using System.Collections.Generic;
using System.Text;
using Tasks.Entities;

namespace Tasks.Common
{
    public interface ITaskOptionsHelper<T> where T: TaskEntity
    {
        void LoadTaskEntity(T entity);
        T SaveTaskEntity(T oldEntity, out bool changed);
        T ConvertTaskEntity(TaskEntity entity);
    }
}
