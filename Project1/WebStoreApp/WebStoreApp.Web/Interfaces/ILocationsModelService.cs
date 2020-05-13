using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebStoreApp.Domain;
using WebStoreApp.Web.Models;

namespace WebStoreApp.Web.Services
{
    public interface ILocationsModelService
    {
        Task<List<Location>> GetLocations();
        Task CreateLocation(Location location);

        Task<Location> GetLocationDetails(Guid? id);
        Task<List<ProductViewModel>> GetLocationProducts(Guid? id);
        Task CreateNewProduct(LocationViewModel locationViewModel);
        Task EditProduct(LocationViewModel locationViewModel);
        Task DeleteProduct(LocationViewModel locationViewModel);
    }
}