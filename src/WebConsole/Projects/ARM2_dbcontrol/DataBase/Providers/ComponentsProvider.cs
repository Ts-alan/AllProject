using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.DataBase
{
    public class ComponentsProvider
    {
        private readonly String connectionString;
        private VlslVConnection connection;

        private ComponentsManager cmptMngr;

        public ComponentsProvider(String connectionString)
        {
            this.connectionString = connectionString;
            connection = new VlslVConnection(connectionString);

            InitManagers();
        }

        private void InitManagers()
        {
            cmptMngr = new ComponentsManager(connection);
        }

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