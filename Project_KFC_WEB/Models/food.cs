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

        public int id { get; set; }

        public int? idCategory { get; set; }

        public string name { get; set; }

        public string image { get; set; }

        public double? price { get; set; }

        public double? discount { get; set; }

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
