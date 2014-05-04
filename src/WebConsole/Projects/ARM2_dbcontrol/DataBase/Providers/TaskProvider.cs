using System;
using System.Collections.Generic;

namespace VirusBlokAda.CC.DataBase
{
    public class TaskProvider
    {
        private TaskManager taskMngr;

        public TaskProvider(String connectionString)
        {
            InitManagers(connectionString);
        }

        private void InitManagers(String connectionString)
        {
            taskMngr = new TaskManager(connectionString);
        }

        #region Methods

        /// <summary>
        /// Get task page
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <param name="order">Sort query</param>
        /// <param name="page">Page index</param>
        /// <param name="size">Page size</param>
        /// <returns></returns>
        public List<TaskEntity> List(String where, String order, Int32 page, Int32 size)
        {
            return taskMngr.List(where, order, page, size);
        }

        /// <summary>
        /// Task types list
        /// </summary>
        /// <returns></returns>
        public List<TaskEntity> ListTaskTypes()
        {
            return taskMngr.ListTaskTypes();
        }

        /// <summary>
        /// Create task and return taskID
        /// </summary>
        /// <param name="computerName"></param>
        /// <param name="taskName"></param>
        /// <param name="taskParams"></param>
        /// <returns></returns>
        public Object CreateTask(String computerName, String taskName, String taskParams, String taskUser)
        {
            return taskMngr.CreateTask(computerName, taskName, taskParams, taskUser);
        }

        /// <summary>
        /// Return tas type id. When insertIfNotExists=1 insert if not exist
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="insertIfNotExists"></param>
        /// <returns></returns>
        public List<Int16> GetTaskTypeID(String taskName, Boolean insertIfNotExists)
        {
            return taskMngr.GetTaskTypeID(taskName, insertIfNotExists);
        }

        /// <summary>
        /// Select entity from database with this id
        /// </summary>
        /// <param name="computersID">ID</param>
        /// <returns>id</returns>
        public TaskEntity Get(Int64 tasksID)
        {
            return taskMngr.Get(tasksID);
        }

        /// <summary>
        /// Возвращает IP-адрес компа, которому была выдана задача с заданным ID
        /// </summary>
        /// <param name="tasksID">ID адрес задачи</param>
        /// <returns></returns>
        public String GetIPAddressByTaskID(Int64 tasksID)
        {
            return taskMngr.GetIPAddressByTaskID(tasksID);
        }

        /// <summary>
        /// Get count of records with filter
        /// </summary>
        /// <param name="where">where clause</param>
        /// <returns></returns>
        public Int32 Count(String where)
        {
            return taskMngr.Count(where);
        }

        /// <summary>
        /// Task states list
        /// </summary>
        /// <returns></returns>
        public List<String> ListTaskStates()
        {
            return taskMngr.ListTaskStates();
        }

        /// <summary>
        /// Select entity from database with this id
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public AutomaticallyTaskEntity GetAutomaticallyTask(String eventName)
        {
            return taskMngr.GetAutomaticallyTask(eventName);
        }
                
        /// <summary>
        /// Get computer names and IP-addresses
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public void GetComputersInfo(ref List<String> compNames, ref List<String> ipAddresses)
        {
            taskMngr.GetComputersInfo(ref compNames, ref ipAddresses);
        }

        /// <summary>
        /// Get state task by computer name and taskID
        /// </summary>
        /// <param name="computerName"></param>
        /// <param name="taskID"></param>
        /// <returns></returns>
        public Boolean IsRunningTask(String computerName, Int16 taskID)
        {
            return taskMngr.IsRunningTask(computerName, taskID);
        }

        /// <summary>
        /// Меняет статус задачи с Delivery на DelivetyTimeout
        /// </summary>
        /// <param name="dt">Дата выдачи, ранее которой будет изменен статус</param>
        /// <returns></returns>
        public void ChangeDeliveryState(DateTime dt)
        {
            taskMngr.ChangeDeliveryState(dt);
        }

        /// <summary>
        /// Clear old task in database
        /// </summary>
        /// <param name="dt"></param>
        public void ClearOldTasks(DateTime dt)
        {
            taskMngr.ClearOldTasks(dt);
        }

        /// <summary>
        /// Get IPAddress list for configure agent
        /// </summary>
        /// <returns></returns>
        public List<String> GetIPAddressListForConfigure()
        {
            return taskMngr.GetIPAddressListForConfigure();
        }        

        #endregion
    }
}
