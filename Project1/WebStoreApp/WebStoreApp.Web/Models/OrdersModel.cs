using System;
using System.Collections.Generic;

namespace WebStoreApp.Web.Models
{
    public class OrdersModel
    {
        public List<OrderModel> OrderModels { get; set; }
        public Guid? LocationId { get; set; }
    }
}