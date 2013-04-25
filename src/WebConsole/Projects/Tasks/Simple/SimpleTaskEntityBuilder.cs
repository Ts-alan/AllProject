using System;
using System.Collections.Generic;
using System.Text;
using Tasks.Entities;

namespace Tasks.Simple
{
    public static class SimpleTaskEntityBuilder
    {
        private static readonly string sVba32LoaderExit = "\"%VBA32%Vba32ldr.exe\" /lu /user";
        private static readonly string sVba32LoaderLaunch = "\"%VBA32%Vba32ldr.exe\" /user";
        private static readonly string sVba32MonitorDisable = "\"%VBA32%Vba32ldr.exe\" /mf /user";
        private static readonly string sVba32MonitorEnable = "\"%VBA32%Vba32ldr.exe\" / mn / user";
        public static TaskEntity Create(SimpleTaskEntityEnum type)
        {           
            switch (type)
            {
                case SimpleTaskEntityEnum.QueryComponentsState :
                    return new QueryComponentsStateTaskEntity();
                case SimpleTaskEntityEnum.QueryProcessesList:
                    return new QueryProcessesListTaskEntity();
                case SimpleTaskEntityEnum.QuerySystemInformation:
                    return new QuerySystemInformationTaskEntity();                 
                case SimpleTaskEntityEnum.Vba32LoaderExit:
                    return new CreateProcessTaskEntity(sVba32LoaderExit);
                case SimpleTaskEntityEnum.Vba32LoaderLaunch:
                    return new CreateProcessTaskEntity(sVba32LoaderLaunch);
                case SimpleTaskEntityEnum.Vba32MonitorDisable:
                    return new CreateProcessTaskEntity(sVba32MonitorDisable);
                case SimpleTaskEntityEnum.Vba32MonitorEnable:
                    return new CreateProcessTaskEntity(sVba32MonitorEnable);
                default:
                    throw new ArgumentException(
                        "SimpleTaskEntityEnum passed to SimpleTaskEntityBuilder can't be of None type", "type");
            }
        }
    }
}
