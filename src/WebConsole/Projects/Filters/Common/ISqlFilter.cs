using System;
using System.Collections.Generic;
using System.Text;

namespace Filters.Common
{
    public interface ISqlFilter
    {
        bool Validate();
        string GenerateSQL();
    }
}
