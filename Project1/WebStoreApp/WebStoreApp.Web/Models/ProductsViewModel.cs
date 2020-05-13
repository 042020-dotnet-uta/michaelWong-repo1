using System;
using System.Collections.Generic;

namespace WebStoreApp.Web.Models
{
    public class ProductsViewModel
    {
        public Guid? LocationId { get; set; }
        public List<ProductItemViewModel> Products { get; set; }
        public List<OrderItemViewModel> Orders { get; set; }
    }
}