using DbLayer.db;
using ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PropertyProject.Controllers
{
    public class HomeController : Controller
    {

        PropertyTypeDb propTypeDb;
        CityDb citydb;

        public HomeController()
        {
            propTypeDb = new PropertyTypeDb();
            citydb = new CityDb();
        }
        public ActionResult Index()
        {
            ViewBag.propertyTypes = propTypeDb.gePropertyTypes();
            ViewBag.cities = citydb.getCities();
            return View();
        }
        // POST: /Account/LogOff
       
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Temp()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}