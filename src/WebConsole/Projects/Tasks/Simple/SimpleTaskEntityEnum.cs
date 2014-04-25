using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.Tasks.Simple
{
    public enum SimpleTaskEntityEnum
    {
        None,
        QuerySystemInformation,
        QueryProcessesList,
        QueryComponentsState,
        Vba32MonitorEnable,
        Vba32MonitorDisable,
        Vba32LoaderLaunch,
        Vba32LoaderExit
    }
}
