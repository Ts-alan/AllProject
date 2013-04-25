using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;

namespace ARM2_dbcontrol.DataBase
{
    /// <summary>
    /// IMPORTANT: This class should never be manually edited.
    /// Generated by VlslV CodeSmith Template.
    /// This class is used to manage the OSTypesEntity object.
    /// </summary>
    public class OSTypesManager
    {

        VlslVConnection database;

        #region Constructors
        public OSTypesManager()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public OSTypesManager(VlslVConnection l_database)
        {
            database = l_database;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Return all values from table 
        /// </summary>
        /// <returns>Result command</returns>
        public List<OSTypesEntity> List()
        {
            IDbCommand command = database.CreateCommand("GetOSTypesList", true);

            SqlDataReader reader = command.ExecuteReader() as SqlDataReader;
            List<OSTypesEntity> list = new List<OSTypesEntity>();
            while (reader.Read())
            {
                OSTypesEntity oSTypes = new OSTypesEntity();
                oSTypes.ID = reader.GetInt16(0);
                oSTypes.OSName = reader.GetString(1);
                list.Add(oSTypes);
            }

            reader.Close();
            return list;

        }


        /// <summary>
        /// Select OSName from database with this id
        /// </summary>
        /// <param name="oSTypesID">ID</param>
        /// <returns>id</returns>
        public string GetOSName(Int16 oSTypesID)
        {
            IDbCommand command = database.CreateCommand("GetOSName", true);

            database.AddCommandParameter(command, "@ID",
                DbType.Int16,(Int16) oSTypesID, ParameterDirection.Input);

            return Convert.ToString(command.ExecuteScalar());

        }

        #endregion


    }
}

