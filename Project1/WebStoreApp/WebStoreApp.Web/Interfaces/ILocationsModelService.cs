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
        Task<LocationsModel> GetLocations();
        Task CreateLocation(LocationModel locationModel);
        Task DeleteLocation(LocationModel locationModel);
        Task EditLocation(LocationModel locationModel);

        Task<LocationModel> GetLocationDetails(Guid? id);
        Task<ProductsModel> GetLocationProducts(Guid? id);
        Task CreateNewProduct(ProductModel productModel);
        Task EditProduct(ProductModel productModel);
        Task DeleteProduct(ProductModel productModel);
        Task PlaceOrders(OrdersModel ordersModel);
    }
}