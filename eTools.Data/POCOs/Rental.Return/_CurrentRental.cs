using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Data.POCOs
{
    //Return POCO
    public class CurrentRental
    {
        public int eqiupmentID { get; set; }
        public string Description { get; set; }
        public string SerailNumber { get; set; }
        public double Rate { get; set; }
        public DateTime OutDate { get; set; }
        public string Comments { get; set; }
        public bool Available { get; set; }

    }
}
