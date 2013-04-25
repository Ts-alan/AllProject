using System;
using System.Reflection;
using System.Runtime.InteropServices;

using System.Threading;

using System.Diagnostics;
using ARM2_dbcontrol.Generation;

namespace ARM2_dbcontrol.Service.TaskAssignment
{
    /// <summary>
    /// Данный класс реализует выдачу задач через сервис VbaTaskAsigment
    /// </summary>
    public class Vba32ControlCenterWrapper
    {
        string service;
        string m_last_error = "";

        /// <summary>
        /// Требует имя сервиса
        /// </summary>
        /// <param name="service">имя сервиса</param>
        public Vba32ControlCenterWrapper(string service)
        {
            this.service = service;
            //Проверим, доступен ли сервис
            //try
            //{
            //  if(Type.GetTypeFromProgID(service)==null)
            //    throw new Exception("Service not exist");
            //}
            //catch (Exception e)
            //{
            //    m_last_error = e.Message;
            //}
        }

        /// <summary>
        /// Возвращает строку с описанием последней ошибки
        /// </summary>
        /// <returns></returns>
        public string GetLastError()
        {
            return m_last_error;
        }

        /// <summary>
        /// Запускается в отдельном потоке.
        /// Обращается к COM-объекту и вызывает его метод
        /// </summary>
        /// <param name="taskInfo">объект типа TaskInfo</param>
        void ThreadProc(object taskInfo)
        {
            try
            {
                TaskInfo tsk = (TaskInfo)taskInfo;

                tsk.ServType = Type.GetTypeFromProgID(tsk.Service);

                tsk.Serv = Activator.CreateInstance(tsk.ServType);
                tsk.ServType.InvokeMember(tsk.Method, tsk.BindFlags, null, tsk.Serv, tsk.Arguments);
            }
            catch(Exception ex)
            {
               // m_last_error = "Message: " + e.Message;
               // EventLog.WriteEntry("Vba32 Control Center WebInterface",
               //     "Vba32ControlCenterWrapper.ThreadProc(): " + ex.Message, EventLogEntryType.Error);
                //!-OPTM Вот эту ошибку как-то надо донести до веб-консоли
                Debug.Write("Vba32ControlCenterWrapper.ThreadProc(): " + ex.Message);
            }
        }

        #region Service methods

        /// <summary>
        /// PacketSystemInfo
        /// </summary>
        /// <param name="taskID">task id</param>
        /// <param name="ipAddrs">array of ip addresses</param>
        /// <returns></returns>
        public int PacketSystemInfo(Int64[] taskID, string[] ipAddrs) 
        {
            object[] args = new object[2]; //arguments
            int retval = 0;         //return value
          
            args[0] = taskID;
            args[1] = PreServAction.ToDWORDArray(ipAddrs);

            //call COM-server method
            try
            {
                TaskInfo ti = new TaskInfo(service, "PacketSystemInfo", BindingFlags.InvokeMethod, args);
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
        /// PacketCreateProcess
        /// </summary>
        /// <param name="taskID">task id</param>
        /// <param name="ipAddrs">ip address</param>
        /// <param name="cmdLine">cmd line</param>
        /// <returns></returns>
        public int PacketCreateProcess(Int64[] taskID, string[] ipAddrs, string cmdLine)
        {
            object[] args = new object[3]; //arguments
            int retval = 0;         //return value

            args[0] = taskID;
            args[1] = PreServAction.ToDWORDArray(ipAddrs);
            args[2] = cmdLine;

            //call COM-server method
            try
            {
                TaskInfo ti = new TaskInfo(service, "PacketCreateProcess", BindingFlags.InvokeMethod, args);
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
        /// PacketSendFile
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="ipAddrs"></param>
        /// <param name="srcPath"></param>
        /// <param name="dstPath"></param>
        /// <returns></returns>
        public int PacketSendFile(Int64[] taskID, string[] ipAddrs, string srcPath, string dstPath)
        {
            //PreServAction translate = new PreServAction();
            object[] args = new object[4]; //arguments
            int retval = 0;         //return value

            args[0] = taskID;
            args[1] = PreServAction.ToDWORDArray(ipAddrs);
            args[2] = srcPath;
            args[3] = dstPath;

            //call COM-server method
            try
            {
                //retval = vbaServ_type.InvokeMember("PacketSendFile", BindingFlags.InvokeMethod, null, vbaServ, args);
                TaskInfo ti = new TaskInfo(service, "PacketSendFile", BindingFlags.InvokeMethod, args);
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
        /// PacketConfigureSettings
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="ipAddrs"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public int PacketConfigureSettings(Int64[] taskID, string[] ipAddrs, string settings)
        {
           // PreServAction translate = new PreServAction();
            object[] args = new object[3]; //arguments
            int retval = 0;         //return value

            args[0] = taskID;
            args[1] = PreServAction.ToDWORDArray(ipAddrs);
            args[2] = settings;

            //call COM-server method
            try
            {
                //retval = vbaServ_type.InvokeMember("PacketConfigureSettings", BindingFlags.InvokeMethod, null, vbaServ, args);
                TaskInfo ti = new TaskInfo(service, "PacketConfigureSettings", BindingFlags.InvokeMethod, args);
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
        /// PacketComponentState
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="ipAddrs"></param>
        /// <returns></returns>
        public int PacketComponentState(Int64[] taskID, string[] ipAddrs)
        {
            object[] args = new object[2]; //arguments
            int retval = 0;         //return value

            args[0] = taskID;
            args[1] = PreServAction.ToDWORDArray(ipAddrs);

            //call COM-server method
            try
            {
                //retval = vbaServ_type.InvokeMember("PacketComponentState", BindingFlags.InvokeMethod, null, vbaServ, args);
                TaskInfo ti = new TaskInfo(service, "PacketComponentState", BindingFlags.InvokeMethod, args);
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
        /// PacketCustomAction
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="ipAddrs"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public int PacketCustomAction(Int64[] taskID, string[] ipAddrs, string options)
        {
            
            object[] args = new object[3]; //arguments
            int retval = 0;         //return value

            args[0] = taskID;
            args[1] = PreServAction.ToDWORDArray(ipAddrs);
            args[2] = options;

            //call COM-server method
            try
            {
                //retval = vbaServ_type.InvokeMember("PacketCustomAction", BindingFlags.InvokeMethod, null, vbaServ, args);
                TaskInfo ti = new TaskInfo(service, "PacketCustomAction", BindingFlags.InvokeMethod, args);
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
        /// PacketCancelTask
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="ipAddrs"></param>
        /// <returns></returns>
        public int PacketCancelTask(Int64[] taskID, string[] ipAddrs)
        {
            object[] args = new object[2]; //arguments
            int retval = 0;         //return value

            args[0] = taskID;
            args[1] = PreServAction.ToDWORDArray(ipAddrs);

            //call COM-server method
            try
            {
                //retval = vbaServ_type.InvokeMember("PacketCancelTask", BindingFlags.InvokeMethod, null, vbaServ, args);
                TaskInfo ti = new TaskInfo(service, "PacketCancelTask", BindingFlags.InvokeMethod, args);
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
        /// PacketListProcesses
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="ipAddrs"></param>
        /// <returns></returns>
        public int PacketListProcesses(Int64[] taskID, string[] ipAddrs)
        {
            object[] args = new object[2]; //arguments
            int retval = 0;         //return value

            args[0] = taskID;
            args[1] = PreServAction.ToDWORDArray(ipAddrs);

            //call COM-server method
            try
            {
                //retval = vbaServ_type.InvokeMember("PacketListProcesses", BindingFlags.InvokeMethod, null, vbaServ, args);
                TaskInfo ti = new TaskInfo(service, "PacketListProcesses", BindingFlags.InvokeMethod, args);
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


        #endregion

    }


}
