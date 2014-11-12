using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace VirusBlokAda.CC.DataBase
{
    public class ScanningObjectProvider
    {
        public const String ProviderName = "ScanningObjectProvider";

        private ScanningObjectManager scanMngr;

        #region Constructors
        public ScanningObjectProvider(String connectionString)
        {
            InitManagers(connectionString);
        }

        private void InitManagers(String connectionString)
        {
            scanMngr = new ScanningObjectManager(connectionString);
        }
        public ScanningObjectProvider(String connectionString,DbProviderFactory factory)
        {
            InitManagers(connectionString,factory);
        }

        private void InitManagers(String connectionString,DbProviderFactory factory)
        {
            scanMngr = new ScanningObjectManager(connectionString,factory);
        }
        public ScanningObjectProvider(String connectionString,String factoryName)
        {
            InitManagers(connectionString,factoryName);
        }

        private void InitManagers(String connectionString,String factoryName)
        {
            scanMngr = new ScanningObjectManager(connectionString,factoryName);
        }
        #endregion
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