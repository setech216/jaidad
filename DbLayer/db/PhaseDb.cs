using ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbLayer.db
{
    public class PhaseDb
    {
        public int addPhase(PhaseModel model)
        {
            using (var context = new propertyDbEntities())
            {
                phase dbObj = new phase()
                {
                    name = model.name,
                    society_id=model.society_id,
                    city_id = model.city_id,
                    lat = model.lat == null ? "0" : model.lat,
                    lng = model.lng == null ? "0" : model.lng

                };
                context.phase.Add(dbObj);
                context.SaveChanges();
                return dbObj.id;
            }
        }


        public List<PhaseModel> getPhases(int cityId,int societyId)
        {
            List<PhaseModel> list = new List<PhaseModel>();
            using (var context = new propertyDbEntities())
            {
                list = context.phase.Where(x => (x.city_id == cityId) && (x.society_id == societyId))
                    .Select(x => new PhaseModel()
                    {
                        id=x.id,
                        society_id=x.society_id,
                        city_id = x.city_id,
                        name = x.name,
                        lat = x.lat,
                        lng = x.lng
                    }).ToList();
            }
            return list;
        }
        public PhaseModel getPhase(int? id)
        {
            PhaseModel model = null;
            using (var context = new propertyDbEntities())
            {
                var temp = context.phase.Where(x => x.id == id).FirstOrDefault();
                model = new PhaseModel()
                {
                    id = temp.id,
                    lat = temp.lat,
                    lng = temp.lng,
                    city_id = temp.city_id,
                    
                    society_id = temp.society_id,
                    name = temp.name
                };
            }
            return model;
        }

        public bool updatePhase(int id, PhaseModel model)
        {
            using (var context = new propertyDbEntities())
            {
                var result = context.phase.SingleOrDefault(b => b.id == id);
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
