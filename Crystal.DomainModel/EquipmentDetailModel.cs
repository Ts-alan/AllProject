using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Crystal.DomainModel
{
    public class EquipmentDetailModel : EquipmentBase
    {
        public BatchForLaunch[] BatchesForLaunch { get; set; }
        public BatchOnOperation[] BatchesOnOperation { get; set; }
    }

    public abstract class BatchOnEquipment
    {
        /// <summary>
        /// Номер партии
        /// NPRT
        /// </summary>
        public string BatchNumber { get; set; }
        /// <summary>
        /// Код изделия
        /// KPR
        /// </summary>
        public string DeviceCode { get; set; } 
        /// <summary>
        /// Наименование изделия
        /// NAPR
        /// </summary>
        public string DeviceName { get; set; } 
        /// <summary>
        /// Номер операции
        /// NOP
        /// </summary>
        public string OperationNumber { get; set; } 
        /// <summary>
        /// Наименование операции
        /// NAOP
        /// </summary>
        public string OperationName { get; set; } 
        /// <summary>
        /// Кол-во пластин
        /// KPLS
        /// </summary>
        public int PlateCount { get; set; } 
    }
    
    public class BatchOnOperation : BatchOnEquipment //на операции
    {
        /// <summary>
        /// Дата-время запуска
        /// DATH
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy - HH:mm}")]
        public DateTime LaunchDate { get; set; } 
        public string OperatorFullName { get; set; } //Оператор
    }

    public class BatchForLaunch : BatchOnEquipment // На запуск
    {
        /// <summary>
        /// Дата-время передачи на операцию
        /// DATO
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy - HH:mm}")]
        public DateTime ArriveDate { get; set; } 
    }

    public class Downtimes //Простои
    {
        /// <summary>
        /// Установка(1)
        /// </summary>
        public string Nobr { get; set; } //Установка // header
        //Заполняется сразу же оператором (всё в один столбец засунуть)
        /// <summary>
        /// Дата начала простоя(2)
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy - HH:mm}")]
        public DateTime? Dnp { get; set; }
        /// <summary>
        /// Идентификатор оператора начала(4)
        /// </summary>
        public string Iopn { get; set; }

        /// <summary>
        /// Причина(5)
        /// </summary>
        public string Napr { set; get; } //Причина(наименование)
        /// <summary>
        /// Часы(6)
        /// </summary>
        public string Hrs { get; set; } //Часы-высчитываются от начальной и конечной даты/времени

        
        /// <summary>
        /// Дата завершения простоя(7)
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy - HH:mm}")]
        public DateTime? Dop { get; set; }

        /// <summary>
        /// Идентификатор оператора завершения(9)
        /// </summary>
        public string Iopo { get; set; }
    }

    public class Approaching  //приближающиеся партии (глубина 10)
    {
        public string Kpr { get; set; } //Код изделия
        [DisplayName("Наименование")]
        public string Napr { get; set; } //Изделие
        public string From { get; set; } //с операции
        public string To { get; set; } //на операцию
        public string Nprt { get; set; } //партия
        public string Kpls { get; set; } //незавершёнка
        public int Distance { get; set; }
    }

    public class LdPlot
    {
        public string Kgt { get; set; }//for sorting
        public string Ust { get; set; }
        public decimal Kpls { get; set; }
    }
    public class LoadMth //Месячная загрузка    
    {
        /// <summary>
        /// Номер бригады
        /// </summary>
        public string Numb { get; set; }
        /// <summary>
        /// Участок(1)
        /// </summary>
        public string Kuch { get; set; }
        /// <summary>
        /// Оборудование(2)
        /// </summary>
        public string Nobr { get; set; }
        /// <summary>
        /// Пластины(3)
        /// </summary>
        public string Kpls { set; get; }
        /// <summary>
        /// Брак(4)
        /// </summary>
        public string Brk { get; set; }
        /// <summary>
        /// Партии(5)
        /// </summary>
        public string Nprt { get; set; }
        /// <summary>
        /// Ремонт(в часах)(6)
        /// </summary>
        public string RepH { get; set; }
        /// <summary>
        /// Коэфицент работы оборудования(7)
        /// Фонд раб.времени за месяц - простои за месяц/фонд рабочего времени
        /// </summary>
        public double KrObr { get; set; }
    }


    public class NzpUch //НЗП по участкам
    {
        /// <summary>
        /// Участок(1)
        /// </summary>
        public string Kuch { get; set; }
        /// <summary>
        /// Поступило(2)
        /// </summary>
        public string Pso { get; set; }
        /// <summary>
        /// Сдано(3)
        /// </summary>
        public string Sdo { set; get; }
        /// <summary>
        /// Брак(4)
        /// </summary>
        public string Brk { get; set; }
        /// <summary>
        /// Нзп(5)
        /// </summary>
        public string Kpls { get; set; }
        /// <summary>
        /// Проценты(6)
        /// </summary>
        public string Percs { get; set; }
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
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy - HH:mm}")]
        public DateTime? Dnp { get; set; }
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
