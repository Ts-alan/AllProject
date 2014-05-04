using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace VirusBlokAda.CC.DataBase
{
    internal sealed class TemporaryGroupManager
    {
        private readonly String connectionString;

        #region Constructors
        public TemporaryGroupManager(String connectionString)
        {
            this.connectionString = connectionString;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get computer name list by type
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="where">Filter query</param>
        /// <returns>Computer name list</returns>
        internal List<String> GetComputerNameList(String type, String where)
        {
            switch (type)
            {
                case "COMPONENTS":
                    return GetComputerNameListFromComponents(where);
                case "COMPUTERS":
                    return GetComputerNameListFromComputers(where);
                case "EVENTS":
                    return GetComputerNameListFromEvents(where);
                case "PROCESSES":
                    return GetComputerNameListFromProcesses(where);
                case "TASKS":
                    return GetComputerNameListFromTasks(where);
            }

            return null;
        }

        /// <summary>
        /// Get computer name list from process
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <returns>Computer name list</returns>
        private List<String> GetComputerNameListFromProcesses(String where)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetComputerNameListFromProcesses", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Where", where);

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
        /// Get computer name list from event
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <returns>Computer name list</returns>
        private List<String> GetComputerNameListFromEvents(String where)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetComputerNameListFromEvents", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Where", where);

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
        /// Get computer name list from task
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <returns>Computer name list</returns>
        private List<String> GetComputerNameListFromTasks(String where)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetComputerNameListFromTasks", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Where", where);

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
        /// Get computer name list from component
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <returns>Computer name list</returns>
        private List<String> GetComputerNameListFromComponents(String where)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetComputerNameListFromComponents", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Where", where);

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
        /// Get computer name list from computer
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <returns>Computer name list</returns>
        private List<String> GetComputerNameListFromComputers(String where)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetComputerNameListFromComputers", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Where", where);

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
