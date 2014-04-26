using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace VirusBlokAda.CC.DataBase
{
    internal sealed class TemporaryGroupManager
    {
        private VlslVConnection database; 
		
		#region Constructors
        public TemporaryGroupManager(VlslVConnection l_database)
		{
			database=l_database;
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
            IDbCommand command = database.CreateCommand("GetComputerNameListFromProcesses", true);

            database.AddCommandParameter(command, "@Where",
                DbType.String, where, ParameterDirection.Input);

			SqlDataReader reader=command.ExecuteReader() as SqlDataReader;
            List<String> list = new List<String>();
			while(reader.Read())
			{
                if (reader.GetValue(0) != DBNull.Value)
                    list.Add(reader.GetString(0));
			}
			
			reader.Close();
			return list;
		}

        /// <summary>
        /// Get computer name list from event
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <returns>Computer name list</returns>
        private List<String> GetComputerNameListFromEvents(String where)
        {
            IDbCommand command = database.CreateCommand("GetComputerNameListFromEvents", true);

            database.AddCommandParameter(command, "@Where",
                DbType.String, where, ParameterDirection.Input);

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

        /// <summary>
        /// Get computer name list from task
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <returns>Computer name list</returns>
        private List<String> GetComputerNameListFromTasks(String where)
        {
            IDbCommand command = database.CreateCommand("GetComputerNameListFromTasks", true);

            database.AddCommandParameter(command, "@Where",
                DbType.String, where, ParameterDirection.Input);

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

        /// <summary>
        /// Get computer name list from component
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <returns>Computer name list</returns>
        private List<String> GetComputerNameListFromComponents(String where)
        {
            IDbCommand command = database.CreateCommand("GetComputerNameListFromComponents", true);

            database.AddCommandParameter(command, "@Where",
                DbType.String, where, ParameterDirection.Input);

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

        /// <summary>
        /// Get computer name list from computer
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <returns>Computer name list</returns>
        private List<String> GetComputerNameListFromComputers(String where)
        {
            IDbCommand command = database.CreateCommand("GetComputerNameListFromComputers", true);

            database.AddCommandParameter(command, "@Where",
                DbType.String, where, ParameterDirection.Input);

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
