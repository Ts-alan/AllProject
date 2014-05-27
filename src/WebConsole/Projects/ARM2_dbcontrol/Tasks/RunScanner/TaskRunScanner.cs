using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.CC.Common.Xml;

namespace ARM2_dbcontrol.Tasks
{
    public class TaskRunScanner: IConfigureTask
    {
        #region Fields

        public const String TaskType = "RunScanner";
        private String _Vba32CCUser;
        private Boolean _IsCheckMemory;
        private List<String> _PathScan;

        #endregion

        #region Properties

        public List<String> PathScan
        {
            get { return _PathScan; }
            set { _PathScan = value; }
        }

        public Boolean IsCheckMemory
        {
            get { return _IsCheckMemory; }
            set { _IsCheckMemory = value; }
        }

        public String Vba32CCUser
        {
            get { return _Vba32CCUser; }
            set { _Vba32CCUser = value; }
        }

        #endregion

        #region Constructors

        public TaskRunScanner()
        {
            _PathScan = new List<String>();
        }

        #endregion

        #region IConfigureTask Members

        public String SaveToXml()
        {
            XmlBuilder xml = new XmlBuilder("task");

            xml.AddNode("IsCheckMemory", IsCheckMemory ? "1" : "0");
            if (PathScan.Count > 0)
            {
                StringBuilder sb = new StringBuilder(64 * PathScan.Count);
                Int32 index = 0;
                foreach (String path in PathScan)
                {
                    if (!String.IsNullOrEmpty(path))
                        sb.AppendFormat("<Path{0}>{1}</Path{0}>", index++, path);
                }
                xml.AddNode("PathesScan", sb.ToString());
            }

            xml.AddNode("Vba32CCUser", Vba32CCUser);
            xml.AddNode("Type", TaskType);

            xml.Generate();

            return xml.Result;
        }

        public String GetTask()
        {
            StringBuilder result = new StringBuilder(256);

            result.Append("<VsisCommand>");
            result.Append("<Args>");

            result.Append(@"<command><arg><key>module-id</key><value>{2E406790-5472-4E0C-9EBF-88D081AA09AC}</value></arg>");
            result.Append(@"<arg><key>command</key><value>scan</value></arg><arg>");

            if (IsCheckMemory)
                result.Append(@"<key>memory</key><value />");

            Int32 index = 0;
            foreach (String path in PathScan)
            {
                result.AppendFormat(@"<key>path{0}</key><value>{1}</value>", index++, path.Replace('\\', '/'));
            }

            result.Append(@"</arg></command>");
            result.Append(@"</Args>");
            result.Append(@"<Async>0</Async>");
            result.Append(@"</VsisCommand>");

            return result.ToString();
        }

        public void LoadFromXml(String xml)
        {
            XmlTaskParser pars = new XmlTaskParser(xml);

            IsCheckMemory = pars.GetValue("IsCheckMemory") == "1";
            Int32 index = 0;
            String val;
            PathScan.Clear();
            while (!String.IsNullOrEmpty(val = pars.GetValue(String.Format("Path{0}", index++))))
            {
                PathScan.Add(val);
            }
        }

        public void LoadFromRegistry(string reg)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
