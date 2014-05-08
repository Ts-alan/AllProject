using System;
using System.Collections.Generic;
using System.Text;

namespace Vba32ControlCenterUpdate.DBPatches
{
    internal sealed class DBPatch_3_12_9_5 : DBPatchBase
    {
        public DBPatch_3_12_9_5()
            : base("3.12.9.5")
        {
        }

        protected override String GetUpdateScript()
        {
            return "script v. 3.12.9.5";
        }
    }
}
