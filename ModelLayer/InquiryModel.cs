using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class InquiryModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string description { get; set; }
        public Nullable<int> property_id { get; set; }
        public string date { get; set; }
    }
}
