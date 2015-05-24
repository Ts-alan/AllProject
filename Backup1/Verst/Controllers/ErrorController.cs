using System;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Verst.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult General(Exception exception)
        {
            ViewBag.Title = "Произошла ошибка!";
            ViewBag.Text = "На сервере произошла ошибка:"+exception.Message + " при попытке выполнить:" + exception.TargetSite+".";
            ViewBag.Solution = exception.Message;
            return View("Error");
        }

        public ActionResult Http404()
        {
            ViewBag.Title = "Ошибка 404! Страница не найдена!";
            ViewBag.Text = "Ошибка 404! Страница не найдена!";
            ViewBag.Solution = "Запрашиваемая вами страница не найдена. Проверьте правильность используемого вами пути.";
            return View("Error");
        }

        public ActionResult Http403()
        {
            ViewBag.Title = "Ошибка 403!";
            ViewBag.Solution = "";
            return View("Error");
        }
    }
}
