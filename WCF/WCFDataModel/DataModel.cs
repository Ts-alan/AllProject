using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CrystalDataSetLib;
namespace ClassLibrary1
{
    public class ComplData
    {

        /// <summary>
        /// Количество пластин в разукомплектовывае¬мой партии
        /// </summary>

        public decimal Kpls { get; set; }

        /// <summary>
        /// Номер разукомплектованной партии
        /// </summary>

        public string Nprtr { get; set; }

        /// <summary>
        /// Количество пластин в разукомплектованной партии
        /// </summary>

        public decimal Kplr { get; set; }

        /// <summary>
        /// Признак разукомплектования 
        /// </summary>

        public string Pcompl { get; set; }

        /// <summary>
        /// Дата разукомплектования
        /// </summary>

        public DateTime Datr { get; set; }

        /// <summary>
        /// Фамилия И.О. абонента
        /// </summary>

        public string Fio { get; set; }
    }

    public class PartRouteData
    {
        /// <summary>
        /// Ошибка
        /// </summary>
        public string Ex { get; set; }
        /// <summary>
        /// Индификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Номер операции
        /// </summary>

        public string Nop { get; set; }
        /// <summary>
        /// Код операции(2)
        /// </summary>

        public string Kop { get; set; }

        /// <summary>
        /// Операция(4)
        /// </summary>

        public string Naop { get; set; }
        /// <summary>
        /// Оборудование(5)
        /// </summary>

        public string SNaobor { get; set; }
        /// <summary>
        /// Дата запуска(6)
        /// </summary>

        public DateTime Dath { get; set; }

        /// <summary>
        /// Дата завершения(8)
        /// </summary>

        public DateTime Dato { get; set; }

        /// <summary>
        /// Кол-во пластин(10)
        /// </summary>

        public decimal Kpls { get; set; }
        /// <summary>
        /// Брак(11)
        /// </summary>

        public int Brak { get; set; } //при клике выводить брак ?/
        /// <summary>
        /// Оператор(12)
        /// </summary>

        public string Fio { get; set; }

        /// <summary>
        /// Массив разукомплектованных партий
        /// </summary>
        public ComplData[] ComplData { get; set; }
        /// <summary>
        /// Сумма разукомплектованных пластин
        /// </summary>
        public decimal SumKplsR { get; set; }
        /// <summary>
        /// Сумма доукомплектованных пластин
        /// </summary>
        public decimal SumKplsD { get; set; }
    }

    public class BatchInfo //Сопроводительный лист
    {

        /// <summary>
        /// Номер партии(1)
        /// </summary>

        public string Nprt { get; set; }
        /// <summary>
        /// Название изделия(3)
        /// </summary>

        public string Napr { get; set; }
        /// <summary>
        /// Дней в маршруте
        /// </summary>
        public int DaysInRoute { get; set; }
        /// <summary>
        /// Информация о партии
        /// </summary>
        public PartRouteData[] RouteData { get; set; }


    }
  
   
    public class Prod2Data
    {
        public decimal Nzp { get; set; }
        public decimal NzpCurrent { get; set; }
        public decimal Defects { get; set; }
    }



   
    #region----------------------TransistorModels-------------------
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
        public IEnumerable<ModuleMap> ModuleMaps { get; set; }
    }



    #endregion
}
