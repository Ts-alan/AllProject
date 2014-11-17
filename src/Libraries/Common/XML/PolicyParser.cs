using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;

namespace VirusBlokAda.CC.Common.Xml
{
    /// <summary>
    /// performs policy analysis to tasks
    /// </summary>
    public class PolicyParser
    {
        private XmlDocument xml = null;

        private XmlNode root = null;

        public PolicyParser(String content)
        {
            xml = new XmlDocument();
            xml.LoadXml("<Tasks>"+content+"</Tasks>");
            root = xml.DocumentElement;
        }

        public String GetParam(String name)
        {
            return GetContentNode(name);
        }

        private String GetContentNode(String name)
        {
            try
            {
                XmlNode node = root.SelectSingleNode(
                String.Format("descendant::{0}", name));
                return node.InnerXml;
            }
            catch
            {
            }
            return String.Empty;
        }

        private String GetTaskConfigureSettingsContentNode(String name)
        {
            try
            {
                XmlNode node = root.SelectSingleNode(
                String.Format("descendant::Task/Content/TaskConfigureSettings/{0}", name));
                return String.Format("<{0}>{1}</{0}>", name, node.InnerXml);
            }
            catch
            {
            }
            return String.Empty;
        }
                
        private String GetTaskConfigureSettingsContentNodeCustomAction(String name)
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

        public List<String> GetTaskCreateProcessContentNodes()
        {
            try
            {
                //<Task><Content><TaskCustomAction><Options><NamedCreateProcess><Name>{0}</Name><TaskCreateProcess>
                List<String> list = new List<String>();
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
            return new List<String>();
        }

    }

}
