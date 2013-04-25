using System;
using System.Collections.Generic;
using System.Text;

namespace ARM2_dbcontrol.Generation
{
    /// <summary>
    /// �����, ������������ xml.������� ������ �������: 
    /// 1. ��������� ��� ���� - AddNode()
    /// 2. ����������� �������� - AddRoot()
    /// 3. ����������� ��������� - AddTop()
    /// </summary>
    public class XmlBuilder
    {
        private string top = @"<?xml version=" + '"' + "1.0" + '"' + " encoding=" + '"' + "utf-8" + '"' + "?>";
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

        public string Result
        {
            get
            {
                return this.result;
            }
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
