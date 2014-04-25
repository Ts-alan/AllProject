using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.DataBase
{
    public class TemporaryGroupProvider
    {
        private readonly String connectionString;
        private TemporaryGroupManager mngr;

        public TemporaryGroupProvider()
        { }

        public TemporaryGroupProvider(String connectionString)
        {
            this.connectionString = connectionString;
        }

        #region Methods

        public List<String> GetComputerNameList(String type, String where)
        {
            List<String> list = null;
            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                mngr = new TemporaryGroupManager(conn);
                conn.OpenConnection();

                list = mngr.GetComputerNameList(type, where);

                conn.CloseConnection();
            }

            return list;
        }
        #endregion
    }
}
