using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Verst.Models
{
    //--------------------------------------------Details Model--------------------------------------------//
    public class DeatilsModel
    {
        public string Kgt { get; set; }
        public string Kus { get; set; }
        public IEnumerable<OnOperation> OnOperations { get; set; }
        public IEnumerable<OnLaunch> OnLaunch { get; set; }
    }
    //--------------------------------------------AreaNzp Model--------------------------------------------//
    public class AreaNzpModel
    {
        public string Nprt { get; set; }
        public IEnumerable<NzpUch> OnLaunch { get; set; }
    }
    //--------------------------------------------Поиск по партии------------------------------------------//

    public class PrtToSearch
    {
        [Required]
        [Display(Name = "Номер партии:")]
        [StringLength(12, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 4)]
        public string Nprt { get; set; }
    }

    public class PartSearch :PrtToSearch
    {
        /// <summary>
        /// Номер партии(1)
        /// </summary>
        public string Nprt { get; set; }
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
    public class StandEquipment
    {
        /// <summary>
        /// Наименование коридора(1)
        /// </summary>
        public string Nakor { get; set; }
        /// <summary>
        /// Наименование участка(2)
        /// </summary>
        public string Nauch { get; set; }
        /// <summary>
        /// Сокращенное наименование оборудования(3)
        /// </summary>
        public string Snaobor { get; set; }
        /// <summary>
        /// Дата начала простоя(4)
        /// </summary>
        public string Dnp { get; set;}
        /// <summary>
        /// Время начала простоя(5)
        /// </summary>
        public string Tnp { get; set; }
        /// <summary>
        /// Ф.И.О оператора(6)
        /// </summary>
        public string Fio { get; set; }
        /// <summary>
        /// Часы простоя(7)
        /// </summary>
        public string StandHour { get; set; }
        /// <summary>
        /// Часы простоя за месяц(8)
        /// </summary>
        public string StandHourMount { get; set; }
        /// <summary>
        /// Количество простоя(9)
        /// </summary>
        public string StandCount { get; set; }
        /// <summary>
        /// Коэф.работы оборудования
        /// </summary>
         public string KrObr { get; set; }
        public string Kgt { get; set; }
        public string Kus { get; set; }
    }

    public class StandTemp
    {
        public string kgt { get; set; }
        public string kus { get; set; }
        public string kpp { get; set; }
        public double hours { get; set; }
    }
}