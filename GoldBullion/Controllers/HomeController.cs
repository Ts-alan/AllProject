using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GoldBullion.Models;
using GoldBullion.WCFService;
using System.Web.Script.Serialization;
using Newtonsoft.Json;


namespace GoldBullion.Controllers
{
    public class HomeController : Controller
    {
        WcfService _access = new WcfService();
        public ActionResult Index(bool refresh = false)
        {
           var result= _access.GetDate(refresh);
           return View(result);
        }
        public ActionResult Date()
        {
           return View();
        }
        public ActionResult Graph()
        {
            int id = (int)TempData.Peek("id");
            string time = TempData.Peek("date").ToString();
            DateTime data = DateTime.ParseExact(time, "d", CultureInfo.InvariantCulture);
            var result = _access.GetPrice(id, data);
            ViewBag.Json = JsonConvert.SerializeObject(result.Select(a => new { a.Date,a.Value}));
            ViewBag.Price = result.SingleOrDefault(a=>a.Date.Date==data.Date.AddDays(-1));
            return View();
        }
       
        protected override void Dispose(bool disposing)
        {
            _access.Dispose();
            base.Dispose(disposing);
        }
    }
}