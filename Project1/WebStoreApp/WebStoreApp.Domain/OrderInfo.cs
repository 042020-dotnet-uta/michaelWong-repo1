using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebStoreApp.Domain
{
    public class OrderInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [Display(Name="Product Name")]
        public string ProductName { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName="decimal(18,2)")]
        [Display(Name="Price")]
        public decimal ProductPrice { get; set; }
        
        [Required]
        [Range(1, Int32.MaxValue)]
        [Display(Name="Quantity")]
        public int ProductQuantity { get; set; }
    }
}