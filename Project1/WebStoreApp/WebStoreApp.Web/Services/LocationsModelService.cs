using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStoreApp.Domain;
using WebStoreApp.Web.Models;
using WebStoreApp.Data;

namespace WebStoreApp.Web.Services
{
    public class LocationsModelService : ILocationsModelService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly ILogger<LocationsModelService> _logger;

        public LocationsModelService(WebStoreAppContext context, ILogger<LocationsModelService> logger)
        {
            this._unitOfWork = new UnitOfWork(context);
            this._logger = logger;
        }

        public async Task<LocationsModel> GetLocations()
        {
            var locations = (await _unitOfWork.LocationRepository.All()).ToList();
            var locationsModel = new LocationsModel
            {
                LocationModels = new List<LocationModel>()
            };

            foreach (var location in locations)
            {
                locationsModel.LocationModels.Add(new LocationModel
                {
                    LocationId = location.Id,
                    LocationName = location.Name
                });
            }

            return locationsModel;
        }

        public async Task CreateLocation(LocationModel locationModel)
        {
            var location = new Location
            {
                Name = locationModel.LocationName
            };
            await _unitOfWork.LocationRepository.Insert(location);
            await _unitOfWork.Save();
        }

        public async Task EditLocation(LocationModel locationModel)
        {
            var location = await _unitOfWork.LocationRepository.GetById(locationModel.LocationId);
            if (location != null)
            {
                location.Name = locationModel.LocationName;
                await _unitOfWork.LocationRepository.Update(location);
                await _unitOfWork.Save();
            }
        }

        public async Task DeleteLocation(LocationModel locationModel)
        {
            var location = await _unitOfWork.LocationRepository.GetById(locationModel.LocationId);
            if (location != null)
            {
                await _unitOfWork.LocationRepository.Delete(location);
                await _unitOfWork.Save();
            }
        }

        public async Task<LocationModel> GetLocationDetails(Guid? id)
        {
            var location = await _unitOfWork.LocationRepository.GetById(id);
            if (location == null) return null;
            return new LocationModel
            {
                LocationName = location.Name,
                LocationId = location.Id
            };
        }

        public async Task<ProductsModel> GetLocationProducts(Guid? id)
        {
            var productsModel = new ProductsModel { ProductModels = new List<ProductModel>() };
            var products = (await _unitOfWork.ProductRepository.GetByLocation(id)).ToList();
            foreach (var product in products)
            {
                var productModel = new ProductModel
                {
                    LocationId = id,
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductPrice = product.Price,
                    ProductQuantity = product.Quantity
                };
                productsModel.ProductModels.Add(productModel);
            }
            return productsModel;
        }

        public async Task CreateNewProduct(ProductModel productModel)
        {
            var location = await _unitOfWork.LocationRepository.GetById(productModel.LocationId);
            if (location == null) return;

            var product = new Product
            {
                Name = productModel.ProductName,
                Price = productModel.ProductPrice,
                Quantity = productModel.ProductQuantity,
                Location = location
            };

            await _unitOfWork.ProductRepository.Insert(product);
            await _unitOfWork.Save();
        }

        public async Task EditProduct(ProductModel productModel)
        {
            var location = await _unitOfWork.LocationRepository.GetById(productModel.LocationId);
            if (location == null) return;
            var product = await _unitOfWork.ProductRepository.GetById(productModel.ProductId);
            if (product == null || product.LocationId != location.Id) return;

            product.Name = productModel.ProductName;
            product.Quantity = productModel.ProductQuantity;
            product.Price = productModel.ProductPrice;

            await _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.Save();
        }

        public async Task DeleteProduct(ProductModel productModel)
        {
            var location = await _unitOfWork.LocationRepository.GetById(productModel.LocationId);
            if (location == null) return;
            var product = await _unitOfWork.ProductRepository.GetById(productModel.ProductId);
            if (product == null || product.LocationId != location.Id) return;

            await _unitOfWork.ProductRepository.Delete(product);
            await _unitOfWork.Save();
        }

        public async Task PlaceOrders(OrdersModel ordersModel)
        {
            var location = await _unitOfWork.LocationRepository.GetById(ordersModel.LocationId);
            if (location == null) return;

            foreach (var order in ordersModel.OrderModels)
            {
                if (order.Quantity <= 0) continue;
                var product = await _unitOfWork.ProductRepository.GetById(order.ProductId);
                if (product == null || product.Quantity < order.Quantity) return;

                var orderInfo = new OrderInfo
                {
                    ProductName = product.Name,
                    ProductPrice = product.Price,
                    ProductQuantity = order.Quantity
                };
                var newOrder = new Order
                {
                    LocationId = location.Id,
                    //TODO USER
                    OrderInfo = orderInfo
                };

                product.Quantity -= orderInfo.ProductQuantity;

                await _unitOfWork.OrderRepository.Insert(newOrder);
                await _unitOfWork.ProductRepository.Update(product);
            }
            await _unitOfWork.Save();
        }
    }
}