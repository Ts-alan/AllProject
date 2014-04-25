using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;

namespace VirusBlokAda.CC.Tasks.Service
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
        public static string ConvertToDumpString(byte[] data)
        {
            string str = String.Empty;
            foreach (byte b in data)
            {
                str += b.ToString("X2");
            }
            return str;
        }
    }
}
