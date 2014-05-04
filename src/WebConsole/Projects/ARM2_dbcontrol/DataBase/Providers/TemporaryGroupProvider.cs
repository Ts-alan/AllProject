using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.DataBase
{
    public class TemporaryGroupProvider
    {
        private TemporaryGroupManager mngr;

        public TemporaryGroupProvider(String connectionString)
        {
            InitManagers(connectionString);
        }

        private void InitManagers(String connectionString)
        {
            mngr = new TemporaryGroupManager(connectionString);
        }

        #region Methods

        public List<String> GetComputerNameList(String type, String where)
        {
                return mngr.GetComputerNameList(type, where);
        }

        #endregion
    }
}
