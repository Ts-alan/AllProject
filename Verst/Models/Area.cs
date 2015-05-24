using System.Collections.Generic;
using System.Linq;
using Verst.Models;

namespace MvcApplication.Models
{
    /// <summary>
    /// Представляет собой участок и информацию по нему
    /// </summary>
    public class Area
    {
        public string kuch { get; set; }

        public Area(){}

        public Area(Crystal.askuchRow ar)
        {
            area = ar;
            kuch = ar.kuch;
        }

        private Crystal.askuchRow _area;

        public Crystal.askuchRow area {
            get
            {
                if (_area == null)
                    _area = FoxRepo.GetTable<Crystal.askuchRow>().Single(a => a.kuch == kuch);
                return _area;
            }
            set { _area = value;}
        }

       /* private IEnumerable<Crystal.mprxopRow> GetFinishedOps()
        {
            return (from astob in GetEquipments()
                    from mprxop in astob.ProcessedParts()
                    //join mprxop in t_mprxop on new { kgt = astob.kgt, kus = astob.kus } equals new { kgt = mprxop.kgt, kus = mprxop.kus}
                    select mprxop);
        }

        /// <summary>
        /// Возвращает список оборудования на участке
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Equipment> GetEquipments() 
        {
            var t_astob = _iFoxProRepo.GetTable<Crystal.astobRow>();

            return (from astob in t_astob
                    where astob.kuch == kuch
                    select new Equipment(astob));
        }*/

        public IEnumerable<Crystal.mparttRow> GetNZP() 
        {
            //return (from eq in GetEquipments()
            //       from parts in (eq.GetStartedParts()).Union(eq.GetWaitingParts())//.Distinct()
            //        select parts).Distinct();

            return (from mpartt in FoxRepo.GetTable<Crystal.mparttRow>()
                   join astmr in FoxRepo.GetTable<Crystal.astmrRow>()
                       on new { mpartt.kmk, mpartt.nop } equals new { astmr.kmk, astmr.nop }
                   join as_wop in FoxRepo.GetTable<Crystal.as_wopRow>()
                       on new { mpartt.kgt, astmr.kop } equals new { as_wop.kgt, as_wop.kop }
                   join astob in FoxRepo.GetTable<Crystal.astobRow>()
                       on new { as_wop.kgt, as_wop.kus } equals new { astob.kgt, astob.kus }
                   join assufl in FoxRepo.GetTable<Crystal.assuflRow>()
                    on new { astob.kgt, astob.kus } equals new { assufl.kgt, assufl.kus } into ufl1
                   let nuf = ufl1.Count()==0? string.Empty : ufl1.Single().nuf
                   where astob.kuch == kuch
                   &&(
                   (!mpartt.dath.isNullDate() && mpartt.kus == astob.kus)
                   || (!mpartt.dato.isNullDate() && ((nuf.Length == 0) || (nuf == mpartt.nprt.Substring(0, 2))))
                   )
                   select mpartt).Distinct();
        }

        public IEnumerable<Crystal.asmnpRow> GetASMNP()
        {
            return (from asmnp in FoxRepo.GetTable<Crystal.asmnpRow>()
                    join astmr in FoxRepo.GetTable<Crystal.astmrRow>()
                        on new { asmnp.kmk, asmnp.nop } equals new { astmr.kmk, astmr.nop }
                    join assop in FoxRepo.GetTable<Crystal.assopRow>()
                        on astmr.kop equals assop.kop
                    join as_wop in FoxRepo.GetTable<Crystal.as_wopRow>()
                        on new { assop.kgt, assop.kop } equals new { as_wop.kgt, as_wop.kop }
                    join astob in FoxRepo.GetTable<Crystal.astobRow>()
                        on new { as_wop.kgt, as_wop.kus} equals new { astob.kgt, astob.kus}
                    where astob.kuch == kuch
                    select asmnp).Distinct();
        }
    }
}