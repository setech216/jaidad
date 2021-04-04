using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class PropertyGetModel
    {
        public int id { get; set; }
        public string type_name { get; set; }
        public string purpose { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public double? area { get; set; }
        public string area_unit { get; set; }
        public string userId { get; set; }
        public int? price { get; set; }
        public string image { get; set; }
        public string dealer_name { get; set; }
        public string dealer_phone { get; set; }
        public string price_with_comma { get; set; }
        public string city_name { get; set; }
        public string society_name { get; set; }
        public string phase_name { get; set; }
        public string block_name { get; set; }
        public string plot_no { get; set; }
        public string plot_type { get; set; }
        public string affidavit_price_with_comma { get; set; }
        public bool? isAvailable { get; set; }
        public bool? isSelected { get; set; }
        public bool? isCorner { get; set; }
    }
}
