using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;


namespace VirusBlokAda.CC.Tasks.Service
{
    /// <summary>
    /// Данный класс реализует выдачу задач через сервис VbaTaskAsigment
    /// </summary>
    public class Vba32ControlCenterWrapper
    {
        String service;
        String m_last_error = "";

        /// <summary>
        /// Требует имя сервиса
        /// </summary>
        /// <param name="service">имя сервиса</param>
        public Vba32ControlCenterWrapper(String service)
        {
            this.service = service;
        }

        /// <summary>
        /// Возвращает строку с описанием последней ошибки
        /// </summary>
        /// <returns></returns>
        public String GetLastError()
        {
            return m_last_error;
        }

        /// <summary>
        /// Запускается в отдельном потоке.
        /// Обращается к COM-объекту и вызывает его метод
        /// </summary>
        /// <param name="taskInfo">объект типа TaskInfo</param>
        void ThreadProc(Object taskInfo)
        {
            try
            {
                TaskInfo tsk = (TaskInfo)taskInfo;

                tsk.ServType = Type.GetTypeFromProgID(tsk.Service);

                tsk.Serv = Activator.CreateInstance(tsk.ServType);
                tsk.ServType.InvokeMember(tsk.Method, tsk.BindFlags, null, tsk.Serv, tsk.Arguments);
            }
            catch (Exception ex)
            {
                Debug.Write("Vba32ControlCenterWrapper.ThreadProc(): " + ex.Message);
            }
        }

        #region Service methods

        /// <summary>
        /// General
        /// </summary>
        /// <param name="taskID">task id</param>
        /// <param name="ipAddrs">array of ip addresses</param>
        /// <returns></returns>
        public Int32 PacketGeneral(Object[] args, String taskType)
        {
            Int32 retval = 0;         //return value
            //call COM-server method
            try
            {
                TaskInfo ti = new TaskInfo(service, taskType, BindingFlags.InvokeMethod, args);
                Thread thr = new Thread(ThreadProc);
                thr.Start(ti);
                //ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadProc), ti);
            }
            catch (Exception e)
            {
                m_last_error = "Message: " + e.Message + " InnerException: " + e.InnerException;
                retval = 1;
            }
            return retval;
        }

        /// <summary>
        /// PacketSystemInfo
        /// </summary>
        /// <param name="taskID">task id</param>
        /// <param name="ipAddrs">array of ip addresses</param>
        /// <returns></returns>
        public Int32 PacketSystemInfo(Int64[] taskID, String[] ipAddrs)
        {
            Object[] args = new Object[2]; //arguments
            args[0] = taskID;
            args[1] = PreServAction.ToDWORDArray(ipAddrs);
            return PacketGeneral(args, "PacketSystemInfo");
        }

        /// <summary>
        /// PacketCreateProcess
        /// </summary>
        /// <param name="taskID">task id</param>
        /// <param name="ipAddrs">ip address</param>
        /// <param name="cmdLine">cmd line</param>
        /// <returns></returns>
        public Int32 PacketCreateProcess(Int64[] taskID, String[] ipAddrs, String cmdLine)
        {
            Object[] args = new Object[3]; //arguments
            args[0] = taskID;
            args[1] = PreServAction.ToDWORDArray(ipAddrs);
            args[2] = cmdLine;
            return PacketGeneral(args, "PacketCreateProcess");
        }

        /// <summary>
        /// PacketSendFile
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="ipAddrs"></param>
        /// <param name="srcPath"></param>
        /// <param name="dstPath"></param>
        /// <returns></returns>
        public Int32 PacketSendFile(Int64[] taskID, String[] ipAddrs, String srcPath, String dstPath)
        {
            Object[] args = new Object[4]; //arguments
            args[0] = taskID;
            args[1] = PreServAction.ToDWORDArray(ipAddrs);
            args[2] = srcPath;
            args[3] = dstPath;
            return PacketGeneral(args, "PacketSendFile");
        }

        /// <summary>
        /// PacketConfigureSettings
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="ipAddrs"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public Int32 PacketConfigureSettings(Int64[] taskID, String[] ipAddrs, String settings)
        {
            Object[] args = new Object[3]; //arguments
            args[0] = taskID;
            args[1] = PreServAction.ToDWORDArray(ipAddrs);
            args[2] = settings;
            return PacketGeneral(args, "PacketConfigureSettings");
        }


        /// <summary>
        /// PacketComponentState
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="ipAddrs"></param>
        /// <returns></returns>
        public Int32 PacketComponentState(Int64[] taskID, String[] ipAddrs)
        {
            Object[] args = new Object[2]; //arguments
            args[0] = taskID;
            args[1] = PreServAction.ToDWORDArray(ipAddrs);
            return PacketGeneral(args, "PacketComponentState");
        }

        /// <summary>
        /// PacketCustomAction
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="ipAddrs"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public Int32 PacketCustomAction(Int64[] taskID, String[] ipAddrs, String options)
        {
            Object[] args = new Object[3]; //arguments
            args[0] = taskID;
            args[1] = PreServAction.ToDWORDArray(ipAddrs);
            args[2] = options;
            return PacketGeneral(args, "PacketCustomAction");
        }

        /// <summary>
        /// PacketCancelTask
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="ipAddrs"></param>
        /// <returns></returns>
        public Int32 PacketCancelTask(Int64[] taskID, String[] ipAddrs)
        {
            Object[] args = new Object[2]; //arguments
            args[0] = taskID;
            args[1] = PreServAction.ToDWORDArray(ipAddrs);
            return PacketGeneral(args, "PacketCancelTask");
        }

        /// <summary>
        /// PacketListProcesses
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="ipAddrs"></param>
        /// <returns></returns>
        public Int32 PacketListProcesses(Int64[] taskID, String[] ipAddrs)
        {
            Object[] args = new Object[2]; //arguments
            args[0] = taskID;
            args[1] = PreServAction.ToDWORDArray(ipAddrs);
            return PacketGeneral(args, "PacketListProcesses");
        }

        #endregion

        //#region Additional methods

        ///// <summary>
        ///// Default configure agent
        ///// </summary>
        ///// <param name="ip"></param>
        ///// <returns></returns>
        //public Int32 DefaultConfigureAgent(String ip)
        //{            
        //    ARM2_dbcontrol.Tasks.ConfigureAgent.TaskConfigureAgent task = new Tasks.ConfigureAgent.TaskConfigureAgent();
            
        //    //Get path current dll (ARM2_dbcontrol) (ex. "file:///C:/Program Files/Vba32 Control Center/Web Console/bin/ARM2_dbcontrol.dll")
        //    //Format path in "C:\Program Files\Vba32 Control Center\Web Console\bin\ARM2_dbcontrol.dll"
        //    String AppPath = System.Reflection.Assembly.GetExecutingAssembly().CodeBase.Replace(@"file:///", "").Replace(@"/", @"\");
        //    //Get full path directory "Vba32 Control Center"
        //    AppPath = System.IO.Directory.GetParent(AppPath).Parent.Parent.FullName + @"\VbaControlAgent.cfg";
        //    task.ConfigFile = AppPath;

        //    return PacketCustomAction(new Int64[] { 0 }, new String[] { ip }, task.BuildTask());
        //}

        //#endregion
    }


}
