using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;


namespace Vba32.ControlCenter.PeriodicalMaintenanceService.Xml
{
    /// <summary>
    /// Класс для сериализации/десериализации данных
    /// </summary>
    public static class ObjectSerializer
    {

        /// <summary>    
        /// Сериализует объект в XML файл
        /// </summary>    
        public static void ObjToXmlStr(String fileName, Object obj)
        {
            Logger.Debug("ObjectSerializer.ObjToXmlStr()::Сериализуем объект в xml файл");
            if (obj == null)
                return;

            XmlSerializer sr = new XmlSerializer(obj.GetType());
            FileStream fs = new FileStream(fileName, FileMode.Create);
            try
            {
                sr.Serialize(fs, obj, new XmlSerializerNamespaces(new XmlQualifiedName[] { new XmlQualifiedName(String.Empty) }));

            }
            finally
            {
                fs.Close();
            }
        }

        /// <summary>    
        /// Десериализует XML файл в объект заданного типа    
        /// </summary>    
        public static T XmlStrToObj<T>(String fileName)
        {
            Logger.Debug("ObjectSerializer.ObjToXmlStr()::Десериализуем объект из xml файла");
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
    }
}
