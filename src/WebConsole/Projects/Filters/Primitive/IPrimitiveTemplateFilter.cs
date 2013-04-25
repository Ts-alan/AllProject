using System;
using System.Collections.Generic;
using System.Text;
using Filters.Common;

namespace Filters.Primitive
{
    public interface IPrimitiveTemplateFilter: IClearableFilter
    {
        PrimitiveFilterState SaveState();
        void LoadState(PrimitiveFilterState savedState);
        string GetID();
    }
}
