using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Crystal.DomainModel;

namespace Crystal.Models
{

    //--------------------------------------------AreaNzp Model--------------------------------------------//
    public class AreaNzpModel
    {
        public string Nprt { get; set; }
        public IEnumerable<NzpUch> OnLaunch { get; set; }
    }
    
    //public class StandEquipment
    //{
    //    /// <summary>
    //    /// Наименование коридора(1)
    //    /// </summary>
    //    public string Nakor { get; set; }
    //    /// <summary>
    //    /// Наименование участка(2)
    //    /// </summary>
    //    public string Nauch { get; set; }
    //    /// <summary>
    //    /// Сокращенное наименование оборудования(3)
    //    /// </summary>
    //    public string Snaobor { get; set; }
    //    /// <summary>
    //    /// Дата начала простоя(4)
    //    /// </summary>
    //    public string Dnp { get; set;}
    //    /// <summary>
    //    /// Время начала простоя(5)
    //    /// </summary>
    //    public string Tnp { get; set; }
    //    /// <summary>
    //    /// Ф.И.О оператора(6)
    //    /// </summary>
    //    public string Fio { get; set; }
    //    /// <summary>
    //    /// Часы простоя(7)
    //    /// </summary>
    //    public string StandHour { get; set; }
    //    /// <summary>
    //    /// Часы простоя за месяц(8)
    //    /// </summary>
    //    public string StandHourMount { get; set; }
    //    /// <summary>
    //    /// Количество простоя(9)
    //    /// </summary>
    //    public string StandCount { get; set; }
    //    /// <summary>
    //    /// Коэф.работы оборудования
    //    /// </summary>
    //     public string KrObr { get; set; }
    //    public string Kgt { get; set; }
    //    public string Kus { get; set; }
    //}

    //public class StandTemp
    //{
    //    public string kgt { get; set; }
    //    public string kus { get; set; }
    //    public string kpp { get; set; }
    //    public double hours { get; set; }
    //}
}