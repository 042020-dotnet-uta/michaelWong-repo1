using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebStoreApp.Domain;

namespace WebStoreApp.Web.Models
{
    public class LocationsViewModel
    {
        [Required]
        [Display(Name="Location Name")]
        public string LocationName { get; set; }

        [Display(Name="Location Names")]
        public IEnumerable<Location> Locations { get; set; }
    }
}