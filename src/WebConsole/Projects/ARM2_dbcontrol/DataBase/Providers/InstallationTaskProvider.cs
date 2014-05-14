using System;
using System.Collections.Generic;

namespace VirusBlokAda.CC.DataBase
{
    public class InstallationTaskProvider
    {
        public const String ProviderName = "InstallationTaskProvider";
        
        private InstallationTaskManager installMngr;

        public InstallationTaskProvider(String connectionString)
        {
            InitManagers(connectionString);
        }

        private void InitManagers(String connectionString)
        {
            installMngr = new InstallationTaskManager(connectionString);            
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


        public List<InstallationTaskEntity> List(String where, String order, Int16 page, Int16 size)
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
