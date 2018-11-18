using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Data.POCOs
{
    public class auxReturnInfo
    {
        public int rentalid { get; set; }
        public string creditcard { get; set; }
        public DateTime dateout { get; set; }
        public double subtotal { get; set; }
        public double gst { get; set; }
        public double discount { get; set; }
        public double total { get; set; }
    }
}
