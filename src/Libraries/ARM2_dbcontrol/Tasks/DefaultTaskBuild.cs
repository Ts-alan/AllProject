using System;
using System.Collections.Generic;
using System.Text;
using ARM2_dbcontrol.Filters;
using VirusBlokAda.CC.Common.Xml;

namespace ARM2_dbcontrol.Tasks
{
    public class DefaultTaskBuild
    {
        private TaskUserCollection collection = null;

        public DefaultTaskBuild(TaskUserCollection collection)
        {
            this.collection = collection;    
        }
        /// <summary>
        /// Добавление заданий по умолчанию
        /// </summary>
        /// <param name="cancelTaskName"></param>
        /// <param name="systemInfoName"></param>
        /// <param name="listProcessesName"></param>
        public void AddDefaultTasks(string cancelTaskName, string systemInfoName, string listProcessesName)
        {
            AddTask(cancelTaskName,TaskType.CancelTask);
            AddTask(systemInfoName,TaskType.SystemInfo);
            AddTask(listProcessesName,TaskType.ListProcesses);
        }
       /// <summary>
       /// Добавление заданий
       /// </summary>
       /// <param name="name">имя</param>
       /// <param name="type">тип</param>
        public void AddTask(string name, TaskType type)
        {
            TaskUserEntity task = new TaskUserEntity();
            XmlBuilder xml = new XmlBuilder("root");
            task.Name = name;
            task.Type = type;

            xml.Generate();
            task.Param = xml.Result;

            collection.Add(task);
        }
       

        #region Property

        public TaskUserCollection Collection
        {
            get { return this.collection; }
            set { this.collection = value; }
        }

        #endregion


    }
}
