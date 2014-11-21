using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;

namespace VirusBlokAda.CC.DataBase
{
    /// <summary>
    /// IMPORTANT: This class should never be manually edited.
    /// Generated by VlslV CodeSmith Template.
    /// This class is used to manage the TaskEntity object.
    /// </summary>
    internal sealed class TaskManager
    {
        private readonly String connectionString; 
        private readonly DbProviderFactory factory;

        #region Constructors
        public TaskManager(String connectionString):this(connectionString,"System.Data.SqlClient")
        {            
        }

        public TaskManager(String connectionString,String DbFactoryName):this(connectionString,DbProviderFactories.GetFactory(DbFactoryName))
        {            
        }
        public TaskManager(String connectionString, DbProviderFactory factory)
        {
            this.connectionString = connectionString;
            this.factory = factory;
            
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get task page
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <param name="order">Sort query</param>
        /// <param name="page">Page index</param>
        /// <param name="size">Page size</param>
        /// <returns></returns>
        internal List<TaskEntity> List(String where, String order, Int32 page, Int32 size)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetTasksPage";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@page";
                param.Value = (Int64)page;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@rowcount";
                param.Value = (Int64)size;
                cmd.Parameters.Add(param);
               
                param=cmd.CreateParameter();
                param.ParameterName="@where";
                param.Value=where;
                cmd.Parameters.Add(param);

                param=cmd.CreateParameter();
                param.ParameterName="@orderby";
                param.Value=order;
                cmd.Parameters.Add(param);
               
                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<TaskEntity> list = new List<TaskEntity>();
                while (reader.Read())
                {
                    TaskEntity tasks = new TaskEntity();
                    if (reader.GetValue(0) != DBNull.Value)
                        tasks.ID = reader.GetInt64(0);
                    if (reader.GetValue(1) != DBNull.Value)
                        tasks.TaskName = reader.GetString(1);
                    if (reader.GetValue(2) != DBNull.Value)
                        tasks.ComputerName = reader.GetString(2);
                    if (reader.GetValue(3) != DBNull.Value)
                        tasks.TaskState = reader.GetString(3);
                    if (reader.GetValue(4) != DBNull.Value)
                        tasks.DateIssued = reader.GetDateTime(4);
                    if (reader.GetValue(5) != DBNull.Value)
                        tasks.DateComplete = reader.GetDateTime(5);
                    if (reader.GetValue(6) != DBNull.Value)
                        tasks.DateUpdated = reader.GetDateTime(6);
                    if (reader.GetValue(7) != DBNull.Value)
                        tasks.TaskParams = reader.GetString(7);
                    if (reader.GetValue(8) != DBNull.Value)
                        tasks.TaskUser = reader.GetString(8);
                    if (reader.GetValue(9) != DBNull.Value)
                        tasks.TaskDescription = reader.GetString(9);

                    list.Add(tasks);
                }

                reader.Close();
                return list;
            }
        }

        /// <summary>
        /// Task types list
        /// </summary>
        /// <returns></returns>
        internal List<TaskEntity> ListTaskTypes()
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetTaskTypesList";
               
                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<TaskEntity> list = new List<TaskEntity>();
                while (reader.Read())
                {
                    TaskEntity task = new TaskEntity();
                    task.ID = reader.GetInt16(0);
                    task.TaskName = reader.GetString(1);
                    list.Add(task);
                }

                reader.Close();
                return list;
            }
        }

        /// <summary>
        /// Create task and return taskID
        /// </summary>
        /// <param name="computerName"></param>
        /// <param name="taskName"></param>
        /// <param name="taskParams"></param>
        /// <returns></returns>
        internal Object CreateTask(String computerName, String taskName, String taskParams, String taskUser)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "CreateTask";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ComputerName";
                param.Value = computerName;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@TaskName";
                param.Value = taskName;
                cmd.Parameters.Add(param);
               
                param=cmd.CreateParameter();
                param.ParameterName="@TaskParams";
                param.Value=taskParams;
                cmd.Parameters.Add(param);

                param=cmd.CreateParameter();
                param.ParameterName="@TaskUser";
                param.Value=taskUser;
                cmd.Parameters.Add(param);

                con.Open();
                return cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Return tas type id. When insertIfNotExists=1 insert if not exist
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="insertIfNotExists"></param>
        /// <returns></returns>
        internal List<Int16> GetTaskTypeID(String taskName, Boolean insertIfNotExists)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetTaskTypeID";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@TaskName";
                param.Value = taskName;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@InsertIfNotExists";
                param.Value = insertIfNotExists;
                cmd.Parameters.Add(param);
               
                con.Open();                
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<Int16> list = new List<Int16>();
                while (reader.Read())
                {
                    list.Add(reader.GetInt16(0));
                }

                return list;
            }
        }

        /// <summary>
        /// Select entity from database with this id
        /// </summary>
        /// <param name="computersID">ID</param>
        /// <returns>id</returns>
        internal TaskEntity Get(Int64 tasksID)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetTaskByID";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ID";
                param.Value = tasksID;
                cmd.Parameters.Add(param);

                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                TaskEntity task = new TaskEntity();
                if (reader.Read())
                {
                    if (reader.GetValue(0) != DBNull.Value)
                        task.ID = reader.GetInt64(0);
                    if (reader.GetValue(5) != DBNull.Value)
                        task.TaskParams = reader.GetString(5);
                    if (reader.GetValue(6) != DBNull.Value)
                        task.TaskName = reader.GetString(6);
                    if (reader.GetValue(7) != DBNull.Value)
                        task.TaskUser = reader.GetString(7);
                }
                reader.Close();
                return task;
            }
        }

        /// <summary>
        /// ���������� IP-����� �����, �������� ���� ������ ������ � �������� ID
        /// </summary>
        /// <param name="tasksID">ID ����� ������</param>
        /// <returns></returns>
        internal String GetIPAddressByTaskID(Int64 tasksID)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetIPAddressByTaskID";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ID";
                param.Value = tasksID;
                cmd.Parameters.Add(param);

                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                String ip = String.Empty;
                if (reader.Read())
                {
                    if (reader.GetValue(0) != DBNull.Value)
                        ip = reader.GetString(0);
                }
                reader.Close();
                return ip;
            }
        }

        /// <summary>
        /// Get count of records with filter
        /// </summary>
        /// <param name="where">where clause</param>
        /// <returns></returns>
        internal Int32 Count(String where)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetTasksCount";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@where";
                param.Value = where;
                cmd.Parameters.Add(param);

                con.Open();
                return (Int32)cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Task states list
        /// </summary>
        /// <returns></returns>
        internal List<String> ListTaskStates()
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetTaskStatesList";
                
                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<String> list = new List<String>();
                while (reader.Read())
                {
                    if (reader.GetValue(0) != DBNull.Value)
                        list.Add(reader.GetString(0));
                }

                reader.Close();
                return list;
            }
        }

        /// <summary>
        /// Select entity from database with this id
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        internal AutomaticallyTaskEntity GetAutomaticallyTask(String eventName)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetTaskByEventType";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@EventName";
                param.Value = eventName;
                cmd.Parameters.Add(param);

                con.Open();                                
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                AutomaticallyTaskEntity task = new AutomaticallyTaskEntity();
                if (reader.Read())
                {
                    if (reader.GetValue(0) != DBNull.Value)
                        task.ID = reader.GetInt32(0);
                    if (reader.GetValue(1) != DBNull.Value)
                        task.EventID = reader.GetInt16(1);
                    if (reader.GetValue(2) != DBNull.Value)
                        task.TaskID = reader.GetInt16(2);
                    if (reader.GetValue(3) != DBNull.Value)
                        task.TaskParams = reader.GetString(3);
                    if (reader.GetValue(4) != DBNull.Value)
                        task.IsAllowed = reader.GetBoolean(4);
                }
                reader.Close();
                return task;
            }
        }

        /// <summary>
        /// Get computer names and IP-addresses
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        internal void GetComputersInfo(ref List<String> compNames, ref List<String> ipAddresses)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetComputerNamesAndIP";
      
                compNames.Clear();
                ipAddresses.Clear();

                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    if (reader.GetValue(0) != DBNull.Value)
                        compNames.Add(reader.GetString(0));
                    if (reader.GetValue(1) != DBNull.Value)
                        ipAddresses.Add(reader.GetString(1));
                }
                reader.Close();
            }
        }

        /// <summary>
        /// Get state task by computer name and taskID
        /// </summary>
        /// <param name="computerName"></param>
        /// <param name="taskID"></param>
        /// <returns></returns>
        internal Boolean IsRunningTask(String computerName, Int16 taskID)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "IsRunningTask";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ComputerName";
                param.Value = computerName;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@TaskID";
                param.Value = taskID;
                cmd.Parameters.Add(param);

                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                String state = String.Empty;
                Int32 minuteDiff = Int32.MaxValue;                

                while (reader.Read())
                {
                    if (reader.GetValue(0) != DBNull.Value)
                        state = reader.GetString(0);
                    if (reader.GetValue(1) != DBNull.Value)
                        minuteDiff = reader.GetInt32(1);
                }
                reader.Close();
                if (state == "Execution" || state == "Delivery" || minuteDiff < 30)
                    return true;

                return false;
            }
        }

        /// <summary>
        /// ������ ������ ������ � Delivery �� DelivetyTimeout
        /// </summary>
        /// <param name="dt">���� ������, ����� ������� ����� ������� ������</param>
        /// <returns></returns>
        internal void ChangeDeliveryState(DateTime dt)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "UpdateDeliveryState";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@Date";
                param.Value = dt;
                cmd.Parameters.Add(param);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Clear old task in database
        /// </summary>
        /// <param name="dt"></param>
        internal void ClearOldTasks(DateTime dt)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteOldTasks";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@Date";
                param.Value = dt;
                cmd.Parameters.Add(param);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        
        /// <summary>
        /// Get IPAddress list for configure agent
        /// </summary>
        /// <returns></returns>
        internal List<String> GetIPAddressListForConfigure()
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetIPAddressListForConfigure";

                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<String> list = new List<String>();
                while (reader.Read())
                {
                    if (reader.GetValue(0) != DBNull.Value)
                        list.Add(reader.GetString(0));
                }
                reader.Close();

                return list;
            }
        }

        #endregion
    }
}
