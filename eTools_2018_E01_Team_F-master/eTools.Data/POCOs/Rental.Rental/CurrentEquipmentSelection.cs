using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Data.POCOs
{
    public class CurrentEquipmentSelection
    {
        public int eqiupmentID { get; set; }
        public string Description { get; set; }
        public string SerailNumber { get; set; }
        public decimal Rate { get; set; }
        public DateTime outDate { get; set; }
    }
}
