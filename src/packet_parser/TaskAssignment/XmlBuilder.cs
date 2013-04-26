using System;
using System.Collections.Generic;
using System.Text;

namespace Vba32CC.TaskAssignment
{
    /// <summary>
    /// Класс, конструирует xml.Порядок вызова методов: 
    /// 1. Создаются все узлы - AddNode()
    /// 2. Добавляется корневой - AddRoot()
    /// 3. Добавляется заголовок - AddTop()
    /// </summary>
    public class XmlBuilder
    {
        private String top = @"<?xml version=" + '"' + "1.0" + '"' + " encoding=" + '"' + "utf-8" + '"' + "?>";
        private String result = String.Empty;
        private String root = String.Empty;

        public XmlBuilder()
        {
        }

        public XmlBuilder(String root)
        {
            this.root = root;
        }

        public void AddNode(String tag, String value)
        {
            result += "<" + tag + ">" + value + "</" + tag + ">\n"; ;
        }

        private void AddTop()
        {
            result = top + "\n" + result;
        }
        private void AddRoot()
        {
            result = "<" + root + ">\n" + result + "</" + root + ">";
        }

        public void Generate()
        {
            AddRoot();
            AddTop();
        }

        #region Property

        public String Result
        {
            get
            {
                return this.result;
            }
        }

        public String Root
        {
            set { this.root = value; }
        }

        public String Top
        {
            set { this.top = value; }
            get { return this.top; }
        }
        #endregion
    }
}
