using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using GoldBullion.Models;
using GoldBullion.ServiceReference1;

namespace GoldBullion.WCFService
{
    public class WcfService : IDisposable
    {
        private readonly ExRatesSoap WcfProvider;
        private readonly GoldBullionData db;

        public WcfService()
        {
            WcfProvider = new ExRatesSoapClient();
            db = new GoldBullionData();
        }

        public IEnumerable<ListOfMetal> GetDate(bool refresh)
        {
            var allList = from c in db.ListOfMetal select c;
            if (refresh == false)
            {
                var data =
                    WcfProvider.MetalsRef().Tables[0].AsEnumerable()
                        .Select(a => new ListOfMetal { Name_Metal = a.Field<string>("Name"), Id = a.Field<int>("Id") })
                        .ToList();
                var allPrice = from c in db.PriceOfMetal select c;
                db.ListOfMetal.RemoveRange(allList);
                db.PriceOfMetal.RemoveRange(allPrice);
                db.ListOfMetal.AddRange(data);
                db.SaveChanges();
                return data;
            }
            return allList;

        }

        public IEnumerable<PriceOfMetal> GetPrice(int id, DateTime time)
        {
            bool condition = false;
            DateTime timenew = time.AddDays(-1);

            var allPrice = from c in db.PriceOfMetal
                           select c;
            foreach (var i in allPrice)
            {
                if (i.Date.Date == timenew.Date && i.ListOfMetaL_Id==id)
                {
                    condition = true;
                }
            }
            try
            {
                if (condition == false)
                {
                    var price = WcfProvider.MetalsPrices(id, time, time).Tables[0].AsEnumerable()
                        .Select(
                            a =>
                                new PriceOfMetal()
                                {
                                    Date = a.Field<DateTime>("Date"),
                                    ListOfMetaL_Id = a.Field<int>("MetalId"),
                                    Value = a.Field<System.Double>("Price")
                                })
                        .ToList().Single();

                    db.PriceOfMetal.Add(price);
                    db.SaveChanges();
                }
            }
            catch
            {
            }
            var all = from c in db.PriceOfMetal select c;
            return all;
            
            
        }


        public void Dispose()
        {
            db.Dispose();
        }
    }
}