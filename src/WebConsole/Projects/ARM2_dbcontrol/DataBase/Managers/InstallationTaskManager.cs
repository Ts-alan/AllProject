using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;

namespace VirusBlokAda.CC.DataBase
{
    internal sealed class InstallationTaskManager
    {
        private readonly String connectionString;
		
		#region Constructors
        public InstallationTaskManager(String connectionString)
		{
            this.connectionString = connectionString;
		}
		#endregion
		
		#region Methods

        /// <summary>
		/// Update task in database
		/// </summary>
        internal void UpdateTask(InstallationTaskEntity task)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UpdateInstallationTask", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", task.ID);
                cmd.Parameters.AddWithValue("@Status", task.Status);
                cmd.Parameters.AddWithValue("@Exitcode", task.ExitCode);
                cmd.Parameters.AddWithValue("@Error", task.Error);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Insert task in database
        /// </summary>
        internal Int64 InsertTask(InstallationTaskEntity task)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("InsertInstallationTask", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ComputerName", task.ComputerName);
                cmd.Parameters.AddWithValue("@IPAddress", task.IPAddress);
                cmd.Parameters.AddWithValue("@TaskType", task.TaskType);
                cmd.Parameters.AddWithValue("@Vba32Version", task.Vba32Version);
                cmd.Parameters.AddWithValue("@Status", task.Status);
                cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                cmd.Parameters.AddWithValue("@Exitcode", task.ExitCode);

                con.Open();
                return Convert.ToInt64(cmd.ExecuteScalar());
            }
        }

        /// <summary>
        /// Get installation task page
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <param name="order">Sort query</param>
        /// <param name="page">Page index</param>
        /// <param name="size">Page size</param>
        /// <returns></returns>
        internal List<InstallationTaskEntity> List(String where, String order, Int16 page, Int16 size)
		{
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetInstallationTasks", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Page", page);
                cmd.Parameters.AddWithValue("@RowCount", size);
                cmd.Parameters.AddWithValue("@Where", where);
                cmd.Parameters.AddWithValue("@OrderBy", order);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<InstallationTaskEntity> list = new List<InstallationTaskEntity>();
                while (reader.Read())
                {
                    InstallationTaskEntity task = new InstallationTaskEntity();
                    if (reader.GetValue(0) != DBNull.Value)
                        task.ID = reader.GetInt32(0);
                    if (reader.GetValue(1) != DBNull.Value)
                        task.ComputerName = reader.GetString(1);
                    if (reader.GetValue(2) != DBNull.Value)
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
		}

        /// <summary>
        /// Get installation task count
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <returns>Count of installation tasks</returns>
        internal Int32 Count(String where)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetInstallationTasksCount", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Where", where);

                con.Open();
                return (Int32)cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Get installation task type list
        /// </summary>
        /// <returns></returns>
        internal List<String> GetTaskTypes()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetListInstallationTaskTypes", con);
                cmd.CommandType = CommandType.StoredProcedure;
                
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
        /// Get installation task status list
        /// </summary>
        /// <returns></returns>
        internal List<String> GetStatuses()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetListInstallationStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
        /// Get Vba32 version list
        /// </summary>
        /// <returns></returns>
        internal List<String> GetVba32Versions()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetListVba32Versions", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
        /// Get computer name list
        /// </summary>
        /// <returns></returns>
        internal List<String> GetComputerNames()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT DISTINCT([ComputerName]) FROM InstallationTasks ORDER BY [ComputerName] ASC", con);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
