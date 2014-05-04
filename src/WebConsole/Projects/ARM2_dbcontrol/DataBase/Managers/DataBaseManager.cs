using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace VirusBlokAda.CC.DataBase
{
    internal sealed class DataBaseManager
    {
        private readonly String connectionString;

        #region Constructors
        public DataBaseManager(String connectionString)
        {
            this.connectionString = connectionString;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Select entity from database with this name
        /// </summary>
        /// <param name="database name">name</param>
        /// <returns>DataBaseEntity</returns>
        internal DataBaseEntity Get(String dbName)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(String.Format("sp_helpfile '{0}'", dbName), con);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                DataBaseEntity db = new DataBaseEntity();
                if (reader.Read())
                {
                    if (reader.GetValue(0) != DBNull.Value)
                        db.Name = reader.GetString(0);
                    if (reader.GetValue(1) != DBNull.Value)
                        db.Path = reader.GetString(1);
                    if (reader.GetValue(3) != DBNull.Value)
                        db.Size = reader.GetString(3);
                    if (reader.GetValue(4) != DBNull.Value)
                        db.MaxSize = reader.GetString(4);

                }
                reader.Close();
                return db;
            }
        }

        /// <summary>
        /// —жатие базы данных
        /// </summary>
        /// <param name="targetPercent">Is the desired percentage of free space left in the database file after the database has been shrunk</param>
        internal void ShrinkDataBase(Int32 targetPercent)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(String.Format("DBCC SHRINKDATABASE (VbaControlCenterDb, {0})", targetPercent.ToString()), con);

                con.Open();
                cmd.ExecuteScalar();
            }
        }

        #endregion
    }
}
