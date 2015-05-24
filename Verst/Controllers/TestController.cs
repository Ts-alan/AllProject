using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcApplication.Models;
using Verst.Models;


namespace Verst.Controllers
{
    public class TestController : Controller
    {
        public ActionResult Graph()
        {
            var result = new List<Graph>();
            result.Add(new Graph { Date = "10.01.2012", PlVal = "5500" });
            result.Add(new Graph { Date = "10.02.2012", PlVal = "4000" });
            result.Add(new Graph { Date = "10.03.2012", PlVal = "7500" });
            result.Add(new Graph { Date = "10.04.2012", PlVal = "8000" });
            result.Add(new Graph { Date = "10.05.2012", PlVal = "3200" });
            result.Add(new Graph { Date = "10.06.2012", PlVal = "5700" });
            result.Add(new Graph { Date = "10.07.2012", PlVal = "6800" });
            result.Add(new Graph { Date = "10.08.2012", PlVal = "2500" });
            result.Add(new Graph { Date = "10.09.2012", PlVal = "4400" });
            return View();
        }

        public ActionResult Graph1()
        {
            var result = new List<Graph>();
            result.Add(new Graph { Date = "10.01.2012", PlVal = "5500" });
            result.Add(new Graph { Date = "10.02.2012", PlVal = "4000" });
            result.Add(new Graph { Date = "10.03.2012", PlVal = "7500" });
            result.Add(new Graph { Date = "10.04.2012", PlVal = "8000" });
            result.Add(new Graph { Date = "10.05.2012", PlVal = "3200" });
            result.Add(new Graph { Date = "10.06.2012", PlVal = "5700" });
            result.Add(new Graph { Date = "10.07.2012", PlVal = "6800" });
            result.Add(new Graph { Date = "10.08.2012", PlVal = "2500" });
            result.Add(new Graph { Date = "10.09.2012", PlVal = "4400" });
            ViewData.Model = result;
            return View();
        }
    }
}
