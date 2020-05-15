using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebStoreApp.Domain;

namespace WebStoreApp.Web.Models
{
    public class LocationsModel
    {
        [Display(Name="Locations")]
        public List<LocationModel> LocationModels { get; set; }
    }
}