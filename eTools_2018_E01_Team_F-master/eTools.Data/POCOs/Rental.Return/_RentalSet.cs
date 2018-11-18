using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Data.POCOs
{
    //Return POCO
    public class RentalSet
    {
        public string Fullname { get; set; }
        public string Address { get; set; }
        public DateTime RentalDate { get; set; }
        public string PhoneNumber { get; set; }
    }
}
