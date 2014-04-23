using System;
using System.Collections.Generic;
using System.Text;

namespace Vba32ControlCenterUpdate.DBPatches
{
    internal sealed class DBPatch_3_12_4_24: DBPatchBase
    {
        public DBPatch_3_12_4_24()
            : base("3.12.4.24", new DBPatch_3_12_3_13())
        {
        }

        protected override String GetUpdateScript()
        {
            base.GetUpdateScript();
            return "script v. 3.12.4.24";
        }
    }
}
