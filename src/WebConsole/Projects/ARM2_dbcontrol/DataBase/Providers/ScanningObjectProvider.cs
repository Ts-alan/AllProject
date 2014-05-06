using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.DataBase
{
    public class ScanningObjectProvider
    {
        public const String ProviderName = "ScanningObjectProvider";

        private ScanningObjectManager scanMngr;

        public ScanningObjectProvider(String connectionString)
        {
            InitManagers(connectionString);
        }

        private void InitManagers(String connectionString)
        {
            scanMngr = new ScanningObjectManager(connectionString);
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