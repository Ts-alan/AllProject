using System.Collections.Generic;
using System.ComponentModel;

namespace Verst.Models
{
    public class AllNzp
    {
        //признак на весь 2 завод - 5 в справочнике assop
        /// <summary>
        /// Нзп Начало месяца(1)
        /// </summary>
        public string Nzp;
        /// <summary>
        /// Нзп Начало месяца пр20(2)
        /// </summary>
        public string NzpPr2;
        /// <summary>
        /// Сформировано всего(3)
        /// </summary>
        public string Sform;
        /// <summary>
        /// НЗП текущее(4)
        /// </summary>
        public string NowNzp;
        /// <summary>
        /// Незавершёнка текущая завода №2(5)
        /// </summary>
        public string NowNzpPr2;
        /// <summary>
        /// Брак(6)
        /// </summary>
        public string Kpls;
        /// <summary>
        /// Брак завода №2(7)
        /// </summary>
        public string KplsPr2;
        /// <summary>
        /// Готовая продукция/Сдано(8)
        /// </summary>
        public string Sdo;
        /// <summary>
        /// Количество передач за сутки(9)
        /// </summary>
        public string KplsForMount;
        /// <summary>
        /// Количество передач за месяц(10)
        /// </summary>
        public string KplsForDay;
    }

    public class ModuleMap
    {
        [DisplayName("Коридор")]
        public string Kkor { get; set; }
        public string Nakor { get; set; }
        public string Kus { get; set; }
        public string Snaobor { get; set; }
        public bool isBusy { get; set; }

        public IEnumerable<FacilityData> obor { get; set; }
    }

    public class FacilityData
    {
        public Crystal.astobRow astob_data { get; set; }
        public decimal Nzp { get; set; }
        public decimal Wait { get; set; }
    }

    //Модель данных используемая для вывода на вьюшку
    public class TransistorViewModel
    {
        // Номер производства
        public string Prn { get; set; }
        public IEnumerable<AllNzp> AllNzps { get; set; }
        public IEnumerable<ModuleMap> ModuleMaps { get; set; }
    }
}