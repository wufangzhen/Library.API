using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Entities
{
    public class SelfCommodity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Barcode { get; set; }


        public string Name { get; set; }

        public string SelfCode { get; set; }

        [MaxLength(50)]
        public string Standard { get; set; }

        [MaxLength(50)]
        public string Unit { get; set; }

        [MaxLength(50)]
        public string FirstClassify { get; set; }

        [MaxLength(50)]
        public string SecondClassify { get; set; }

        [MaxLength(50)]
        public string Tag { get; set; }

        public decimal Repertory { get; set; }

        public decimal PriceFixing { get; set; }

        public decimal Cost { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [ForeignKey("GroupId")]
        public Group Group  { get; set; }

        public Guid GroupId { get; set; }


    }
}
