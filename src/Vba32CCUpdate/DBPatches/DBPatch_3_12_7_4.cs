using System;
using System.Collections.Generic;
using System.Text;

namespace Vba32ControlCenterUpdate.DBPatches
{
    internal sealed class DBPatch_3_12_7_4: DBPatchBase
    {
        public DBPatch_3_12_7_4()
            : base("3.12.7.4", new DBPatch_3_12_4_25())
        {
        }

        protected override String GetUpdateScript()
        {
            base.GetUpdateScript();
            return "script v. 3.12.7.4";
        }
    }
}
