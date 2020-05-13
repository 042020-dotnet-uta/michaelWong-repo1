using System;
using System.ComponentModel.DataAnnotations;

namespace WebStoreApp.Web.Models
{
    public class ProductDeleteViewModel
    {
        [Required]
        public Guid? LocationId { get; set; }

        [Required]
        public Guid? ProductId { get; set; }
    }
}