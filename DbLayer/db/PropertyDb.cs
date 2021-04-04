using ModelLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbLayer.db
{
    public class PropertyDb
    {
        public int addProperty(PropertyAddModel model)
        {
            using (var context = new propertyDbEntities())
            {
                property dbObj = new property()
                {
                    purpose = model.purpose,
                    type_id = model.type_id,
                    title = model.title,
                    description = model.description,
                    society_id = model.society_id,
                    city_id = model.city_id,
                    phase_id = model.phase_id,
                    block_id = model.block_id,
                    lat = model.lat == null ? "0" : model.lat,
                    lng = model.lng == null ? "0" : model.lng,
                    area = model.area,
                    area_in_marla = model.area_in_marla,
                    area_unit = model.area_unit,
                    price = model.price,
                    is_corner = model.is_corner,
                    image = model.image,
                    owner_name = model.owner_name,
                    owner_contact1 = model.owner_contact1,
                    owner_contact2 = model.owner_contact2,
                    is_available = model.is_available,
                    dealer_id = model.dealer_id,
                    affidavit_price=model.affidavit_price,
                    type_name = model.type_name,
                    affidavit_price_with_comma=model.affidavit_price_with_comma,
                    price_with_comma = model.price_with_comma,
                    views = 0,
                    isSelected = model.isSelected,
                    userId = model.userId,
                    plot_no=model.plot_no,
                    plot_type=model.plot_type,
                    city_name=model.city_name,
                    society_name=model.society_name,
                    phase_name=model.phase_name,
                    block_name=model.block_name
                };
                context.property.Add(dbObj);
                context.SaveChanges();
                return dbObj.id;
            }
        }
        public PropertyGetModel getProperty(int id)
        {
            PropertyGetModel model = null;
            using (var context = new propertyDbEntities())
            {
                var temp = context.property.Where(x => x.id == id).FirstOrDefault();
                model = new PropertyGetModel()
                {
                    id = temp.id,
                    type_name = temp.type_name,
                    title = temp.title,
                    description = temp.description,
                    purpose = temp.purpose,
                    userId=temp.userId,
                    lat = temp.lat,
                    lng = temp.lng,
                    area = temp.area,
                    area_unit = temp.area_unit,
                    price = temp.price,
                    image = temp.image,
                    price_with_comma = temp.price_with_comma,
                    city_name = temp.city_name,
                    society_name = temp.society_name,
                    phase_name = temp.phase_name,
                    block_name = temp.block_name,
                    plot_no=temp.plot_no,
                    plot_type=temp.plot_type,
                    affidavit_price_with_comma=temp.affidavit_price_with_comma,
                    isSelected=temp.isSelected,
                    isAvailable=temp.is_available,
                    isCorner=temp.is_corner
                };
                if (temp.AspNetUsers != null)
                {
                    model.dealer_name = temp.AspNetUsers.name;
                    model.dealer_phone = temp.AspNetUsers.PhoneNumber;
                }
                
            }

            return model;
        }

        public PropertyEditingGetModel getPropertyForEditing(int id)
        {
            PropertyEditingGetModel model = null;
            using (var context = new propertyDbEntities())
            {
                var temp = context.property.Where(x => x.id == id).FirstOrDefault();
                model = new PropertyEditingGetModel()
                {
                    id = temp.id,
                    title = temp.title,
                    description = temp.description,
                    purpose = temp.purpose,
                    area = temp.area,
                    area_unit = temp.area_unit,
                    price = temp.price,
                    plot_no = temp.plot_no,
                    plot_type = temp.plot_type,
                    affidavit_price = temp.affidavit_price,
                    ownerContact1=temp.owner_contact1,
                    ownerContact2 = temp.owner_contact2,
                    ownerName = temp.owner_name
                };
                if (temp.property_type != null)
                {
                    model.propertyTypeModel = new PropertyTypeModel();
                    model.propertyTypeModel.id = temp.property_type.id;
                    model.propertyTypeModel.name = temp.property_type.name;
                }
                
                if (temp.AspNetUsers1 != null)
                {
                    model.dealerModel = new UserModel();
                    model.dealerModel.Id = temp.AspNetUsers1.Id;
                    model.dealerModel.Name = temp.AspNetUsers1.name;
                }
                if(temp.is_corner==true)
                {
                    model.isCorner = "Yes";
                }
                else
                {
                    model.isCorner = "No";
                }
            }

            return model;
        }
        public List<PropertyListingModel> getPropertyList(string query)
        {
            string cs = @"Trusted_Connection=True;Initial Catalog=propertyDb;Data Source=DESKTOP-NT6624J";
           // var context = new propertyDbEntities();
           // var efConnection = context.Database.Connection;

           // var efConn = new System.Data.EntityClient.EntityConnectionStringBuilder(efConnection);
          //  string adoConn = efConn.ProviderConnectionString;

            using (SqlConnection connection = new SqlConnection(cs))
            {
                List<PropertyListingModel> list = new List<PropertyListingModel>();
                var sql = @query;
                // Create a SqlCommand, and identify it as a stored procedure.
                using (SqlCommand sqlCommand = new SqlCommand(sql, connection))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    try
                    {
                        connection.Open();

                        SqlDataReader reader = sqlCommand.ExecuteReader();
                        if (reader != null && reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                PropertyListingModel obj = new PropertyListingModel();
                                obj.id = Convert.ToInt32(reader["id"]);
                                obj.type_name = reader["type_name"].ToString();
                                obj.title = reader["title"].ToString();
                                obj.description = reader["description"].ToString();
                                obj.purpose = reader["purpose"].ToString();
                                obj.lat = reader["lat"].ToString();
                                obj.lng = reader["lng"].ToString();
                                obj.area = float.Parse(reader["area"].ToString());
                                obj.area_unit = reader["area_unit"].ToString();
                                obj.price = Convert.ToInt32(reader["price"]);
                                obj.image = reader["image"].ToString();
                                obj.dealer_name = reader["name"].ToString();
                                obj.dealer_phone = reader["PhoneNumber"].ToString();
                                obj.price_with_comma = reader["price_with_comma"].ToString();
                                obj.city_name = reader["city_name"].ToString();
                                obj.society_name = reader["society_name"].ToString();
                                obj.phase_name = reader["phase_name"].ToString();
                                obj.block_name = reader["block_name"].ToString();
                                obj.plot_type = reader["plot_type"].ToString();
                                obj.plot_no = reader["plot_no"].ToString();
                                obj.affidavit_price_with_comma = reader["affidavit_price_with_comma"].ToString();
                                obj.isSelected = bool.Parse(reader["isSelected"].ToString());
                                obj.isAvailable = bool.Parse(reader["is_available"].ToString());
                                obj.dealer_id = reader["dealer_id"].ToString();
                                list.Add(obj);
                            }
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.InnerException.ToString();
                    }
                    finally
                    {
                        connection.Close();
                    }
                    return list;
                }
            }

        }

        public List<PropertyListingModel> getUnselectedProperties()
        {
            List<PropertyListingModel> list = null; 
            using (var context = new propertyDbEntities())
            {
                list = context.property.Where(x => x.isSelected == false).OrderByDescending(x => x.id)
                    .Select(temp => new PropertyListingModel()
                    {
                        id = temp.id,
                        type_name = temp.type_name,
                        title = temp.title,
                        description = temp.description,
                        purpose = temp.purpose,
                        lat = temp.lat,
                        lng = temp.lng,
                        area = temp.area,
                        area_unit = temp.area_unit,
                        image = temp.image,
                        price_with_comma = temp.price_with_comma,
                        city_name = temp.city_name,
                        society_name = temp.society_name,
                        phase_name = temp.phase_name,
                        block_name = temp.block_name,
                        plot_no = temp.plot_no,
                        plot_type = temp.plot_type,
                        affidavit_price_with_comma = temp.affidavit_price_with_comma,
                        isSelected = temp.isSelected,
                        dealer_id=temp.dealer_id,
                isAvailable = temp.is_available,
                        dealer_name = temp.AspNetUsers==null?null: temp.AspNetUsers.name,
                        dealer_phone = temp.AspNetUsers == null ? null : temp.AspNetUsers.PhoneNumber
                    }).ToList();
            }
            return list;
        }

        public bool updateProperty(int id,PropertyUpdateModel model)
        {
            using (var context = new propertyDbEntities())
            {
                var result = context.property.SingleOrDefault(b => b.id == id);
                if (result != null)
                {
                    result.title = model.title;
                    result.purpose = model.purpose;
                    result.description = model.description;
                    result.dealer_id = model.dealer_id;
                    result.is_corner = model.isCorner;
                    result.area = model.area;
                    result.area_unit = model.area_unit;
                    result.area_in_marla = model.area_in_marla;
                    result.price = model.price;
                    result.price_with_comma = model.price_with_comma;
                    result.title = model.title;
                    result.plot_no = model.plot_no;
                    result.plot_type = model.plot_type;
                    result.affidavit_price = model.affidavit_price;
                    result.affidavit_price_with_comma = model.affidavit_price_with_comma;
                    result.owner_contact1 = model.owner_contact1;
                    result.owner_contact2 = model.owner_contact2;
                    result.owner_name = model.owner_name;
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<PropertyListingModel> getUnAvailableProperties()
        {
            List<PropertyListingModel> list = null;
            using (var context = new propertyDbEntities())
            {
                list = context.property.Where(x => x.is_available == false).OrderByDescending(x => x.id)
                    .Select(temp => new PropertyListingModel()
                    {
                        id = temp.id,
                        type_name = temp.type_name,
                        title = temp.title,
                        description = temp.description,
                        purpose = temp.purpose,
                        lat = temp.lat,
                        lng = temp.lng,
                        area = temp.area,
                        area_unit = temp.area_unit,
                        image = temp.image,
                        price_with_comma = temp.price_with_comma,
                        city_name = temp.city_name,
                        society_name = temp.society_name,
                        phase_name = temp.phase_name,
                        block_name = temp.block_name,
                        plot_no = temp.plot_no,
                        plot_type = temp.plot_type,
                        affidavit_price_with_comma = temp.affidavit_price_with_comma,
                        isSelected = temp.isSelected,
                        isAvailable = temp.is_available,
                        dealer_name = temp.AspNetUsers == null ? null : temp.AspNetUsers.name,
                        dealer_phone = temp.AspNetUsers == null ? null : temp.AspNetUsers.PhoneNumber
                    }).ToList();
            }
            return list;
        }

        public List<PropertyListingModel> getMyProperties(string userId)
        {
            List<PropertyListingModel> list = null;
            using (var context = new propertyDbEntities())
            {
                list = context.property.Where(x => x.userId == userId).OrderByDescending(x => x.id)
                    .Select(temp => new PropertyListingModel()
                    {
                        id = temp.id,
                        type_name = temp.type_name,
                        title = temp.title,
                        description = temp.description,
                        purpose = temp.purpose,
                        lat = temp.lat,
                        lng = temp.lng,
                        area = temp.area,
                        area_unit = temp.area_unit,
                        image = temp.image,
                        price_with_comma = temp.price_with_comma,
                        city_name = temp.city_name,
                        society_name = temp.society_name,
                        phase_name = temp.phase_name,
                        block_name = temp.block_name,
                        plot_no = temp.plot_no,
                        plot_type = temp.plot_type,
                        affidavit_price_with_comma = temp.affidavit_price_with_comma,
                        isSelected = temp.isSelected,
                        isAvailable = temp.is_available,
                        dealer_name = temp.AspNetUsers == null ? null : temp.AspNetUsers.name,
                        dealer_phone = temp.AspNetUsers == null ? null : temp.AspNetUsers.PhoneNumber
                    }).ToList();
            }
            return list;
        }

        public bool delete(int id)
        {
            using (var context = new propertyDbEntities())
            {
                deleteFromInquiry(id);
                property prop = context.property.Find(id);
                if (prop != null)
                {
                    
                    context.property.Remove(prop);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void deleteFromInquiry(int propId)
        {
            using (var context = new propertyDbEntities())
            {
                var inquiryArray = context.inquiry.Where(x=>x.property_id==propId);
                foreach (var inq in inquiryArray)
                {
                    context.inquiry.Remove(inq);
                    
                }
                context.SaveChanges();
            }
        }

        public bool markAsAvailable(int id)
        {
            using (var context = new propertyDbEntities())
            {
                var result = context.property.SingleOrDefault(b => b.id == id);
                if (result != null)
                {
                    result.is_available = true;
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool markAsUnAvailable(int id)
        {
            using (var context = new propertyDbEntities())
            {
                var result = context.property.SingleOrDefault(b => b.id == id);
                if (result != null)
                {
                    result.is_available = false;
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool markAsSelected(int id,string dealerId)
        {
            using (var context = new propertyDbEntities())
            {
                var result = context.property.SingleOrDefault(b => b.id == id);
                if (result != null)
                {
                    result.isSelected = true;
                    if(dealerId!="")
                    {
                        result.dealer_id = dealerId;
                    }
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool markAsUnSelected(int id)
        {
            using (var context = new propertyDbEntities())
            {
                var result = context.property.SingleOrDefault(b => b.id == id);
                if (result != null)
                {
                    result.isSelected = false;
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
