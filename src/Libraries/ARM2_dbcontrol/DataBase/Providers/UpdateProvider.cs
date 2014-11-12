using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace VirusBlokAda.CC.DataBase
{
    public class UpdateProvider
    {
        public const String ProviderName = "UpdateProvider";

        private UpdateManager updMngr;

        #region Constructors
        public UpdateProvider(String connectionString)
        {
            InitManagers(connectionString);
        }

        private void InitManagers(String connectionString)
        {
            updMngr = new UpdateManager(connectionString);
        }
        public UpdateProvider(String connectionString,DbProviderFactory factory)
        {
            InitManagers(connectionString,factory);
        }

        private void InitManagers(String connectionString,DbProviderFactory factory)
        {
            updMngr = new UpdateManager(connectionString,factory);
        }
        public UpdateProvider(String connectionString,String factoryName)
        {
            InitManagers(connectionString,factoryName);
        }

        private void InitManagers(String connectionString,String factoryName)
        {
            updMngr = new UpdateManager(connectionString,factoryName);
        }
        #endregion
        #region Methods

        /// <summary>
        /// Get last update entity
        /// </summary>
        /// <param name="state">state</param>
        /// <returns></returns>
        public UpdateEntity GetLast(UpdateStateEnum state)
        {
            return updMngr.GetLast(state);
        }

        /// <summary>
        /// Insert new update entity
        /// </summary>
        public void InsertUpdate()
        {
            updMngr.InsertUpdate();
        }

        /// <summary>
        /// Update fields for processing entity
        /// </summary>
        /// <param name="ent"></param>
        public void Update(UpdateEntity ent)
        {
            updMngr.Update(ent);
        }

        #endregion
    }
}
