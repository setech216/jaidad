using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DbLayer.db;
using ModelLayer;
namespace PropertyProject.Controllers
{
    public class PropertyTypeController : Controller
    {
        PropertyTypeDb db;
        public PropertyTypeController()
        {

            db = new PropertyTypeDb();
        }
        // GET: PropertyType
        public ActionResult Index()
        {
            return View();
        }

        // GET: PropertyType/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PropertyType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: City/Create
        [HttpPost]
        public ActionResult Create(PropertyTypeModel model)
        {
            try
            {

                int id = db.addProopertyType(model);
                if (id > 0)
                {
                    ViewBag.postPropertyType = "Data Inserted !";
                }
                else
                {
                    ViewBag.postPropertyType = "Data Not Inserted!";
                }
                return View();
            }
            catch
            {
                ViewBag.postPropertyType = "Data Not Inserted!";
                return View();
            }
        }

        // GET: PropertyType/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PropertyType/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: PropertyType/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PropertyType/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
