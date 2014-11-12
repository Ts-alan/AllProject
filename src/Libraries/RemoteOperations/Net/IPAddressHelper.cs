using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Text;
using System.Net;
using System.Collections.Generic;

namespace VirusBlokAda.CC.RemoteOperations.Net
{
    public static class IPAddressHelper
    {
        public static UInt32 IPAddressToLongBackwards(IPAddress ip)
        {
            byte[] byteIP = ip.GetAddressBytes();
            UInt32 lip = 0;
            for (int i = 0; i < 4; i++)
            {
                lip += (UInt32)byteIP[i] << 8 * (3 - i);
            }
            return lip;
        }
        public static IPAddress LongBackwardsToIPAddress(UInt32 lip)
        {
            byte[] byteIP = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                byteIP[3 - i] = (byte)((lip << ((3 - i) * 8)) >> 24);
                lip -= (UInt32)byteIP[3 - i] << 8 * i;
            }
            string sip = String.Format("{0}.{1}.{2}.{3}", byteIP[0], byteIP[1], byteIP[2], byteIP[3]);
            return IPAddress.Parse(sip);
        }

        public static List<IPAddress> GetIPAddressList(IPRange range)
        {
            List<IPAddress> list = new List<IPAddress>();
            for (UInt32 i = IPAddressToLongBackwards(range.From);
                i <= IPAddressToLongBackwards(range.To); i++)
            {
                list.Add(LongBackwardsToIPAddress(i));
            }
            return list;
        }

        public static List<IPAddress> GetIPAddressList(List<IPRange> ranges)
        {
            List<IPAddress> result = new List<IPAddress>();
            foreach (IPRange nextRange in ranges)
            {
                List<IPAddress> list = GetIPAddressList(nextRange);
                foreach (IPAddress nextAddress in list)
                {
                    if (!result.Contains(nextAddress))
                    {
                        result.Add(nextAddress);
                    }
                }
            }
            return result;
        }
    }
}