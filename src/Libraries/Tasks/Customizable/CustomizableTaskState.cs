using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.CC.Common;
using System.Web;
using System.Reflection;
using System.Xml.Serialization;
using VirusBlokAda.CC.Common.Collection;
using VirusBlokAda.CC.Tasks.Common;
using VirusBlokAda.CC.Tasks.Entities;

namespace VirusBlokAda.CC.Tasks.Customizable
{
    public class CustomizableTaskState :TaskState
    {
        #region Static Fields
        protected static readonly string temproraryTaskKey = "TemporaryTask";
        #endregion

        #region TaskState
        public bool IsValid
        {
            get
            {
                return (selectedTaskKey != temproraryTaskKey || !isTemporaryTaskClear);
            }
        }
        #endregion

        #region Tasks
        protected bool isTemporaryTaskClear;
        public bool IsTemporaryTaskClear
        {
            get { return isTemporaryTaskClear; }
        }

        private TaskEntity temproraryTask;

        private void VerifyTaskEntity(TaskEntity entity) {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            if (entity.GetType() != _taskType)
            {
                throw new ArgumentException(
                    String.Format("TaskEntity passed to CustomizableTaskState must be of {0} type", _taskType.Name),
                    "entity");
            }
        }

        public void SetTemporaryTask(TaskEntity entity)
        {
            VerifyTaskEntity(entity);
            temproraryTask = entity;
            isTemporaryTaskClear = false;
        }

        public void ClearTemporaryTask()
        {
            temproraryTask = null;
            isTemporaryTaskClear = true;
        }

        private SerializableDictionary<string, TaskEntity> storedTasks;

        public void SetStoredTask(string taskKey, TaskEntity entity)
        {
            VerifyTaskEntity(entity);
            storedTasks[taskKey] = entity;
            UpdateStorage();
        }

        public void DeleteStoredTask(string taskKey)
        {
            storedTasks.Remove(taskKey);
            UpdateStorage();
        }

        public TaskEntity GetSelectedTask()
        {
            TaskEntity task;
            if (selectedTaskKey == temproraryTaskKey)
            {
                task = temproraryTask;
            }
            else
            {
                task = storedTasks[selectedTaskKey];
            }
            return task;
        }
        #endregion

        #region Storage
        private void UpdateStorage()
        {
            CustomizableTaskStateCollectionStorage.Update(this);
        }

        private static CustomizableTaskState GetFromStorage(string taskName)
        {
            return CustomizableTaskStateCollectionStorage.Get(taskName);
        }
        #endregion

        #region Keys
        protected string selectedTaskKey;
        public string SelectedTaskKey
        {
            get { return selectedTaskKey; }
            set { selectedTaskKey = value; }
        }

        public List<String> GetStoredTaskKeys()
        {
            List<String> list = new List<String>();
            foreach (string key in storedTasks.Keys)
            {
                list.Add(key);
            }
            return list;
        }

        public string GetTemproraryTaskKey()
        {
            return temproraryTaskKey;
        }
        #endregion

        #region ITask
        public override bool IsActive()
        {
            return (IsSelected && IsValid);
        }

        public override string GetXmlString()
        {
            return GetSelectedTask().ToXmlString();
        }

        public override string GetTaskXml()
        {
            return GetSelectedTask().ToTaskXml();
        }

        public override string GetTaskName()
        {
            return GetSelectedTask().Type;
        }
        #endregion

        #region TaskType
        private Type _taskType;
        public Type GetTaskType()
        {
            return _taskType; 
        }
        #endregion

        #region Constructor
        protected CustomizableTaskState(Type taskType)
        {
            _taskType = taskType;
            selectedTaskKey = temproraryTaskKey;
            isTemporaryTaskClear = true;
            storedTasks = new SerializableDictionary<string, TaskEntity>();
        }
        #endregion

        #region Helper
        private static void VerifyType(Type type)
        {
            if (!type.IsSubclassOf(typeof(TaskEntity)))
            {
                throw new ArgumentException(
                    "Type passed to CustomizableTaskState.VerifyType has to be inherited from TaskEntity.", "type");
            }
        }
        #endregion

        #region Factory
        public static CustomizableTaskState Load(Type type)
        {
            VerifyType(type);
            CustomizableTaskState task = GetFromStorage(type.AssemblyQualifiedName);
            if (task == null)
            {
                task = new CustomizableTaskState(type);
            }
            return task;
        }
        #endregion

        #region SerializationInfo
        internal CustomizableTaskStateSerializationInfo ConvertToCustomizableTaskStateSerializationInfo()
        {
            CustomizableTaskStateSerializationInfo info = new CustomizableTaskStateSerializationInfo();
            info.TypeAssemblyQualifiedName = GetTaskType().AssemblyQualifiedName;
            List<NamedTaskEntity> list = new List<NamedTaskEntity>();
            foreach (string key in storedTasks.Keys)
            {
                NamedTaskEntity task = new NamedTaskEntity();
                task.Name = key;
                task.Entity = storedTasks[key];
                list.Add(task);
            }
            info.NamedTaskEntityList = list;
            return info;
        }

        internal static CustomizableTaskState LoadFromCustomizableTaskStateSerializationInfo
            (CustomizableTaskStateSerializationInfo info)
        {
            Type type = Type.GetType(info.TypeAssemblyQualifiedName);
            VerifyType(type);

            CustomizableTaskState state = new CustomizableTaskState(type);
            foreach (NamedTaskEntity next in info.NamedTaskEntityList)
            {
                state.storedTasks.Add(next.Name, next.Entity);
            }
            return state;
        }
        #endregion
    }
}
