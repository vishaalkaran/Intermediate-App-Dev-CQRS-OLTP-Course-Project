namespace eTools.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Rental
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Rental()
        {
            RentalDetails = new HashSet<RentalDetail>();
        }

        public int RentalID { get; set; }

        public int CustomerID { get; set; }

        public int EmployeeID { get; set; }

        public int? CouponID { get; set; }

        [Column(TypeName = "money")]
        public decimal SubTotal { get; set; }

        [Column(TypeName = "money")]
        public decimal TaxAmount { get; set; }

        public DateTime RentalDate { get; set; }

        [Required(ErrorMessage = "PaymentType is Required")]
        [StringLength(1, ErrorMessage = "Description should not be more than 1 Char")]
        public string PaymentType { get; set; }

        [StringLength(20, ErrorMessage = "CreditCard should not be more than 20 words")]
        public string CreditCard { get; set; }

        public virtual Coupon Coupon { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Employee Employee { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RentalDetail> RentalDetails { get; set; }
    }
}
