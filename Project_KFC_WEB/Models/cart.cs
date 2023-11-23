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
        public int id { get; set; }

        public int? idFood { get; set; }

        [StringLength(50)]
        public string userName { get; set; }

        public int? quantity { get; set; }

        public double? totalPrice { 
            get {
                if (quantity > 0)
                {
                    return quantity * food.price;
                }
                else return 0;
            }
        }

        public virtual account account { get; set; }

        public virtual food food { get; set; }
    }
}
