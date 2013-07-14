using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;
using Vba32.ControlCenter.PeriodicalMaintenanceService.DataBase;
using Vba32.ControlCenter.PeriodicalMaintenanceService.TaskAssignment.DataBase;
using Vba32.ControlCenter.PeriodicalMaintenanceService.TaskAssignment.Entities;

namespace Vba32.ControlCenter.PeriodicalMaintenanceService.TaskAssignment
{
    /// <summary>
    /// ����������� �������� ��� ���������� ������ ����� �������
    /// ��������������� ip-����� �� DWORD<->string
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
    }
}
