using System;
using System.Collections.Generic;
using System.Text;

namespace ARM2_dbcontrol.DataBase
{
    public class ComponentsProvider
    {
        private readonly String connectionString;
        private ComponentsManager cmptMngr;

        public ComponentsProvider()
        { }

        public ComponentsProvider(String connectionString)
        {
            this.connectionString = connectionString;
        }

        #region Methods

        public List<ComponentsEntity> List(String where, String order, Int32 page, Int32 size)
        {
            List<ComponentsEntity> list = new List<ComponentsEntity>();

            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                cmptMngr = new ComponentsManager(conn);
                conn.OpenConnection();

                list = cmptMngr.List(where, order, page, size);

                conn.CloseConnection();
            }

            return list;
        }

        public List<ComponentsEntity> GetComponentsPageByComputerID(Int16 ID)
        {
            List<ComponentsEntity> list = new List<ComponentsEntity>();

            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                cmptMngr = new ComponentsManager(conn);
                conn.OpenConnection();

                list = cmptMngr.GetComponentsPageByComputerID(ID);

                conn.CloseConnection();
            }

            return list;
        }

        public Int32 Count(String where)
        {
            Int32 count = Int32.MinValue;
            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                cmptMngr = new ComponentsManager(conn);
                conn.OpenConnection();

                count = cmptMngr.Count(where);

                conn.CloseConnection();
            }

            return count;
        }

        public List<ComponentsEntity> ListComponentState()
        {
            List<ComponentsEntity> list = new List<ComponentsEntity>();
            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                cmptMngr = new ComponentsManager(conn);
                conn.OpenConnection();

                list = cmptMngr.ListComponentState();

                conn.CloseConnection();
            }

            return list;
        }

        public List<ComponentsEntity> ListComponentType()
        {
            List<ComponentsEntity> list = new List<ComponentsEntity>();
            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                cmptMngr = new ComponentsManager(conn);
                conn.OpenConnection();

                list = cmptMngr.ListComponentType();

                conn.CloseConnection();
            }

            return list;
        }
        #endregion
    }
}
