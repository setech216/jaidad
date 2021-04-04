using DbLayer.db;
using ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PropertyProject.Controllers
{
    public class SocietyController : Controller
    {
        SocietyDb db;
        CityDb citydb;
        public SocietyController()
        {
            citydb = new CityDb();
            db = new SocietyDb();
        }

        // GET: Society/getSocieties
        public ActionResult getSocieties(int cityId)
        {
            List<SocietyModel> list= db.getSocities(cityId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        // GET: Society/Create
        public ActionResult Create()
        {
            ViewBag.cities = citydb.getCities();
            return View();
        }

        // POST: City/Create
        [HttpPost]
        public ActionResult Create(SocietyModel model)
        {
            ViewBag.cities = citydb.getCities();
            try
            {

                int id = db.addSociety(model);
                if (id > 0)
                {
                    ViewBag.postSociety = "Data Inserted !";
                }
                else
                {
                    ViewBag.postSociety  = "Data Not Inserted!";
                }
                return View();
            }
            catch
            {
                ViewBag.postSociety = "Data Not Inserted!";
                return View();
            }
        }


        public ActionResult Index()
        {
            if ((Request.Cookies["user"] != null))
            {
               
                int roleId;
                Int32.TryParse(Request.Cookies["user"]["roleId"], out roleId);
                if ((roleId == (int)Role.admin) || (roleId == (int)Role.superAdmin))
                {
                    ViewBag.cities = citydb.getCities();
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

        public ActionResult Edit(int id)
        {
            if ((Request.Cookies["user"] != null))
            {
                int roleId;
                Int32.TryParse(Request.Cookies["user"]["roleId"], out roleId);
                if ((roleId == (int)Role.admin) || (roleId == (int)Role.superAdmin))
                {
                    SocietyModel model = db.getSociety(id);
                    if (model != null)
                    {
                        return View(model);
                    }
                    else
                    {
                        RedirectToAction("Index", "Society");
                    }
                    return View(model);
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
        [HttpPost]
        public ActionResult Edit(SocietyModel model)
        {
            if ((Request.Cookies["user"] != null))
            {
                int roleId;
                Int32.TryParse(Request.Cookies["user"]["roleId"], out roleId);
                if ((roleId == (int)Role.admin) || (roleId == (int)Role.superAdmin))
                {
                    int id = Int32.Parse(Request.Form["id"]);
                    bool flag = db.updateSociety(id, model);
                    if (flag == true)
                    {
                        ViewBag.postCity = "Data Updated";
                    }
                    else
                    {
                        ViewBag.postCity = "Data Not Updated";
                    }
                    return View(model);
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
    }
}
