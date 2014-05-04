using System;
using System.Collections.Generic;

namespace VirusBlokAda.CC.DataBase
{
    public class ComputerProvider
    {
        private ComputersManager compMngr;

        public ComputerProvider(String connectionString)
        {
            InitManagers(connectionString);
        }

        private void InitManagers(String connectionString)
        {
            compMngr = new ComputersManager(connectionString);
        }

        #region Methods

        /// <summary>
        /// Delete entity from database with this id
        /// </summary>
        /// <param name="computersID">ID</param>
        /// <returns>id</returns>
        public void Delete(Int16 computersID)
        {
            compMngr.Delete(computersID);
        }

        /// <summary>
        /// Get page with sorting and filter
        /// </summary>
        /// <param name="where">where clause</param>
        /// <param name="order">order clause</param>
        /// <param name="page">page number</param>
        /// <param name="size">records per page</param>
        /// <returns></returns>
        public List<ComputersEntity> List(String where, String order, Int16 page, Int16 size)
        {
            return compMngr.List(where, order, page, size);
        }

        /// <summary>
        /// Get count of records with filter
        /// </summary>
        /// <param name="where">where clause</param>
        /// <returns></returns>
        public Int32 Count(String where)
        {
            return compMngr.Count(where);
        }

        /// <summary>
        /// Get computer list
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <param name="orderBy">Sort query</param>
        /// <returns></returns>
        public List<ComputersEntity> GetComputers(String where, String orderBy)
        {
            return compMngr.GetComputers(where, orderBy);
        }

        /// <summary>
        /// Get computer extended list without components
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <param name="orderBy">Sort query</param>
        /// <returns></returns>
        public List<ComputersEntityEx> GetComputersEx(String where, String orderBy)
        {
            return compMngr.GetComputersEx(where, orderBy);
        }

        /// <summary>
        /// Update computer's description
        /// </summary>
        /// <param name="id">Computer ID</param>
        /// <param name="description">New description</param>
        public void UpdateDescription(Int16 id, String description)
        {
            compMngr.UpdateDescription(id, description);
        }

        /// <summary>
        /// Get computer ID by computer name
        /// </summary>
        /// <param name="computerName">Computer name</param>
        /// <returns>Computer ID. If computer wasn't existed return -1</returns>
        public Int16 GetComputerID(String computerName)
        {
            return compMngr.GetComputerID(computerName);
        }

        /// <summary>
        /// Get computer by ID
        /// </summary>
        /// <param name="id">Computer ID</param>
        /// <returns>Computer entity</returns>
        public ComputersEntity GetComputer(Int16 id)
        {
            return compMngr.GetComputer(id);
        }

        /// <summary>
        /// Get list of IPAddresses
        /// </summary>
        /// <returns>List of IPAddresses</returns>
        public List<String> GetRegisteredCompList()
        {
            return compMngr.GetRegisteredCompList();
        }

        /// <summary>
        /// Get extension version of computer entity by ID without components
        /// </summary>
        /// <param name="computersID">Computer ID</param>
        /// <returns>Extension version of computer entity</returns>
        public ComputersEntityEx GetComputerEx(Int16 computersID)
        {
            return compMngr.GetComputerEx(computersID);
        }

        /// <summary>
        /// Get Names & IPAddresses for filtered computers
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <returns>Lists of Names & IPAddresses</returns>
        public SelectedComputersForTask GetSelectionComputerForTask(String where)
        {
            return compMngr.GetSelectionComputerForTask(where);
        }

        /// <summary>
        /// Clear old computers
        /// </summary>
        /// <param name="dt">Date</param>
        public void ClearOldComputers(DateTime dt)
        {
            compMngr.ClearOldComputers(dt);
        }

        #endregion
    }
}
