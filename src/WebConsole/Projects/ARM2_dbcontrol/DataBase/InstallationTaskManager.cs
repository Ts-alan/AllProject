using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;

namespace VirusBlokAda.Vba32CC.DataBase
{
    public class InstallationTaskManager
    {
        VlslVConnection database; 
		
		#region Constructors
		public InstallationTaskManager()
		{
			//
			// TODO: Add constructor logic here
			//
		}
        public InstallationTaskManager(VlslVConnection l_database)
		{
			database=l_database;
		}
		#endregion
		
		#region Methods

        /// <summary>
		/// Update task in database
		/// </summary>
		public void UpdateTask(InstallationTaskEntity task)
		{
            IDbCommand command = database.CreateCommand("UpdateInstallationTask", true);

            database.AddCommandParameter(command, "@ID",
                DbType.Int64, task.ID, ParameterDirection.Input);

            database.AddCommandParameter(command, "@Status",
                DbType.String, task.Status, ParameterDirection.Input);

            database.AddCommandParameter(command, "@Exitcode",
                DbType.Int16, task.ExitCode, ParameterDirection.Input);

            database.AddCommandParameter(command, "@Error",
                DbType.String, task.Error, ParameterDirection.Input);
			
			command.ExecuteNonQuery();
		}

        /// <summary>
        /// Insert task in database
        /// </summary>
        public Int64 InsertTask(InstallationTaskEntity task)
        {
            IDbCommand command = database.CreateCommand("InsertInstallationTask", true);

            database.AddCommandParameter(command, "@ComputerName",
                DbType.String, task.ComputerName, ParameterDirection.Input);

            database.AddCommandParameter(command, "@IPAddress",
                DbType.String, task.IPAddress, ParameterDirection.Input);

            database.AddCommandParameter(command, "@TaskType",
                DbType.String, task.TaskType, ParameterDirection.Input);
            
            database.AddCommandParameter(command, "@Vba32Version",
                DbType.String, task.Vba32Version, ParameterDirection.Input);

            database.AddCommandParameter(command, "@Status",
                DbType.String, task.Status, ParameterDirection.Input);

             database.AddCommandParameter(command, "@Date",
                DbType.DateTime, DateTime.Now, ParameterDirection.Input);

            database.AddCommandParameter(command, "@Exitcode",
                DbType.Int16, task.ExitCode, ParameterDirection.Input);
                        
            return Convert.ToInt64(command.ExecuteScalar());
        }


        public List<InstallationTaskEntity> List(String where, String order, Int32 page, Int32 size)
		{
            IDbCommand command = database.CreateCommand("GetInstallationTasks", true);

            database.AddCommandParameter(command, "@Page",
                DbType.Int16, (Int16)page, ParameterDirection.Input);

            database.AddCommandParameter(command, "@RowCount",
                DbType.Int16, (Int16)size, ParameterDirection.Input);

            database.AddCommandParameter(command, "@Where",
                DbType.String, where, ParameterDirection.Input);

            database.AddCommandParameter(command, "@OrderBy",
                DbType.String, order, ParameterDirection.Input);


			SqlDataReader reader=command.ExecuteReader() as SqlDataReader;
            List<InstallationTaskEntity> list = new List<InstallationTaskEntity>();
			while(reader.Read())
			{
				InstallationTaskEntity task = new InstallationTaskEntity();
                if (reader.GetValue(0) != DBNull.Value)
                    task.ID = reader.GetInt32(0);
				if(reader.GetValue(1)!= DBNull.Value)
					task.ComputerName = reader.GetString(1);
				if(reader.GetValue(2)!= DBNull.Value)
					task.IPAddress = reader.GetString(2);
                if (reader.GetValue(3) != DBNull.Value)
                    task.TaskType = reader.GetString(3);
                if (reader.GetValue(4) != DBNull.Value)
                    task.Vba32Version = reader.GetString(4);
                if (reader.GetValue(5) != DBNull.Value)
                    task.Status = reader.GetString(5);
                if (reader.GetValue(6) != DBNull.Value)
                    task.InstallationDate = reader.GetDateTime(6);
                if (reader.GetValue(7) != DBNull.Value)
                    task.ExitCode = reader.GetInt16(7);
                if (reader.GetValue(8) != DBNull.Value)
                    task.Error = reader.GetString(8);

				list.Add(task);		
			}
			
			reader.Close();
			return list;
			
		}

		public Int32 Count(String where)
		{

            IDbCommand command = database.CreateCommand("GetInstallationTasksCount", true);

            database.AddCommandParameter(command, "@Where",
                DbType.String, where, ParameterDirection.Input);


			return (Int32)command.ExecuteScalar();
		}

        public List<String> GetTaskTypes()
        {
            IDbCommand command = database.CreateCommand("GetListInstallationTaskTypes", true);

            SqlDataReader reader = command.ExecuteReader() as SqlDataReader;
            List<String> list = new List<String>();
            while (reader.Read())
            {
                if (reader.GetValue(0) != DBNull.Value)
                    list.Add(reader.GetString(0));
            }

            reader.Close();
            return list;
        }

        public List<String> GetStatuses()
        {
            IDbCommand command = database.CreateCommand("GetListInstallationStatus", true);

            SqlDataReader reader = command.ExecuteReader() as SqlDataReader;
            List<String> list = new List<String>();
            while (reader.Read())
            {
                if (reader.GetValue(0) != DBNull.Value)
                    list.Add(reader.GetString(0));
            }

            reader.Close();
            return list;
        }

        public List<String> GetVba32Versions()
        {
            IDbCommand command = database.CreateCommand("GetListVba32Versions", true);

            SqlDataReader reader = command.ExecuteReader() as SqlDataReader;
            List<String> list = new List<String>();
            while (reader.Read())
            {
                if (reader.GetValue(0) != DBNull.Value)
                    list.Add(reader.GetString(0));
            }

            reader.Close();
            return list;
        }

        public List<String> GetComputerNames()
        {
            IDbCommand command = database.CreateCommand("GetListComputerNames", true);

            SqlDataReader reader = command.ExecuteReader() as SqlDataReader;
            List<String> list = new List<String>();
            while (reader.Read())
            {
                if (reader.GetValue(0) != DBNull.Value)
                    list.Add(reader.GetString(0));
            }

            reader.Close();
            return list;
        }
		#endregion
    }
}
