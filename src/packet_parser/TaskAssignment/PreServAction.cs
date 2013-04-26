using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

using System.Runtime.InteropServices;
using Vba32CC.DataBase;

namespace Vba32CC.Service.TaskAssignment
{
    /// <summary>
    /// Необходимые действия для подготовки выдачи задач сервису
    /// Преобразовывает ip-адрес из DWORD<->string
    /// </summary>
    public static class PreServAction
    {

        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern UInt32 inet_addr(String str_ip);


        public static UInt32 ToDWORD(String ip)
        {
            return inet_addr(ip);
        }

        public static UInt32[] ToDWORDArray(String[] ipAddrs)
        {
            UInt32[] dwIpAddr = new UInt32[ipAddrs.Length];
            //translate ip
            for (Int32 i = 0; i < ipAddrs.Length; i++)
                dwIpAddr[i] = PreServAction.ToDWORD(ipAddrs[i]);

            return dwIpAddr;
        }

        /// <summary>
        /// Returns ip array in string
        /// </summary>
        /// <param name="list">id's</param>
        /// <returns></returns>
        public static String[] GetIPArray(List<Int16> list, String connStr)
        {
            String[] strIP = new String[list.Count];
            for (Int32 i = 0; i < list.Count; i++)
                strIP[i] = GetComputerById(list[i],connStr).IPAddress;

            return strIP;
        }

        public static String[] GetIPArrayByTaskID(Int64[] taskId, String connStr)
        {
            String[] strIP = new String[taskId.Length];
            using (VlslVConnection conn = new VlslVConnection(connStr))
            {
                TaskManager db = new TaskManager(conn);
                conn.OpenConnection();
                conn.CheckConnectionState(true);
                for (Int32 i = 0; i < taskId.Length; i++)
                {
                    strIP[i] = db.GetIPAddressByTaskID(taskId[i]);
                }
                conn.CloseConnection();
            }

            return strIP;
        }

        /// <summary>
        /// Returns computernam array
        /// </summary>
        /// <param name="list">id's</param>
        /// <returns></returns>
        public static String[] GetComputerNameArray(List<Int16> list, String connStr)
        {
            String[] strComp = new String[list.Count];
            for (Int32 i = 0; i < list.Count; i++)
            {
                strComp[i] = GetComputerById(list[i],connStr).ComputerName;
            }

            return strComp;
        }

        /// <summary>
        /// Call create task stored procedure and return taskID
        /// </summary>
        /// <param name="computerName"></param>
        /// <param name="taskName"></param>
        /// <param name="taskParams"></param>
        /// <returns></returns>
        public static Int64 CreateTask(String computerName, String taskName, String taskParams, String taskUser, String connStr)
        {
            Int64 ret = 0;
            using (VlslVConnection conn = new VlslVConnection(connStr))
            {
                TaskManager db = new TaskManager(conn);
                conn.OpenConnection();

                ret = Convert.ToInt64(db.CreateTask(computerName, taskName, taskParams, taskUser));

                conn.CloseConnection();
            }

            return ret;

        }

        /// <summary>
        /// Return computer name by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ComputersEntity GetComputerById(Int16 id, String connStr)
        {
            ComputersEntity comp = new ComputersEntity();
            using (VlslVConnection conn = new VlslVConnection(connStr))
            {
                TaskManager tmng = new TaskManager(conn);
                conn.OpenConnection();

                comp = tmng.GetComputer(id);

                conn.CloseConnection();
            }
            return comp;
        }

        /// <summary>
        /// Returns vbaversion array
        /// </summary>
        /// <param name="list">id's</param>
        /// <returns></returns>
        public static String[] GetVbaVersionArray(List<Int16> list, String connStr)
        {
            String[] vbaVersion = new String[list.Count];
            for (Int32 i = 0; i < list.Count; i++)
            {
                vbaVersion[i] = GetComputerById(list[i], connStr).Vba32Version;
            }
            return vbaVersion;
        }
    }
}
