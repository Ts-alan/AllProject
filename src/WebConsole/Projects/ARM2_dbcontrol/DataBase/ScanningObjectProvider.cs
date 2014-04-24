using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.Vba32CC.DataBase
{
    public class ScanningObjectProvider
    {
        private readonly String connectionString;
        private ScanningObjectManager scanMngr;

        public ScanningObjectProvider()
        { }

        public ScanningObjectProvider(String connectionString)
        {
            this.connectionString = connectionString;
        }

        #region Methods
                
        /// <summary>
        /// Add comment in database
        /// </summary>
        public void AddComment(ScanningObjectEntity entity)
        {
            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                scanMngr = new ScanningObjectManager(conn);
                conn.OpenConnection();

                scanMngr.AddComment(entity);

                conn.CloseConnection();
            }
        }

        /// <summary>
        /// Delete comment in database
        /// </summary>
        public void DeleteComment(String ip)
        {
            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                scanMngr = new ScanningObjectManager(conn);
                conn.OpenConnection();

                scanMngr.DeleteComment(ip);

                conn.CloseConnection();
            }
        }

        /// <summary>
        /// Get comment by IPAddress
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public String GetComment(String ip)
        {
            String comment = String.Empty;
            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                scanMngr = new ScanningObjectManager(conn);
                conn.OpenConnection();

                comment = scanMngr.GetComment(ip);

                conn.CloseConnection();
            }

            return comment;
        }

        #endregion
    }
}
