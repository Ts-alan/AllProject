using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Crystal.DomainModel
{
    //public enum Plants { plant43 = 3, plant47 = 4, plant12 = 12, plant20 = 20, plant_none };
    

    public class BrakInfo
    {
        /// <summary>
        /// Номер пластины
        /// </summary>
        public string Npls { get; set; }

        /// <summary>
        /// Наименование причины брака
        /// </summary>
        public string Naprb { get; set; }

        /// <summary>
        /// Дата брака
        /// </summary>
        public DateTime Datbr { get; set; }

        /// <summary>
        /// Время брака
        /// </summary>
        public string Timbr { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Fio { get; set; }
         
    }

    public class ComplData
    {

        /// <summary>
        /// Количество пластин в разукомплектовывае¬мой партии
        /// </summary>

        public int Kpls { get; set; }

        /// <summary>
        /// Номер разукомплектованной партии
        /// </summary>

        public string Nprtr { get; set; }

        /// <summary>
        /// Количество пластин в разукомплектованной партии
        /// </summary>

        public int Kplr { get; set; }

        /// <summary>
        /// Признак разукомплектования 
        /// </summary>

        public string Pcompl { get; set; }

        /// <summary>
        /// Дата разукомплектования
        /// </summary>

        public DateTime? Datr { get; set; }

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

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy - HH:mm}")]
        public DateTime? Dath { get; set; }

        /// <summary>
        /// Дата завершения(8)
        /// </summary>

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy - HH:mm}")]
        public DateTime? Dato { get; set; }

        /// <summary>
        /// Кол-во пластин(10)
        /// </summary>

        public int Kpls { get; set; }
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
        /// Массив брака
        /// </summary>
        public BrakInfo[] BrakInfo { get; set; }
        /// <summary>
        /// Сумма разукомплектованных пластин
        /// </summary>
        public double SumKplsR { get; set; }
        /// <summary>
        /// Сумма доукомплектованных пластин
        /// </summary>
        public double SumKplsD { get; set; }
    }

    public class BatchInfo //Сопроводительный лист
    {  
        /// <summary>
        /// Признак архивности талиц
        /// </summary>
        public bool Archived { get; set; }
        /// <summary>
        /// Номер партии
        /// </summary>

        public string Nprt { get; set; }
        /// <summary>
        /// Название изделия
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

    //--------------------------------------------Поиск по партии------------------------------------------//

    public class PrtToSearch
    {
        [Required]
        [Display(Name = "Номер партии:")]
        [StringLength(12, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 4)]
        public string Nprt { get; set; }
    }

    public class PartSearch : PrtToSearch
    {
        /// <summary>
        /// Номер партии(1)
        /// </summary>
        //public string Nprt { get; set; }
        /// <summary>
        /// Код прибора(2)
        /// </summary>
        public string Kpr { get; set; }
        /// <summary>
        /// Наименование прибора(3)
        /// </summary>
        public string Napr { get; set; }
        /// <summary>
        /// Номер операции(4)
        /// </summary>
        public string Nop { get; set; }
        /// <summary>
        /// Наименование операции(5)
        /// </summary>
        public string Naop { get; set; }
        /// <summary>
        /// Кол-во пластин(6)
        /// </summary>
        public string Kpls { get; set; }
        /// <summary>
        /// Дата поступления(7)
        /// </summary>
        public string Dato { get; set; }
        /// <summary>
        /// Время поступления(8)
        /// </summary>
        public string Timo { get; set; }
        /// <summary>
        /// Дата запуска(9)
        /// </summary>
        public string Dath { get; set; }
        /// <summary>
        /// Время запуска(10)
        /// </summary>
        public string Timh { get; set; }
        /// <summary>
        /// Наименование оборудования(11)
        /// </summary>
        public string Nobr { get; set; }

        /// <summary>
        /// Наименование коридора(12)
        /// </summary>
        public string Nakor { get; set; }
    }

    public class CrystalDomainModel
    {


    }

    public class ProductionMap
    {
        /// <summary>
        /// ASKOR
        /// </summary>
        public class Corridor
        {
            /// <summary>
            /// kkor
            /// </summary>
            public string Code { get; set; }
            /// <summary>
            /// nakor
            /// </summary>
            public string Name { get; set; }

            public Equipment[] equipments { get; set; }
        }

        public Corridor[] corridors { get; set; }

        public int MalfunctionCount { get; set; } 

    }

    /// <summary>
    /// ASTOB
    /// </summary>
    public abstract class EquipmentBase
    {
        /// <summary>
        /// kgt
        /// </summary>
        public string TechGroupCode { get; set; }

        /// <summary>
        /// kus
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// snaobor
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// ks
        /// </summary>
        public string StateCode { get; set; }

        public string Id { get { return TechGroupCode + '|' + Code; } }

        /// <summary>
        /// assufl.nuf
        /// </summary>
        public string PhotoLitCode { get; set; }
    }

    
    public class Equipment : EquipmentBase
    {
        public class BatchesInfo {
            public int Nzp { get; set; }
            public int Wait { get; set; }
        }

        public BatchesInfo SumInfo { get; set; }

    }


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

    
    //Модель данных используемая для вывода на вьюшку
    public class TransistorViewModel
    {
        // Номер производства
        public string PlantNumber { get; set; }
        public AllNzp[] AllNzps { get; set; }
        //public IEnumerable<Corridor> Corridors { get; set; }
        public ProductionMap ModuleMap { get; set; }
        public bool ShowProd2Data { get {return (this.PlantNumber == "3" || this.PlantNumber == "4");} }
    }

    //-------------------------------------Кол-во передач за месяц/сутки---------------------------------//
    public class PribKplsForDay
    {
        /// <summary>
        /// Код прибора(1)
        /// </summary>
        public string Kpr { get; set; }
        /// <summary>
        /// Наименование прибора(2)
        /// </summary>
        public string Napr { get; set; }
        /// <summary>
        /// Количество передач за сутки(3)
        /// </summary>
        public string KplsForDay { get; set; }
    }
    public class GangPribKplsForDay
    {
        /// <summary>
        /// Номер операции(1)
        /// </summary>
        public string Nop { get; set; }
        /// <summary>
        /// Наименование технолргической операции(2)
        /// </summary>
        public string Naop { get; set; }
        /// <summary>
        /// Кол-ва передач по операциям(3)
        /// </summary>
        public string KplsForNop { get; set; }
    }

    public class PribKplsForMonth
    {
        /// <summary>
        /// Дата
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime Date { get; set; }
        /// <summary>
        /// Кол-во за сутки(2)
        /// </summary>
        public string KplsForDay { get; set; }
        /// <summary>
        /// Кол-во за первую смену(3)
        /// </summary>
        public string KplsForFirstTour { get; set; }
        /// <summary>
        /// Кол-во за вторую смену(4)
        /// </summary>
        public string KplsForSecondTour { get; set; }
        /// <summary>
        /// Кол-во за третюю смену(5)
        /// </summary>
        public string KplsForThirdTour { get; set; }
    }
    public class Temporary
    {
        public System.DateTime Date { get; set; }
        public double Total3 { get; set; }
        public double Day { get; set; }
        public double Total2 { get; set; }
    }
    //-------------------------------------Отчёт по приборам за месяц---------------------------------//
    
}
