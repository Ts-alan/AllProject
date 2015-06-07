using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crystal.DomainModel
{
    public class GetAnalysis1
    {
        /// <summary>
        /// Наименование прибора(1)
        /// </summary>
        public string Napr;
        /// <summary>
        /// Номер прибора(2)
        /// </summary>
        public string Kpr;
    }

    public class GetAnalysis2
    {
        /// <summary>
        /// Код прибора(1)
        /// </summary>
        public string Kpr;
        /// <summary>
        /// Кол-во годных пластин(2)
        /// </summary>
        public string Kpls;
        /// <summary>
        /// Дата поступления на операцию(3)
        /// </summary>
        public string Dato;

    }

    public class GetAnalysisDiag
    {
        /// <summary>
        /// Дата(1)
        /// </summary>
        public string Dato;
        /// <summary>
        /// Кол-во пластин(2)
        /// </summary>
        public int Kpls;
        /// <summary>
        /// Коффициет оборачиваемости(3)
        /// </summary>
        public string Kob;
    }

    public class UchList
    {
        /// <summary>
        /// Номер участка
        /// </summary>
        public string Kuch;
        /// <summary>
        /// Наименование
        /// </summary>
        public string Nauch;
    }

    public class ParetoData
    {
        public UchList[] UchList;

        public PribList[] PribList;
    }

    public class ParetoDiag
    {
        /// <summary>
        /// Кол-во брака
        /// </summary>
        public string Brk { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Naprb { get; set; }
    }



}
