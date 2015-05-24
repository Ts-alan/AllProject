using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Store.Models;


namespace Store.Controllers
{

    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SecondGrid()
        {
            return PartialView();
        }
        public ActionResult FirstGrid()
        {
            return PartialView();
        }
    }
}
