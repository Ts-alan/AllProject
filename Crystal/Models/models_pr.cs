using System.ComponentModel;
using System.Web.Mvc;
using MvcApplication.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Crystal.Models
{

    //public class Approaching  //приближающиеся партии (глубина 10)
    //{
    //    public string Kpr { get; set; } //Код изделия
    //    [DisplayName("Наименование")]
    //    public string Napr { get; set; } //Изделие
    //    public string From { get; set; } //с операции
    //    public string To { get; set; } //на операцию
    //    public string Nprt { get; set; } //партия
    //    public string Kpls { get; set; } //незавершёнка
    //    public int Distance { get; set; }
    //}

    public class PartInfo //Сопроводительный лист
    {
        /// <summary>
        /// Номер операции
        /// </summary>
        public string Nop { get; set; }
        /// <summary>
        /// Номер партии(1)
        /// </summary>
        public string Nprt { get; set; }
        /// <summary>
        /// Код операции(2)
        /// </summary>
        public string Kop { get; set; }
        /// <summary>
        /// Название изделия(3)
        /// </summary>
        public string Napr { get; set; }
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
        public string Dath { get; set; }
        /// <summary>
        /// Время запуска(7)
        /// </summary>
        public string Timh { get; set; }
        /// <summary>
        /// Дата завершения(8)
        /// </summary>
        public string Dato { get; set; }
        /// <summary>
        /// Время завершения(9)
        /// </summary>
        public string Timo { get; set; }
        /// <summary>
        /// Кол-во пластин(10)
        /// </summary>
        public string Kpls { get; set; }
        /// <summary>
        /// Брак(11)
        /// </summary>
        public string Brak { get; set; } //при клике выводить брак ?/
        /// <summary>
        /// Оператор(12)
        /// </summary>
        public string Fio { get; set; }
    }

    public class Defects //Брак - всплывающее
    {
        /// <summary>
        /// Номер пластины(1)
        /// </summary>
        public string Npls { get; set; }
        /// <summary>
        /// Причины брака(2)
        /// </summary>
        public string Naprb { get; set; } //Причины брака
    }

    //public class Downtimes //Простои
    //{
    //    /// <summary>
    //    /// Установка(1)
    //    /// </summary>
    //    public string Nobr { get; set; } //Установка // header
    //    //Заполняется сразу же оператором (всё в один столбец засунуть)
    //    /// <summary>
    //    /// Дата начала простоя(2)
    //    /// </summary>
    //    public string Dnp { get; set; }
    //    /// <summary>
    //    /// Время начала простоя(3)
    //    /// </summary>
    //    public string Tnp { get; set; }
    //    /// <summary>
    //    /// Идентификатор оператора начала(4)
    //    /// </summary>
    //    public string Iopn { get; set; }

    //    /// <summary>
    //    /// Причина(5)
    //    /// </summary>
    //    public string Napr { set; get; } //Причина(наименование)
    //    /// <summary>
    //    /// Часы(6)
    //    /// </summary>
    //    public string Hrs { get; set; } //Часы-высчитываются от начальной и конечной даты/времени

    //    //Заполняется оператором последовательно (всё в один столбец засунуть) 
    //    /// <summary>
    //    /// Дата завершения простоя(7)
    //    /// </summary>
    //    public string Dop { get; set; }
    //    /// <summary>
    //    /// Время завершения простоя(8)
    //    /// </summary>
    //    public string Top { get; set; }
    //    /// <summary>
    //    /// Идентификатор оператора завершения(9)
    //    /// </summary>
    //    public string Iopo { get; set; }
    //}

    
    // + доп таблица на той же странице
    public class Reasons //Причины отказа (все)
    {
        public string Napr { get; set; } //Причина (наименование)
        /// <summary>
        ///Кол-во отказов(2)
        /// </summary>
        public string SumN { get; set; }
        /// <summary>
        /// Часы(3)
        /// </summary>
        public string Hrs { set; get; }
        /// <summary>
        /// Коэфицент работы оборудования по заводу
        /// Сумируются коэффиценты по каждой установке и делится на кол-во установок
        /// Нужен один раз в конце таблицы, сделай отдельно в контроллере общёт и в отдельный viewBag
        /// </summary>
        public string KrObr { get; set; }
    }

    //public class NzpUch //НЗП по участкам
    //{
    //    /// <summary>
    //    /// Участок(1)
    //    /// </summary>
    //    public string Kuch { get; set; }
    //    /// <summary>
    //    /// Поступило(2)
    //    /// </summary>
    //    public string Pso { get; set; }
    //    /// <summary>
    //    /// Сдано(3)
    //    /// </summary>
    //    public string Sdo { set; get; }
    //    /// <summary>
    //    /// Брак(4)
    //    /// </summary>
    //    public string Brk { get; set; }
    //    /// <summary>
    //    /// Нзп(5)
    //    /// </summary>
    //    public string Kpls { get; set; }
    //    /// <summary>
    //    /// Проценты(6)
    //    /// </summary>
    //    public string Percs { get; set; }
    //}

    //--------------------------------------------------Циклы------------------------------------------//

    public class PrCode //Код прибора
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
        /// Кол-во годных пластин(3)
        /// </summary>
        public string Kpls { get; set; }
        /// <summary>
        /// Кол-во брака(4)
        /// </summary>
        public string Brk { get; set; }
        /// <summary>
        /// Процент выхода годных(5)
        /// </summary>
        public string Proc { get; set; }
        /// <summary>
        /// Начальная операция(6)
        /// </summary>
        public string Nop1 { get; set; }
        /// <summary>
        /// Наименование нач.операции(7)
        /// </summary>
        public string NachOpr { get; set; }
        /// <summary>
        /// Конечная операция(8)
        /// </summary>
        public string Nop2 { get; set; }
        /// <summary>
        /// Наименование кон.операции(9)
        /// </summary>
        public string KonOpr { get; set; }
        /// <summary>
        /// Поизводственный цикл до ф/к(в днях)(10)
        /// </summary>
        public string PrCycl { get; set; }
        /// <summary>
        /// Плановый производственный цикл(11)
        /// </summary>
        public string PlCycl { get; set; }
        /// <summary>
        /// Полный производственный цикл(12)
        /// </summary>
        public string FCycl { get; set; }
    }
    public class Cycle_brak
    {
        /// <summary>
        /// Номер партии
        /// </summary>
        public string Nprt { get; set; }

        /// <summary>
        /// Брак
        /// </summary>
        public string Kpls { get; set; }
        /// <summary>
        /// Наименование брака
        /// </summary>
        public string Napr { get; set; }
        /// <summary>
        /// Операция
        /// </summary>
        public string Nop { get; set; }
        /// <summary>
        /// Дата
        /// </summary>
        public string Datbr { get; set; }
        /// <summary>
        /// Ф.И.О. абонента
        /// </summary>
        public string FIO { get; set; }
    }

    //----------------------------------------------Statisticks---------------------------------------------//

    //public class UchList
    //{
    //    /// <summary>
    //    /// Номер участка
    //    /// </summary>
    //    public string Kuch;
    //    /// <summary>
    //    /// Наименование
    //    /// </summary>
    //    public string Nauch;
    //}

    public class BrakList
    {
        /// <summary>
        /// Код причины брака(1)
        /// </summary>
        public string Kprb;
        /// <summary>
        /// Наименование(2)
        /// </summary>
        public string Naprb;
    }
    //public class GetAnalysis1
    //{
    //    /// <summary>
    //    /// Наименование прибора(1)
    //    /// </summary>
    //    public string Napr;
    //    /// <summary>
    //    /// Номер прибора(2)
    //    /// </summary>
    //    public string Kpr;
    //}

    //public class GetAnalysis2
    //{
    //    /// <summary>
    //    /// Код прибора(1)
    //    /// </summary>
    //    public string Kpr;
    //    /// <summary>
    //    /// Кол-во годных пластин(2)
    //    /// </summary>
    //    public string Kpls;
    //    /// <summary>
    //    /// Дата поступления на операцию(3)
    //    /// </summary>
    //    public string Dato;

    //}
    //public class GetAnalysisDiag
    //{
    //    /// <summary>
    //    /// Дата(1)
    //    /// </summary>
    //    public string Dato;
    //    /// <summary>
    //    /// Кол-во пластин(2)
    //    /// </summary>
    //    public int Kpls;
    //    /// <summary>
    //    /// Коффициет оборачиваемости(3)
    //    /// </summary>
    //    public string Kob;
    //}
    //-------------------------------------------------Test------------------------------------------------//
    public class Graph
    {
        public string PlVal { get; set; }
        public string Date { get; set; }
    }

    public class AreaNzpGraf
    {
        public string Uch { get; set; }
        public string Brk { get; set; }
    }

    //public class ParetoDiag
    //{
    //    /// <summary>
    //    /// Кол-во брака
    //    /// </summary>
    //    public string Brk { get; set; }
    //    /// <summary>
    //    /// Наименование
    //    /// </summary>
    //    public string Naprb { get; set; }
    //}

    public class PrNumber
    {
        public string Number { get; set; }
    }

    public class Post
    {
        public int PostId;
        public string Title;
        public string Message;
        public string Author;
        public DateTime Created;
    }

    public class EqPlotJsonData
    {
        public JsonResult Data;
    }

    public class LogString
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string Message { get; set; }
    }

    public class ShowLogsModel
    {
        public IEnumerable<System.IO.FileInfo> FindedLogFiles;
        public IEnumerable<LogString> LatestLogData;
    }
    //#region Производственный цикл

    ///// <summary>
    ///// Данные о цикле
    ///// </summary>
    //public class TestCycleInfo
    //{

    //    [DisplayName("Код прибора")]
    //    public string kpr { get; set; }

    //    [DisplayName("Наименование прибора")]
    //    public string napr { get; set; }
        
    //    [DisplayName("Кол-во годных пластин")]
    //    public int kpls { get; set; }

    //    [DisplayName("Кол-во бракованных пластин")]
    //    public int brk { get; set; }

    //    [DisplayName("% вых. годных")]
    //    public string proc { get; set; }

    //    [DisplayName("Нач. оп.")]
    //    public string Nop1 { get; set; }

    //}


    ///// <summary>
    ///// Данные о цикле
    ///// </summary>
    //public class CycleInfo
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
    //    /// Кол-во годных пластин(3)
    //    /// </summary>
    //    public string Kpls { get; set; }
    //    /// <summary>
    //    /// Кол-во брака(4)
    //    /// </summary>
    //    public string Brk { get; set; }
    //    /// <summary>
    //    /// Процент выхода годных(5)
    //    /// </summary>
    //    public string Proc { get; set; }
    //    /// <summary>
    //    /// Начальная операция(6)
    //    /// </summary>
    //    public string Nop1 { get; set; }
    //    /// <summary>
    //    /// Наименование нач.операции(7)
    //    /// </summary>
    //    public string NachOpr { get; set; }
    //    /// <summary>
    //    /// Конечная операция(8)
    //    /// </summary>
    //    public string Nop2 { get; set; }
    //    /// <summary>
    //    /// Наименование кон.операции(9)
    //    /// </summary>
    //    public string KonOpr { get; set; }
    //    /// <summary>
    //    /// Поизводственный цикл до ф/к(в днях)(10)
    //    /// </summary>
    //    public string PrCycl { get; set; }
    //    /// <summary>
    //    /// Плановый производственный цикл(11)
    //    /// </summary>
    //    public string PlCycl { get; set; }
    //    /// <summary>
    //    /// Полный производственный цикл(12)
    //    /// </summary>
    //    public string FCycl { get; set; }
    //    /// <summary>
    //    /// Кол-во Ф/Л(12)
    //    /// </summary>
    //    public string Nfl { get; set; }
    //    /// <summary>
    //    /// Кол-во Ф/Л(12)
    //    /// </summary>
    //    public string PrCyclfl { get; set; }
    //    /// <summary>
    //    /// Кол-во Ф/Л(12)
    //    /// </summary>
    //    public string PlCyclfl { get; set; }
    //    /// <summary>
    //    /// EZ(13)
    //    /// </summary>
    //    public decimal Ez { get; set; }
    //    /// <summary>
    //    /// Ex(14)
    //    /// </summary>
    //    public decimal Ex { get; set; }
    //}

    ///// <summary>
    ///// Данные о браке
    ///// </summary>
    //public class CyclesDefectsInfo
    //{
    //    /// <summary>
    //    /// Номер партии(1)
    //    /// </summary>
    //    public string Nprt { get; set; }
    //    /// <summary>
    //    /// Наименование причины брака(2)
    //    /// </summary>
    //    public string Naprb { get; set; }
    //    /// <summary>
    //    /// Кол-во пластин(3)
    //    /// </summary>
    //    public string Kpls { get; set; }
    //    /// <summary>
    //    /// Дата брака(4)
    //    /// </summary>
    //    public string Datbr { get; set; }
    //    /// <summary>
    //    /// Время брака(5)
    //    /// </summary>
    //    public string Timtbr { get; set; }
    //    /// <summary>
    //    /// Номер операции(6)
    //    /// </summary>
    //    public string Nop { get; set; }
    //    /// <summary>
    //    /// Ф.И.О. (7)
    //    /// </summary>
    //    public string Fio { get; set; }

    //    public string Kpr { get; set; }
    //    /// <summary>
    //    /// Наименование прибора
    //    /// </summary>
    //    public string Napr { get; set; }
    //}
    //public class cycle_period
    //{
    //    /// <summary>
    //    /// Код прибора
    //    /// </summary>
    //    public string Kpr { get; set; }
    //    /// <summary>
    //    /// Номер партии
    //    /// </summary>
    //    public string Nprt { get; set; }
    //    /// <summary>
    //    /// Дата запуска 
    //    /// </summary>
    //    public DateTime dath { get; set; }
    //    /// <summary>
    //    /// Дата окончания
    //    /// </summary>
    //    public DateTime dato { get; set; }
    //    /// <summary>
    //    /// Полный произв.цикл
    //    /// </summary>
    //    public string cycle_pln { get; set; }
    //    /// <summary>
    //    /// Средний произв.цикл
    //    /// </summary>
    //    public string cycle_sredn { get; set; }
    //    /// <summary>
    //    /// Кол-во брака
    //    /// </summary>
    //    public string Npls { get; set; }
    //    /// <summary>
    //    /// Кол-во годных
    //    /// </summary>
    //    public string kpls { get; set; }
    //    /// <summary>
    //    /// Наименование прибора
    //    /// </summary>
    //    public string Napr { get; set; }
    //}
    

    //#endregion
}
