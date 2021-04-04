using ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbLayer.db
{
    public class SocietyDb
    {
        public int addSociety(SocietyModel model)
        {
            using (var context = new propertyDbEntities())
            {
                society dbObj = new society()
                {
                    name = model.name,
                    city_id = model.city_id,
                    lat =  model.lat==null?"0":model.lat,
                    lng = model.lng == null ? "0" : model.lng

                };
                context.society.Add(dbObj);
                context.SaveChanges();
                return dbObj.society_id;
            }
        }

        public List<SocietyModel> getSocities(int cityId)
        {
            List<SocietyModel> list = new List<SocietyModel>();
            using (var context = new propertyDbEntities())
            {
                list = context.society.Where(x => x.city_id == cityId)
                    .Select(x => new SocietyModel()
                    {
                        id=x.society_id,
                        city_id = x.city_id,
                        name = x.name,
                        lat = x.lat,
                        lng = x.lng
                    }).ToList();
            }
            return list;
        }
        public SocietyModel getSociety(int? id)
        {
            SocietyModel model = null;
            using (var context = new propertyDbEntities())
            {
                var temp = context.society.Where(x => x.society_id == id).FirstOrDefault();
                model = new SocietyModel()
                {
                    id = temp.society_id,
                    lat = temp.lat,
                    lng = temp.lng,
                    city_id = temp.city_id,
                   
                    name = temp.name
                };
            }
            return model;
        }
        public bool updateSociety(int id, SocietyModel model)
        {
            using (var context = new propertyDbEntities())
            {
                var result = context.society.SingleOrDefault(b => b.society_id == id);
                if (result != null)
                {
                    result.name = model.name;
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
