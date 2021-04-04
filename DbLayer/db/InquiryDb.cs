using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer;
namespace DbLayer.db
{
    public class InquiryDb
    {
        public int addInquiry(InquiryModel model)
        {
            using (var context = new propertyDbEntities())
            {
                inquiry dbObj = new inquiry()
                {
                    name = model.name,
                    description = model.description,
                    phone = model.phone,
                    email = model.email,
                    date=model.date,
                    property_id=model.property_id
                };
                context.inquiry.Add(dbObj);
                context.SaveChanges();
                return dbObj.id;
            }
        }
        public bool delete(int id)
        {
            using (var context = new propertyDbEntities())
            {
                inquiry inq = context.inquiry.Find(id);
                if (inq != null)
                {
                    context.inquiry.Remove(inq);
                    context.SaveChanges();
                    return true;
                }
                else {
                    return false;
                }    
            }
        }

        public List<InquiryModel> getList()
        {
            List<InquiryModel> list = new List<InquiryModel>();
            using (var context = new propertyDbEntities())
            {
                list = context.inquiry.OrderByDescending(x=>x.id)
                    .Select(x => new InquiryModel()
                    {
                        id = x.id,
                        name = x.name,
                         phone = x.phone,  
                         email=x.email,
                         date=x.date,
                        description = x.description,
                        property_id = x.property_id
                    }).ToList();
            }
            return list;
        }
    }
}
