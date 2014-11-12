using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections.Specialized;
using ARM2_dbcontrol.Tasks;
using VirusBlokAda.CC.DataBase;

namespace Vba32CC
{
    public partial class PacketParser
    {
        #region Element Parsing Functions
        private Boolean ParseXmlFragment(XmlTextReader xml_reader)
        {
            Boolean result = true;
            try
            {
                while (xml_reader.Read())
                {
                    if (xml_reader.NodeType == XmlNodeType.Element)
                    {
                        switch (xml_reader.Name)
                        {
                            case "ComponentSettings":
                                result = ParseComponentSettings(xml_reader);
                                if (!result)
                                    return result;
                                break;
                            case "ComponentState":
                                result = ParseComponentState(xml_reader);
                                if (!result)
                                    return result;
                                break;
                            case "Events":
                                result = ParseEvents(xml_reader);
                                if (!result)
                                    return result;
                                break;
                            case "ProcessInfo":
                                result = ParseProcessInfo(xml_reader);
                                if (!result)
                                    return result;
                                break;
                            case "SystemInfo":
                                result = ParseSystemInfo(xml_reader);
                                if (!result)
                                    return result;
                                break;
                            case "TaskStates":
                                result = ParseTaskStates(xml_reader);
                                if (!result)
                                    return result;
                                break;
                            case "TaskState":
                                result = ParseTaskState(xml_reader);
                                if (!result)
                                    return result;
                                break;
                            case "SettingsStates":
                                result = ParseSettingsStates(xml_reader);
                                if (!result)
                                    return result;
                                break;
                            default:
                                result = false;
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                m_error_info = e.Message;
                LoggerPP.log.Error("ParseXmlFragment():: " + m_error_info);
                result = false;
            }
            return result;
        }

        private Boolean ParseCommonElement(XmlTextReader xml_reader, StringDictionary name_value_map)
        {
            Boolean result = true;
            String element_name = null;
            String element_value = null;
            while (xml_reader.Read())
            {
                switch (xml_reader.NodeType)
                {
                    case XmlNodeType.Element:
                        element_name = xml_reader.Name;
                        break;
                    case XmlNodeType.Text:
                        element_value = xml_reader.Value;
                        break;
                    case XmlNodeType.EndElement:
                        if ((element_name != null) /*&& (element_value != null)*/)
                        {
                            try
                            {
                                name_value_map.Add(element_name, element_value);
                            }
                            catch (Exception e)
                            {
                                m_error_info = e.Message;
                                result = false;
                                return result;
                            }
                            element_name = null;
                            element_value = null;
                        }
                        else
                        {
                            return result;
                        }
                        break;
                    default:
                        break;
                }
            }
            return result;
        }

        private Boolean ParseComponentSettings(XmlTextReader xml_reader)
        {
            Boolean result = true;
            String computer_name = null;
            if (xml_reader.MoveToFirstAttribute())
            {
                computer_name = xml_reader.Value;
            }
            String element_name = null;
            String element_value = null;
            while (xml_reader.Read())
            {
                if ((xml_reader.NodeType == XmlNodeType.Element) && (xml_reader.Name.Equals("Component")))
                {
                    if (xml_reader.MoveToFirstAttribute())
                    {
                        element_name = xml_reader.Value;
                        if (xml_reader.MoveToElement())
                        {
                            element_value = xml_reader.ReadInnerXml();
                            result = true;
                            try
                            {
                                db.InsertComponentSettings(computer_name, element_name, element_value);
                            }
                            catch (Exception e)
                            {
                                m_error_info = e.Message;
                                LoggerPP.log.Error("ParseComponentSettings() :: " + m_error_info);
                                result = false;
                            }
                            if (!result)
                                return result;
                        }
                    }
                }
            }
            return result;
        }

        private Boolean ParseSettingsStates(XmlTextReader xml_reader)
        {
            Boolean result = true;
            xml_reader.MoveToContent();
            String xml = xml_reader.ReadInnerXml();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            String computerName = String.Empty;
            String macAddress = String.Empty;
            foreach (XmlNode node in doc.GetElementsByTagName("SettingsState"))
            {
                if (node.Attributes["computerName"] != null && node.Attributes["mac"] != null)
                {
                    computerName = node.Attributes["computerName"].Value;
                    macAddress = node.Attributes["mac"].Value;
                    break;
                }
            }

            if (String.IsNullOrEmpty(computerName) || String.IsNullOrEmpty(macAddress))
            {
                m_error_info = "ParseSettingsStates() :: ComputerName or MAC are empty.";
                return false;
            }


            foreach (XmlNode node in doc.GetElementsByTagName("Key"))
            {
                if (node.Attributes["name"].Value == "Vba32")
                {
                    IConfigureTask task;
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        if (child.Name.ToLower() == "key")
                        {
                            switch (child.Attributes["name"].Value.ToLower())
                            {
                                case "loader":
                                    task = new TaskConfigureLoader();
                                    task.LoadFromRegistry(child.OuterXml);
                                    try
                                    {
                                        db.InsertSettings(macAddress, "Vba32 Loader", task.SaveToXml());
                                    }
                                    catch (Exception e)
                                    {
                                        m_error_info = e.Message;
                                        result = false;
                                    }
                                    break;
                                case "monitor":
                                    task = new TaskConfigureMonitor();
                                    task.LoadFromRegistry(child.OuterXml);
                                    try
                                    {
                                        db.InsertSettings(macAddress, "Vba32 Monitor", task.SaveToXml());
                                    }
                                    catch (Exception e)
                                    {
                                        m_error_info = e.Message;
                                        result = false;
                                    }
                                    break;
                                case "qtn":
                                    task = new TaskConfigureQuarantine();
                                    task.LoadFromRegistry(child.OuterXml);
                                    try
                                    {
                                        db.InsertSettings(macAddress, "Vba32 Quarantine", task.SaveToXml());
                                    }
                                    catch (Exception e)
                                    {
                                        m_error_info = e.Message;
                                        result = false;
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                }
            }

            return result;
        }

        private Boolean ParseComponentState(XmlTextReader xml_reader)
        {
            Boolean result = true;
            String computer_name = null;
            if (xml_reader.MoveToFirstAttribute())
            {
                computer_name = xml_reader.Value;
            }
            while (xml_reader.Read())
            {
                if ((xml_reader.NodeType == XmlNodeType.Element) && (xml_reader.Name.Equals("Component")))
                {
                    StringDictionary name_value_map = new StringDictionary();
                    result = ParseCommonElement(xml_reader, name_value_map);
                    if (!result)
                        return result;
                    result = true;
                    try
                    {
                        db.InsertComponentState(computer_name, name_value_map["Name"], name_value_map["State"], name_value_map["Version"]);
                    }
                    catch (Exception e)
                    {
                        m_error_info = e.Message;
                        LoggerPP.log.Error("ParseComponentState() :: " + m_error_info);
                        result = false;
                    }
                    if (!result)
                        return result;
                }
            }
            return result;
        }

        private Boolean ParseEvents(XmlTextReader xml_reader)
        {
            Boolean result = true;
            while (xml_reader.Read())
            {
                if ((xml_reader.NodeType == XmlNodeType.Element) && (xml_reader.Name.Equals("Event")))
                {
                    StringDictionary name_value_map = new StringDictionary();
                    result = ParseCommonElement(xml_reader, name_value_map);
                    if (!result)
                        return result;
                    result = true;
                    try
                    {
                        db.InsertEvent(new EventsEntity(name_value_map), (Int16)CompUsbCount);
                    }
                    catch (Exception e)
                    {
                        m_error_info = e.Message;
                        LoggerPP.log.Error("ParseEvents() :: " + m_error_info);
                        result = false;
                    }
                    if (!result)
                        return result;

                    OnEventInsert(name_value_map);
                }
            }
            return result;
        }

        private Boolean ParseProcessInfo(XmlTextReader xml_reader)
        {
            Boolean result = true;
            String computer_name = null;
            if (xml_reader.MoveToFirstAttribute())
            {
                computer_name = xml_reader.Value;
                result = true;
                try
                {
                    db.DeleteProcessInfo(computer_name);
                }
                catch (Exception e1)
                {
                    m_error_info = e1.Message;
                    LoggerPP.log.Error("ParseProcessInfo() :: delete process info: " + m_error_info);
                    result = false;
                }
            }
            if (!result)
            {
                return result;
            }
            while (xml_reader.Read())
            {
                if ((xml_reader.NodeType == XmlNodeType.Element) && (xml_reader.Name.Equals("Process")))
                {
                    StringDictionary name_value_map = new StringDictionary();
                    result = ParseCommonElement(xml_reader, name_value_map);
                    if (!result)
                        return result;
                    result = true;
                    try
                    {
                        db.InsertProcessInfo(computer_name, name_value_map["ProcessName"], Convert.ToInt32(name_value_map["MemorySize"]));
                    }
                    catch (Exception e2)
                    {
                        m_error_info = e2.Message;
                        LoggerPP.log.Error("ParseProcessInfo() :: insert process info: " + m_error_info);
                        result = false;
                    }
                    if (!result)
                        return result;
                }
            }
            return result;
        }

        private Boolean ParseSystemInfo(XmlTextReader xml_reader)
        {
            Boolean result = true;
            StringDictionary name_value_map = new StringDictionary();
            result = ParseCommonElement(xml_reader, name_value_map);
            if (!result)
                return result;
            result = true;
            try
            {
                db.InsertSystemInfo(new ComputersEntity(name_value_map), (Int16)CompCount);
            }
            catch (Exception e)
            {
                m_error_info = e.Message;
                LoggerPP.log.Error("ParseSystemInfo() :: " + m_error_info);
                result = false;
            }
            return result;
        }

        private Boolean ParseTaskStates(XmlTextReader xml_reader)
        {
            Boolean result = true;
            while (xml_reader.Read())
            {
                if ((xml_reader.NodeType == XmlNodeType.Element) && (xml_reader.Name.Equals("TaskState")))
                {
                    result = ParseTaskState(xml_reader);
                }
                if (!result)
                {
                    return result;
                }
            }
            return result;
        }

        private Boolean ParseTaskState(XmlTextReader xml_reader)
        {
            Boolean result = true;
            StringDictionary name_value_map = new StringDictionary();
            result = ParseCommonElement(xml_reader, name_value_map);
            if (!result)
                return result;
            result = true;
            try
            {
                db.InsertTaskState(new TaskEntity(name_value_map));
            }
            catch (Exception e)
            {
                m_error_info = e.Message;
                LoggerPP.log.Error("ParseTaskState() :: " + m_error_info);
                result = false;                
            }
            return result;
        }

        #endregion
    }
}
