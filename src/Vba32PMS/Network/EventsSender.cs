using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using Vba32.ControlCenter.PeriodicalMaintenanceService.Xml;

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
        private static Socket ConnectSocket(String server, Int32 port)
        {
            LoggerPMS.log.Debug("EventsSender.ConnectSocket():: оннектимс€ к удаленному серверу");
            Socket s = null;
            try
            {
                //ѕолучаем IP из строки
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
        public static String SocketSendReceive(String server, Int32 port, String message)
        {
            LoggerPMS.log.Debug("EventsSender.SocketSendReceive()::ѕосылаем пакет");
            //≈сли здесь помен€ем кодировку, то необходимо внести соответствующие изменени€
            //в EventsToControlAgentXml.Convert, т.к. иначе размер может здесь быть больше допустимого
            Byte[] mes = Encoding.UTF8.GetBytes(message);
            Byte[] bytesSent = new Byte[mes.Length+2];
            LoggerPMS.log.Debug("–азмер отсылаемого сообщени€: "+mes.Length);
            LoggerPMS.log.Debug("–азмер отсылаемого пакета: "+bytesSent.Length);
            bytesSent[1] = Convert.ToByte(bytesSent.Length >> 8);
            bytesSent[0] = Convert.ToByte(bytesSent.Length & 255);
            for (Int32 i = 0; i < mes.Length; i++)
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

            String retVal = "OK";
            if (s == null)
                return "Connection failed";

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
