using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EntityFramework.Extensions;
using Testangularjs.Models;

namespace Testangularjs.Controllers
{
    public class ValuesController : ApiController
    {

        public AngularjsDatabaseEntities db = new AngularjsDatabaseEntities();
       
        // GET api/values
        [HttpGet]
        public DbSet<TableModal> Get()
        {
            return db.TableModals;
        }

        [HttpGet]
        public TableModal Get(int id)
        {
            return db.TableModals.ToList().ElementAt(id);
        }
        
        [HttpPost]
        public void Post(TableModal idModel)
        {
            db.TableModals.Add(idModel);
            db.SaveChanges();
            
        }

        // GET api/values/5
        [HttpPut]
        public void Put(int id, TableModal idModel)
        {
            //var model = table.Where(c => c.Id == idModel.Id);
            

            //table.Single(a => a.Id == idModel.Id).Name = idModel.Name;
            //table.Single(a => a.Id == idModel.Id).DateChange = DateTimeOffset.Now;
            //table.Single(a => a.Id == idModel.Id).Description = idModel.Description;
            //return table ;

            db.TableModals.Where(a => a.Id == id).Update(a => idModel);
            //db.TableModals.Update(a => a.Id == id, x => idModel);
        }
        
        [HttpDelete]
        public void Delete(int key1)
        {
            db.TableModals.Where(a => a.Id == key1).Delete();
        }

      

        //[HttpPut]
        //public void qwe(TableModel idModel)
        //{
        //    idModel.DateChange = DateTimeOffset.Now;
        //    //table[id] = idModel;
        //}
    }
}
