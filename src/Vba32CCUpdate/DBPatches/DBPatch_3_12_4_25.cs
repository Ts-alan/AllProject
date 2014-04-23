using System;
using System.Collections.Generic;
using System.Text;

namespace Vba32ControlCenterUpdate.DBPatches
{
    internal sealed class DBPatch_3_12_4_25: DBPatchBase
    {
        public DBPatch_3_12_4_25()
            : base("3.12.4.25", new DBPatch_3_12_4_24())
        {
        }

        protected override String GetUpdateScript()
        {
            base.GetUpdateScript();
            return "script v. 3.12.4.25";
        }
    }
}
