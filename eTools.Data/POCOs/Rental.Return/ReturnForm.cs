using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Data.POCOs 
{
    public class ReturnForm
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string SerialNumber { get; set; }
        public decimal Rate { get; set; }
        public DateTime OutDate { get; set; }
        public string CoditionCommets { get; set; }
        public string CustomerCommets { get; set; }
        public bool Av { get; set; }
    }
}
