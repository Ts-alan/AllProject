using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Crystal.DomainModel
{
    #region Отчеты по приборам и фотолитографии  + раскрытие партии

    public class SimplePribList
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
        /// Незавершёнка(3)
        /// </summary>
        public int Nzp { get; set; }
        /// <summary>
        /// Диаметр(4)
        /// </summary>
        public string Diameter { get; set; }
    }




    public  class NzpBatchInfo //Раскрываем партии 
    {
        /// <summary>
        /// Код прибора
        /// </summary>
        public string kpr;
        /// <summary>
        /// Номер партии(1)
        /// </summary>
        public string Nprt;
        /// <summary>
        /// Дата окончания операции(2)
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy - HH:mm}")]
        public DateTime? Dato{ get; set; }
        /// <summary>
        /// Время окончания операции(3)
        /// </summary>
        public string Timo;
        /// <summary>
        /// Кол-во годных пластин(4)
        /// </summary>
        public string Kpls;
        /// <summary>
        /// Дата формирования(5)
        /// </summary>
        public string Datf;
        /// <summary>
        /// Оператор(6)
        /// </summary>
        public string Abon;
        /// <summary>
        /// Дата запуска на операцию(7)
        /// </summary>
        public DateTime Dath;

        /// <summary>
        /// Кол-во суток на операции
        /// </summary>
        public int TimeProc;
    }

    public class PribList : SimplePribList
    {

    }

    //------------------------------------------НЗП по прибору------- ---------------------------------//
    public class NzpPribList : PribList //Список всех приборов из MPARTT и AMPARTT за текущее время
    {
       // public string Diameter { get; set; }
    }

    public class NzpPrib //Незавершёнка по прибору
    {
        /// <summary>
        /// Номер операции(1)
        /// </summary>
        public string Nop;
        /// <summary>
        /// Наименование операции(2)
        /// </summary>
        public string Naop;
        /// <summary>
        /// НЗП
        /// </summary>
        public int Nzp;

        /// <summary>
        /// Признак оп. завода 2
        /// </summary>
        public string Pfl;
    }


    //------------------------------------------НЗП по фотолитографии--------------------------------------//
    public class NzpFLitList : PribList //НЗП по фотолитографии - список приборов
    {
        /// <summary>
        /// Код прибора(1) --Служебная инфа
        /// </summary>
        public string Kpr;
        /// <summary>
        /// Наименование прибора(2)
        /// </summary>
        public string Napr;
        /// <summary>
        /// Незавершёнка(3)
        /// </summary>
        public int Nzp;
        /// <summary>
        /// Код маршрута(4)
        /// </summary>
        public string Kmk;
    }

    public class NzpFLit //НЗП по фотолитографии
    {
        /// <summary>
        /// Код прибора(1) --Служебная инфа
        /// </summary>
        public string Kpr;
        /// <summary> Признаком операции фотолитографии является PRFL=1(в ASSOP)
        /// Номер фотолитографии(1)
        /// </summary>
        public string Nfl;
        /// <summary>
        /// Код операции(2)
        /// </summary>
        public string Nop;
        /// <summary>
        /// Наименование тех.операции(3)
        /// </summary>
        public string Naop;
        /// <summary>
        /// Кол-во партий(4)
        /// </summary>
        public string KolPr;
        /// <summary>
        /// НЗП(5)
        /// </summary>
        public int Nzp;
        public string Kmk;
        public string Pfl;
    }

    public class Blocks //промежуточный этап-блоки
    {
        public string BlockN;
        public string Pfl;//Признак
        public string FNop;
        public string LNop;
    }

    public class FlitPartNzp //Нзп по партиям на блоке фл.
    {
        /// <summary>
        /// Код прибора(1) --Служебная инфа
        /// </summary>
        public string Kpr;
        /// <summary>
        /// Номер партии(1)
        /// </summary>
        public string Nprt;
        /// <summary>
        /// Номер операции(2)
        /// </summary>
        public string Nop;
        /// <summary>
        /// Наименование операции(3)
        /// </summary>
        public string Naop;
        /// <summary>
        /// Дата поступления(4)
        /// </summary>
        public DateTime? Dato;
        /// <summary>
        /// Время поступления(5)
        /// </summary>
        public string Timo;
        /// <summary>
        /// Незавершёнка(6)
        /// </summary>
        public int Nzp;
    }

    public class FlitOprNzp //Нзп по операциям на блоке фл.
    {

        /// <summary>
        /// Код прибора(1) --Служебная инфа
        /// </summary>
        public string Kpr;
        /// <summary>
        /// Код операции
        /// </summary>
        public string Nop;
        /// <summary>
        /// Операция
        /// </summary>
        public string Naop;
        /// <summary>
        /// НЗП
        /// </summary>
        public int Nzp;
    }



    #endregion
    //------------------------------------------Отчёт по кристаллам---------------------------------------//
    public class Crystals
    {
        /// <summary>
        /// Код прибора(1)
        /// </summary>
        public string Kpr;
        /// <summary>
        /// Наименование прибора(2)
        /// </summary>
        public string Napr;
        /// <summary>
        /// Поступило кристалов(3)
        /// </summary>
        public string Pkrist;
        /// <summary>
        /// Годных кристалов(4)
        /// </summary>
        public string Godn;
        /// <summary>
        /// Процент выхода(5)
        /// </summary>
        public string ProcVux;
    }

    public class CrystalsSecond
    {
        /// <summary>
        /// Поступило кристалов(1)
        /// </summary>
        public string Pkrist;
        /// <summary>
        /// Годных кристалов(2)
        /// </summary>
        public string Godn;
        /// <summary>
        /// Процент выхода(3)
        /// </summary>
        public string ProcVux;
        /// <summary>
        /// Оператор(4)
        /// </summary>
        public string Fio { get; set; }
    }
    //--------------------------------------------Отчёты-брак-за месяц----------------------------------------------------------//
    public class DefectsDistributionPrib : NzpPribList
    {

    }

    public class DefectsDistributionOperations
    {
        /// <summary>
        /// Номер операции(1)
        /// </summary>
        public string Nop;
        /// <summary>
        /// Наименование операции(2)
        /// </summary>
        public string Naop;
        /// <summary>
        /// Кол-во брака(3)
        /// </summary>
        public string Nzp;

        /// <summary>
        /// Номер партии
        /// </summary>
        public string Kmk;
    }

    public class DefDistrData
    {
        /// <summary>
        /// Номер партии(1)
        /// </summary>
        public string Nprt;
        /// <summary>
        /// Пластина(2)
        /// </summary>
        public string Npls;
        /// <summary>
        /// Причина брака(3)
        /// </summary>
        public string Naprb;
        /// <summary>
        /// Дата брака(4)
        /// </summary>
        public string Datbr;
    }
    //----------------------------------------Отчёт за день/смену------------------------------------------//
    public class UchSelect
    {
        /// <summary>
        /// Код участка(1)
        /// </summary>
        public string Kuch;
        /// <summary>
        /// Участок(2)
        /// </summary>
        public string Nauch;
    }

    public class ReportsDay
    {
        /// <summary>
        /// Поступило(1)
        /// </summary>
        public string Income;
        /// <summary>
        /// Сдано(2)
        /// </summary>
        public string Processed;
        /// <summary>
        /// Незавершёнка(3)
        /// </summary>
        public string Nzp;
        /// <summary>
        /// Коэфицент передачи(4)
        /// </summary>
        public string TrCof;
    }

    public class ReportsDayAll : ReportsDay
    {
        /// <summary>
        /// Название участка
        /// </summary>
        public string Nuch;
    }

    public class ReportShift : ReportsDay
    {
        /// <summary>
        /// Смена
        /// </summary>
        public string Shift;
    }

    public class ReportShiftAll : ReportShift
    {
        /// <summary>
        /// Название участка
        /// </summary>
        public string Nuch;
    }

    public class MonthReportData
    {
        /// <summary>
        /// Номер операции(1)
        /// </summary>
        public string Nop { get; set; }
        /// <summary>
        /// Наименование операции(2)
        /// </summary>
        public string Naop { get; set; }
        /// <summary>
        /// Поступило(3)
        /// </summary>
        public string In { get; set; }
        /// <summary>
        /// Сдано(4)
        /// </summary>
        public string Out { get; set; }
        /// <summary>
        /// Брак(5)
        /// </summary>
        public string Brk { get; set; }
        /// <summary>
        /// % Выхода(6)
        /// </summary>
        public string OutPer { get; set; }
        /// <summary>
        /// НЗПФ(7)
        /// </summary>
        public string Nzpf { get; set; }
    }

    public class MonthReportDataBrak
    {
        /// <summary>
        /// Номер партии(1)
        /// </summary>
        public string Nprt { get; set; }
        /// <summary>
        /// НЗП(2)
        /// </summary>
        public string Nzp { get; set; }
        /// <summary>
        /// Наименование брака(3)
        /// </summary>
        public string Naprb { get; set; }
        /// <summary>
        /// Номер пластины(4)
        /// </summary>
        public string[] Npls { get; set; }
        /// <summary>
        /// Установка(5)
        /// </summary>
        public string Ust { get; set; }
        /// <summary>
        /// Дата(6)
        /// </summary>
        public string Date { get; set; }
    }

    public class MonthReportDataNzp
    {
        /// <summary>
        /// Номер партии(1)
        /// </summary>
        public string Nprt { get; set; }
        /// <summary>
        /// Дата поступления(2)
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// Годных(3)
        /// </summary>
        public string Kpls { get; set; }
        /// <summary>
        /// Дней на операции(4)
        /// </summary>
        public string DaysOnOp { get; set; }
    }

    #region Отчеты по загрузке оборудования  + раскрытие причин простоев
    public class EquipmentLoad //Загрузка оборудования    
    {
        /// <summary>
        /// KGT+KUS 
        /// </summary>
        public string EquipmentUniqCode { get; set; }
        /// <summary>
        /// Наименование участка (Kuch)
        /// </summary>
        public string AreaName { get; set; }
        /// <summary>
        /// Наименование оборудования (Nobr)
        /// </summary>
        public string EquipmentName { get; set; }
        /// <summary>
        /// Кол-во пластин (Kpls)
        /// </summary>
        public int PlateCount { set; get; }
        /// <summary>
        /// Кол-во брак. пластин
        /// </summary>
        public int BrkCount { get; set; }
        /// <summary>
        /// Кол-во партий
        /// </summary>
        public int BatchCount { get; set; }

        /// <summary>
        /// Ремонт в часах
        /// </summary>
        public double RepairDurationH { get; set; }

        /// <summary>
        /// Коэффициент использования оборудования
        /// </summary>
        public double EquipmentEfficiency { get; set; }


        public class RepairReason
        {
            /// <summary>
            /// Код причины ремонта
            /// </summary>
            public string ReasonCode { get; set; }
            
            /// <summary>
            /// Причина ремонта (наименование)
            /// </summary>
            public string ReasonName { get; set; } 
            /// <summary>
            /// Кол-во ремонтов
            /// </summary>
            public int RepairCount { get; set; }
            /// <summary>
            /// Продолжительность ремонта (в часах)
            /// </summary>
            public double DurationH { set; get; }

            

        }
        
        /// <summary>
        /// Ремонты
        /// </summary>
        public RepairReason[] Repairs { get; set; }
    }
    #endregion
}
