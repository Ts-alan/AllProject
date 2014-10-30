using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace VirusBlokAda.CC.DataBase
{
    public class ComponentsProvider
    {
        public const String ProviderName = "ComponentsProvider";

        private ComponentsManager cmptMngr;

        #region Constructors
        public ComponentsProvider(String connectionString)
        {
            InitManagers(connectionString);
        }

        private void InitManagers(String connectionString)
        {
            cmptMngr = new ComponentsManager(connectionString);
        }

        public ComponentsProvider(String connectionString, DbProviderFactory factory)
        {
            InitManagers(connectionString,factory);
        }

        private void InitManagers(String connectionString, DbProviderFactory factory)
        {
            cmptMngr = new ComponentsManager(connectionString,factory);
        }

        public ComponentsProvider(String connectionString,String factoryName)
        {
            InitManagers(connectionString,factoryName);
        }

        private void InitManagers(String connectionString,String factoryName)
        {
            cmptMngr = new ComponentsManager(connectionString,factoryName);
        }
        #endregion
        #region Methods

        public List<ComponentsEntity> List(String where, String order, Int32 page, Int32 size)
        {
            return cmptMngr.List(where, order, page, size);
        }

        public List<ComponentsEntity> GetComponentsPageByComputerID(Int16 ID)
        {
            return cmptMngr.GetComponentsPageByComputerID(ID);
        }

        public Int32 Count(String where)
        {
            return cmptMngr.Count(where);
        }

        public List<ComponentsEntity> ListComponentState()
        {
            return cmptMngr.ListComponentState();
        }

        public List<ComponentsEntity> ListComponentType()
        {
            return cmptMngr.ListComponentType();
        }

        public String GetCurrentSettings(Int16 compID, String componentName)
        {
            return cmptMngr.GetCurrentSettings(compID, componentName);
        }

        #endregion
    }
}