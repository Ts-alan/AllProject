using System;
using System.Collections.Generic;
using System.Text;

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
                    return Convert.ToInt32(ConvertFromBase64(value, encoded));
                case 2://String
                    return ConvertFromBase64(value, encoded);
                case 3://Binary
                    return ConvertToDumpString(Encoding.Unicode.GetBytes(ConvertFromBase64(value, encoded)));
                case 4://Int64
                    return Convert.ToInt64(ConvertFromBase64(value, encoded));
                case 5://String[]
                    List<String> list = new List<String>();
                    String[] splitted = ConvertFromBase64(value, encoded).Split('?');
                    foreach (String item in splitted)
                    {
                        if (!String.IsNullOrEmpty(item))
                            list.Add(item);
                    }
                    
                    return list;
                default:
                    throw new Exception("Type value is incorrect.");
            }
        }

        public static String ConvertFromBase64(String value, Int32 encoded)
        {
            if (encoded == 1)
            {
                return FromBase64UnicodeString(value);
            }
            else
                return value;
        }

        public static String FromBase64UnicodeString(String source)
        {
            Byte[] bs = Convert.FromBase64String(source);
            return Encoding.Unicode.GetString(bs);
        }

        public static String FromBase64String(String source)
        {
            Byte[] bs = Convert.FromBase64String(source);
            return Encoding.ASCII.GetString(bs);
        }

        public static String ToBase64String(String source)
        {
            Byte[] bs = Encoding.ASCII.GetBytes(source);
            return Convert.ToBase64String(bs);
        }

        public static String GetMd5Hash(String input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            System.Security.Cryptography.MD5 md5Hasher = System.Security.Cryptography.MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            Byte[] data = md5Hasher.ComputeHash(Encoding.Unicode.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (Int32 i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
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

        public static String ConvertToDumpString(Byte[] data)
        {
            StringBuilder str = new StringBuilder();
            foreach (Byte b in data)
            {
                str.Append(b.ToString("X2"));
            }
            return str.ToString();
        }
    }
}
