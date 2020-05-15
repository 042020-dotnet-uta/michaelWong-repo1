using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebStoreApp.Domain;

namespace WebStoreApp.Web.Models
{
    public class LocationsViewModel
    {
        public LocationModel LocationModel { get; set; }
        public LocationsModel LocationsModel { get; set; }
    }
}