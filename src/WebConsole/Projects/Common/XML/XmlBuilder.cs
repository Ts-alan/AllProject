using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.Common.Xml
{
    /// <summary>
    /// Класс, конструирует xml.Порядок вызова методов: 
    /// 1. Создаются все узлы - AddNode()
    /// 2. Добавляется корневой - AddRoot()
    /// 3. Добавляется заголовок - AddTop()
    /// </summary>
    public class XmlBuilder
    {
        #region Fields

        private String top = @"<?xml version=" + '"' + "1.0" + '"' + " encoding=" + '"' + "utf-8" + '"' + "?>";
        private String result = String.Empty;
        private String root = String.Empty;

        #endregion

        #region Property

        public String Result
        {
            get { return result; }
        }

        public String Root
        {
            set { root = value; }
        }

        public String Top
        {
            set { top = value; }
            get { return top; }
        }

        #endregion

        #region Constructors

        public XmlBuilder()
        {
        }

        public XmlBuilder(String root)
        {
            this.root = root;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add node to XML
        /// </summary>
        /// <param name="tag">Tag name</param>
        /// <param name="value">Value</param>
        public void AddNode(String tag, String value)
        {
            result += "<" + tag + ">" + value + "</" + tag + ">\n"; ;
        }

        /// <summary>
        /// Add top part to XML
        /// </summary>
        private void AddTop()
        {
            result = top + "\n" + result;
        }

        /// <summary>
        /// Add root to XML
        /// </summary>
        private void AddRoot()
        {
            result = "<" + root + ">\n" + result + "</" + root + ">";
        }
        
        /// <summary>
        /// Generate XML
        /// </summary>
        public void Generate()
        {
            AddRoot();
            AddTop();
        }

        #endregion
    }
}
