using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DbLayer.db;
using ModelLayer;
namespace PropertyProject.Controllers
{
    public class BlockController : Controller
    {
        CityDb citydb;
        BlockDb db;
        public BlockController()
        {
            citydb = new CityDb();
            db = new BlockDb();
        }
        // GET: Block/getBlocks
        public ActionResult getBlocks(int cityId, int socId,int phaseId)
        {
            List<BlockModel> list = null;
            if (phaseId==0)
            {
                 list= db.getBlocks(cityId, socId, null);
            }
            else
            {
                list = db.getBlocks(cityId, socId, phaseId);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        // GET: Block/Create
        public ActionResult Create()
        {
            ViewBag.cities = citydb.getCities();
            return View();
        }
                
        // POST: City/Create
        [HttpPost]
        public ActionResult Create(BlockModel model)
        {
            if (model.phase_id == 0)
            {
                model.phase_id = null;
            }
            ViewBag.cities = citydb.getCities();
            try
            {
                int id = db.addBlock(model);
                if (id > 0)
                {
                    ViewBag.postBlock = "Data Inserted !";
                }
                else
                {
                    ViewBag.postBlock = "Data Not Inserted!";
                }
                return View();
            }
            catch
            {
                ViewBag.postBlock = "Data Not Inserted!";
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
                    BlockModel model = db.getBlock(id);
                    if (model != null)
                    {
                        return View(model);
                    }
                    else
                    {
                        RedirectToAction("Index", "Block");
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
        public ActionResult Edit(BlockModel model)
        {
            if ((Request.Cookies["user"] != null))
            {
                int roleId;
                Int32.TryParse(Request.Cookies["user"]["roleId"], out roleId);
                if ((roleId == (int)Role.admin) || (roleId == (int)Role.superAdmin))
                {
                    int id = Int32.Parse(Request.Form["id"]);
                    bool flag = db.updateBlock(id, model);
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
