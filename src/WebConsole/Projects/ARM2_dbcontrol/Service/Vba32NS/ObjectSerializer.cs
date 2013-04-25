using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;


namespace ARM2_dbcontrol.Service.Vba32NS
{
    /// <summary>
    /// Класс для сериализации/десериализации данных
    /// </summary>
    public static class ObjectSerializer
    {

        /// <summary>    
        /// Сериализует объект в XML файл
        /// </summary>    
        public static void ObjToXmlStr(string fileName, object obj)
        {
            if (obj == null)
                return;

            XmlSerializer sr = new XmlSerializer(obj.GetType());
            FileStream fs = new FileStream(fileName, FileMode.Create);
            try
            {
                sr.Serialize(fs, obj, new XmlSerializerNamespaces(new XmlQualifiedName[] { new XmlQualifiedName(string.Empty) }));

            }
            finally
            {
                fs.Close();
            }
        }

        /// <summary>    
        /// Десериализует XML файл в объект заданного типа    
        /// </summary>    
        public static T XmlFileToObj<T>(string fileName)
        {
            if (!File.Exists(fileName))
                return default(T);

            FileStream fs = new FileStream(fileName, FileMode.Open);
            try
            {
                XmlSerializer sr = new XmlSerializer(typeof(T));
                return (T)sr.Deserialize(fs);
            }
            finally
            {
                fs.Close();
            }
        }

        /// <summary>    
        /// Сериализует объект в XML строку
        /// </summary> 
        public static string ObjToXmlStr(object obj)
        {
            if (obj == null)
                return String.Empty;

            XmlSerializer sr = new XmlSerializer(obj.GetType());
            MemoryStream memoryStream = new MemoryStream();            
            try
            {
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                sr.Serialize(xmlTextWriter, obj);
                memoryStream = (MemoryStream)xmlTextWriter.BaseStream;

                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
            catch
            {
                return String.Empty;
            }                        
        }

        /// <summary>    
        /// Десериализует XML строку в объект заданного типа    
        /// </summary>  
        ///         
        public static T XmlStrToObj<T>(string xmlStr)
        {
            if (String.IsNullOrEmpty(xmlStr))
                return default(T);
                        
            try
            {
                XmlSerializer sr = new XmlSerializer(typeof(T));
                MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlStr));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                
                return (T)sr.Deserialize(memoryStream);
            }
            catch
            {
                return default(T);
            }
        }
    }
}
