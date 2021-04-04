using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
   public class PhaseModel
    {
        public int id { get; set; }
        public int? city_id { get; set; }
        public int? society_id { get; set; }
        public string name { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
    }
}
