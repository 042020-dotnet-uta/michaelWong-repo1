using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStoreApp.Web.Models;
using WebStoreApp.Domain;
using WebStoreApp.Web.Services;

namespace WebStoreApp.Web
{
    public class LocationsController : Controller
    {
        private readonly ILocationsModelService _service;
        private readonly ILogger<LocationsController> _logger;

        public LocationsController(ILocationsModelService locationsModelService, ILogger<LocationsController> logger)
        {
            this._service = locationsModelService;
            this._logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var locationsViewModel = new LocationsViewModel();
            locationsViewModel.Locations = await _service.GetLocations();
            return View(locationsViewModel);
        }

        [HttpPost]
        [ActionName(nameof(Index))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNewLocation([Bind("Name")] Location location)
        {
            var locationsViewModel = new LocationsViewModel();
            if (ModelState.IsValid)
            {
                await _service.CreateLocation(location);
                return RedirectToAction(nameof(Index));
            }
            locationsViewModel.Locations = await _service.GetLocations();
            locationsViewModel.Name = location.Name;
            return View(locationsViewModel);
        }
    }
}