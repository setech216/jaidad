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
    public class InquiryController : Controller
    {
        private InquiryDb db;
        public InquiryController()
        {
            db = new InquiryDb();
        }

        [HttpPost]
        public ActionResult Create(InquiryModel model)
        {    
            int id = 0;
            if (model!=null)
            {
                DateTime now = DateTime.Now;
                model.date = now.ToString("dd/MM/yyyy");
                id = db.addInquiry(model);
            }
            return Json(id, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult List(int? i)
        {
            List<InquiryModel> list = db.getList();
            if (list == null)
            {
                list = new List<InquiryModel>();
            }
            return View(list.ToPagedList(i ?? 1, 20));
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            bool flag;
            if (id < 1)
            {
                flag = false;
            }
            else
            {
                flag= db.delete(id);
            }  
            return Json(flag, JsonRequestBehavior.AllowGet);
        }
    }
}