using System;

namespace ARM2_dbcontrol.Tasks
{
    public interface IConfigureTask
    {
        String SaveToXml();
        void LoadFromXml(String xml);
        void LoadFromRegistry(String reg);
        String GetTask();
    }
}
