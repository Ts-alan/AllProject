using System.Collections.Generic;
using System.Linq;
using Verst.Models;

namespace MvcApplication.Models
{
    /// <summary>
    /// Представлякт собой оборудование и информацию по нему
    /// </summary>
    public class Equipment
    {
        private Crystal.astobRow _equipment;
        private string _nuf;
        public string kgt { get; set; }
        public string kus { get; set; }

        public Equipment()
        {
            _nuf = "empty";
        }

        public Equipment(Crystal.astobRow eq):this()
        {
            equipment = eq;
            kgt = eq.kgt;
            kus = eq.kus;
        }

        /// <summary>
        /// Информация из ASTOB
        /// </summary>
        public Crystal.astobRow equipment {
            get {return _equipment ??(_equipment = FoxRepo.GetTable<Crystal.astobRow>(filter: "WHERE astob.kgt == ? AND astob.kus == ?", parameters: new[] {kgt, kus}).Single());}
            set { _equipment = value; }
        }

        private IEnumerable<Crystal.mparttRow> GetParts()
        {
            return FoxRepo.GetTable<Crystal.mparttRow>(filter: "WHERE  mpartt.kgt == ?", parameters: new[] { kgt });
        }

        public string nuf {
            get 
            { 
                if (_nuf.Equals("empty"))
                {
                    var t = FoxRepo.GetTable<Crystal.assuflRow>(filter: "WHERE kgt == ? AND kus == ? ", parameters: new[] { kgt, kus }).SingleOrDefault();//.SingleOrDefault(a => a.kgt == kgt && a.kus == kus);
                    _nuf = t == null ? string.Empty : t.nuf;
                }
                return _nuf;
            }
            set { _nuf = value; }
        }

        /// <summary>
        /// Партии, которые уже прошли через оборудование
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Crystal.mprxopRow> ProcessedParts()
        {
            return (from mprxop in FoxRepo.GetTable<Crystal.mprxopRow>()
                    where mprxop.kgt == kgt && mprxop.kus == kus
                    select mprxop);
        }

        /// <summary>
        /// Пластины, забракованные на данном оборудовании
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Crystal.mpartnRow> DefectedPlates() {
             return (from mpartn in FoxRepo.GetTable<Crystal.mpartnRow>()
                    where mpartn.kgt == kgt && mpartn.kus == kus
                    select mpartn);
        }
        
        /// <summary>
        /// Список запущенных в работу партий
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Crystal.mparttRow> GetStartedParts() {
            return FoxRepo.GetTable<Crystal.mparttRow>(filter: "WHERE mpartt.kgt == ? AND mpartt.kus == ? AND !EMPTY(mpartt.dath) ", parameters: new[] { kgt, kus });
        }

        /// <summary>
        /// Список ожидаюших запуска в работу партий
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Crystal.mparttRow> GetWaitingParts()
        {
            return (from mpartt in FoxRepo.GetTable<Crystal.mparttRow>(filter: "WHERE mpartt.kgt == ? AND !EMPTY(mpartt.dato) ", parameters: new[] { kgt })
                    join astmr in FoxRepo.GetTable<Crystal.astmrRow>()
                        on new { kmk = mpartt.kmk, nop = mpartt.nop } equals new { kmk = astmr.kmk, nop = astmr.nop }
                    join as_wop in FoxRepo.GetTable<Crystal.as_wopRow>(filter:"WHERE as_wop.kus == ?", parameters: new[] {kus})
                        on new { kgt = mpartt.kgt, kop = astmr.kop } equals new { kgt = as_wop.kgt, kop = as_wop.kop }
                    where ((nuf.Length == 0) || (nuf == mpartt.nprt.Substring(0, 2)))
                    select mpartt);
        }

        /// <summary>
        /// История простоев оборудования
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Crystal.asmbnRow> GetDowntimes() {
            return FoxRepo.GetTable<Crystal.asmbnRow>(filter: "WHERE asmbn.kgt == ? AND asmbn.kus == ?", parameters: new[] { kgt, kus });
        }

        /// <summary>
        /// Партии на подходе к оборудованию 
        /// </summary>
        /// <param name="ops">На сколько операций смотреть</param>
        /// <returns></returns>
        public IEnumerable<BatchNear> GetNearParts(int ops = 10)
        {
            var t_astmr = FoxRepo.GetTable<Crystal.astmrRow>();

            var res = from mpartt in FoxRepo.GetTable<Crystal.mparttRow>()
                      join astmr in t_astmr
                        on mpartt.kmk equals astmr.kmk
                      join as_wop in FoxRepo.GetTable<Crystal.as_wopRow>(filter:"WHERE (as_wop.kgt == ? AND as_wop.kus == ?)",
                      parameters: new[] {kgt, kus}
                      )
                        on astmr.kop equals as_wop.kop
                      where int.Parse(mpartt.nop) < int.Parse(astmr.nop)
                        && ((nuf.Length == 0) || (nuf == mpartt.nprt.Substring(0, 2)))
                      select new BatchNear(mpartt)
                      {
                          distance = t_astmr.Count(a => a.kmk == mpartt.kmk && int.Parse(a.nop) > int.Parse(mpartt.nop) && int.Parse(a.nop) <= int.Parse(astmr.nop)),
                          nop = astmr.nop,
                          kop = astmr.kop
                      }; 
                      
            return (from batchNear in res
                    where batchNear.distance <= ops
                    select batchNear);
        }

        //public Area GetArea() { return new Area { kuch = equipment.kuch }; }
    }


}