using DbLayer.db;
using ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PropertyProject.Controllers
{
    public class CityController : Controller
    {

        CityDb db;
        public CityController()
        {

            db = new CityDb();
        }
        // GET: City/getCities
        public ActionResult getCities()
        {
            List<CityModel> list = db.getCities();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
       
        // GET: City/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: City/Create
        [HttpPost]
        public ActionResult Create(CityModel model)
        {
            try
            {
                int id=db.addCity(model);
                if (id>0)
                {
                    ViewBag.postCity = "Data Inserted !";
                }
                else
                {
                    ViewBag.postCity = "Data Not Inserted!";
                }
                return View();
            }
            catch
            {
                ViewBag.postCity = "Data Not Inserted!";
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
                    CityModel model = db.getCity(id);
                    if (model != null)
                    {
                        return View(model);
                    }
                    else
                    {
                        RedirectToAction("Index", "City");
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
        public ActionResult Edit(CityModel model)
        {
            if ((Request.Cookies["user"] != null))
            {
                int roleId;
                Int32.TryParse(Request.Cookies["user"]["roleId"], out roleId);
                if ((roleId == (int)Role.admin) || (roleId == (int)Role.superAdmin))
                {
                    int id = Int32.Parse(Request.Form["id"]);
                    bool flag = db.updateCity(id, model);
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
