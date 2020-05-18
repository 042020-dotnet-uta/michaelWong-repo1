using System;
using System.ComponentModel.DataAnnotations;
using WebStoreApp.Domain;

namespace WebStoreApp.Web.Models
{
    public class LocationModel
    {
        public Guid? LocationId { get; set; }

        [Required]
        [Display(Name="Location Name")]
        [RegularExpression(@"^(?=.*[A-Za-z0-9]).*$", ErrorMessage = "Location name must contain at least one letter or number.")]
        public string LocationName { get; set; }
    }
}