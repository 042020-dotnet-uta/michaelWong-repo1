using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        /// <summary>Creates a new instance of a location and inserts it into the database.</summary>
        /// <param name="locationModel">A Location Model.</param>
        public async Task CreateLocation(LocationModel locationModel)
        {
            var location = new Location
            {
                Name = locationModel.LocationName
            };
            await _unitOfWork.LocationRepository.Insert(location);
            await _unitOfWork.Save();
        }

        /// <summary>Edit the name of a location in the database.</summary>
        /// <param name="locationModel">A Location Model.</param>
        public async Task EditLocation(LocationModel locationModel)
        {
            var location = await _unitOfWork.LocationRepository.GetById(locationModel.LocationId);
            if (location == null) throw new KeyNotFoundException("Specified location was not found.");

            location.Name = locationModel.LocationName;
            await _unitOfWork.LocationRepository.Update(location);
            await _unitOfWork.Save();
        }

        /// <summary>Delete a location in the database.</summary>
        /// <param name="locationModel">A Location Model.</summary>
        public async Task DeleteLocation(LocationModel locationModel)
        {
            var location = await _unitOfWork.LocationRepository.GetById(locationModel.LocationId);
            if (location == null) throw new KeyNotFoundException("Specified location was not found.");

            await _unitOfWork.LocationRepository.Delete(location);
            await _unitOfWork.Save();

        }

        /// <summary>Gets the name of a location from the database.</summary>
        /// <param name="id">Id of the location.</param>
        /// <returns>Location Model with details.</returns>
        public async Task<LocationModel> GetLocationDetails(Guid? id)
        {
            var location = await _unitOfWork.LocationRepository.GetById(id);
            if (location == null) throw new KeyNotFoundException("Specified location was not found.");

            return new LocationModel
            {
                LocationName = location.Name,
                LocationId = location.Id
            };
        }

        /// <summary>Get all products at a location from the database.</summary>
        /// <param name="id">Id of the location.</param>
        /// <returns>Products Model with all products from a location.</returns>
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

        /// <summary>Get order history of a location from the database.</summary>
        /// <param name="id">Id of the location.</param>
        /// <returns>Orders Model with order history from a location.</returns>
        public async Task<OrdersModel> GetLocationHistory(Guid? id)
        {
            var location = await _unitOfWork.LocationRepository.GetById(id);
            if (location == null) throw new KeyNotFoundException("Specified location was not found.");
            var orders = (await _unitOfWork.OrderRepository.GetByLocation(id));
            var ordersModel = new OrdersModel { OrderModels = new List<OrderModel>() };
            foreach (var order in orders)
            {
                var orderModel = new OrderModel
                {
                    Quantity = order.OrderInfo.ProductQuantity,
                    ProductName = order.OrderInfo.ProductName,
                    ProductPrice = order.OrderInfo.ProductPrice,
                    Timestamp = order.Timestamp
                };
                ordersModel.OrderModels.Add(orderModel);
            }
            return ordersModel;
        }

        /// <summary>Creates a new product and inserts it into the database.</summary>
        /// <param name="id">Id of the location.</param>
        public async Task CreateNewProduct(ProductModel productModel)
        {
            var location = await _unitOfWork.LocationRepository.GetById(productModel.LocationId);
            if (location == null) throw new KeyNotFoundException("Specified location was not found.");

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

        /// <summary>Updates a product in the database.</summary>
        /// <param name="productModel">Product Model containing updated information.</param>
        public async Task EditProduct(ProductModel productModel)
        {
            var location = await _unitOfWork.LocationRepository.GetById(productModel.LocationId);
            if (location == null) throw new KeyNotFoundException("Specified location was not found.");
            var product = await _unitOfWork.ProductRepository.GetById(productModel.ProductId);
            if (product == null || product.LocationId != location.Id) throw new KeyNotFoundException("Specified product was not found.");

            product.Name = productModel.ProductName;
            product.Quantity = productModel.ProductQuantity;
            product.Price = productModel.ProductPrice;

            await _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.Save();
        }

        /// <summary>Deletes a product from the database.</summary>
        /// <param name="productModel">Product Model of the product to be deleted.</param>
        public async Task DeleteProduct(ProductModel productModel)
        {
            var location = await _unitOfWork.LocationRepository.GetById(productModel.LocationId);
            if (location == null) throw new KeyNotFoundException("Specified location was not found.");
            var product = await _unitOfWork.ProductRepository.GetById(productModel.ProductId);
            if (product == null || product.LocationId != location.Id) throw new KeyNotFoundException("Specified product was not found.");

            await _unitOfWork.ProductRepository.Delete(product);
            await _unitOfWork.Save();
        }

        /// <summary>Places orders for products.</summary>
        /// <param name="ordersModel">Orders Model with order information.</param>
        /// <param name="userId">Id of the user placing orders.</param>
        public async Task<decimal> PlaceOrders(OrdersModel ordersModel, Guid? userId)
        {
            var location = await _unitOfWork.LocationRepository.GetById(ordersModel.LocationId);
            if (location == null) throw new KeyNotFoundException("Specified location was not found.");

            var user = await _unitOfWork.UserRepository.GetById(userId);
            if (user == null) throw new KeyNotFoundException("User was not found.");

            decimal total = 0;

            foreach (var order in ordersModel.OrderModels)
            {
                if (order.Quantity <= 0) continue;
                var product = await _unitOfWork.ProductRepository.GetById(order.ProductId);
                if (product == null) throw new KeyNotFoundException("Specified product was not found.");
                if (product.Quantity < order.Quantity) throw new ArgumentOutOfRangeException("Invalid order quantity.");
                if (order.Quantity > 5000) throw new ArgumentOutOfRangeException("Order quantity was deemed too high. Order rejected.");

                var orderInfo = new OrderInfo
                {
                    ProductName = product.Name,
                    ProductPrice = product.Price,
                    ProductQuantity = order.Quantity
                };
                var newOrder = new Order
                {
                    User = user,
                    Location = location,
                    OrderInfo = orderInfo,
                    Timestamp = DateTime.Now
                };

                product.Quantity -= orderInfo.ProductQuantity;

                await _unitOfWork.OrderRepository.Insert(newOrder);
                await _unitOfWork.ProductRepository.Update(product);

                total += product.Price * order.Quantity;
            }
            await _unitOfWork.Save();
            return total;
        }
    }
}