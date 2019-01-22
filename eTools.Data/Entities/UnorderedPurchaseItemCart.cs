using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UnorderedPurchaseItemCart")]
    public partial class UnorderedPurchaseItemCart
    {
        [Key]
        public int CartID { get; set; }

        public int PurchaseOrderID { get; set; }

        public string Description { get; set; }

        public string VendorStockNumber { get; set; }

        public int Quantity { get; set; }
    }
}
