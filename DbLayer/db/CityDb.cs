using ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbLayer.db
{
     public class CityDb
    {
        public int addCity(CityModel model)
        {
            using (var context = new propertyDbEntities())
            {
                city obj = new city()
                {
                    name = model.name,
                    lat = model.lat == null ? "0" : model.lat,
                    lng = model.lng == null ? "0" : model.lng

                };
                context.city.Add(obj);
                context.SaveChanges();
                return obj.city_id;
            }
        }
        public List<CityModel> getCities()
        {
            List<CityModel> list = new List<CityModel>();
            using (var context = new propertyDbEntities())
            {
                list = context.city
                    .Select(x => new CityModel()
                    {
                        id = x.city_id,
                        name = x.name,
                        lat = x.lat,
                        lng = x.lng
                    }).ToList();
            }
            return list;
        }
        public CityModel getCity(int? id)
        {
            CityModel model = null;
            using (var context = new propertyDbEntities())
            {
                var temp = context.city.Where(x => x.city_id == id).FirstOrDefault();
                model = new CityModel()
                {
                    id = temp.city_id,
                    lat = temp.lat,
                    lng = temp.lng,
                    name = temp.name
                };
            }
            return model;
        }
        public bool updateCity(int id,CityModel model)
        {
            using (var context = new propertyDbEntities())
            {
                var result = context.city.SingleOrDefault(b => b.city_id == id);
                if (result != null)
                {
                    result.name=model.name;
                    result.lat = model.lat;
                    result.lng = model.lng;
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
