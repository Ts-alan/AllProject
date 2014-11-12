using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.Tasks.Common
{
    public interface ITask
    {
        bool IsActive();
        string GetXmlString();
        string GetTaskXml();
        string GetTaskName();
    }
}
