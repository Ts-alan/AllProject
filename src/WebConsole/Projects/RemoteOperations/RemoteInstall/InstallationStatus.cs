using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.RemoteOperations.RemoteInstall
{
    /// <summary>
    /// Enumeration representing status of remote installation.
    /// </summary>
    public enum InstallationStatusEnum
    {
        Initializing,
        Connecting,
        Copying,
        Processing,
        Parsing,
        Success,
        Fail,
        Error
    }
}
