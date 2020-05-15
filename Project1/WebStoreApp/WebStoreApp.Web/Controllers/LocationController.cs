using System;
using System.Web;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStoreApp.Web.Models;
using WebStoreApp.Web.Services;

namespace WebStoreApp.Web
{
    public class LocationController : Controller
    {
        private readonly ILocationsModelService _service;
        private readonly ILogger<LocationsController> _logger;

        public LocationController(ILocationsModelService locationsModelService, ILogger<LocationsController> logger)
        {
            this._service = locationsModelService;
            this._logger = logger;
        }

        public async Task<IActionResult> Index(ProductModel productModel, Guid? id)
        {
            if (!id.HasValue) return RedirectToAction(nameof(Index), "Locations");

            var locationModel = await _service.GetLocationDetails(id);
            if (locationModel == null) return RedirectToAction(nameof(Index), "Locations");

            var productsModel = await _service.GetLocationProducts(id);
            return View(new LocationViewModel
            {
                LocationModel = locationModel,
                ProductsModel = productsModel,
                ProductModel = new ProductModel { LocationId = id }
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LocationId", "ProductName", "ProductPrice", "ProductQuantity")] ProductModel productModel, Guid? id)
        {
            if (ModelState.IsValid && productModel.LocationId == id)
            {
                await _service.CreateNewProduct(productModel);
                return RedirectToAction(nameof(Index), "Location", new { id });
            }

            ViewData["State"] = "Create";
            return RedirectToAction(nameof(Index), "Location", new { productModel, id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("LocationId", "ProductId", "ProductName", "ProductPrice", "ProductQuantity")] ProductModel productModel, Guid? id)
        {
            if (ModelState.IsValid && productModel.LocationId == id)
                await _service.EditProduct(productModel);

            return RedirectToAction(nameof(Index), "Location", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("LocationId", "ProductId")] ProductModel productModel, Guid? id)
        {
            if (productModel.LocationId == id)
                await _service.DeleteProduct(productModel);

            return RedirectToAction(nameof(Index), "Location", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Order(OrdersModel ordersModel, Guid? id)
        {
            await _service.PlaceOrders(ordersModel);

            return RedirectToAction(nameof(Index), "Location", new { id = id });
        }
    }
}