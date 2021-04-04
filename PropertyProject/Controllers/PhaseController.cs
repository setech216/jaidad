using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DbLayer.db;
using ModelLayer;
namespace PropertyProject.Controllers
{
    public class PhaseController : Controller
    {
        CityDb citydb;
        PhaseDb db;
        public PhaseController()
        {
            citydb = new CityDb();
            db = new PhaseDb();
        }

        // GET: Phase/getPhases
        public ActionResult getPhases(int cityId,int socId)
        {
            List<PhaseModel> list = db.getPhases(cityId,socId);
            return Json( list , JsonRequestBehavior.AllowGet);
        }
      
        // GET: Phase/Create
        public ActionResult Create()
        {
            ViewBag.cities = citydb.getCities();
            return View();
        }


        [HttpPost]
        public ActionResult Create(PhaseModel model)
        {
            ViewBag.cities = citydb.getCities();
            try
            {
            
                int id = db.addPhase(model);
                if (id > 0)
                {
                    ViewBag.postPhase = "Data Inserted !";
                }
                else
                {
                    ViewBag.postPhase = "Data Not Inserted!";
                }
                return View();
            }
            catch
            {
                ViewBag.postPhase = "Data Not Inserted!";
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
                    PhaseModel model = db.getPhase(id);
                    if (model != null)
                    {
                        return View(model);
                    }
                    else
                    {
                        RedirectToAction("Index", "Phase");
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
        public ActionResult Edit(PhaseModel model)
        {
            if ((Request.Cookies["user"] != null))
            {
                int roleId;
                Int32.TryParse(Request.Cookies["user"]["roleId"], out roleId);
                if ((roleId == (int)Role.admin) || (roleId == (int)Role.superAdmin))
                {
                    int id = Int32.Parse(Request.Form["id"]);
                    bool flag = db.updatePhase(id, model);
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
