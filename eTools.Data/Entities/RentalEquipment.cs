namespace eTools.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RentalEquipment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RentalEquipment()
        {
            RentalDetails = new HashSet<RentalDetail>();
        }

        public int RentalEquipmentID { get; set; }

        [Required(ErrorMessage = "Description is Required")]
        [StringLength(50, ErrorMessage = "Description should not be more than 50 words")]
        public string Description { get; set; }

        [Required(ErrorMessage = "ModelNumber is Required")]
        [StringLength(15, ErrorMessage = "Description should not be more than 15 words")]
        public string ModelNumber { get; set; }

        [Required(ErrorMessage = "SerialNumber is Required")]
        [StringLength(20, ErrorMessage = "SerialNumber should not be more than 20 words")]
        public string SerialNumber { get; set; }

        [Column(TypeName = "money")]
        public decimal DailyRate { get; set; }

        [Required(ErrorMessage = "Condition is Required")]
        [StringLength(100, ErrorMessage = "Condition should not be more than 100 words")]
        public string Condition { get; set; }

        public bool Available { get; set; }

        public bool Retired { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RentalDetail> RentalDetails { get; set; }
    }
}
