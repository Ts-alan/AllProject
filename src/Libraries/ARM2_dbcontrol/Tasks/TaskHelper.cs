using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.CC.Common;

namespace ARM2_dbcontrol.Tasks
{
    internal static class TaskHelper
    {
        public static String GetValueForList(System.Xml.XmlNode child)
        {
            return (child.Attributes["name"].Value[0] == 'N' ? "-" : "+") + GetValue(child).ToString();
        }

        public static Object GetValue(System.Xml.XmlNode node)
        {
            Int32 type = 0;
            Int32 encoded = 0;
            String value = String.Empty;
            if (node.Attributes["type"] != null)
            {
                type = Convert.ToInt32(node.Attributes["type"].Value);
            }
            if (node.Attributes["value"] != null)
            {
                value = node.Attributes["value"].Value;
            }
            if (node.Attributes["encoded"] != null)
            {
                encoded = Convert.ToInt32(node.Attributes["encoded"].Value);
            }

            switch (type)
            {
                case 1://Int32
                    return Convert.ToInt32(Anchor.ConvertFromBase64(value, encoded));
                case 2://String
                    return Anchor.ConvertFromBase64(value, encoded);
                case 3://Binary
                    return Anchor.ConvertToDumpString(Encoding.Unicode.GetBytes(Anchor.ConvertFromBase64(value, encoded)));
                case 4://Int64
                    return Convert.ToInt64(Anchor.ConvertFromBase64(value, encoded));
                case 5://String[]
                    List<String> list = new List<String>();
                    String[] splitted = Anchor.ConvertFromBase64(value, encoded).Split('?');
                    foreach (String item in splitted)
                    {
                        if (!String.IsNullOrEmpty(item))
                            list.Add(item.Replace("\0", ""));
                    }
                    
                    return list;
                default:
                    throw new Exception("Type value is incorrect.");
            }
        }        

        public static List<String> ParseMD5Path(String path)
        {
            List<String> parsePath = new List<String>();
            path = path.Replace("reg_sz:", "");
            String tmp = String.Empty;

            while (path.Length != 0)
            {
                path = path.Remove(0, path.IndexOf(">") + 1);
                tmp = path.Substring(0, path.IndexOf("<"));
                path = path.Remove(0, path.IndexOf("<") + 2);
                tmp = (path.Substring(0, 1) == "N" ? "-" : "+") + tmp;
                parsePath.Add(tmp);
                path = path.Remove(0, path.IndexOf(">") + 1);
            }

            return parsePath;
        }              
    }
}
