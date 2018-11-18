using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Data.POCOs
{
    public class returnCustomer
    {
        public int rentalid { get; set; }
        public int customerid { get; set; }
        public string fullname { get; set; }
        public string address { get; set; }
        public DateTime mmddyy { get; set; }
    }
}
