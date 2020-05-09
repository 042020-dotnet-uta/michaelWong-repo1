using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace WebStoreApp.Domain
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }
        
        [Required]
        public int Quantity { get; set; }

        [Required]
        public Guid LocationId { get; set; }
        public Location Location { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}