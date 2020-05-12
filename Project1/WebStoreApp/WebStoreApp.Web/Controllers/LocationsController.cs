using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStoreApp.Web.Models;
using WebStoreApp.Data;
using WebStoreApp.Domain;

namespace WebStoreApp.Web
{
    public class LocationsController : Controller
    {
        private readonly ILogger<LocationsController> _logger;
        private readonly UnitOfWork _unitOfWork;

        public LocationsController(ILogger<LocationsController> logger, WebStoreAppContext context)
        {
            this._unitOfWork = new UnitOfWork(context);
            this._logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            LocationsViewModel locationsVM = new LocationsViewModel { Locations = await _unitOfWork.LocationRepository.All() };
            return View(locationsVM);
        }

        [HttpPost]
        [ActionName("Index")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNewLocation([Bind("LocationName")] LocationsViewModel locationsVM)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.LocationRepository.Insert(new Location { Name = locationsVM.LocationName });
                await _unitOfWork.Save();
                locationsVM.LocationName = "";
            }
            locationsVM.Locations = await _unitOfWork.LocationRepository.All();
            return View(locationsVM);
        }
    }
}