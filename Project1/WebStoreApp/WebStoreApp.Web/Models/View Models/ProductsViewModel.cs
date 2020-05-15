using System;
using System.Collections.Generic;

namespace WebStoreApp.Web.Models
{
    public class ProductsViewModel
    {
        public Guid? LocationId { get; set; }
        public ProductsModel ProductsModel{ get; set; }
    }
}