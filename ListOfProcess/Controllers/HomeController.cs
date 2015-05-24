using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Web;
using System.Web.Mvc;
using ListOfProcess.Models;
using ListOfProcess.Services;

namespace ListOfProcess.Controllers
{
    public class HomeController : Controller
    {
        private IConnection _cnt;
        public HomeController(IConnection cnt)
        {
            this._cnt = cnt;
        }
        public ActionResult Index()
        {
           return View();
        }
        public ViewResult LocalComputer()
        {
            var process= _cnt.GetProcessOnLocalMachine();
            return View(process);

        }
        public ViewResult RemoteComputer(RemoteModel model)
        {
            Exception Ex;
            var process = _cnt.GetProcessOnRemoteMachine(model.Login, model.Password, model.Ip, out  Ex);
            try
            {
                ViewBag.Error = Ex.Message;
            }
            catch { }
            return View(process);
        }

        public void StartProcess(string[] startprocess)
        {
            _cnt.StartProcess(startprocess[0]);
        }
    }
}
