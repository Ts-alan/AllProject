using System;
using System.Collections.Generic;
using System.Text;

using System.Diagnostics;
using System.Globalization;

using Vba32.ControlCenter.PeriodicalMaintenanceService.DataBase;

namespace Vba32.ControlCenter.PeriodicalMaintenanceService.Xml
{
    /// <summary>
    /// ������ ����� ������������ ������� �� ���� ������ � ������ ������(+������� ��������� ����)
    /// </summary>
    public class EventsToControlAgentXml
    {
        private const int maxSizePacket = 65530;            //������������ ������ ������
        private string childArmComputerName = String.Empty; //��� ����� � �������� �����
        List<EventsEntity> list = null;                     //�������, ������� �������� � ������� ���� ������

        private string rootTag = "Events";                  //��� ��������� ��������

        public EventsToControlAgentXml(List<EventsEntity> list, string childArmComputerName)
        {
            this.list = list;
            this.childArmComputerName = childArmComputerName;
        }

        public string Convert()
        {
            Debug.WriteLine("EventsToControlAgentXml.Convert::������������ � ������ ������ ������");
            XmlBuilder xml = new XmlBuilder(rootTag);   //��������� �������
            for (int i = 0; i < list.Count; i++)
            {
                XmlBuilder childXml = new XmlBuilder();
                childXml.AddNode("Computer", childArmComputerName);
                childXml.AddNode("EventName", list[i].Event);

                IFormatProvider culture = new CultureInfo("ru-RU");
                childXml.AddNode("EventTime", list[i].EventTime.ToString(culture));
                
                childXml.AddNode("Component", list[i].Component);
                childXml.AddNode("Object", list[i].Computer + ':' + list[i].Object);
                childXml.AddNode("Comment", list[i].Comment);
                xml.AddNode("Event", childXml.Result);
            }
            xml.Generate();

            //if (xml.Result.Length > maxSizePacket)
            if(Encoding.UTF8.GetBytes(xml.Result).Length > maxSizePacket)
            {
                return String.Empty;
            }

            return xml.Result;
        }
    }
}
