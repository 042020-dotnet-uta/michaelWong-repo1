using System;
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

        public async Task<IActionResult> Index(Guid? id)
        {
            if (id.HasValue)
            {
                var locationViewModel = new LocationViewModel();
                locationViewModel.Location = await _service.GetLocationDetails(id);
                if (locationViewModel.Location != null)
                    locationViewModel.Products = await _service.GetLocationProducts(id);
                return View(locationViewModel);
            }
            _logger.LogInformation("REDIRECTED");
            return RedirectToAction("Index", "Locations");
        }

        [HttpPost]
        [ActionName(nameof(Index))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNewProduct([Bind("Id", "ProductName", "ProductPrice", "ProductQuantity")] LocationViewModel locationViewModel)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateNewProduct(locationViewModel);
                return RedirectToAction(nameof(Index), locationViewModel.Id);
            }
            locationViewModel.Location = await _service.GetLocationDetails(locationViewModel.Id);
            if (locationViewModel.Location != null)
            {
                locationViewModel.Products = await _service.GetLocationProducts(locationViewModel.Id);
                locationViewModel.State = "create";
                return View(locationViewModel);
            }
            return RedirectToAction("Index", "Locations");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id", "ProductId", "ProductName", "ProductPrice", "ProductQuantity")] LocationViewModel locationViewModel)
        {
            if (ModelState.IsValid)
            {
                await _service.EditProduct(locationViewModel);
            }
            return RedirectToAction(nameof(Index), "Location", new { id = locationViewModel.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(LocationViewModel locationViewModel)
        {
            await _service.DeleteProduct(locationViewModel);
            return RedirectToAction(nameof(Index), "Location", new { id = locationViewModel.Id });
        }
    }
}