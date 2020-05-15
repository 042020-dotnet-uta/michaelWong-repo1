using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using WebStoreApp.Domain;

namespace WebStoreApp.Web.Models
{
    public class LocationViewModel
    {
        public LocationModel LocationModel { get; set; }
        public ProductsModel ProductsModel { get; set; }
        public ProductModel ProductModel { get; set; }
    }
}