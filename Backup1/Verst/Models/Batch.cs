using System;
using System.Collections.Generic;
using System.Linq;
using Verst.Models;


namespace MvcApplication.Models
{
    /// <summary>
    /// Представляет собой партию и информацию по ней
    /// </summary>
    public class Batch
    {
        public string nprt { get; set; }
        private Crystal.mparttRow _batch;

        public Batch() { }
        public Batch(Crystal.mparttRow ba) {
            batch = ba;
            nprt = ba.nprt;
        }

        /// <summary>
        /// Информация из MPARTT
        /// </summary>
        public Crystal.mparttRow batch
        {
            get
            {
                if (_batch == null)
                    _batch = FoxRepo.GetTable<Crystal.mparttRow>(filter: "WHERE mpartt.nprt == ?", parameters: new[] { nprt }).Single();//.Single(a => a.nprt == nprt);
                return _batch;
            }
            set { _batch = value; }
        }

        /// <summary>
        /// Сопроводительный лист партии
        /// </summary>
        public IEnumerable<Crystal.mprxopRow> GetRouteSheet() {
            return FoxRepo.GetTable<Crystal.mprxopRow>(filter: "WHERE mprxop.nprt == ? ORDER BY mprxop.nop", parameters: new[] {nprt});
        }
    }

    public class BatchNear : Batch
    {
        public BatchNear() : base() { }
        public BatchNear(Crystal.mparttRow ba) : base(ba) { }
        public int distance { get; set; }
        public string nop { get; set; }
        public string kop { get; set; }
    }
}