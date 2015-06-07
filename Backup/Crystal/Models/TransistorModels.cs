using Crystal.DomainModel;
using CrystalDataSetLib;
using System.Collections.Generic;
using System.ComponentModel;

namespace Crystal.Models
{
    public class AllNzp
    {
        //признак на весь 2 завод - 5 в справочнике assop
        /// <summary>
        /// Нзп Начало месяца(1)
        /// </summary>
        public int Nzp;
        /// <summary>
        /// Нзп Начало месяца пр20(2)
        /// </summary>
        public int NzpPr2;
        /// <summary>
        /// Сформировано всего(3)
        /// </summary>
        public int Sform;
        /// <summary>
        /// НЗП текущее(4)
        /// </summary>
        public int NowNzp;
        /// <summary>
        /// Незавершёнка текущая завода №2(5)
        /// </summary>
        public int NowNzpPr2;
        /// <summary>
        /// Брак(6)
        /// </summary>
        public int Kpls;
        /// <summary>
        /// Брак завода №2(7)
        /// </summary>
        public int KplsPr2;
        /// <summary>
        /// Готовая продукция/Сдано(8)
        /// </summary>
        public int Sdo;
        /// <summary>
        /// Количество передач за сутки(9)
        /// </summary>
        public int KplsForMount;
        /// <summary>
        /// Количество передач за месяц(10)
        /// </summary>
        public int KplsForDay;
    }

    public class Corridor
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
        public CrystalDS.astobRow astob_data { get; set; }
        public decimal Nzp { get; set; }
        public decimal Wait { get; set; }
    }

    //Модель данных используемая для вывода на вьюшку
    public class TransistorViewModel
    {
        // Номер производства
        public string PlantNumber { get; set; }
        public IEnumerable<AllNzp> AllNzps { get; set; }
        //public IEnumerable<Corridor> Corridors { get; set; }
        public ProductionMap ModuleMaps { get; set; }
    }
}