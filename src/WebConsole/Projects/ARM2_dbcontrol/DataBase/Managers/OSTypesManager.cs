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
    /// This class is used to manage the OSTypesEntity object.
    /// </summary>
    internal sealed class OSTypesManager
    {
        private readonly String connectionString;

        #region Constructors
        public OSTypesManager(String connectionString)
        {
            this.connectionString = connectionString;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Return all values from table 
        /// </summary>
        /// <returns>Result command</returns>
        internal List<OSTypesEntity> List()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetOSTypesList", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
        }

        /// <summary>
        /// Select OSName from database with this id
        /// </summary>
        /// <param name="oSTypesID">ID</param>
        /// <returns>id</returns>
        internal string GetOSName(Int16 oSTypesID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetOSName", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", oSTypesID);

                con.Open();
                return Convert.ToString(cmd.ExecuteScalar());
            }
        }

        #endregion
    }
}