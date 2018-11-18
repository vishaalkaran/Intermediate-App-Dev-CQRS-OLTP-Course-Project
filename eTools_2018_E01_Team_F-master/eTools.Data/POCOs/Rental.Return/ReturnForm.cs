using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Data.POCOs 
{
    public class ReturnForm
    {
        public int eqiupmentID { get; set; }
        public string Description { get; set; }
        public string SerialNumber { get; set; }
        public decimal Rate { get; set; }
        public DateTime outDate { get; set; }
        public string coditionCommets { get; set; }
        public string customerCommets { get; set; }
        public bool Av { get; set; }
    }
}
