using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Data.POCOs
{
    public class CartSelection
    {
        public int ShoppingCartitemID { get; set; }
        public decimal SellingPrice { get; set; }
        public string Description { get; set; }
        public decimal Total { get { return SellingPrice * QuantitySelected; } set { } }
        public int QuantitySelected { get; set; }
    }
}
