using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GeneralStoreApII.Models
{
    public class Transactions
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ItemCount { get; set; }

        [Required]
        public DateTime TimeOfTransaction { get; set; }

        [ForeignKey(nameof(Product))]
        public string Sku { get; set; }
        public virtual Products Product { get; set; }

        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
    }
}