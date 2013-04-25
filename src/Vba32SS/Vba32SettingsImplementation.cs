using System;
using System.Collections.Generic;
using System.Text;

using System.Xml;
using System.Diagnostics;

using Microsoft.Win32;

namespace Vba32.ControlCenter.SettingsService
{
    internal class Vba32SettingsImplementation : MarshalByRefObject, IVba32Settings
    {
        private static object synch = new object();

        /// <summary>
        /// Экспортируемый метод
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public bool ChangeRegistry(string xml)
        {
            Vba32SettingsService.LogMessage("Vba32SettingsImplementation.ChangeRegistry()::Вызван");
            try
            {
                if((xml[0]!='<')&&(xml[1]!='?'))
                    xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" + xml;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                XmlNode root = doc.SelectSingleNode("VbaSettings");

                string registryVba32KeyName;
                if (System.Runtime.InteropServices.Marshal.SizeOf(typeof(IntPtr)) == 8)
                    registryVba32KeyName = "SOFTWARE\\Wow6432Node\\Vba32\\";
                else
                    registryVba32KeyName = "SOFTWARE\\Vba32\\";


                ReverseNode(root, registryVba32KeyName);
            }
            catch (Exception ex)
            {
                Vba32SettingsService.LogError("Vba32SettingsImplementation.ChangeRegistry()::"
                  + ex.Message, EventLogEntryType.Error);
                return false;
            }
            return true;
        }

        #region Парсинг xml

        private void ReverseNode(XmlNode root, string key)
        {
            if (root.Name != "VbaSettings")
                key += root.Name + "\\";

            foreach (XmlNode node in root.ChildNodes)
            {
                if ((node.HasChildNodes) && (node.ChildNodes[0].NodeType != XmlNodeType.Text))
                {
                    ReverseNode(node, key);
                }
                else
                {
                    string attrName = String.Empty;
                    string attrValue = String.Empty;
                    if (node.Attributes != null)
                        foreach (XmlAttribute attr in node.Attributes)
                        {
                            attrName = attr.Name.ToLower();
                            attrValue = attr.Value.ToLower();
                        }

                    switch (attrName)
                    {
                        case "type":
                            //Это value
                            ChangeValue(key, attrValue, node.Name, node.InnerText);
                            break;

                        case "delete":
                            //Это key для уделения
                            if (attrValue == "true")
                                ChangeKey(key, node.Name, "delete");
                            break;

                        case "":
                            //Это key для создания
                            ChangeKey(key, node.Name, "create");
                            break;
                    }
                }
            }
        }

        #endregion 

        #region  Создание нужных ключей и параметров реестра

        /// <summary>
        /// Создает или удаляет ключ реестра
        /// </summary>
        /// <param name="key">путь</param>
        /// <param name="name">имя ключа</param>
        /// <param name="mode">режим</param>
        /// <returns></returns>
        private bool ChangeKey(string key, string name, string mode)
        {
            RegistryKey regkey;
            try
            {
                lock (synch)
                {
                    regkey = Registry.LocalMachine.CreateSubKey(key);
                    switch (mode)
                    {
                        case "create":
                            regkey.CreateSubKey(name);
                            break;
                        case "delete":
                            regkey.DeleteSubKey(name, false);
                            break;
                    }
                    regkey.Close();
                }
            }
            catch (Exception ex)
            {
                Vba32SettingsService.LogError("Vba32SettingsImplementation.ChangeKey()::"
                    + ex.Message, EventLogEntryType.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Изменяет параметр реестра
        /// </summary>
        /// <param name="key">путь</param>
        /// <param name="type">тип параметра</param>
        /// <param name="name">имя</param>
        /// <param name="value">значение</param>
        /// <returns></returns>
        private bool ChangeValue(string key, string type, string name, string value)
        {
            RegistryKey regkey;
            try
            {
                lock (synch)
                {
                    regkey = Registry.LocalMachine.CreateSubKey(key);

                    switch (type)
                    {
                        case "reg_sz":
                            regkey.SetValue(name, value, RegistryValueKind.String);
                            break;

                        case "reg_dword":
                            regkey.SetValue(name, Convert.ToUInt32(value), RegistryValueKind.DWord);
                            break;

                        case "reg_binary":
                            regkey.SetValue(name, Encoding.Default.GetBytes(value), RegistryValueKind.Binary);
                            break;

                        case "delete":
                            regkey.DeleteValue(name, false);
                            break;
                    }
                    regkey.Close();
                }
            }
            catch (Exception ex)
            {
                Vba32SettingsService.LogError("Vba32SettingsImplementation.ChangeValue()::"
                    + ex.Message, EventLogEntryType.Error);
                return false;
            }

            return true;
        }
        #endregion
    }
}
