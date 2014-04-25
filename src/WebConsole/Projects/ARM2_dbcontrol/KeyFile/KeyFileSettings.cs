using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace VirusBlokAda.CC.KeyFile
{
    /// <summary>
    /// Class for getting informations from key file
    /// </summary>
    public static class KeyFileSettings
    {
        private static String KeyFileName = @"vba32.key";
        private static String KeyUSB = @"VBA32USB";

        public static Boolean IsKeyUSBExist
        {
            get { return IsKeyExist(KeyUSB); }
        }

        //If "key file not fount", maybe current dll removed or replaced
        private static String GetKeyFileContent()
        {
            String keyFileContent = String.Empty;
            StreamReader reader = null;
            //Get path current dll (ARM2_dbcontrol) (ex. "file:///C:/Program Files/Vba32 Control Center/Web Console/bin/ARM2_dbcontrol.dll")
            //Format path in "C:\Program Files\Vba32 Control Center\Web Console\bin\ARM2_dbcontrol.dll"
            String AppPath = System.Reflection.Assembly.GetExecutingAssembly().CodeBase.Replace(@"file:///", "").Replace(@"/", @"\");
            //Get full path directory "Vba32 Control Center"
            AppPath = Directory.GetParent(AppPath).Parent.Parent.FullName + @"\";

            try
            {
                reader = new StreamReader(AppPath + KeyFileName);
                keyFileContent = reader.ReadToEnd();
                reader.Close();
            }
            catch
            {
                if (reader != null)
                    reader.Close();
                throw new Exception("Key file not found.");
            }

            return keyFileContent;
        }

        private static Boolean IsKeyExist(String keyName)
        {
            String keyFileContent = GetKeyFileContent();

            Int32 indexKey = keyFileContent.IndexOf(String.Format(@"[{0}]", keyName));
            if (indexKey < 0) return false;
            Int32 indexBegin = keyFileContent.IndexOf("ComputersLimit=", indexKey) + 15;
            Int32 indexEnd = keyFileContent.IndexOfAny(new char[] { ' ', '\r', '\n' }, indexBegin);

            String val;
            if (indexEnd != -1)
                val = keyFileContent.Substring(indexBegin, indexEnd - indexBegin);
            else val = keyFileContent.Substring(indexBegin);

            try
            {
                if (Int32.Parse(val) > 0)
                    return true;
                else return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
