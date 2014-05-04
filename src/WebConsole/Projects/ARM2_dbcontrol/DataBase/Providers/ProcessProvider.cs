using System;
using System.Collections.Generic;

namespace VirusBlokAda.CC.DataBase
{
    public class ProcessProvider
    {
        private ProcessesManager prcMngr;

        public ProcessProvider(String connectionString)
        {
            InitManagers(connectionString);
        }

        private void InitManagers(String connectionString)
        {
            prcMngr = new ProcessesManager(connectionString);
        }

        #region Methods

        /// <summary>
        /// Get process page
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <param name="order">Sort query</param>
        /// <param name="page">Page index</param>
        /// <param name="size">Page size</param>
        /// <returns></returns>
        public List<ProcessesEntity> List(String where, String order, Int32 page, Int32 size)
        {
            return prcMngr.List(where, order, page, size);
        }

        /// <summary>
        /// Get count of records with filter
        /// </summary>
        /// <param name="where">where clause</param>
        /// <returns></returns>
        public Int32 Count(String where)
        {
            return prcMngr.Count(where);
        }

        #endregion
    }
}
