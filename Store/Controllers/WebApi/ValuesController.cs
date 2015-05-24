using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Store.Models;

namespace Store.Controllers
{
    public class ValuesController : ApiController
    {
        UnitOfWork.UnitOfWork ctx = new UnitOfWork.UnitOfWork();
        // GET api/values
        public DbSet<Shop> Get()
        {
       
           return ctx.Repository.Shop;
        }

        // GET api/values/5
        public IQueryable Get(int id)
        {
            return ctx.Repository.Goods.Where(a => a.id_Shop == id).Select(a=>new {id=a.id,NameOfGoods=a.name,Description=a.description});
        }

        protected  override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            ctx.Dispose();
        }
     
    }
}
