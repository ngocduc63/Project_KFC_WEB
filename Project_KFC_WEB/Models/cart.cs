namespace Project_KFC_WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("cart")]
    public partial class cart
    {
        [StringLength(50)]
        public string id { get; set; }

        [StringLength(50)]
        public string idFood { get; set; }

        public int? quantity { get; set; }

        public double? totalPrice { get; set; }

        public virtual food food { get; set; }
    }
}
