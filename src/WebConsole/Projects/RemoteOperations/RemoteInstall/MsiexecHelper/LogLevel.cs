using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.RemoteOperations.RemoteInstall.MsiexecHelper
{
    /// <summary>
    /// Logging level of msiexec
    /// </summary>
    enum LogLevel
    {
        None = 0x00,
        Status = 0x01,
        AllExceptVerbose = 0x02,
        Verbose = 0x04,
    }
}

