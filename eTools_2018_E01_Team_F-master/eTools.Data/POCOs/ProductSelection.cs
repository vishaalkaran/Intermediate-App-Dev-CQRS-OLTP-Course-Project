using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Data.POCOs
{
    public class ProductSelection
    {
        public int StockItemID { get; set; }
        public int QuantitySelected { get; set; }
        public decimal SellingPrice { get; set; }
        public string Description { get; set; }
        public int QuantityOnHand { get; set; }
    }
}
