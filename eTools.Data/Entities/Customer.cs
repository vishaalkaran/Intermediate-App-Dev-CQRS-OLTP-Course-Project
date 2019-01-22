namespace eTools.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            Rentals = new HashSet<Rental>();
        }

        public int CustomerID { get; set; }

        [Required(ErrorMessage = "LastName is Required")]
        [StringLength(50, ErrorMessage = "LastName should not be more than 50 words")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "FirstName is Required")]
        [StringLength(50, ErrorMessage = "FirstName should not be more than 50 words")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Address is Required")]
        [StringLength(75, ErrorMessage = "Address should not be more than 75 words")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is Required")]
        [StringLength(30, ErrorMessage = "City should not be more than 30 words")]
        public string City { get; set; }

        [Required]
        [StringLength(2)]
        public string Province { get; set; }

        [Required(ErrorMessage = "PostalCode is Required")]
        [StringLength(6, ErrorMessage = "PostalCode should not be more than 6 words")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "ContactPhone is Required")]
        [StringLength(12, ErrorMessage = "ContactPhone should not be more than 12 words")]
        public string ContactPhone { get; set; }

        [Required(ErrorMessage = "EmailAddress is Required")]
        [StringLength(50, ErrorMessage = "EmailAddress should not be more than 50 words")]
        public string EmailAddress { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rental> Rentals { get; set; }
    }
}
