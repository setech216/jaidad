using ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbLayer.db
{
    public class PropertyTypeDb
    {
        public int addProopertyType(PropertyTypeModel model)
        {
            using (var context = new propertyDbEntities())
            {
                property_type obj = new property_type()
                {
                    name = model.name
                };
                context.property_type.Add(obj);
                context.SaveChanges();
                return obj.id;
            }
        }
        public List<PropertyTypeModel> gePropertyTypes()
        {
            List<PropertyTypeModel> list = new List<PropertyTypeModel>();
            using (var context = new propertyDbEntities())
            {
                list = context.property_type
                    .Select(x => new PropertyTypeModel()
                    {
                        id = x.id,
                        name = x.name
                    }).ToList();
            }
            return list;
        }
        public PropertyTypeModel gePropertyType(int id)
        {
            PropertyTypeModel obj = new PropertyTypeModel();
            using (var context = new propertyDbEntities())
            {
                var temp = context.property_type.Where(x => x.id == id).FirstOrDefault();
                obj = new PropertyTypeModel()
                {
                   id=temp.id,
                   name=temp.name
                };
            }
            return obj;
        }
    }
}
