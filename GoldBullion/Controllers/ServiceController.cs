using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoldBullion.Controllers
{
    public class ServiceController : Controller
    {
        //
        // GET: /Service/
        public void SetId(int id)
        {
            TempData["id"] = id;
            
        }
        public void SetDate(string date)
        {
            TempData["date"] = date;
          }
	}
}