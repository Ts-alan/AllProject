using System;
using System.Collections.Generic;
using System.Text;

using System.Xml;

namespace ARM2_dbcontrol.Tasks
{
    /// <summary>
    /// Работа с произвольным xml (используется для разбора xml,
    /// созданного с помощью ~.Generation.XmlBuilder())
    /// </summary>
    public class XmlTaskParser
    {
        private string xml = String.Empty;

        public XmlTaskParser()
        {
        }

        public XmlTaskParser(string xml)
        {
            this.xml = xml;
        }

        /// <summary>
        /// Читает значение из xml файла
        /// </summary>
        /// <param name="tag">тэг</param>
        /// <returns>значение</returns>
        public string GetValue(string tag)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            
            foreach (XmlNode str in (doc.GetElementsByTagName(tag)))
              return str.InnerText;

            return String.Empty;
        }


        /// <summary>
        /// Читает значение из xml файла
        /// </summary>
        /// <param name="tag">тэг</param>
        /// <returns>значение</returns>
        public string GetValue(string tag, string exclude)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            string str1 = String.Empty;
            foreach (XmlNode str in (doc.GetElementsByTagName(tag)))
            {
                 str1 = str.InnerText;
            }

            return str1.TrimStart(exclude.ToCharArray());
        }

        public string GetXmlTagContent(string key)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            return GetXmlTagContent(doc.FirstChild, key);
        }

        private string GetXmlTagContent(XmlNode root, string key)
        {
            foreach (XmlNode node in root.ChildNodes)
            {
                if (node.Name.CompareTo(key) == 0)
                {
                    return node.InnerXml;
                }
                if ((node.HasChildNodes) && (node.ChildNodes[0].NodeType != XmlNodeType.Text))
                {
                    string tmp = GetXmlTagContent(node, key);
                    if (!String.IsNullOrEmpty(tmp)) return tmp;                    
                }
            }

            return String.Empty;
        }


    }
}
