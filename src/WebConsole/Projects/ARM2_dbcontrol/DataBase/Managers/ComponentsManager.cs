using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;

namespace VirusBlokAda.CC.DataBase
{
    /// <summary>
    /// IMPORTANT: This class should never be manually edited.
    /// Generated by VlslV CodeSmith Template.
    /// This class is used to manage the ComponentEntity object.
    /// </summary>
    internal sealed class ComponentsManager
    {
        private VlslVConnection database;

        #region Constructors
        public ComponentsManager()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public ComponentsManager(VlslVConnection l_database)
        {
            database = l_database;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get component from SqlDataReader
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        internal static ComponentsEntity GetComponentFromReader(IDataReader reader)
        {
            ComponentsEntity component = new ComponentsEntity();

            if (reader.GetValue(0) != DBNull.Value)
                component.ComputerName = reader.GetString(0);
            if (reader.GetValue(1) != DBNull.Value)
                component.ComponentName = reader.GetString(1);
            if (reader.GetValue(2) != DBNull.Value)
                component.ComponentState = reader.GetString(2);
            if (reader.GetValue(3) != DBNull.Value)
                component.Version = reader.GetString(3);
            if (reader.GetValue(4) != DBNull.Value)
                component.Name = reader.GetString(4);

            return component;
        }

        /// <summary>
        /// Get components from SqlDataReader
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        internal static List<ComponentsEntity> GetComponentsFromReader(IDataReader reader)
        {
            List<ComponentsEntity> components = new List<ComponentsEntity>();

            while (reader.Read())
            {
                components.Add(GetComponentFromReader(reader));
            }

            return components;
        }

        /// <summary>
        /// Get page with sorting and filter
        /// </summary>
        /// <param name="where">where clause</param>
        /// <param name="order">order clause</param>
        /// <param name="page">page number</param>
        /// <param name="size">records per page</param>
        /// <returns></returns>
        internal List<ComponentsEntity> List(String where, String order, Int32 page, Int32 size)
        {
            IDbCommand command = database.CreateCommand("GetComponentsPage", true);

            database.AddCommandParameter(command, "@page",
                DbType.Int32, (Int32)page, ParameterDirection.Input);

            database.AddCommandParameter(command, "@rowcount",
                DbType.Int32, (Int32)size, ParameterDirection.Input);

            database.AddCommandParameter(command, "@where",
                DbType.String, where, ParameterDirection.Input);

            database.AddCommandParameter(command, "@orderby",
                DbType.String, order, ParameterDirection.Input);


            SqlDataReader reader = command.ExecuteReader() as SqlDataReader;
            List<ComponentsEntity> list = GetComponentsFromReader(reader);
            reader.Close();
            return list;
        }

        /// <summary>
        /// Get component page for computer
        /// </summary>
        /// <param name="ID">Computer's ID</param>
        /// <returns>List of components</returns>
        internal List<ComponentsEntity> GetComponentsPageByComputerID(Int16 ID)
        {
            IDbCommand command = database.CreateCommand("GetComponentsPageByComputerID", true);

            database.AddCommandParameter(command, "@ID",
                DbType.Int16, ID, ParameterDirection.Input);

            SqlDataReader reader = command.ExecuteReader() as SqlDataReader;
            List<ComponentsEntity> list = GetComponentsFromReader(reader);
            reader.Close();
            return list;
        }

        /// <summary>
        /// Get count of records with filter
        /// </summary>
        /// <param name="where">where clause</param>
        /// <returns></returns>
        internal Int32 Count(String where)
        {
            IDbCommand command = database.CreateCommand("GetComponentsCount", true);

            database.AddCommandParameter(command, "@where",
                DbType.String, where, ParameterDirection.Input);

            return (Int32)command.ExecuteScalar();
        }

        /// <summary>
        /// Component types list
        /// </summary>
        /// <returns></returns>
        internal List<ComponentsEntity> ListComponentState()
        {
            IDbCommand command = database.CreateCommand("GetComponentStateList", true);

            SqlDataReader reader = command.ExecuteReader() as SqlDataReader;
            List<ComponentsEntity> list = new List<ComponentsEntity>();
            while (reader.Read())
            {
                ComponentsEntity cmpt = new ComponentsEntity();
                cmpt.ComponentState = reader.GetString(0);
                list.Add(cmpt);
            }

            reader.Close();
            return list;

        }

        /// <summary>
        /// Component types list
        /// </summary>
        /// <returns></returns>
        internal List<ComponentsEntity> ListComponentType()
        {
            IDbCommand command = database.CreateCommand("GetComponentTypeList", true);

            SqlDataReader reader = command.ExecuteReader() as SqlDataReader;
            List<ComponentsEntity> list = new List<ComponentsEntity>();
            while (reader.Read())
            {
                ComponentsEntity cmpt = new ComponentsEntity();
                cmpt.ComponentName = reader.GetString(0);
                list.Add(cmpt);
            }

            reader.Close();
            return list;

        }

        /// <summary>
        /// Get component settings 
        /// </summary>
        /// <param name="compID">Computer's ID</param>
        /// <param name="componentName">Component name</param>
        /// <returns>XML serializable settings</returns>
        internal String GetCurrentSettings(Int16 compID, String componentName)
        {
            IDbCommand command = database.CreateCommand("GetComponentCurrentSettings", true);

            database.AddCommandParameter(command, "@compId", DbType.Int16, compID, ParameterDirection.Input);
            database.AddCommandParameter(command, "@name", DbType.String, componentName, ParameterDirection.Input);

            SqlDataReader reader = (SqlDataReader)command.ExecuteReader();
            if (reader.Read())
                if (reader.GetValue(1) != DBNull.Value)
                    return reader.GetString(1);

            return null;
        }
        
        #endregion
    }
}

