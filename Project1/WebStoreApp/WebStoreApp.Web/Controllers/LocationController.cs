using System;
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
            try
            {
                var locationModel = await _service.GetLocationDetails(id);

                var productsModel = await _service.GetLocationProducts(id);

                ViewData["Role"] = User.FindFirst(ClaimTypes.Role)?.Value;

                if(TempData.ContainsKey("total"))
                    ViewData["total"] = TempData["total"].ToString();

                return View(new LocationViewModel
                {
                    LocationModel = locationModel,
                    ProductsModel = productsModel,
                    ProductModel = new ProductModel { LocationId = id }
                });
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index), "Locations");
            }
        }

        public async Task<IActionResult> Orders(Guid? id)
        {
            if (id == null || User.FindFirst(ClaimTypes.Role)?.Value != "Admin")
                return RedirectToAction("Index", "Location", new { id });

            try
            {
                var ordersModel = await _service.GetLocationHistory(id);

                var ordersViewModel = new OrdersViewModel
                {
                    OrdersModel = ordersModel,
                    LocationModel = await _service.GetLocationDetails(id)

                };

                ViewData["Role"] = User.FindFirst(ClaimTypes.Role)?.Value;
                return View(ordersViewModel);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Location", new { id });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LocationId", "ProductName", "ProductPrice", "ProductQuantity")] ProductModel productModel, Guid? id)
        {
            if (ModelState.IsValid && productModel.LocationId == id && User.FindFirst(ClaimTypes.Role)?.Value == "Admin")
            {
                try
                {
                    await _service.CreateNewProduct(productModel);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }

            return RedirectToAction(nameof(Index), "Location", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("LocationId", "ProductId", "ProductName", "ProductPrice", "ProductQuantity")] ProductModel productModel, Guid? id)
        {
            if (ModelState.IsValid && productModel.LocationId == id && User.FindFirst(ClaimTypes.Role)?.Value == "Admin")
                try
                {
                    await _service.EditProduct(productModel);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }

            return RedirectToAction(nameof(Index), "Location", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("LocationId", "ProductId")] ProductModel productModel, Guid? id)
        {
            if (productModel.LocationId == id && User.FindFirst(ClaimTypes.Role)?.Value == "Admin")
                try
                {
                    await _service.DeleteProduct(productModel);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }

            return RedirectToAction(nameof(Index), "Location", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Order(OrdersModel ordersModel, Guid? id)
        {
            Guid userId;
            if (Guid.TryParse(User.FindFirst(ClaimTypes.Sid)?.Value, out userId))
                try
                {
                    TempData["total"] = (await _service.PlaceOrders(ordersModel, userId)).ToString();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }

            return RedirectToAction(nameof(Index), "Location", new { id });
        }
    }
}