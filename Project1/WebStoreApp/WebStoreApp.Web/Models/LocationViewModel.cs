using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebStoreApp.Domain;

namespace WebStoreApp.Web.Models
{
    public class LocationViewModel
    {
        public Location Location { get; set; }
        public List<ProductViewModel> Products { get; set; }

        public string State { get; set; }

        [Required]
        public Guid? Id { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Price")]
        public decimal ProductPrice { get; set; }

        [Required]
        [Range(0, Int32.MaxValue, ErrorMessage = "Value should not be negative.")]
        [Display(Name = "Quantity")]
        public int ProductQuantity { get; set; }

        public Guid? ProductId { get; set; }
    }
}