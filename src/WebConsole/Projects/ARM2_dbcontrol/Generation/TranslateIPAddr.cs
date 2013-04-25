using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices; 

namespace ARM2_dbcontrol.Generation
{
    /// <summary>
    /// Преобразовывает ip-адрес из DWORD<->string
    /// </summary>
    public class TranslateIPAddr
    {

        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern UInt32 inet_addr(string str_ip);


        public UInt32 TranslateToDWORD(string ip)
        {
            return inet_addr(ip);
        }

        
        /*public string TranslateToString(UInt32 ip)
        {

            string strIP = String.Empty;
           
            //strIP += Convert.ToString(ip >> 24)+'.';
            //strIP += Convert.ToString((ip >> 16) & 255) + '.';
            //strIP += Convert.ToString((ip >> 8) & 255) + '.';
            //strIP += Convert.ToString((ip)&255);

            return strIP;
        }*/

        public UInt32[] TranslateToDWORDArray(string[] ipAddrs)
        {
            UInt32[] dwIpAddr = new UInt32[ipAddrs.Length];

            //translate ip
            for (int i = 0; i < ipAddrs.Length; i++)
                dwIpAddr[i] = this.TranslateToDWORD(ipAddrs[i]);

            return dwIpAddr;

        }

    }
}
