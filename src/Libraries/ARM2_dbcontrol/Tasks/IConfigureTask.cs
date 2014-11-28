using System;

namespace ARM2_dbcontrol.Tasks
{
    public interface IConfigureTask
    {
        /// <summary>
        /// Сохранить в Xml
        /// </summary>
        /// <returns></returns>
        String SaveToXml();
        /// <summary>
        ///  Загрузить из Xml
        /// </summary>
        /// <param name="xml">Xml</param>
        void LoadFromXml(String xml);
        /// <summary>
        ///  Загрузить из реестра
        /// </summary>
        /// <param name="reg"></param>
        void LoadFromRegistry(String reg);
        /// <summary>
        ///  Получить задачу
        /// </summary>
        /// <returns></returns>
        String GetTask();
    }
}
