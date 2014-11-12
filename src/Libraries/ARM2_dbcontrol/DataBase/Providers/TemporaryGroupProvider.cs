using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace VirusBlokAda.CC.DataBase
{
    public class TemporaryGroupProvider
    {
        public const String ProviderName = "TemporaryGroupProvider";

        private TemporaryGroupManager mngr;

        #region Constructors
        public TemporaryGroupProvider(String connectionString)
        {
            InitManagers(connectionString);
        }

        private void InitManagers(String connectionString)
        {
            mngr = new TemporaryGroupManager(connectionString);
        }
        public TemporaryGroupProvider(String connectionString,DbProviderFactory factory)
        {
            InitManagers(connectionString,factory);
        }

        private void InitManagers(String connectionString,DbProviderFactory factory)
        {
            mngr = new TemporaryGroupManager(connectionString,factory);
        }
        public TemporaryGroupProvider(String connectionString,String factoryName)
        {
            InitManagers(connectionString,factoryName);
        }

        private void InitManagers(String connectionString,String factoryName)
        {
            mngr = new TemporaryGroupManager(connectionString,factoryName);
        }
        #endregion
        #region Methods

        public List<String> GetComputerNameList(String type, String where)
        {
                return mngr.GetComputerNameList(type, where);
        }

        #endregion
    }
}
