using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class PropertyEditingGetModel
    {
        public int id { get; set; }
        public PropertyTypeModel propertyTypeModel { get; set; }
        public UserModel dealerModel { get; set; }
        public string purpose { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public double? area { get; set; }
        public string area_unit { get; set; }
        public int? price { get; set; }
        public string image { get; set; }      
        public string plot_no { get; set; }
        public string plot_type { get; set; }
        public string affidavit_price { get; set; }
        public string isCorner { get; set; }
        public string ownerName { get; set; }
        public string ownerContact1 { get; set; }
        public string ownerContact2 { get; set; }
    }
}
