using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStoreApp.Domain;
using WebStoreApp.Web.Models;
using WebStoreApp.Data;

namespace WebStoreApp.Web.Services
{
    public class LocationsModelService : ILocationsModelService
    {
        private readonly UnitOfWork _unitOfWork;

        public LocationsModelService(WebStoreAppContext context)
        {
            this._unitOfWork = new UnitOfWork(context);
        }

        public async Task<List<Location>> GetLocations()
        {
            return (await _unitOfWork.LocationRepository
                .Get(null, locations => locations
                .OrderBy(location => location.Name)
                .ThenByDescending(location => location.Id)))
                .ToList();
        }

        public async Task CreateLocation(Location location)
        {
            await _unitOfWork.LocationRepository.Insert(location);
            await _unitOfWork.Save();
        }

        public async Task<Location> GetLocationDetails(Guid? id)
        {
            return await _unitOfWork.LocationRepository.GetById(id);
        }

        public async Task<List<ProductItemViewModel>> GetLocationProducts(Guid? id)
        {
            var productVMList = new List<ProductItemViewModel>();
            var productList = (await _unitOfWork.ProductRepository.GetByLocation(id)).ToList();
            foreach (var product in productList)
            {
                var productViewModel = new ProductItemViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = product.Quantity
                };
                productVMList.Add(productViewModel);
            }
            return productVMList;
        }

        public async Task CreateNewProduct(LocationViewModel locationViewModel)
        {
            var location = await _unitOfWork.LocationRepository.GetById(locationViewModel.Id);
            var product = new Product
            {
                Name = locationViewModel.ProductName,
                Price = locationViewModel.ProductPrice,
                Quantity = locationViewModel.ProductQuantity,
                Location = location
            };
            await _unitOfWork.ProductRepository.Insert(product);
            await _unitOfWork.Save();
        }

        public async Task EditProduct(ProductEditViewModel productEditViewModel)
        {
            var location = await _unitOfWork.LocationRepository.GetById(productEditViewModel.LocationId);
            if (location == null) return;
            var product = await _unitOfWork.ProductRepository.GetById(productEditViewModel.ProductId);
            if (product == null || product.LocationId != location.Id) return;
            product.Name = productEditViewModel.ProductName;
            product.Quantity = productEditViewModel.ProductQuantity;
            product.Price = productEditViewModel.ProductPrice;
            await _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.Save();
        }

        public async Task DeleteProduct(ProductDeleteViewModel productDeleteViewModel)
        {
            var location = await _unitOfWork.LocationRepository.GetById(productDeleteViewModel.LocationId);
            if (location == null) return;
            var product = await _unitOfWork.ProductRepository.GetById(productDeleteViewModel.ProductId);
            if (product == null || product.LocationId != location.Id) return;
            await _unitOfWork.ProductRepository.Delete(product);
            await _unitOfWork.Save();
        }

        public async Task PlaceOrders(ProductsViewModel productsViewModel)
        {
            var location = await _unitOfWork.LocationRepository.GetById(productsViewModel.LocationId);
            if (location == null) return;
            foreach (var orderItem in productsViewModel.Orders)
            {
                if (orderItem.Quantity == 0) continue;
                var product = await _unitOfWork.ProductRepository.GetById(orderItem.ProductId);
                if (product == null || product.Quantity < orderItem.Quantity) return;
                var orderInfo = new OrderInfo
                {
                    ProductName = product.Name,
                    ProductPrice = product.Price,
                    ProductQuantity = orderItem.Quantity
                };
                var order = new Order
                {
                    LocationId = location.Id,
                    //TODO USER
                    OrderInfo = orderInfo
                };
                product.Quantity -= orderInfo.ProductQuantity;
                await _unitOfWork.OrderRepository.Insert(order);
                await _unitOfWork.ProductRepository.Update(product);
            }
            await _unitOfWork.Save();
        }
    }
}