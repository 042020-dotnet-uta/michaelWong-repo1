using System;
using System.Data;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebStoreApp.Web.Models
{
    public class OrderModel
    {
        [Required]
        public Guid? ProductId { get; set; }

        [Required]
        public Guid? LocationId { get; set; }

        [Required]
        [Display(Name="order Quantity")]
        [Range(0, Int32.MaxValue)]
        public int Quantity { get; set; }

        [Display(Name="Product Name")]
        public string ProductName { get; set; }

        [Display(Name="Product Price")]
        public decimal ProductPrice { get; set; }
    }
}