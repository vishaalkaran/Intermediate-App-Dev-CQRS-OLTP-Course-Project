using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Data.POCOs
{
    public class OpenPurchaseOrderDetails
    {
        public int PurchaseOrderDetailID { get; set; }
        public string VendorStockNumber { get; set; }
        public int StockItemID { get; set; }
        public string StockItemDescription { get; set; }
        public int QuantityOnOrder { get; set; }
        public int QuantityOutstanding { get; set; }
        public int ReceivedQuantity { get; set; }
        public int ReturnedQuantity { get; set; }
        public string ReturnReason { get; set; }
    }
}
