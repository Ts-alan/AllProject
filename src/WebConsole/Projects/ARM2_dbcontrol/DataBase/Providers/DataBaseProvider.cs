using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace VirusBlokAda.CC.DataBase
{
    public class DataBaseProvider
    {
        private DataBaseManager dbMngr;

        public DataBaseProvider(String connectionString)
        {
            InitManagers(connectionString);
        }

        private void InitManagers(String connectionString)
        {
            dbMngr = new DataBaseManager(connectionString);
        }

        #region Methods

        /// <summary>
        /// Select entity from database with this name
        /// </summary>
        /// <param name="database name">name</param>
        /// <returns>DataBaseEntity</returns>
        public DataBaseEntity Get(String dbName)
        {
            return dbMngr.Get(dbName);
        }

        /// <summary>
        /// Сжатие базы данных
        /// </summary>
        /// <param name="targetPercent">Is the desired percentage of free space left in the database file after the database has been shrunk</param>
        public void ShrinkDataBase(Int32 targetPercent)
        {
            dbMngr.ShrinkDataBase(targetPercent);
        }

        /// <summary>
        /// Check connection
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static Boolean CheckConnection(String connectionString, out Exception error)
        {
            Boolean result = true;
            error = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                }
            }
            catch(Exception e)
            {
                result = false;
                error = e;
            }

            return result;
        }

        #endregion
    }
}
