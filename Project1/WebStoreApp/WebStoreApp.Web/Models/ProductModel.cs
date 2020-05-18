using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebStoreApp.Web.Models
{
    public class ProductModel
    {
        public Guid? ProductId { get; set; }
        
        [Required]
        public Guid? LocationId { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        [RegularExpression(@"^(?=.*[A-Za-z]).*$", ErrorMessage = "Product name must contain at least one letter.")]
        public string ProductName { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(0.01, Double.MaxValue, ErrorMessage = "Invalid price input.")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Price")]
        public decimal ProductPrice { get; set; }

        [Required]
        [Range(0, Int32.MaxValue, ErrorMessage = "Product inventory cannot be negative.")]
        [Display(Name = "Stock")]
        public int ProductQuantity { get; set; }
    }
}