using System;
using System.Collections.Generic;

namespace Crystal.Models
{
    ////-------------------------------------Кол-во передач за месяц/сутки---------------------------------//
    //public class PribKplsForDay
    //{
    //    /// <summary>
    //    /// Код прибора(1)
    //    /// </summary>
    //    public string Kpr { get; set; }
    //    /// <summary>
    //    /// Наименование прибора(2)
    //    /// </summary>
    //    public string Napr { get; set; }
    //    /// <summary>
    //    /// Количество передач за сутки(3)
    //    /// </summary>
    //    public string KplsForDay { get; set; }
    //}
    //public class GangPribKplsForDay
    //{
    //   /// <summary>
    //   /// Номер операции(1)
    //   /// </summary>
    //    public string Nop { get; set; } 
    //    /// <summary>
    //    /// Наименование технолргической операции(2)
    //    /// </summary>
    //    public string Naop { get; set; }
    //    /// <summary>
    //    /// Кол-ва передач по операциям(3)
    //    /// </summary>
    //    public string KplsForNop { get; set; }
    //}

    //public class PribKplsForMonth
    //{
    //    /// <summary>
    //    /// Дата(1)
    //    /// </summary>
    //    public string Date { get; set;}
    //    /// <summary>
    //    /// Кол-во за сутки(2)
    //    /// </summary>
    //    public string KplsForDay { get; set; }
    //    /// <summary>
    //    /// Кол-во за первую смену(3)
    //    /// </summary>
    //    public string KplsForFirstTour { get; set; }
    //    /// <summary>
    //    /// Кол-во за вторую смену(4)
    //    /// </summary>
    //    public string KplsForSecondTour { get; set; }
    //    /// <summary>
    //    /// Кол-во за третюю смену(5)
    //    /// </summary>
    //    public string KplsForThirdTour { get; set; }
    //}
    //public class Temporary
    //{
    //    public System.DateTime Date { get; set; }
    //    public decimal Total3 { get; set; }
    //    public decimal Day { get; set; }
    //    public decimal Total2 { get; set; }
    //}
    ////-------------------------------------Отчёт по приборам за месяц---------------------------------//
    //public class SimplePribList
    //{
    //    /// <summary>
    //    /// Код прибора(1)
    //    /// </summary>
    //    public string Kpr { get; set; }

    //    /// <summary>
    //    /// Наименование прибора(2)
    //    /// </summary>
    //    public string Napr { get; set; }
    //    /// <summary>
    //    /// Незавершёнка(3)
    //    /// </summary>
    //    public decimal Nzp { get; set; }
    //}

    //public class PribList :SimplePribList
    //{

    //}

    //public class MonthReportData
    //{
    //    /// <summary>
    //    /// Номер операции(1)
    //    /// </summary>
    //    public string Nop { get; set; }
    //    /// <summary>
    //    /// Наименование операции(2)
    //    /// </summary>
    //    public string Naop { get; set; }
    //    /// <summary>
    //    /// Поступило(3)
    //    /// </summary>
    //    public string In { get; set; }
    //    /// <summary>
    //    /// Сдано(4)
    //    /// </summary>
    //    public string Out { get; set; }
    //    /// <summary>
    //    /// Брак(5)
    //    /// </summary>
    //    public string Brk { get; set; }
    //    /// <summary>
    //    /// % Выхода(6)
    //    /// </summary>
    //    public string OutPer { get; set; }
    //    /// <summary>
    //    /// НЗПФ(7)
    //    /// </summary>
    //    public string Nzpf { get; set; }
    //}

    //public class MonthReportDataBrak
    //{
    //    /// <summary>
    //    /// Номер партии(1)
    //    /// </summary>
    //    public string Nprt { get; set; }
    //    /// <summary>
    //    /// НЗП(2)
    //    /// </summary>
    //    public string Nzp { get; set; }
    //    /// <summary>
    //    /// Наименование брака(3)
    //    /// </summary>
    //    public string Naprb { get; set; }
    //    /// <summary>
    //    /// Номер пластины(4)
    //    /// </summary>
    //    public IEnumerable<string> Npls { get; set; }
    //    /// <summary>
    //    /// Установка(5)
    //    /// </summary>
    //    public string Ust { get; set; }
    //    /// <summary>
    //    /// Дата(6)
    //    /// </summary>
    //    public string Date { get; set; }
    //}

    //public class MonthReportDataNzp
    //{
    //    /// <summary>
    //    /// Номер партии(1)
    //    /// </summary>
    //    public string Nprt { get; set; }
    //    /// <summary>
    //    /// Дата поступления(2)
    //    /// </summary>
    //    public string Date { get; set; }
    //    /// <summary>
    //    /// Годных(3)
    //    /// </summary>
    //    public string Kpls { get; set; }
    //    /// <summary>
    //    /// Дней на операции(4)
    //    /// </summary>
    //    public string DaysOnOp { get; set; }
    //}
    ////------------------------------------------НЗП по прибору------- ---------------------------------//
    //public class NzpPribList:PribList //Список всех приборов из MPARTT и APARTT за текущее время
    //{
      
    //}
    
    //public class NzpPrib //Незавершёнка по прибору
    //{
    //    /// <summary>
    //    /// Номер операции(1)
    //    /// </summary>
    //    public string Nop;
    //    /// <summary>
    //    /// Наименование операции(2)
    //    /// </summary>
    //    public string Naop;
    //    /// <summary>
    //    /// НЗП
    //    /// </summary>
    //    public int Nzp;

    //    /// <summary>
    //    /// Признак оп. завода 2
    //    /// </summary>
    //    public string Pfl;
    //}

    //public class Par //Раскрываем партии 
    //{   
    //    /// <summary>
    //    /// Код прибора
    //    /// </summary>
    //    public string kpr;
    //    /// <summary>
    //    /// Номер партии(1)
    //    /// </summary>
    //    public string Nprt;
    //    /// <summary>
    //    /// Дата окончания операции(2)
    //    /// </summary>
    //    public string Dato;
    //    /// <summary>
    //    /// Время окончания операции(3)
    //    /// </summary>
    //    public string Timo;
    //    /// <summary>
    //    /// Кол-во годных пластин(4)
    //    /// </summary>
    //    public string Kpls;
    //    /// <summary>
    //    /// Дата формирования(5)
    //    /// </summary>
    //    public string Datf;
    //    /// <summary>
    //    /// Оператор(6)
    //    /// </summary>
    //    public string Abon;
    //    /// <summary>
    //    /// Дата запуска на операцию(7)
    //    /// </summary>
    //    public string Dath;
    //    /// <summary>
    //    /// Время запуска на операцию(8)
    //    /// </summary>
    //    public string Timh;
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public int TimeProc;
    //}
    ////------------------------------------------НЗП по фотолитографии--------------------------------------//
    //public class NzpFLitList //НЗП по фотолитографии - список приборов
    //{
    //    /// <summary>
    //    /// Код прибора(1) --Служебная инфа
    //    /// </summary>
    //    public string Kpr;
    //    /// <summary>
    //    /// Наименование прибора(2)
    //    /// </summary>
    //    public string Napr;
    //    /// <summary>
    //    /// Незавершёнка(3)
    //    /// </summary>
    //    public decimal Nzp;
    //    /// <summary>
    //    /// Код маршрута(4)
    //    /// </summary>
    //    public string Kmk;
    //}

    //public class NzpFLit //НЗП по фотолитографии
    //{
    //    /// <summary> Признаком операции фотолитографии является PRFL=1(в ASSOP)
    //    /// Номер фотолитографии(1)
    //    /// </summary>
    //    public string Nfl;
    //    /// <summary>
    //    /// Код операции(2)
    //    /// </summary>
    //    public string Nop;
    //    /// <summary>
    //    /// Наименование тех.операции(3)
    //    /// </summary>
    //    public string Naop;
    //    /// <summary>
    //    /// Кол-во партий(4)
    //    /// </summary>
    //    public string KolPr;
    //    /// <summary>
    //    /// НЗП(5)
    //    /// </summary>
    //    public string Nzp;
    //    public string Kmk;
    //    public string Kpr;
    //    public string Pfl;
    //}

    //public class Blocks //промежуточный этап-блоки
    //{
    //    public string BlockN;
    //    public string Pfl;//Признак
    //    public string FNop;
    //    public string LNop;
    //}

    //public class FlitPartNzp //Нзп по партиям на блоке фл.
    //{
    //    /// <summary>
    //    /// Номер партии(1)
    //    /// </summary>
    //    public string Nprt;
    //    /// <summary>
    //    /// Номер операции(2)
    //    /// </summary>
    //    public string Nop;
    //    /// <summary>
    //    /// Наименование операции(3)
    //    /// </summary>
    //    public string Naop;
    //    /// <summary>
    //    /// Дата поступления(4)
    //    /// </summary>
    //    public string Dato;
    //    /// <summary>
    //    /// Время поступления(5)
    //    /// </summary>
    //    public string Timo;
    //    /// <summary>
    //    /// Незавершёнка(6)
    //    /// </summary>
    //    public string Nzp;
    //}

    //public class FlitOprNzp //Нзп по операциям на блоке фл.
    //{
    //    /// <summary>
    //    /// Код операции
    //    /// </summary>
    //    public string Nop;
    //    /// <summary>
    //    /// Операция
    //    /// </summary>
    //    public string Naop;
    //    /// <summary>
    //    /// НЗП
    //    /// </summary>
    //    public string Nzp;
    //}

    ////------------------------------------------Отчёт по кристаллам---------------------------------------//
    //public class Crystals
    //{
    //    /// <summary>
    //    /// Код прибора(1)
    //    /// </summary>
    //    public string Kpr;
    //    /// <summary>
    //    /// Наименование прибора(2)
    //    /// </summary>
    //    public string Napr;
    //    /// <summary>
    //    /// Поступило кристалов(3)
    //    /// </summary>
    //    public string Pkrist;
    //    /// <summary>
    //    /// Годных кристалов(4)
    //    /// </summary>
    //    public string Godn;
    //    /// <summary>
    //    /// Процент выхода(5)
    //    /// </summary>
    //    public string ProcVux;
    //}

    //public class CrystalsSecond
    //{
    //    /// <summary>
    //    /// Поступило кристалов(1)
    //    /// </summary>
    //    public string Pkrist;
    //    /// <summary>
    //    /// Годных кристалов(2)
    //    /// </summary>
    //    public string Godn;
    //    /// <summary>
    //    /// Процент выхода(3)
    //    /// </summary>
    //    public string ProcVux;
    //    /// <summary>
    //    /// Оператор(4)
    //    /// </summary>
    //    public string Fio { get; set; }
    //}
    ////--------------------------------------------Отчёты-брак-за месяц----------------------------------------------------------//
    //public class DefectsDistributionPrib : NzpPribList
    //{

    //}

    //public class DefectsDistributionOperations
    //{
    //    /// <summary>
    //    /// Номер операции(1)
    //    /// </summary>
    //    public string Nop;
    //    /// <summary>
    //    /// Наименование операции(2)
    //    /// </summary>
    //    public string Naop;
    //    /// <summary>
    //    /// Кол-во брака(3)
    //    /// </summary>
    //    public string Nzp;
    //}

    //public class DefDistrData
    //{
    //    /// <summary>
    //    /// Номер партии(1)
    //    /// </summary>
    //    public string Nprt;
    //    /// <summary>
    //    /// Пластина(2)
    //    /// </summary>
    //    public string Npls;
    //    /// <summary>
    //    /// Причина брака(3)
    //    /// </summary>
    //    public string Naprb;
    //    /// <summary>
    //    /// Дата брака(4)
    //    /// </summary>
    //    public string Datbr;
    //}
    ////----------------------------------------Отчёт за день/смену------------------------------------------//
    //public class UchSelect
    //{
    //    /// <summary>
    //    /// Код участка(1)
    //    /// </summary>
    //    public string Kuch;
    //    /// <summary>
    //    /// Участок(2)
    //    /// </summary>
    //    public string Nauch;
    //}

    //public class ReportsDay
    //{
    //    /// <summary>
    //    /// Поступило(1)
    //    /// </summary>
    //    public string Income;
    //    /// <summary>
    //    /// Сдано(2)
    //    /// </summary>
    //    public string Processed;
    //    /// <summary>
    //    /// Незавершёнка(3)
    //    /// </summary>
    //    public string Nzp;
    //    /// <summary>
    //    /// Коэфицент передачи(4)
    //    /// </summary>
    //    public string TrCof;
    //}

    //public class ReportsDayAll : ReportsDay
    //{
    //    /// <summary>
    //    /// Название участка
    //    /// </summary>
    //    public string Nuch;
    //}

    //public class ReportShift : ReportsDay
    //{
    //    /// <summary>
    //    /// Смена
    //    /// </summary>
    //    public string Shift;
    //}

    //public class ReportShiftAll : ReportShift
    //{
    //    /// <summary>
    //    /// Название участка
    //    /// </summary>
    //    public string Nuch;
    //}
}