using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.DataBase
{
    public class InstallationTaskProvider
    {
        private readonly String connectionString;
        private VlslVConnection connection;

        private InstallationTaskManager installMngr;

        public InstallationTaskProvider(String connectionString)
        {
            this.connectionString = connectionString;
            connection = new VlslVConnection(connectionString);

            InitManagers();
        }

        ~InstallationTaskProvider()
        {
            connection.Dispose();
        }

        private void InitManagers()
        {
            installMngr = new InstallationTaskManager(connection);
        }

        #region Methods

        /// <summary>
        /// Update task in database
        /// </summary>
        public void UpdateTask(InstallationTaskEntity task)
        {
            installMngr.UpdateTask(task);
        }

        /// <summary>
        /// Insert task in database
        /// </summary>
        public Int64 InsertTask(InstallationTaskEntity task)
        {
            return installMngr.InsertTask(task);
        }


        public List<InstallationTaskEntity> List(String where, String order, Int32 page, Int32 size)
        {
            return installMngr.List(where, order, page, size);
        }

        public Int32 Count(String where)
        {
            return installMngr.Count(where);
        }

        public List<String> GetVba32Versions()
        {
            return installMngr.GetVba32Versions();
        }

        public List<String> GetTaskTypes()
        {
            return installMngr.GetTaskTypes();
        }

        public List<String> GetStatuses()
        {
            return installMngr.GetStatuses();
        }

        public List<String> GetComputerNames()
        {
            return installMngr.GetComputerNames();
        }

        #endregion
    }
}
