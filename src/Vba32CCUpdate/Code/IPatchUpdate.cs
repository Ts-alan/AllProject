using System;
using System.Collections.Generic;
using System.Text;

namespace Vba32ControlCenterUpdate
{
    internal interface IPatchUpdate
    {
        String Version { get; }
        Boolean Update(String currentVersion, String connectionString, out String errorVersion);
        Boolean Rollback(String oldVersion, String connectionString, out String errorVersion);
    }
}
