using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebStoreApp.Web.Models
{
    public class ProductEditViewModel
    {
        [Required]
        public Guid? LocationId { get; set; }

        [Required]
        public Guid? ProductId { get; set; }

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
    }
}