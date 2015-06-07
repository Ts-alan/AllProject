using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Crystal.DomainModel
{
    #region Производственный цикл

    /// <summary>
    /// Данные о цикле
    /// </summary>
    public class TestCycleInfo
    {

        [DisplayName("Код прибора")]
        public string kpr { get; set; }

        [DisplayName("Наименование прибора")]
        public string napr { get; set; }

        [DisplayName("Кол-во годных пластин")]
        public int kpls { get; set; }

        [DisplayName("Кол-во бракованных пластин")]
        public int brk { get; set; }

        [DisplayName("% вых. годных")]
        public string proc { get; set; }

        [DisplayName("Нач. оп.")]
        public string Nop1 { get; set; }

    }


    /// <summary>
    /// Данные о цикле
    /// </summary>
    public class CycleInfo
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
        /// <summary>
        /// Кол-во Ф/Л(12)
        /// </summary>
        public string Nfl { get; set; }
        /// <summary>
        /// Кол-во Ф/Л(12)
        /// </summary>
        public string PrCyclfl { get; set; }
        /// <summary>
        /// Кол-во Ф/Л(12)
        /// </summary>
        public string PlCyclfl { get; set; }
        /// <summary>
        /// EZ(13)
        /// </summary>
        public decimal Ez { get; set; }
        /// <summary>
        /// Ex(14)
        /// </summary>
        public decimal Ex { get; set; }
    }

    /// <summary>
    /// Данные о браке
    /// </summary>
    public class CyclesDefectsInfo
    {
        /// <summary>
        /// Номер партии(1)
        /// </summary>
        public string Nprt { get; set; }
        /// <summary>
        /// Наименование причины брака(2)
        /// </summary>
        public string Naprb { get; set; }
        /// <summary>
        /// Кол-во пластин(3)
        /// </summary>
        public string Kpls { get; set; }
        /// <summary>
        /// Дата брака
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy - HH:mm}")]
        public DateTime? DefectDate { get; set; }
        /// <summary>
        /// Номер операции(6)
        /// </summary>
        public string Nop { get; set; }
        /// <summary>
        /// Ф.И.О. (7)
        /// </summary>
        public string Fio { get; set; }

        public string Kpr { get; set; }
        /// <summary>
        /// Наименование прибора
        /// </summary>
        public string Napr { get; set; }
    }
    public class cycle_period
    {
        /// <summary>
        /// Код прибора
        /// </summary>
        public string Kpr { get; set; }
        /// <summary>
        /// Номер партии
        /// </summary>
        public string Nprt { get; set; }
        /// <summary>
        /// Дата запуска 
        /// </summary>
        public DateTime dath { get; set; }
        /// <summary>
        /// Дата окончания
        /// </summary>
        public DateTime dato { get; set; }
        /// <summary>
        /// Полный произв.цикл
        /// </summary>
        public string cycle_pln { get; set; }
        /// <summary>
        /// Средний произв.цикл
        /// </summary>
        public string cycle_sredn { get; set; }
        /// <summary>
        /// Кол-во брака
        /// </summary>
        public string Npls { get; set; }
        /// <summary>
        /// Кол-во годных
        /// </summary>
        public string kpls { get; set; }
        /// <summary>
        /// Наименование прибора
        /// </summary>
        public string Napr { get; set; }
    }


    #endregion
}
