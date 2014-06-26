using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.DataBase
{
    public class UpdateProvider
    {
        public const String ProviderName = "UpdateProvider";

        private UpdateManager updMngr;

        public UpdateProvider(String connectionString)
        {
            InitManagers(connectionString);
        }

        private void InitManagers(String connectionString)
        {
            updMngr = new UpdateManager(connectionString);
        }

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
