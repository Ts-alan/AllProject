using System.Collections.Generic;
using System.Linq;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Crystal.Models;
using NLog;
using MvcApplication.Models;
using CrystalDataSetLib;
using Crystal.BusinessLogic;
using Crystal.Models.View;
using MvcApi;
using Crystal.Authentication;


namespace Crystal.Controllers
{
    public class HomeController : Controller
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();


        [Authorize]
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult LogOff()
        {
            _log.Info("Пользователь {0} вышел из системы.", User.Identity.Name);
            FormsAuthentication.SignOut();
            return RedirectToAction("SiteEntry", "Home");
        }


        //[HttpPost]
        //public ActionResult AndroidGate()
        //{
        //    FormsAuthentication.SetAuthCookie("Admin", false);
        //    return RedirectToAction("Index", "Home");
        //}
        
        [HttpGet]
        //[RequireHttps]
        public ActionResult SiteEntry()
        {
            _log.Info("Загружена страница входа");
            return View();
        }
        
        [HttpPost]
        //[RequireHttps]
        public ActionResult SiteEntry(LogOn model, string returnUrl, string css)
        {
            if (ModelState.IsValid)
            {
                //_log.Info("В систему входит пользователь: {0}", model.Login);
                //FormsAuthentication.SetAuthCookie(model.Login, model.RememberMe);
                //_log.Info("В систему вошёл пользователь: {0}", model.Login);
                //return RedirectToAction("Index", "Home");
                bool rez = false;

                rez = AuthService.ValidateUser(model.Login, model.Passw);

                if (rez)
                {
                    FormsAuthentication.SetAuthCookie(model.Login, model.RememberMe);
                    var cookie = new HttpCookie("SelectedCSS", css) { Expires = DateTime.Now.AddMonths(7) };
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
                    //var tmpUsr = Membership.GetUser(model.Login);
                    //if (tmpUsr != null)
                    //{
                    //    if (tmpUsr.IsLockedOut || (!tmpUsr.IsApproved))
                    //    {
                    //        _log.Info("Неудачная попытка входа используя заблокированную запись:{0} с паролем:{1}", model.Login, model.Passw);
                    //        return RedirectToAction("UserBlocked");
                    //    }
                    //}
                    _log.Info("Неудачная попытка входа используя логин:{0} и пароль:{1}", model.Login, model.Passw);
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

        [Api]
        public ActionResult AllProductions()
        {
            const string badConnection = "нет подключения";
            const string infoFormat = "{0} ({1})";
            string mpartt43 = null, mpartt47 = null, mpartt12 = null, mpartt20 = null;

            if (User.IsInRole("plant43"))
                try
                {
                    var info = BusinessLogicFactory.GetBL(Plants.plant43).GetProductionSummary();
                    mpartt43 = string.Format(infoFormat, info.BatchCount, info.PlastCount);
                }
                catch { }
            if (User.IsInRole("plant47"))
                try
                {
                    var info = BusinessLogicFactory.GetBL(Plants.plant47).GetProductionSummary();
                    mpartt47 = string.Format(infoFormat, info.BatchCount, info.PlastCount);
                }
                catch { }
            if (User.IsInRole("plant12"))
                try
                {
                    var info = BusinessLogicFactory.GetBL(Plants.plant12).GetProductionSummary();
                    mpartt12 = string.Format(infoFormat, info.BatchCount, info.PlastCount);
                }
                catch { }
            if (User.IsInRole("plant20"))
                try
                {
                    var info = BusinessLogicFactory.GetBL(Plants.plant20).GetProductionSummary();
                    mpartt20 = string.Format(infoFormat, info.BatchCount, info.PlastCount);
                }
                catch { }
            ViewData.Model = new AllProductions
            {
                nzp43 = mpartt43 ?? badConnection,
                nzp47 = mpartt47 ?? badConnection,
                nzp12 = mpartt12 ?? badConnection,
                nzp20 = mpartt20 ?? badConnection
            };
            return PartialView();
        }
    }
}
