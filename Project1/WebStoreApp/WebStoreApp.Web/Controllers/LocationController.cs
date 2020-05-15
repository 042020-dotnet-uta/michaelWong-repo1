using System;
using System.Web;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
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

        public async Task<IActionResult> Index(Guid? id)
        {
            if (!id.HasValue) return RedirectToAction(nameof(Index), "Locations");

            var locationModel = await _service.GetLocationDetails(id);
            if (locationModel == null) return RedirectToAction(nameof(Index), "Locations");

            var productsModel = await _service.GetLocationProducts(id);

            ViewData["Role"] = HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.Role)?.Value;

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
            if (ModelState.IsValid && productModel.LocationId == id && HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.Role)?.Value == "Admin")
            {
                await _service.CreateNewProduct(productModel);
                return RedirectToAction(nameof(Index), "Location", new { id });
            }

            return RedirectToAction(nameof(Index), "Location", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("LocationId", "ProductId", "ProductName", "ProductPrice", "ProductQuantity")] ProductModel productModel, Guid? id)
        {
            if (ModelState.IsValid && productModel.LocationId == id && HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.Role)?.Value == "Admin")
                await _service.EditProduct(productModel);

            return RedirectToAction(nameof(Index), "Location", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("LocationId", "ProductId")] ProductModel productModel, Guid? id)
        {
            if (productModel.LocationId == id && HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.Role)?.Value == "Admin")
                await _service.DeleteProduct(productModel);

            return RedirectToAction(nameof(Index), "Location", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Order(OrdersModel ordersModel, Guid? id)
        {
            Guid userId;
            if (Guid.TryParse(HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.Sid)?.Value, out userId))
                await _service.PlaceOrders(ordersModel, userId);

            return RedirectToAction(nameof(Index), "Location", new { id });
        }
    }
}