using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace Vba32.ControlCenter.PeriodicalMaintenanceService.Network
{
    public static class EventsSender
    {
         /// <summary>
        /// Open socket
        /// </summary>
        /// <param name="server">Server</param>
        /// <param name="port">Port</param>
        /// <returns></returns>
        private static Socket ConnectSocket(string server, int port)
        {
            Debug.WriteLine("EventsSender.ConnectSocket()::����������� � ���������� �������");
            Socket s = null;
            try
            {
                //�������� IP �� ������
                IPAddress address = IPAddress.Parse(server);

                IPEndPoint ipe = new IPEndPoint(address, port);

                Socket tempSocket =
                    new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                tempSocket.Connect(ipe);

                if (tempSocket.Connected)
                {
                    s = tempSocket;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return s;
        }

        /// <summary>
        ///  This method requests the home page content for the specified server.
        /// </summary>
        /// <param name="server">Server</param>
        /// <param name="port">Port</param>
        /// <param name="message">Message to send</param>
        /// <returns></returns>
        public static string SocketSendReceive(string server, int port, string message)
        {
            Debug.WriteLine("EventsSender.SocketSendReceive()::�������� �����");
            //���� ����� �������� ���������, �� ���������� ������ ��������������� ���������
            //� EventsToControlAgentXml.Convert, �.�. ����� ������ ����� ����� ���� ������ �����������
            Byte[] mes = Encoding.UTF8.GetBytes(message);
            Byte[] bytesSent = new byte[mes.Length+2];
            Debug.WriteLine("������ ����������� ���������: "+mes.Length);
            Debug.WriteLine("������ ����������� ������: "+bytesSent.Length);
            bytesSent[1] = Convert.ToByte(bytesSent.Length >> 8);
            bytesSent[0] = Convert.ToByte(bytesSent.Length & 255);
            for (int i = 0; i < mes.Length; i++)
                bytesSent[i + 2] = mes[i];

            // Create a socket connection with the specified server and port.
            Socket s = null;
            try
            {
                s = ConnectSocket(server, port);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            string retVal = "OK";
            if (s == null)
                return ("Connection failed");

            // Send request to the server.
            try
            {
                s.Send(bytesSent, bytesSent.Length, 0);
            }
            catch (Exception ex)
            {
                retVal = ex.Message;
            }
            finally
            {
                s.Close();
            }
            return retVal;
        }

    }
}
