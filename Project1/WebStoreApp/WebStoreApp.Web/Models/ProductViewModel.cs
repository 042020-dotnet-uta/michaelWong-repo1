using System;

namespace WebStoreApp.Web.Models
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set ;}
        public int Quantity { get; set; }
    }
}