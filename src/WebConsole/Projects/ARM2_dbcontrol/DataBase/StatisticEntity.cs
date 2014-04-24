using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.Vba32CC.DataBase
{
    /// <summary>
    /// Используется для получения статистики ввиде "имя-значение"
    /// </summary>
    public class StatisticEntity
    {
        private string name = String.Empty;
        private int count = Int32.MinValue;

        public StatisticEntity()
        {

        }

        public StatisticEntity(string name, int count)
        {
            this.name = name;
            this.count = count;
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public int Count
        {
            get { return this.count; }
            set { this.count = value; }
        }
    }
}
