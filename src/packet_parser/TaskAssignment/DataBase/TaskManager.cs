using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;

namespace Vba32CC.DataBase
{
    /// <summary>
    /// IMPORTANT: This class should never be manually edited.
    /// Generated by VlslV CodeSmith Template.
    /// This class is used to manage the TaskEntity object.
    /// </summary>
    public class TaskManager
    {

        VlslVConnection database;

        #region Constructors
        public TaskManager()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public TaskManager(VlslVConnection l_database)
        {
            database = l_database;
        }
        #endregion

        #region Methods
        
        /// <summary>
        /// Task types list
        /// </summary>
        /// <returns></returns>
        public List<TaskEntity> ListTaskTypes()
        {
            IDbCommand command = database.CreateCommand("GetTaskTypesList", true);

            SqlDataReader reader = command.ExecuteReader() as SqlDataReader;
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

        /// <summary>
        /// Create task and return taskID
        /// </summary>
        /// <param name="computerName"></param>
        /// <param name="taskName"></param>
        /// <param name="taskParams"></param>
        /// <returns></returns>
        public Object CreateTask(String computerName, String taskName, String taskParams, String taskUser)
        {
            IDbCommand command = database.CreateCommand("CreateTask", true);

            database.AddCommandParameter(command, "@ComputerName",
                DbType.String, computerName, ParameterDirection.Input);

            database.AddCommandParameter(command, "@TaskName",
                DbType.String, taskName, ParameterDirection.Input);

            database.AddCommandParameter(command, "@TaskParams",
                DbType.String, taskParams, ParameterDirection.Input);
            database.AddCommandParameter(command, "@TaskUser",
                DbType.String, taskUser, ParameterDirection.Input);

            return command.ExecuteScalar();
        }

        /// <summary>
        /// Return tas type id. When insertIfNotExists=1 insert if not exist
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="insertIfNotExists"></param>
        /// <returns></returns>
        public List<Int16> GetTaskTypeID(String taskName, Boolean insertIfNotExists)
        {
            IDbCommand command = database.CreateCommand("GetTaskTypeID", true);

            database.AddCommandParameter(command, "@TaskName",
                DbType.String, taskName, ParameterDirection.Input);

            database.AddCommandParameter(command, "@InsertIfNotExists",
                DbType.Boolean, insertIfNotExists, ParameterDirection.Input);

            List<Int16> list = new List<Int16>();
            SqlDataReader reader = command.ExecuteReader() as SqlDataReader;
            while (reader.Read())
            {
                list.Add(reader.GetInt16(0));
            }

            return list;
        }

        /// <summary>
        /// Select entity from database with this id
        /// </summary>
        /// <param name="computersID">ID</param>
        /// <returns>id</returns>
        public TaskEntity Get(Int64 tasksID)
        {
            IDbCommand command = database.CreateCommand("GetTaskByID", true);

            database.AddCommandParameter(command, "@ID",
                DbType.Int64, (Int64)tasksID, ParameterDirection.Input);

            TaskEntity task = new TaskEntity();
            SqlDataReader reader = command.ExecuteReader() as SqlDataReader;

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

        /// <summary>
        /// Select entity from database with this id
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public AutomaticallyTaskEntity GetAutomaticallyTask(String eventName)
        {
            IDbCommand command = database.CreateCommand("GetTaskByEventType", true);

            database.AddCommandParameter(command, "@EventName",
                DbType.String, eventName, ParameterDirection.Input);

            AutomaticallyTaskEntity task = new AutomaticallyTaskEntity();
            SqlDataReader reader = command.ExecuteReader() as SqlDataReader;

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

        /// <summary>
        /// ���������� IP-����� �����, �������� ���� ������ ������ � �������� ID
        /// </summary>
        /// <param name="tasksID">ID ����� ������</param>
        /// <returns></returns>
        public String GetIPAddressByTaskID(Int64 tasksID)
        {
            IDbCommand command = database.CreateCommand("GetIPAddressByTaskID", true);

            database.AddCommandParameter(command, "@ID",
                DbType.Int64, (Int64)tasksID, ParameterDirection.Input);

            SqlDataReader reader = command.ExecuteReader() as SqlDataReader;
            String ip = String.Empty;
            if (reader.Read())
            {
                if (reader.GetValue(0) != DBNull.Value)
                    ip = reader.GetString(0);
            }
            reader.Close();
            return ip;
        }

        /// <summary>
        /// Select entity from database with this id
        /// </summary>
        /// <param name="computersID">ID</param>
        /// <returns>id</returns>
        public ComputersEntity GetComputer(Int16 computersID)
        {
            IDbCommand command = database.CreateCommand("GetComputer", true);

            database.AddCommandParameter(command, "@ID",
                DbType.Int16, (Int16)computersID, ParameterDirection.Input);

            ComputersEntity computers = new ComputersEntity();
            SqlDataReader reader = command.ExecuteReader() as SqlDataReader;

            if (reader.Read())
            {
                if (reader.GetValue(0) != DBNull.Value)
                    computers.ID = reader.GetInt16(0);
                if (reader.GetValue(1) != DBNull.Value)
                    computers.ComputerName = reader.GetString(1);
                if (reader.GetValue(2) != DBNull.Value)
                    computers.IPAddress = reader.GetString(2);
                if (reader.GetValue(3) != DBNull.Value)
                    computers.ControlCenter = reader.GetBoolean(3);
                if (reader.GetValue(4) != DBNull.Value)
                    computers.DomainName = reader.GetString(4);
                if (reader.GetValue(5) != DBNull.Value)
                    computers.UserLogin = reader.GetString(5);
                if (reader.GetValue(6) != DBNull.Value)
                    computers.OSTypeID = reader.GetInt16(6);
                if (reader.GetValue(7) != DBNull.Value)
                    computers.RAM = reader.GetInt16(7);
                if (reader.GetValue(8) != DBNull.Value)
                    computers.CPUClock = reader.GetInt16(8);
                if (reader.GetValue(9) != DBNull.Value)
                    computers.RecentActive = reader.GetDateTime(9);
                if (reader.GetValue(10) != DBNull.Value)
                    computers.LatestUpdate = reader.GetDateTime(10);
                if (reader.GetValue(11) != DBNull.Value)
                    computers.Vba32Version = reader.GetString(11);
                if (reader.GetValue(12) != DBNull.Value)
                    computers.LatestInfected = reader.GetDateTime(12);
                if (reader.GetValue(13) != DBNull.Value)
                    computers.LatestMalware = reader.GetString(13);
                if (reader.GetValue(14) != DBNull.Value)
                    computers.Vba32Integrity = reader.GetBoolean(14);
                if (reader.GetValue(15) != DBNull.Value)
                    computers.Vba32KeyValid = reader.GetBoolean(15);
                if (reader.GetValue(16) != DBNull.Value)
                    computers.Description = reader.GetString(16);
            }
            reader.Close();
            return computers;
        }

        /// <summary>
        /// Get computer names and IP-addresses
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public void GetComputersInfo(ref List<String> compNames, ref List<String> ipAddresses)
        {
            IDbCommand command = database.CreateCommand("GetComputerNamesAndIP", true);

            compNames.Clear();
            ipAddresses.Clear();

            SqlDataReader reader = command.ExecuteReader() as SqlDataReader;

            while (reader.Read())
            {
                if (reader.GetValue(0) != DBNull.Value)
                    compNames.Add(reader.GetString(0));
                if (reader.GetValue(1) != DBNull.Value)
                    ipAddresses.Add(reader.GetString(1));
            }
            reader.Close();
        }

        /// <summary>
        /// Get state task by computer name and taskID
        /// </summary>
        /// <param name="computerName"></param>
        /// <param name="taskID"></param>
        /// <returns></returns>
        public Boolean IsRunningTask(String computerName, Int16 taskID)
        {            
            IDbCommand command = database.CreateCommand("IsRunningTask", true);

            database.AddCommandParameter(command, "@ComputerName",
                DbType.String, computerName, ParameterDirection.Input);

            database.AddCommandParameter(command, "@TaskID",
                DbType.Int16, taskID, ParameterDirection.Input);

            SqlDataReader reader = command.ExecuteReader() as SqlDataReader;

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
            if (state == "Execution" || state == "Delivery" || minuteDiff < 30 )
                return true;

            return false;
        }
        
        #endregion
    }
}
