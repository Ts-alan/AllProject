using System;
using System.Collections.Generic;
using System.Text;
using Filters.Common;

namespace Filters.Composite
{
    public interface ICompositeFilter: IClearableFilter, ISqlFilter
    {
    }
}
