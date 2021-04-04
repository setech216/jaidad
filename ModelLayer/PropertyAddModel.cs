using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class PropertyAddModel
    {
        public int id { get; set; }
        public string purpose { get; set; }
        public Nullable<int> type_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public Nullable<int> city_id { get; set; }
        public Nullable<int> society_id { get; set; }
        public Nullable<int> phase_id { get; set; }
        public Nullable<int> block_id { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public Nullable<double> area { get; set; }
        public string area_unit { get; set; }
        public Nullable<double> area_in_marla { get; set; }
        public int price { get; set; }
        public Nullable<bool> is_corner { get; set; }
        public string image { get; set; }
        public string owner_name { get; set; }
        public string owner_contact1 { get; set; }
        public string owner_contact2 { get; set; }
        public Nullable<bool> is_available { get; set; }
        public string dealer_id { get; set; }
        public string price_with_comma { get; set; }
        public Nullable<int> views { get; set; }
        public Nullable<bool> isSelected { get; set; }
        public string userId { get; set; }
        public string type_name { get; set; }
        public string city_name { get; set; }
        public string society_name { get; set; }
        public string phase_name { get; set; }
        public string block_name { get; set; }
        public string plot_no { get; set; }
        public string plot_type { get; set; }
        public string affidavit_price_with_comma { get; set; }
        public string affidavit_price { get; set; }
    }
}
