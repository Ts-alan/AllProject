using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using book_keeping.Models;

namespace book_keeping.Controllers
{
    public class HomeController : Controller
    {
        private ListOfExpense _db=new ListOfExpense();
        public ActionResult Index()
        {
            return View(_db.Table_1);
        }

        public ActionResult SaveExpense( string Category,int SumOfMoneySpent=0)
        {
            _db.Table_1.Add(new Table_1(){Category = Category,Date = DateTime.Now,SumOfMoneySpent = SumOfMoneySpent});
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AggregateExpense()
        {
            return View();
        }
        public int AggregateBy(string month, string category)
        {
            string time = "1/"+month+"/2014";
            DateTime date = DateTime.Parse(time);
            var data =
                _db.Table_1.Where(a => a.Date.Month == date.Month && a.Category == category).Sum(a => a.SumOfMoneySpent);
            return data;
        }
        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }

    }
}
