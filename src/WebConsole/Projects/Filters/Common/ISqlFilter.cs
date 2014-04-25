using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.Filters.Common
{
    public interface ISqlFilter
    {
        bool Validate();
        string GenerateSQL();
    }
}
