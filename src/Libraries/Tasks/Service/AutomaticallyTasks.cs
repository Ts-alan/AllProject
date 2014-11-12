using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.CC.DataBase;
using ARM2_dbcontrol.Tasks;


namespace VirusBlokAda.CC.Tasks.Service
{
    public static class AutomaticallyTasks
    {
        public static String ConnectionString = String.Empty;
        public const String UserName = "Automatically";

        #region Events
        private const String VirusFound = "vba32.virus.found";
        private const String MonitorDeactivated = "vba32.monitor.deactivated";
        #endregion

        public static void GiveTask(EventsEntity entity)
        {
            //Log.Write("GiveTask begin. " + entity.Event);
            try
            {
                Vba32ControlCenterWrapper control = new Vba32ControlCenterWrapper("VbaTaskAssignment.Service");
                AutomaticallyTaskEntity task = null;
                #region Get Task
                TaskProvider db = new TaskProvider(ConnectionString);
                task = db.GetAutomaticallyTask(entity.EventName);
                #endregion

                if (task != null && task.IsAllowed)
                {
                    List<String> compName = new List<String>();
                    List<String> ipAddress = new List<String>();
                    #region Get computer names & IP-addresses
                    db.GetComputersInfo(ref compName, ref ipAddress);
                    #endregion

                    List<Int64> taskId = new List<Int64>();
                    List<String> ipAddr = new List<String>();

                    switch (entity.EventName)
                    {
                        case VirusFound:
                            TaskRunScanner taskScanner = new TaskRunScanner();
                            taskScanner.LoadFromXml(task.TaskParams);

                            //change properties

                            for (Int32 i = 0; i < compName.Count; i++)
                            {
                                //Check running task
                                if (!IsRunning(compName[i], task.TaskID))
                                {
                                    ipAddr.Add(ipAddress[i]);
                                    taskId.Add(PreServAction.CreateTask(compName[i], TaskRunScanner.TaskType, taskScanner.SaveToXml(), UserName, ConnectionString));
                                }
                            }
                            control.PacketCreateProcess(taskId.ToArray(), ipAddr.ToArray(), taskScanner.GetTask());

                            break;
                    }
                }
            }
            catch (Exception)
            {
                //Log.Write(String.Format("Error: {0}", ex.Message));
            }
        }

        private static Boolean IsRunning(String compName, Int16 taskID)
        {
            TaskProvider db = new TaskProvider(ConnectionString);
            return db.IsRunningTask(compName, taskID);
        }
    }
}
