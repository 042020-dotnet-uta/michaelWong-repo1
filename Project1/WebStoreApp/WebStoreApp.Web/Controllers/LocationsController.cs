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
            locationsViewModel.LocationsModel = await _service.GetLocations();
            return View(locationsViewModel);
        }

        [HttpPost]
        [ActionName(nameof(Index))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNewLocation([Bind("LocationName")] LocationModel locationModel)
        {
            if (ModelState.IsValid)
                await _service.CreateLocation(locationModel);

            return RedirectToAction("Index", "Location");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("LocationId")] LocationModel locationModel)
        {
            await _service.DeleteLocation(locationModel);
            return RedirectToAction("Index", "Location");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("LocationId", "LocationName")] LocationModel locationModel)
        {
            if (ModelState.IsValid)
                await _service.EditLocation(locationModel);
            return RedirectToAction("Index", "Locations");
        }
    }
}