using System;

namespace ARM2_dbcontrol.Tasks
{
    public interface IConfigureTask
    {
        /// <summary>
        /// ��������� � Xml
        /// </summary>
        /// <returns></returns>
        String SaveToXml();
        /// <summary>
        ///  ��������� �� Xml
        /// </summary>
        /// <param name="xml">Xml</param>
        void LoadFromXml(String xml);
        /// <summary>
        ///  ��������� �� �������
        /// </summary>
        /// <param name="reg"></param>
        void LoadFromRegistry(String reg);
        /// <summary>
        ///  �������� ������
        /// </summary>
        /// <returns></returns>
        String GetTask();
    }
}
