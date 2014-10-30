using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Common;

namespace VirusBlokAda.CC.DataBase
{
    public class DataBaseProvider
    {
        public const String ProviderName = "DataBaseProvider";

        private DataBaseManager dbMngr;

        #region Constructors
        public DataBaseProvider(String connectionString)
        {
            InitManagers(connectionString);
        }

        private void InitManagers(String connectionString)
        {
            dbMngr = new DataBaseManager(connectionString);
        }
        public DataBaseProvider(String connectionString,DbProviderFactory factory)
        {
            InitManagers(connectionString,factory);
        }

        private void InitManagers(String connectionString,DbProviderFactory factory)
        {
            dbMngr = new DataBaseManager(connectionString,factory);
        }
        public DataBaseProvider(String connectionString,String factoryName)
        {
            InitManagers(connectionString,factoryName);
        }

        private void InitManagers(String connectionString,String factoryName)
        {
            dbMngr = new DataBaseManager(connectionString,factoryName);
        }
        #endregion
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
