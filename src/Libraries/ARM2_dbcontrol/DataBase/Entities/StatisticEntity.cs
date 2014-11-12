using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.DataBase
{
    /// <summary>
    /// Используется для получения статистики ввиде "имя-значение"
    /// </summary>
    public class StatisticEntity
    {
        private String name = String.Empty;
        private Int32 count = Int32.MinValue;

        public StatisticEntity()
        {

        }

        public StatisticEntity(String name, Int32 count)
        {
            this.name = name;
            this.count = count;
        }

        public String Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public Int32 Count
        {
            get { return this.count; }
            set { this.count = value; }
        }
    }
}
