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
    public class LocationController : Controller
    {
        private readonly ILogger<LocationsController> _logger;
        private readonly UnitOfWork _unitOfWork;

        public LocationController(ILogger<LocationsController> logger, WebStoreAppContext context)
        {
            this._unitOfWork = new UnitOfWork(context);
            this._logger = logger;
        }

        public async Task<IActionResult> Index(Guid? id)
        {
            if (id.HasValue)
            {
                var location = await _unitOfWork.LocationRepository.GetById(id);
                if (location != null)
                {
                    var products = await _unitOfWork.ProductRepository.GetByLocation(id);
                    return View(new LocationViewModel { Location = location, Products = products });
                }
            }
            return RedirectToAction("Index", "Locations");
        }
    }
}