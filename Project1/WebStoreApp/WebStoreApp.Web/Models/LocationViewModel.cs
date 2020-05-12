using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebStoreApp.Domain;

namespace WebStoreApp.Web.Models
{
    public class LocationViewModel
    {
        public Location Location;
        public IEnumerable<Product> Products;
    }
}