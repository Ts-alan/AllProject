using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using VirusBlokAda.CC.Common.Xml;
using VirusBlokAda.CC.DataBase;

namespace Vba32.ControlCenter.PeriodicalMaintenanceService.Xml
{
    /// <summary>
    /// Данный класс конвертирует события из базы данных в формат агента(+события дочернего ЦУ)
    /// </summary>
    public class EventsToControlAgentXml
    {
        private const Int32 maxSizePacket = 65530;            //максимальный размер пакета
        private String childArmComputerName = String.Empty; //имя компа с дочерним армом
        List<EventsEntity> list = null;                     //события, которые отсылать в формате базы данных

        private String rootTag = "Events";                  //имя корневого элемента

        public EventsToControlAgentXml(List<EventsEntity> list, String childArmComputerName)
        {
            this.list = list;
            this.childArmComputerName = childArmComputerName;
        }

        public String Convert()
        {
            LoggerPMS.log.Debug("EventsToControlAgentXml.Convert::Конвертируем в формат пакета агента");
            XmlBuilder xml = new XmlBuilder(rootTag);   //Коллекция событий
            for (Int32 i = 0; i < list.Count; i++)
            {
                XmlBuilder childXml = new XmlBuilder();
                childXml.AddNode("Computer", childArmComputerName);
                childXml.AddNode("EventName", list[i].EventName);

                IFormatProvider culture = new CultureInfo("ru-RU");
                childXml.AddNode("EventTime", list[i].EventTime.ToString(culture));
                
                childXml.AddNode("Component", list[i].ComponentName);
                childXml.AddNode("Object", list[i].ComputerName + ':' + list[i].Object);
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
