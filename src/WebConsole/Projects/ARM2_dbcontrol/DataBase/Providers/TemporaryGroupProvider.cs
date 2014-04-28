using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.DataBase
{
    public class TemporaryGroupProvider
    {
        private readonly String connectionString;
        private VlslVConnection connection;

        private TemporaryGroupManager mngr;

        public TemporaryGroupProvider(String connectionString)
        {
            this.connectionString = connectionString;
            connection = new VlslVConnection(connectionString);

            InitManagers();
        }

        private void InitManagers()
        {
            mngr = new TemporaryGroupManager(connection);
        }

        #region Methods

        public List<String> GetComputerNameList(String type, String where)
        {
                return mngr.GetComputerNameList(type, where);
        }

        #endregion
    }
}
