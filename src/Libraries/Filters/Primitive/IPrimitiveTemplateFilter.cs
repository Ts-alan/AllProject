using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.CC.Filters.Common;

namespace VirusBlokAda.CC.Filters.Primitive
{
    public interface IPrimitiveTemplateFilter: IClearableFilter
    {
        PrimitiveFilterState SaveState();
        void LoadState(PrimitiveFilterState savedState);
        string GetID();
    }
}
