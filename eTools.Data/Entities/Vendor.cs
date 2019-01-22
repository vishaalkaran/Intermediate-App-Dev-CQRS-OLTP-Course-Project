namespace eTools.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Vendor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vendor()
        {
            PurchaseOrders = new HashSet<PurchaseOrder>();
            StockItems = new HashSet<StockItem>();
        }

        public int VendorID { get; set; }

        [Required(ErrorMessage = "VendorName is Required")]
        [StringLength(100, ErrorMessage = "VendorName should not be more than 100 words")]
        public string VendorName { get; set; }

        [Required(ErrorMessage = "Phone is Required")]
        [StringLength(12, ErrorMessage = "Phone should not be more than 12 words")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Address is Required")]
        [StringLength(30, ErrorMessage = "Address should not be more than 30 words")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is Required")]
        [StringLength(50, ErrorMessage = "City should not be more than 50 words")]
        public string City { get; set; }

        [Required(ErrorMessage = "Province is Required")]
        [StringLength(2, ErrorMessage = "Province should not be more than 2 words")]
        public string Province { get; set; }

        [Required(ErrorMessage = "PostalCode is Required")]
        [StringLength(6, ErrorMessage = "PostalCode should not be more than 6 words")]
        public string PostalCode { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockItem> StockItems { get; set; }
    }
}
