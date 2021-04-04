using ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbLayer.db
{
    public class BlockDb
    {
        public int addBlock(BlockModel model)
        {
            using (var context = new propertyDbEntities())
            {
                block dbObj = new block()
                {
                    name = model.name,
                    phase_id = model.phase_id,
                    society_id = model.society_id,
                    city_id = model.city_id,
                    lat = model.lat == null ? "0" : model.lat,
                    lng = model.lng == null ? "0" : model.lng

                };
                context.block.Add(dbObj);
                context.SaveChanges();
                return dbObj.id;
            }
        }
        public List<BlockModel> getBlocks(int cityId, int societyId,int? phaseId)
        {
            List<BlockModel> list = new List<BlockModel>();
            using (var context = new propertyDbEntities())
            {
                list = context.block.Where(x => (x.city_id == cityId) && (x.society_id == societyId) && (x.phase_id == phaseId))
                    .Select(x => new BlockModel()
                    {
                        id = x.id,
                        phase_id=x.phase_id,
                        society_id = x.society_id,
                        city_id = x.city_id,
                        name = x.name,
                        lat = x.lat,
                        lng = x.lng
                    }).ToList();
            }
            return list;
        }
        public BlockModel getBlock(int? id)
        {
            BlockModel model = null;
            using (var context = new propertyDbEntities())
            {
                var temp = context.block.Where(x => x.id == id).FirstOrDefault();
                model = new BlockModel()
                {
                    id = temp.id,
                    lat = temp.lat,
                    lng = temp.lng,
                    city_id = temp.city_id,
                    phase_id = temp.phase_id,
                    society_id=temp.society_id,
                    name=temp.name
                };
            }
            return model;
        }

        public bool updateBlock(int id, BlockModel model)
        {
            using (var context = new propertyDbEntities())
            {
                var result = context.block.SingleOrDefault(b => b.id == id);
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
