using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Data.POCOs
{
    public class CurrentOrderSummary
    {
        public int PurchaseOrderID { get; set; }
        public int? PurchaseOrderNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public int VendorID { get; set; }
        public string VendorName { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }        
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public int OrderDetailCount { get; set; }


    }
}
