using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebStoreApp.Domain;

namespace WebStoreApp.Web.Models
{
    public class LocationsViewModel
    {
        [Display(Name="Location Name")]
        public string Name { get; set; }

        public List<Location> Locations { get; set; }
    }
}