using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.DataBase
{
    public class ScanningObjectProvider
    {
        private readonly String connectionString;
        private VlslVConnection connection;

        private ScanningObjectManager scanMngr;

        public ScanningObjectProvider(String connectionString)
        {
            this.connectionString = connectionString;
            connection = new VlslVConnection(connectionString);

            InitManagers();
        }

        ~ScanningObjectProvider()
        {
            connection.Dispose();
        }

        private void InitManagers()
        {
            scanMngr = new ScanningObjectManager(connection);
        }

        #region Methods

        /// <summary>
        /// Add comment in database
        /// </summary>
        public void AddComment(ScanningObjectEntity entity)
        {
            scanMngr.AddComment(entity);
        }

        /// <summary>
        /// Delete comment in database
        /// </summary>
        public void DeleteComment(String ip)
        {
            scanMngr.DeleteComment(ip);
        }

        /// <summary>
        /// Get comment by IPAddress
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public String GetComment(String ip)
        {
            return scanMngr.GetComment(ip);
        }

        #endregion
    }
}