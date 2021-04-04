using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DbLayer.db;
using ModelLayer;
using PagedList.Mvc;
using PagedList;
namespace PropertyProject.Controllers
{
    public class DashboardController : Controller
    {
        private PropertyDb propertyDb=null;
        
        public DashboardController()
        {
            propertyDb = new PropertyDb();
            
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UpdateLocations()
        {
            if ((Request.Cookies["user"] != null))
            {
                int roleId;
                Int32.TryParse(Request.Cookies["user"]["roleId"], out roleId);
                if ((roleId == (int)Role.admin) || (roleId == (int)Role.superAdmin))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
        }

        public ActionResult Requests(int? i)
        {
            if ((Request.Cookies["user"] != null))
            {
                int roleId;
                Int32.TryParse(Request.Cookies["user"]["roleId"], out roleId);
                if ((roleId == (int)Role.admin)|| (roleId == (int)Role.superAdmin))
                {
                    return View(propertyDb.getUnselectedProperties().ToPagedList(i ?? 1, 20));
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        public ActionResult UnavailableProperties(int? i)
        {
            if ((Request.Cookies["user"] != null))
            {
                int roleId;
                Int32.TryParse(Request.Cookies["user"]["roleId"], out roleId);
                    if ((roleId == (int)Role.admin) || (roleId == (int)Role.superAdmin))
                    {
                        return View(propertyDb.getUnAvailableProperties().ToPagedList(i ?? 1, 20));
                    }
                    else
                    {
                        return RedirectToAction("Login", "Account");
                    }
             }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult MyProperties(int? i)
        {
            if ((Request.Cookies["user"] != null))
            {
                string id = Request.Cookies["user"]["id"];
                return View(propertyDb.getMyProperties(id).ToPagedList(i ?? 1, 20));
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }


    }
}
