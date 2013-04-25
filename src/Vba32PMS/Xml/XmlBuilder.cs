using System;
using System.Collections.Generic;
using System.Text;

namespace Vba32.ControlCenter.PeriodicalMaintenanceService.Xml
{
    /// <summary>
    /// Класс, конструирует xml.Порядок вызова методов: 
    /// 1. Создаются все узлы - AddNode()
    /// 2. Добавляется корневой - AddRoot()
    /// 3. Добавляется заголовок - AddTop()
    /// </summary>
    public class XmlBuilder
    {
        private string top = @"<?xml version=" + '"' + "1.0" + '"' + " encoding=" + '"' + "UTF-8" + '"' + "?>";
        private string result = String.Empty;
        private string root = String.Empty;

        public XmlBuilder()
        {
        }

        public XmlBuilder(string root)
        {
            this.root = root;
        }

        public void AddNode(string tag, string value)
        {
            result += "<" + tag + ">" + value + "</" + tag + ">"; ;
        }

        private void AddTop()
        {
            result = top + "" + result;
        }
        private void AddRoot()
        {
            result = "<" + root + ">" + result + "</" + root + ">";
        }
        

        public void Generate()
        {
            AddRoot();
            AddTop();
        }

        #region Property

        public string Result
        {
            set { this.result = value; }
            get { return this.result; }
        }

        public string Root
        {
            set { this.root = value; }
        }

        public string Top
        {
            set { this.top = value; }
            get { return this.top; }
        }

        #endregion

    }
}
