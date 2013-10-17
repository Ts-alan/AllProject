using System;
using System.Collections.Generic;
using System.Text;

namespace Vba32CC.TaskAssignment.Tasks
{
    public interface IConfigureTask
    {
        String SaveToXml();
        void LoadFromXml(String xml);
        void LoadFromRegistry(String reg);
    }
}
