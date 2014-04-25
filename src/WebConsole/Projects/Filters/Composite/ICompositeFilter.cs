using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.CC.Filters.Common;

namespace VirusBlokAda.CC.Filters.Composite
{
    public interface ICompositeFilter: IClearableFilter, ISqlFilter
    {
    }
}
