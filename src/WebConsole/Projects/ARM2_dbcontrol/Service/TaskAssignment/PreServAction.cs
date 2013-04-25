using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

using System.Runtime.InteropServices;
using ARM2_dbcontrol.DataBase;

namespace ARM2_dbcontrol.Service.TaskAssignment
{
    /// <summary>
    /// Необходимые действия для подготовки выдачи задач сервису
    /// Преобразовывает ip-адрес из DWORD<->string
    /// </summary>
    public static class PreServAction
    {

        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern UInt32 inet_addr(string str_ip);


        public static UInt32 ToDWORD(string ip)
        {
            return inet_addr(ip);
        }

        public static UInt32[] ToDWORDArray(string[] ipAddrs)
        {
            UInt32[] dwIpAddr = new UInt32[ipAddrs.Length];

            //translate ip
            for (int i = 0; i < ipAddrs.Length; i++)
                dwIpAddr[i] = PreServAction.ToDWORD(ipAddrs[i]);

            return dwIpAddr;

        }

        /// <summary>
        /// Returns ip array in string
        /// </summary>
        /// <param name="list">id's</param>
        /// <returns></returns>
        public static string[] GetIPArray(List<Int16> list, string connStr)
        {
            string[] strIP = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
                strIP[i] = GetComputerById(list[i],connStr).IPAddress;

            return strIP;
        }



        public static string[] GetIPArrayByTaskID(Int64[] taskId, string connStr)
        {
            string[] strIP = new string[taskId.Length];
            using (VlslVConnection conn = new VlslVConnection(connStr))
            {
                TaskManager db = new TaskManager(conn);
                conn.OpenConnection();
                conn.CheckConnectionState(true);
                for (int i = 0; i < taskId.Length; i++)
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
        public static string[] GetComputerNameArray(List<Int16> list, string connStr)
        {
            string[] strComp = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                strComp[i] = GetComputerById(list[i],connStr).ComputerName;
            }

            return strComp;
        }





        /// <summary>
        /// Return computer name by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ComputersEntity GetComputerById(Int16 id, string connStr)
        {
            ComputersEntity comp = new ComputersEntity();
            using (VlslVConnection conn = new VlslVConnection(connStr))
            {
                ComputersManager cmng = new ComputersManager(conn);
                conn.OpenConnection();

                comp = cmng.Get(id);

                conn.CloseConnection();
            }
            return comp;
        }

        /// <summary>
        /// Call create task stored procedure and return taskID
        /// </summary>
        /// <param name="computerName"></param>
        /// <param name="taskName"></param>
        /// <param name="taskParams"></param>
        /// <returns></returns>
        public static Int64 CreateTask(string computerName, string taskName, string taskParams, string taskUser, string connStr)
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
        /// Returns vbaversion array
        /// </summary>
        /// <param name="list">id's</param>
        /// <returns></returns>
        public static string[] GetVbaVersionArray(List<Int16> list, string connStr)
        {
            string[] vbaVersion = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                vbaVersion[i] = GetComputerById(list[i], connStr).Vba32Version;
            }
            return vbaVersion;
        }
    }
}
