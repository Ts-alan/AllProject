using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Verst.Models;
using NLog;

namespace Verst.Controllers
{
    public class HomeController : Controller
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SiteEntry()
        {
            _log.Info("Загружена страница входа");
            return View();
        }

        public ActionResult LogOff()
        {
            _log.Info("Пользователь {0} вышел из системы.",User.Identity.Name);
            FormsAuthentication.SignOut();
            return RedirectToAction("SiteEntry", "Home");
        }

        [HttpPost]
        public ActionResult SiteEntry(LogOn model, string returnUrl, string css)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.Login, model.Passw))
                {
                    FormsAuthentication.SetAuthCookie(model.Login, model.RememberMe);
                    var cookie = new HttpCookie("SelectedCSS", css) {Expires = DateTime.Now.AddMonths(7)};
                    Response.Cookies.Add(cookie);
                    _log.Info("В систему вошёл пользователь: {0}", model.Login);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                        return RedirectToAction("Index", "Home");
                }
                else
                {
                    var tmpUsr = Membership.GetUser(model.Login);
                    if (tmpUsr != null)
                    {
                        if (tmpUsr.IsLockedOut || (!tmpUsr.IsApproved))
                        {
                            _log.Info("Неудачная попытка входа используя заблокированную запись:{0} с паролем:{1}", model.Login, model.Passw);
                            return RedirectToAction("UserBlocked");
                        }
                    }
                    _log.Info("Неудачная попытка входа используя логин:{0} и пароль:{1}",model.Login, model.Passw);
                    ModelState.AddModelError("", "Имя пользователя или пароль указаны неверно.");
                }
            }
            return View(model);
        }

        public ActionResult UserBlocked()
        {
            return View("UserBlockedPage");
        }

        [HttpGet]
        public PartialViewResult Sidemenu()
        {
            return PartialView("_sidemenu");
        }

        [HttpGet]
        public ActionResult About()
        {
            _log.Info("Кто-то читает о системе... Умничка...");
            return View();
        }
    }
}
