using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace WebStoreApp.Domain
{
    public class Location
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [Display(Name="Location Name")]
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}