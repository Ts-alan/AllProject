using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;


using System.Xml;

using VirusBlokAda.Vba32CC.Policies.General;

namespace VirusBlokAda.Vba32CC.Policies.Xml
{
/// <summary>
/// performs policy analysis to tasks
/// </summary>
    public class PolicyParser
    {

        private XmlDocument xml = null;

        private XmlNode root = null;

        public PolicyParser(string content)
        {
            xml = new XmlDocument();
            xml.LoadXml("<Tasks>"+content+"</Tasks>");
            root = xml.DocumentElement;
        }

        public PolicyParser(Policy policy):this(policy.Content)
        {
            
        }


        public string GetParamToLoader()
        {
            return GetTaskConfigureSettingsContentNode("loader");
        }

        public string GetParamToMonitor()
        {
            return GetTaskConfigureSettingsContentNode("monitor") + GetTaskConfigureSettingsContentNode("exclusions") + GetTaskConfigureSettingsContentNode("idlecheck");
        }

        public string GetParamToQtn()
        {
            return GetTaskConfigureSettingsContentNode("qtn");
        }

        public string GetParamToDeviceProtect()
        {
            return GetTaskConfigureSettingsContentNodeCustomAction("SetRegistrySettings");
        }

        public string GetParamToDailyDeviceProtect()
        {
            return GetTaskConfigureSettingsContentNodeCustomAction("DailyProtect");
        }

        private string GetTaskConfigureSettingsContentNode(string name)
        {
            try
            {
                XmlNode node = root.SelectSingleNode(
                    String.Format("descendant::Task/Content/TaskConfigureSettings/{0}", name));
                return String.Format("<{0}>{1}</{0}>",name,node.InnerXml);
            }
            catch
            {
            }
            return String.Empty;
        }
                
        private string GetTaskConfigureSettingsContentNodeCustomAction(string name)
        {
            try
            {
                XmlNode node = root.SelectSingleNode(
                    String.Format("descendant::Task/Content/TaskCustomAction/Options/{0}", name));
                return String.Format("<{0}>{1}</{0}>", name, node.InnerXml);
            }
            catch
            {
            }
            return String.Empty;
        }

        public List<string> GetTaskCreateProcessContentNodes()
        {
            try
            {
                //<Task><Content><TaskCustomAction><Options><NamedCreateProcess><Name>{0}</Name><TaskCreateProcess>
                List<string> list = new List<string>();
                foreach (XmlNode task in 
                    root.SelectNodes(String.Format("descendant::Task/Content/TaskCustomAction/Options/NamedCreateProcess/TaskCreateProcess/CommandLine")))
                {
                    list.Add(task.LastChild.InnerText);
                }
                return list;
            }
            catch
            {
            }
            return new List<string>();
        }

    }

}
