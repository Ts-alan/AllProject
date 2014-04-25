using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace VirusBlokAda.CC.Common.Xml
{
    /// <summary>
    /// Работа с произвольным xml (используется для разбора xml,
    /// созданного с помощью XmlBuilder())
    /// </summary>
    public class XmlTaskParser
    {
        private String xml = String.Empty;

        public XmlTaskParser()
        {
        }

        public XmlTaskParser(String xml)
        {
            this.xml = xml;
        }

        /// <summary>
        /// Читает значение из xml файла
        /// </summary>
        /// <param name="tag">тэг</param>
        /// <returns>значение</returns>
        public String GetValue(String tag)
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
        public String GetValue(String tag, String exclude)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            String str1 = String.Empty;
            foreach (XmlNode str in (doc.GetElementsByTagName(tag)))
            {
                 str1 = str.InnerText;
            }
            if (str1.StartsWith(exclude))
                return str1.Substring(exclude.Length);
            else return str1;
        }

        public String GetXmlTagContent(String key)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            return GetXmlTagContent(doc.FirstChild, key);
        }

        private String GetXmlTagContent(XmlNode root, String key)
        {
            foreach (XmlNode node in root.ChildNodes)
            {
                if (node.Name.CompareTo(key) == 0)
                {
                    return node.InnerXml;
                }
                if ((node.HasChildNodes) && (node.ChildNodes[0].NodeType != XmlNodeType.Text))
                {
                    String tmp = GetXmlTagContent(node, key);
                    if (!String.IsNullOrEmpty(tmp)) return tmp;                    
                }
            }

            return String.Empty;
        }


    }
}
