using System;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WebStoreApp.Web.Models;
using WebStoreApp.Web.Services;

namespace WebStoreApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserModelService _service;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IUserModelService userModelService, ILogger<HomeController> logger)
        {
            _service = userModelService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            Guid userId;
            if (Guid.TryParse(HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.Sid)?.Value, out userId))
                return RedirectToAction("Index", "Locations");
            return View(new LoginRegisterViewModel());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}