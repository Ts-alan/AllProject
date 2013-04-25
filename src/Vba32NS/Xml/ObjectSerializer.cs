using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;


namespace Vba32.ControlCenter.NotificationService.Xml
{
    /// <summary>
    /// ����� ��� ������������/�������������� ������
    /// </summary>
    public static class ObjectSerializer
    {

        /// <summary>    
        /// ����������� ������ � XML ����
        /// </summary>    
        public static void ObjToXmlStr(string fileName, object obj)
        {
            //Debug.WriteLine("ObjectSerializer.ObjToXmlStr()::����������� ������ � xml ����");
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
        /// ������������� XML ���� � ������ ��������� ����    
        /// </summary>    
        public static T XmlStrToObj<T>(string fileName)
        {
            //Debug.WriteLine("ObjectSerializer.ObjToXmlStr()::������������� ������ �� xml �����");

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
