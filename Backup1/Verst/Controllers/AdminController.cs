using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Profile;
using System.Web.Security;
using MvcApplication.Models;
using NLog;
using Verst.Models;

namespace Verst.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();

        [HttpGet]
        public ActionResult UsersManagment()
        {
            _log.Info("Пользователь {0} просматривает страницу управления пользователями!", User.Identity.Name);
            return View();
        }

        [HttpGet]
        public ActionResult GetUsersList()
        {
            var preResults = from user in Membership.GetAllUsers().OfType<MembershipUser>()
                             select new User { UserId = user.ProviderUserKey.GetHashCode(), Username = user.UserName, IsBlocked = user.IsLockedOut, IsApproved = user.IsApproved };

            var results = (from user in preResults let pr = ProfileBase.Create(user.Username) select new UserProf { UserId = user.UserId, Fio = pr["FIO"] as string, Username = user.Username, Unit = pr["Unit"] as string, IsBlocked = user.IsBlocked,IsApproved = user.IsApproved}).ToList();
            return View("UsersList",results);
        }

        [HttpGet]
        public ActionResult GetRolesList()
        {
            var roles = from role in Roles.GetAllRoles()
                        select new Role { RoleId = role.GetHashCode(), RoleName = role };
            return View("RolesList", roles);
        }

        [HttpPost]
        public ActionResult CreateUser(string username, string fio, string unit, string password)
        {
            if (Request.IsAjaxRequest())
            {
                Membership.CreateUser(username, password);
                var newProfile = ProfileBase.Create(username);
                newProfile["FIO"] = fio;
                newProfile["Unit"] = unit;
                newProfile.Save();
                _log.Info("Успешно создан новый пользователь с логином:{0}", username);
                return Content("Пользователь успешно создан!");
            }
            return Content("Ajax Request only!");
        }

        [HttpPost]
        public ActionResult DeleteUser(string userName)
        {
            if (Request.IsAjaxRequest())
            {
                Membership.DeleteUser(userName);
                _log.Info("Успешно удалён пользователь {0}", userName);
                return Content("Пользователь успешно удалён!");
            }
            return Content("Ajax request only!");
        }

        [HttpGet]
        public ActionResult EditUser(string userId)
        {
            var result = (from user in Membership.GetAllUsers().OfType<MembershipUser>()
                          select new EditUser { Username = user.UserName, UserId = user.ProviderUserKey.GetHashCode()}).Single(x => x.UserId == int.Parse(userId));

            ProfileBase userProfile = ProfileBase.Create(result.Username);

            var result2 = from roles in Roles.GetRolesForUser(result.Username)
                            select new Role
                            {
                              RoleId = roles.GetHashCode(),
                              RoleName = roles
                            };

            ViewBag.UserFIO = userProfile["FIO"] == null ? "" : userProfile["FIO"].ToString();
            ViewBag.UserUnit = userProfile["Unit"] == null ? "" : userProfile["Unit"].ToString();

            ViewBag.UserLogin = result.Username;
            TempData["Login"] = result.Username;
            ViewBag.RolesForUser = result2;
            ViewBag.Roles = from role in Roles.GetAllRoles()
                        select new Role { RoleId = role.GetHashCode(), RoleName = role };
            return View(result);
        }

        [HttpPost]
        public ActionResult EditUser(string username, string fio, string unit, string password)
        {
            if (Request.IsAjaxRequest())
            {
                try
                {
                    var member = Membership.GetUser((string)TempData.Peek("Login"));
                    ProfileBase userProfile = ProfileBase.Create(member.UserName);
                    userProfile["FIO"] = fio;
                    userProfile["Unit"] = unit;
                    userProfile.Save();
                    //Имя через мембершип менять нельзя - только лезть в ручную
                    if (password!="")
                    {
                        member.ChangePassword(member.ResetPassword(), password);
                    }
                    _log.Info("Данные пользователя {0} были успешно изменены {1}.", username, User.Identity.Name);
                    return Content("Данные успешно обновлены!", "text/html");
                }
                catch (Exception)
                {
                    return Content("Произошла ошибка! Данные не были обновлены!", "text/html");
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult DeleteRole(string roleName)
        {
            if (Request.IsAjaxRequest())
            {
                Roles.DeleteRole(roleName);
                _log.Info("Роль {0} была успешно удалена {1}!", roleName,User.Identity.Name);
                return Content("Роль была успешно удалена!");
            }
            return Content("Ajax request only!");
        }

        [HttpPost]
        public ActionResult CreateRole(string newRole)
        {
            if (Request.IsAjaxRequest())
            {
             if (!Roles.RoleExists(newRole))
                {
                  Roles.CreateRole(newRole);
                }
             _log.Info("Роль {0} была успешно создана пользователем {1}.", newRole, User.Identity.Name);
             return Content("Роль была успешно создана!");
            }
            return Content("Ajax request only!");
        }

        [HttpPost]
        public ActionResult AddRoleToUser(string userName, string roleName)
        {
            if (Request.IsAjaxRequest())
            {
                if (!Roles.IsUserInRole(userName,roleName))
                {
                    Roles.AddUserToRole(userName, roleName);
                    _log.Info("Пользователю {0} была успешно добавлена роль {1}.",userName,roleName);
                }
            }

            ViewBag.RolesForUser = from roles in Roles.GetRolesForUser(userName)
                            select new Role
                            {
                              RoleId = roles.GetHashCode(),
                              RoleName = roles
                            };
            return View("UserRolesList");
        }

        [HttpPost]
        public ActionResult DeleteUserRole(string userName, string roleName)
        {

            if (Request.IsAjaxRequest())
            {
                Roles.RemoveUserFromRole(userName,roleName);
                _log.Info("У ользователя {0} была успешно удалена роль {1}.", userName, roleName);
            }

            ViewBag.RolesForUser = from roles in Roles.GetRolesForUser(userName)
                                   select new Role
                                   {
                                       RoleId = roles.GetHashCode(),
                                       RoleName = roles
                                   };
            return View("UserRolesList");
        }

        [HttpGet]
        public ActionResult ShowLogs()
        {
            
            string[] files_ = Directory.GetFiles(@"C:\_Logs_\OldCrystal", "*.log");
            foreach (var file1 in files_)
            {
                DateTime now = DateTime.Parse(file1.Substring(21, 10));

                if (now.Month < DateTime.Now.Month-1 && now.Year <= DateTime.Now.Year)
                {
                    Console.WriteLine(file1.Substring(3, 10));
                    System.IO.File.Delete(@file1);
                }
            }
            _log.Info("Пользователь {0} просматривает логи системы Crystal.", User.Identity.Name);
            var dir = new DirectoryInfo("C:\\_Logs_\\OldCrystal");
            FileInfo[] files = dir.GetFiles("*.log", SearchOption.TopDirectoryOnly);
            FileInfo latestLog = files.OrderByDescending(x=>x.CreationTime).First();
            var model = new ShowLogsModel { FindedLogFiles = files, LatestLogData = GetLogs(latestLog) };
            return View(model);
        }

        //Считывает строки из файла
        private List<LogString> GetLogs(FileInfo workFile)
        {
            string[] lines = System.IO.File.ReadAllLines(workFile.FullName,System.Text.Encoding.GetEncoding(1251));
            return lines.Select(line => line.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries)).Select(tmp => new LogString { Date = tmp[0], Time = tmp[1].Replace('-', ':'), Message = tmp[2] }).ToList();
        }

        [HttpGet]
        public ActionResult ReadSelectedLog(string fn)
        {
            var dir = new DirectoryInfo("C:\\_Logs_\\OldCrystal");
            FileInfo file = dir.GetFiles("*.log", SearchOption.TopDirectoryOnly).First(x=>x.Name==fn);
            List<LogString> results = GetLogs(file);
           
            _log.Info("Пользователь {0} просматривает лог системы Crystal -> {1}.",User.Identity.Name, file);
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UnblockUser(string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                var member = Membership.GetUser(username);
                if (member != null)
                {
                    member.UnlockUser();
                    member.IsApproved = true;
                    Membership.UpdateUser(member);
                    _log.Info("Пользователь {0} успешно разблокирован!", username);
                    return Content("Пользователь успешно разблокирован!");
                }
                return Content("Такой пользователь не найден!");
            }
            return Content("Неверный параметр!");
        }

        [HttpPost]
        public ActionResult BlockUser(string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                var member = Membership.GetUser(username);
                if (member != null)
                {
                    member.IsApproved = false;
                    Membership.UpdateUser(member);
                    _log.Info("Пользователь {0} успешно заблокирован!", username);
                    return Content("Пользователь успешно заблокирован!");
                }
                return Content("Пользователь не найден!");
            }
            return Content("Неверный параметр!");
        }

        [HttpPost]
        public ActionResult ClearCache()
        {
          
            try
            {
                foreach (DictionaryEntry dEntry in HttpContext.Cache)
                {
                    HttpContext.Cache.Remove(dEntry.Key.ToString());
                }
                _log.Info("Кэш был успешно очищен!");
                return Content("Кэш был успешно очищен!");
            }
            catch (Exception e)
            {
                _log.Error("Произошла ошибка!!! Возможно кэш не был очищен. Текст ошибки:{0}", e.Message);
                return Content("Произошла ошибка!!! Возможно кэш не был очищен.");
            }
        }

        [HttpPost]
        public ActionResult RefreshCyclesCache()
        {
          /*  ClearCache();
            _log.Info("Пользователем {0} запущено обновление кэша!", User.Identity.Name);
            var plants = Enum.GetValues(typeof (Plants));
            Plants dd = Plants.plant43;
            try
            {

                var tempAmpartn = FoxRepo.GetTable<Crystal.mpartnRow>(isArchived: true, useCache: true,
                    plant: (Plants) dd);
                var tempAskrm = FoxRepo.GetTable<Crystal.askrmRow>(useCache: true, plant: (Plants) dd);
                var tempAmpartt = FoxRepo.GetTable<Crystal.mparttRow>(isArchived: true, useCache: true,
                    plant: (Plants) dd);
                //заменено на ручной запрос изз-за известной ошибки numerick to decimal fails на FoxPro драйверах
                //http://social.msdn.microsoft.com/Forums/sqlserver/en-US/035b6676-441f-47df-9272-3b2bc4f8861a/error-reading-certain-numeric-values-with-vfpoledb-driver
               /* var tempAmprxop = FoxRepo.GetTable<Crystal.mprxopRow>(isArchived: true, useCache: true,
                    plant: (Plants) dd);*/
                //_log.Info("Запрос Amprxop не удался -> используется альтернативный способ!");
                //var tempAmprxop = FoxRepo.GetTableSql<Crystal.mprxopRow>(sql: "SELECT NPRT,NOP,KMK,KGT,KUS,DATH,TIMH,DATO,TIMO,NPRO,Cast(KPLS As NUMERIC(8,2)),Cast(KPLR As NUMERIC(8,2)),PRRO,PROT,KPR,OWNER,NPROC FROM AMPRXOP", useCache: true, plant: (Plants)dd);


                //_log.Info("Обновление кэша для циклов прошло успешно!");
                return Content("Обновление прошло успешно!");
           /* }
            catch (Exception ex)
            {
                _log.Error("Произошла ошибка!!! Обновление не удалось!. " + ex.Message);
                return Content("Произошла ошибка!!! Обновление не удалось!. \n" + ex.Message);
            }*/
        }
    }
}