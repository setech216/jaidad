using ModelLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbLayer.db
{
    public class UserDb
    {
       
        public UserModel getUserModel(string email)
        {
            UserModel model=null ;
            using (var context = new propertyDbEntities())
            {
                var temp = context.AspNetUsers.Where(x => x.Email==email).FirstOrDefault();
                if(temp!=null)
                {
                    model = new UserModel()
                    {
                        Email = temp.Email,
                        Id = temp.Id,
                        Name = temp.name,
                        roleId = temp.roleId,
                        PhoneNumber = temp.PhoneNumber
                    };
                }
                
            }

            return model;
        }

        public UserModel getUserById(string id)
        {
            UserModel model = null;
            using (var context = new propertyDbEntities())
            {
                var temp = context.AspNetUsers.Where(x => x.Id == id).FirstOrDefault();
                model = new UserModel()
                {
                    Email = temp.Email,
                    Id = temp.Id,
                    Name = temp.name,
                    roleId = temp.roleId,
                    PhoneNumber = temp.PhoneNumber
                };
            }

            return model;
        }
        public List<UserModel> getAgents()
        {
            List<UserModel> list;
            using (var context = new propertyDbEntities())
            {
                list = context.AspNetUsers.Where(x => ( x.roleId==(int)Role.admin)|| (x.roleId == (int)Role.superAdmin)|| ( x.roleId == (int)Role.agent))
                .Select(x => new UserModel()
                {
                    Email = x.Email,
                    Id = x.Id,
                    Name = x.name,
                    roleId = x.roleId,
                    PhoneNumber = x.PhoneNumber
                }).ToList();
                
            }
            return list;
        }
        public bool updatePhone(string userId,string phone)
        {
            using (var context = new propertyDbEntities())
            {
                var user = context.AspNetUsers.First(a => a.Id == userId);
                user.PhoneNumber= phone;
                context.SaveChanges();
                return true;
            }
        }
        public bool updateName(string userId, string name)
        {
            using (var context = new propertyDbEntities())
            {
                var user = context.AspNetUsers.First(a => a.Id == userId);
                user.name = name;
                context.SaveChanges();
                return true;
            }
        }
        public bool makeAgent(string userId, int roleId)
        {
            using (var context = new propertyDbEntities())
            {
                var user = context.AspNetUsers.First(a => a.Id == userId);
                user.roleId = roleId;
                context.SaveChanges();
                return true;
            }
        }
        public bool updateEmail(string userId, string email)
        {
            using (var context = new propertyDbEntities())
            {
                var user = context.AspNetUsers.First(a => a.Id == userId);
                user.Email = email;
                user.UserName = email;
                context.SaveChanges();
                return true;
            }
        }
    }
}
