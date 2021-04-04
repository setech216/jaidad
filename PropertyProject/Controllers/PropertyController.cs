using DbLayer.db;
using ModelLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
namespace PropertyProject.Controllers
{
    public class PropertyController : Controller
    {
        PropertyDb db;
        PropertyTypeDb propTypeDb;
        List<PropertyTypeModel> propertyTypes;
        CityDb citydb;
        SocietyDb socdb;
        PhaseDb phdb;
        BlockDb blockdb;
        UserDb userdb;
        public PropertyController()
        {
            userdb = new UserDb();
            phdb = new PhaseDb();
            blockdb = new BlockDb();
            socdb = new SocietyDb();
            propTypeDb=new PropertyTypeDb();
            citydb = new CityDb();
            db = new PropertyDb();
        }

        public ActionResult Edit(int id)
        {
            ViewBag.agents = userdb.getAgents();
            ViewBag.propertyTypes = propTypeDb.gePropertyTypes();
            ViewBag.cities = citydb.getCities();
            return View(db.getPropertyForEditing(id));
        }

        [HttpPost]
        public ActionResult Edit(PropertyUpdateModel model)
        {
            string isCorner = Request.Form["corner"];
           int id=Int32.Parse(Request.Form["id"]);
            model.price_with_comma = model.price.ToString("#,##0");
            if (model.affidavit_price != null)
            {
                int affidavitprice;
                Int32.TryParse(model.affidavit_price, out affidavitprice);
                model.affidavit_price_with_comma = affidavitprice.ToString("#,##0");
            }
            if (isCorner == "Yes")
            { model.isCorner = true; }
            else
            { model.isCorner = false; }

            if (model.area_unit == "Marla")
                model.area_in_marla = model.area;
            else if (model.area_unit == "Kanal")
                model.area_in_marla = model.area * 20;
            else if (model.area_unit == "SquareFeet")
                model.area_in_marla = model.area * 0.00367309;
            else if (model.area_unit == "SquareYard")
                model.area_in_marla = model.area * 0.0330578;
            else if (model.area_unit == "SquareMeter")
                model.area_in_marla = model.area * 0.0395368;
            try
            {
                bool retID = db.updateProperty(id,model);
                if (retID ==true)
                {
                    return RedirectToAction("Details", "Property", new { @id = id });
                }
                else
                {
                    ViewBag.postPhase = "Data Not Updated !";
                }
                return View();
            }
            catch (Exception ex)
            {

                ViewBag.postPhase = ex.InnerException.ToString();
                return View();
            }
        }

        // GET: Property/Details/5
        public ActionResult Details(int id)
        {     
            return View(db.getProperty(id));
        }
        // GET: Property/Listing
        public ActionResult Listing(int? i, string purpose="sell", int propertyType=-1, int cityId = -1, int societyId = -1, int phaseId = -1, int blockId = -1, int priceFrom = -1, int priceTo = -1, int? areaFrom = null, int? areaTo = null, string unit ="")
        {
            string lat= "30.3753";
            string lng= "69.3451";
            string zoom="5";
            string cityZoom = "10";
            string societyZoom = "11";
            string phaseZoom = "12";
            string blockZoom = "13";
            string area = "Pakistan";
            string type = "Properties";

            if(propertyType!=-1)
            {
                type = propTypeDb.gePropertyType(propertyType).name;
            }
            if (cityId != -1)
            {
               
                CityModel city = citydb.getCity(cityId);
                area = city.name;
                if (city.lat != "0" && city.lng != "0")
                {
                    lat = city.lat;
                    lng = city.lng;
                    zoom = cityZoom;
                }
                if (societyId != -1)
                {
                    SocietyModel soc = socdb.getSociety(societyId);
                    area = area + "," + soc.name;
                    if (soc.lat != "0" && soc.lng != "0")
                    {
                        lat = soc.lat;
                        lng = soc.lng;
                        zoom = societyZoom;
                    }
                    if (phaseId != -1)
                    {
                        PhaseModel ph = phdb.getPhase(phaseId);
                        area = area + "," + ph.name;
                        if (ph.lat != "0" && ph.lng != "0")
                        {
                            lat = ph.lat;
                            lng = ph.lng;
                            zoom = phaseZoom;
                        }
                        if (blockId != -1)
                        {
                            BlockModel b = blockdb.getBlock(blockId);
                            area = area + "," + b.name;
                            if (b.lat != "0" && b.lng != "0")
                            {
                                lat = b.lat;
                                lng = b.lng;
                                zoom = blockZoom;
                            }
                        }
                    }
                    else if (blockId != -1)
                    {
                        BlockModel b = blockdb.getBlock(blockId);
                        area = area + "," + b.name;
                        if (b.lat != "0" && b.lng != "0")
                        {
                            lat = b.lat;
                            lng = b.lng;
                            zoom = blockZoom;
                        }
                    }
                }
            }
            ViewBag.lat = lat;
            ViewBag.lng = lng;
            ViewBag.zoom = zoom;
            ViewBag.area = area;
            ViewBag.purpose = purpose == "sell" ? "For Sale" : "For Rent";
            ViewBag.type = type;
            string query = getQuery(purpose, propertyType, cityId, societyId, phaseId,  blockId,  priceFrom,  priceTo,  areaFrom, areaTo, unit);
            List<PropertyListingModel> list= db.getPropertyList(query);
            return View(list.ToPagedList(i ?? 1,20));
        }

        // GET: Property/Listing
        public ActionResult ListingJson(string purpose = "sell", int propertyType = -1, int cityId = -1, int societyId = -1, int phaseId = -1, int blockId = -1, int priceFrom = -1, int priceTo = -1, int? areaFrom = null, int? areaTo = null, string unit = "")
        {
            string query = getQuery(purpose, propertyType, cityId, societyId, phaseId, blockId, priceFrom, priceTo, areaFrom, areaTo, unit);
            List<PropertyListingModel> list = db.getPropertyList(query);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        // GET: Property/Create
        public ActionResult Create()
        {
            ViewBag.agents = userdb.getAgents();
            ViewBag.propertyTypes=propTypeDb.gePropertyTypes();
            ViewBag.cities = citydb.getCities();
            return View();
        }

        // POST: Property/Create
        [HttpPost]
        public ActionResult Create(PropertyAddModel model, IEnumerable<HttpPostedFileBase> files)
        {
            string lat = Request.Form["lat"];
            string lng = Request.Form["lng"];
            string isCorner = Request.Form["corner"];
            string plot_no = Request.Form["plot_no"];
            string plot_type = Request.Form["plot_type"];
            string affPrice = Request.Form["affidavit_price"];
            ViewBag.agents = userdb.getAgents();
            propertyTypes = propTypeDb.gePropertyTypes();
            ViewBag.propertyTypes = propertyTypes;
            ViewBag.cities = citydb.getCities();

            model.plot_no = plot_no;
            model.plot_type = plot_type;
            model.society_id = model.society_id == 0 ? null : model.society_id;
            model.lat = lat;
            model.lng = lng;
  
            model.price_with_comma = model.price.ToString("#,##0");
            if (affPrice != null)
            {
                int affidavitprice;
                Int32.TryParse(affPrice, out affidavitprice);
                model.affidavit_price = affPrice;
                model.affidavit_price_with_comma = affidavitprice.ToString("#,##0");
            }
            

            if (propertyTypes != null)
            {
               PropertyTypeModel obj= propertyTypes.Where(x => x.id == model.type_id).FirstOrDefault();
                model.type_name= obj.name;
            }

            if (model.city_id != null)
            {
                model.city_name = citydb.getCity(model.city_id).name;
            }
            if (model.society_id != null)
            {
                model.society_name = socdb.getSociety(model.society_id).name;
            }
            if (model.phase_id != null)
            {
                model.phase_name = phdb.getPhase(model.phase_id).name;
            }
            if (model.block_id != null)
            {
                model.block_name = blockdb.getBlock(model.block_id).name;
            }
            if (isCorner=="1")
            { model.is_corner = true; }
            else
            { model.is_corner = false; }

            if (model.area_unit=="Marla")
                model.area_in_marla = model.area;
            else if (model.area_unit =="Kanal")
                model.area_in_marla = model.area * 20;
            else if (model.area_unit == "SquareFeet")
                model.area_in_marla = model.area * 0.00367309;
            else if (model.area_unit == "SquareYard")
                model.area_in_marla = model.area * 0.0330578;
            else if (model.area_unit == "SquareMeter")
                model.area_in_marla = model.area * 0.0395368;

            if ((Request.Cookies["user"] != null))
            {
                model.userId = Request.Cookies["user"]["id"];
                int roleId;
                Int32.TryParse(Request.Cookies["user"]["roleId"], out roleId);
                if ((roleId == (int)Role.admin) || (roleId == (int)Role.superAdmin))
                {
                    model.is_available = true;
                    model.isSelected = true;
                  
                }
                if (roleId == (int)Role.agent )
                {
                    model.is_available = true;
                    model.isSelected = true;
                    model.dealer_id = Request.Cookies["user"]["id"];

                }
                if (roleId == (int)Role.user)
                {
                    model.is_available = true;
                    model.isSelected = false;
                    
                }
            }
            string imageUrl = "";
            bool hasImage = false;
            foreach (var dataitem in files)
            {
                if (dataitem != null && dataitem.ContentLength > 0)
                {
                    //Replace / from file name
                    string name = dataitem.FileName.Replace("\"", "");
                    //Create New file name using GUID to prevent duplicate file name
                    string newFileName = Guid.NewGuid() + Path.GetExtension(name);
                    //Move file from current location to target folder.
                    dataitem.SaveAs(Server.MapPath("~/uploadedImages/") + newFileName);
                    imageUrl = imageUrl + "/uploadedImages/";
                    imageUrl = imageUrl + newFileName;
                    imageUrl = imageUrl + ",";
                    hasImage = true;
                }
            }

            if (!hasImage)
            {
                if(model.purpose=="sell")
                {
                    imageUrl = "/uploadedImages/sell.jpg,";
                }
                if (model.purpose == "rent")
                {
                    imageUrl = "/uploadedImages/rent.jpg,";
                }
            }

            
            model.image = imageUrl;
            try
            {

                int id = db.addProperty(model);
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
            catch (Exception ex)
            {
                
                ViewBag.postPhase = ex.InnerException.ToString();
                return View();
            }
        }
        // GET: Property
        public ActionResult Index()
        {
            return View();
        }

       
     
 

        // POST: Property/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            PropertyGetModel model = db.getProperty(id);
            bool flag = false;
            if (model != null)
            {
                string img = model.image;
                string path;
                if (img != null)
                {
                    string[] temp = img.Split(',');
                    foreach (string url in temp)
                    {
                        if ((url != "/uploadedImages/rent.jpg") && (url != "/uploadedImages/sell.jpg"))
                        {
                            path = "~" + url;
                            string imageFilePath = Server.MapPath(path);
                            if (System.IO.File.Exists(imageFilePath))
                            {
                                System.IO.File.Delete(imageFilePath);
                            }
                        }
                    }
                }
                return Json(db.delete(id), JsonRequestBehavior.AllowGet);
            }
            else {
                return Json(flag, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult MarkAsAvailable(int id)
        {
            return Json(db.markAsAvailable(id), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult MarkAsUnAvailable(int id)
        {
            return Json(db.markAsUnAvailable(id), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult MarkAsSelected(int id,string dealerId)
        {
            return Json(db.markAsSelected(id,dealerId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult MarkAsUnSelectd(int id)
        {
            return Json(db.markAsUnSelected(id), JsonRequestBehavior.AllowGet);
        }

        public string getQuery(string purpos,int propertyType, int cityId, int societyId, int phaseId, int blockId,  int minPrice, int maxPrice,int? areaFrom, int? areaTo, string unit)
        {
            double? minAreaMarla=0;
            double? maxAreaMarla=0;
            String q = "select * from property INNER JOIN AspNetUsers ON property.dealer_id = AspNetUsers.Id where property.isSelected=1";
            if (purpos != "-1")
                q = q + " AND property.purpose=" + "'" + purpos + "' ";
            if (cityId != -1)
            {
                q = q + " AND property.city_id=" + cityId;
                if (societyId != -1)
                {
                    q = q + " AND property.society_id=" + societyId;
                    if (phaseId != -1)
                    {
                        q = q + " AND property.phase_id=" + phaseId;
                        if (blockId != -1)
                        {
                            q = q + " AND property.block_id=" + blockId;
                        }
                    }
                    else if (blockId != -1)
                    {
                        q = q + " AND property.block_id=" + blockId;
                    }
                }
            }
            if (propertyType != -1)
                q = q + " AND property.type_id=" + propertyType;
            if ((minPrice != -1) && (maxPrice != -1))
                q = q + " AND property.price >= " + minPrice + " AND price <= " + maxPrice;
            if ((minPrice != -1) && (maxPrice == -1))
                q = q + " AND property.price >= " + minPrice ;
            if ((minPrice == -1) && (maxPrice != -1))
                q = q + " AND property.price <= " + maxPrice;
            if ((areaFrom != null) && (areaTo == null))
            {
                minAreaMarla = getAreaInMarla(areaFrom, unit);
                q = q + " AND property.area_in_marla >= " + minAreaMarla ;
            }
            if ((areaFrom == null) && (areaTo != null))
            {
                maxAreaMarla = getAreaInMarla(areaTo, unit);
                q = q + " AND property.area_in_marla <= " + maxAreaMarla;
            }
            if ((areaFrom != null) && (areaTo != null))
            {
                minAreaMarla = getAreaInMarla(areaFrom, unit);
                maxAreaMarla = getAreaInMarla(areaTo, unit);
                q = q + " AND property.area_in_marla >= " + minAreaMarla + " AND property.area_in_marla <= " + maxAreaMarla;
            }
            q = q + "ORDER BY property.id DESC ";
            return q;
        }
        double? getAreaInMarla(int? area,string unit)
        {
            double? AreaMarla=0;

            if (unit == "Marla")
                AreaMarla = area;
            else if (unit == "Kanal")
                AreaMarla = area * 20;
            else if (unit == "SquareFeet")
                AreaMarla = area * 0.00367309;
            else if (unit == "SquareYard")
                AreaMarla = area * 0.0330578;
            else if (unit == "SquareMeter")
                AreaMarla = area * 0.0395368;

            return AreaMarla;
        }
    }
}
