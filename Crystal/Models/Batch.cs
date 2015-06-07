//using CrystalDataSetLib;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Crystal.Models;


//namespace MvcApplication.Models
//{
//    /// <summary>
//    /// Представляет собой партию и информацию по ней
//    /// </summary>
//    public class Batch
//    {
//        public string nprt { get; set; }
//        private CrystalDS.mparttRow _batch;

//        public Batch() { }
//        public Batch(CrystalDS.mparttRow ba)
//        {
//            batch = ba;
//            nprt = ba.nprt;
//        }

//        /// <summary>
//        /// Информация из MPARTT
//        /// </summary>
//        public CrystalDS.mparttRow batch
//        {
//            get
//            {
//                if (_batch == null)
//                    _batch = FoxRepo.GetTable<CrystalDS.mparttRow>(filter: "WHERE mpartt.nprt == ?", parameters: new[] { nprt }).Single();//.Single(a => a.nprt == nprt);
//                return _batch;
//            }
//            set { _batch = value; }
//        }

//        /// <summary>
//        /// Сопроводительный лист партии
//        /// </summary>
//        public IEnumerable<CrystalDS.mprxopRow> GetRouteSheet()
//        {
//            return FoxRepo.GetTable<CrystalDS.mprxopRow>(filter: "WHERE mprxop.nprt == ? ORDER BY mprxop.nop", parameters: new[] { nprt });
//        }
//    }

//    public class BatchNear : Batch
//    {
//        public BatchNear() : base() { }
//        public BatchNear(CrystalDS.mparttRow ba) : base(ba) { }
//        public int distance { get; set; }
//        public string nop { get; set; }
//        public string kop { get; set; }
//    }
//}