
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Data.POCOs
{
    //Rental POCO
    public class EquipmentSelection
    {
        public int eqiupmentID { get; set; }
        public string Description { get; set; }
        public string SerailNumber { get; set; }
        public decimal Rate { get; set; }
    }
}
