using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class PropertyUpdateModel
    {
        public int id { get; set; }
        public string dealer_id { get; set; }
        public string purpose { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public double? area { get; set; }
        public string area_unit { get; set; }
        public int price { get; set; }
        public string price_with_comma { get; set; }
        public string plot_no { get; set; }
        public string plot_type { get; set; }
        public Nullable<double> area_in_marla { get; set; }
        public string affidavit_price { get; set; }
        public string affidavit_price_with_comma { get; set; }
        public bool isCorner { get; set; }
        public string owner_name { get; set; }
        public string owner_contact1 { get; set; }
        public string owner_contact2 { get; set; }
    }
}
