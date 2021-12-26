using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.API.Entities
{
    public class CommodityForUser
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        public string Barcode { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Number { get; set; }
        [Required]
        public DateTimeOffset PurchaseDate { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey("Commodity")]
        public Commodity Commodity { get; set; }

        public Guid CommodityId { get; set; }
    }
}
