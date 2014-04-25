using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace VirusBlokAda.CC.DataBase
{
    public class TemporaryGroupManager
    {
        VlslVConnection database; 
		
		#region Constructors
		public TemporaryGroupManager()
		{
		}
        public TemporaryGroupManager(VlslVConnection l_database)
		{
			database=l_database;
		}
		#endregion
		
		#region Methods

        public List<String> GetComputerNameList(String type, String where)
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
