using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.API.Entities
{
    public class ShoppingCart
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public DateTimeOffset BirthDate { get; set; }
        [Required]
        public int State { get; set; }
        [ForeignKey("UserId")]
        public Store Store { get; set; }

        public Guid StoreId { get; set; }
        public ICollection<CommodityForUser> CommodityForUsers { get; set; } = new List<CommodityForUser>();
    }
}
