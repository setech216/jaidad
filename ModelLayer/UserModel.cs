﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
   public class UserModel
    {

        public string Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }     
        public string Name { get; set; }
        public Nullable<int> roleId { get; set; }
    }
}
