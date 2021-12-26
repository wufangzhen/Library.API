using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.API.Entities
{
    public class Group
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public int UpGroupCode { get; set; }

        public int Level { get; set; }

        public int GroupCode { get; set; }

        public string Name { get; set; }
    }
}
