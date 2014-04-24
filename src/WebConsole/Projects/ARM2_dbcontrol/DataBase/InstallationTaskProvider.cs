using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.Vba32CC.DataBase
{
    public class InstallationTaskProvider
    {
        private readonly String connectionString;
        private InstallationTaskManager installMngr;

        public InstallationTaskProvider()
        { }

        public InstallationTaskProvider(String connectionString)
        {
            this.connectionString = connectionString;
        }

        #region Methods

        /// <summary>
        /// Update task in database
        /// </summary>
        public void UpdateTask(InstallationTaskEntity task)
        {
            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                installMngr = new InstallationTaskManager(conn);
                conn.OpenConnection();

                installMngr.UpdateTask(task);

                conn.CloseConnection();
            }            
        }

        /// <summary>
        /// Insert task in database
        /// </summary>
        public Int64 InsertTask(InstallationTaskEntity task)
        {
            Int64 id = Int64.MinValue;
            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                installMngr = new InstallationTaskManager(conn);
                conn.OpenConnection();

                id = installMngr.InsertTask(task);

                conn.CloseConnection();
            }

            return id;
        }


        public List<InstallationTaskEntity> List(String where, String order, Int32 page, Int32 size)
        {
            List<InstallationTaskEntity> list = new List<InstallationTaskEntity>();
            
            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                installMngr = new InstallationTaskManager(conn);
                conn.OpenConnection();

                list = installMngr.List(where, order, page, size);

                conn.CloseConnection();
            }

            return list;
        }

        public Int32 Count(String where)
        {
            Int32 count = Int32.MinValue;
            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                installMngr = new InstallationTaskManager(conn);
                conn.OpenConnection();

                count = installMngr.Count(where);

                conn.CloseConnection();
            }

            return count;
        }

        public List<String> GetVba32Versions()
        {
            List<String> list = new List<String>();
            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                installMngr = new InstallationTaskManager(conn);
                conn.OpenConnection();

                list = installMngr.GetVba32Versions();

                conn.CloseConnection();
            }

            return list;
        }

        public List<String> GetTaskTypes()
        {
            List<String> list = new List<String>();
            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                installMngr = new InstallationTaskManager(conn);
                conn.OpenConnection();

                list = installMngr.GetTaskTypes();

                conn.CloseConnection();
            }

            return list;
        }

        public List<String> GetStatuses()
        {
            List<String> list = new List<String>();
            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                installMngr = new InstallationTaskManager(conn);
                conn.OpenConnection();

                list = installMngr.GetStatuses();

                conn.CloseConnection();
            }

            return list;
        }

        public List<String> GetComputerNames()
        {
            List<String> list = new List<String>();
            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                installMngr = new InstallationTaskManager(conn);
                conn.OpenConnection();

                list = installMngr.GetComputerNames();

                conn.CloseConnection();
            }

            return list;
        }
        #endregion
    }
}
