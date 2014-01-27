using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Vba32.ControlCenter.PeriodicalMaintenanceService.Xml;

namespace Vba32.ControlCenter.PeriodicalMaintenanceService.DataBase
{
	/// <summary>
	/// IMPORTANT: This class should never be manually edited.
	/// Generated by VlslV CodeSmith Template.
	/// This class is used to manage the EventsEntity object.
	/// </summary>
	public class EventsManager
	{
		VlslVConnection database; 
		
		#region Constructors
		public EventsManager()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public EventsManager(VlslVConnection l_database)
		{
			database=l_database;
		}
		#endregion
		
		#region Methods

        public List<EventsEntity> List(String where, String order, Int32 page, Int32 size)
		{
			IDbCommand command=database.CreateCommand("GetEventsPage",true);

			database.AddCommandParameter(command,"@page",
				DbType.Int32,(Int32)page,ParameterDirection.Input);

			database.AddCommandParameter(command,"@rowcount",
                DbType.Int32, (Int32)size, ParameterDirection.Input);

			database.AddCommandParameter(command,"@where",
				DbType.String,where,ParameterDirection.Input);

			database.AddCommandParameter(command,"@orderby",
				DbType.String,order,ParameterDirection.Input);

			SqlDataReader reader=command.ExecuteReader() as SqlDataReader;
            List<EventsEntity> list = new List<EventsEntity>();
			while(reader.Read())
			{
				EventsEntity events = new EventsEntity();
                if (reader.GetValue(0) != DBNull.Value)
                    events.Computer = reader.GetString(0);
				if(reader.GetValue(1)!= DBNull.Value)
					events.Event = reader.GetString(1);
                //if(reader.GetValue(2)!= DBNull.Value)
                //    events.Color = reader.GetString(2);
				if(reader.GetValue(3)!= DBNull.Value)
					events.Component = reader.GetString(3);
				if(reader.GetValue(4)!= DBNull.Value)
					events.EventTime = reader.GetDateTime(4);
				if(reader.GetValue(5)!= DBNull.Value)
					events.Object = reader.GetString(5);
				if(reader.GetValue(6)!= DBNull.Value)
					events.Comment = reader.GetString(6);
                
				list.Add(events);		
			}
			
			reader.Close();
			return list;
		}

		/// <summary>
		/// Get count of records with filter
		/// </summary>
		/// <param name="where">where clause</param>
		/// <returns></returns>
		public Int32 Count(String where)
		{
            IDbCommand command = database.CreateCommand("GetEventsCount", true);
			database.AddCommandParameter(command,"@where",
				DbType.String,where,ParameterDirection.Input);

			return (Int32)command.ExecuteScalar();
		}

        /// <summary>
        /// ������ ������ ������ � Delivery �� DelivetyTimeout
        /// </summary>
        /// <param name="dt">���� ������, ����� ������� ����� ������� ������</param>
        /// <returns></returns>
        public void ChangeDeliveryState(DateTime dt)
        {
            IDbCommand command = database.CreateCommand("UpdateDeliveryState", true);

            database.AddCommandParameter(command, "@Date",
                DbType.Date, dt, ParameterDirection.Input);

            command.ExecuteNonQuery();
        }

        public void ClearOldEvents(DateTime dt)
        {
            IDbCommand command = database.CreateCommand("DeleteOldEvents", true);

            database.AddCommandParameter(command, "@Date",
                DbType.Date, dt, ParameterDirection.Input);

            command.ExecuteNonQuery();
        }

        public void ClearOldTasks(DateTime dt)
        {
            IDbCommand command = database.CreateCommand("DeleteOldTasks", true);

            database.AddCommandParameter(command, "@Date",
                DbType.Date, dt, ParameterDirection.Input);

            command.ExecuteNonQuery();
        }

        public void ClearOldComputers(DateTime dt)
        {
            IDbCommand command = database.CreateCommand("DeleteOldComputers", true);

            if (DateTime.Now.Subtract(dt).Days > 0)
                database.AddCommandParameter(command, "@Date",
                    DbType.Date, dt, ParameterDirection.Input);

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// ������ ���� ������
        /// </summary>
        /// <param name="connectionString">������ ����������� � ��</param>
        /// <param name="targetPercent">Is the desired percentage of free space left in the database file after the database has been shrunk</param>
        public static void ShrinkDataBase(String connectionString, Int32 targetPercent)
        {
            Logger.Warning("ShrinkDataBase():: ������������� ����������!");
            System.Text.StringBuilder query = new System.Text.StringBuilder(64);
            query.AppendFormat("DBCC SHRINKDATABASE (VbaControlCenterDb, {0})", targetPercent);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query.ToString(), connection);
                connection.Open();
                command.ExecuteScalar();
                connection.Close();
            }
        }
		
		#endregion
    }		
}

