namespace Project_KFC_WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("food")]
    public partial class food
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public food()
        {
            carts = new HashSet<cart>();
        }

        [StringLength(50)]
        public string id { get; set; }

        [StringLength(50)]
        public string idCategory { get; set; }

        [StringLength(50)]
        public string name { get; set; }

        [StringLength(50)]
        public string image { get; set; }

        public double? price { get; set; }

        public double? discount { get; set; }

        [StringLength(100)]
        public string description { get; set; }

        [Column(TypeName = "date")]
        public DateTime? timeSellStart { get; set; }

        [Column(TypeName = "date")]
        public DateTime? timeSellEnd { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cart> carts { get; set; }

        public virtual foodCategory foodCategory { get; set; }
    }
}
