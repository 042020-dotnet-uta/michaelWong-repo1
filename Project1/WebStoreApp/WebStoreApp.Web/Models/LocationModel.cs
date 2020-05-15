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
        public string LocationName { get; set; }
    }
}