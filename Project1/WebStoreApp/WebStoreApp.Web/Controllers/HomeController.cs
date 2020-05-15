using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
            if (HttpContext.Session.GetString("_User") != null)
                return RedirectToAction("Index", "Locations");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Login()
        {
            if (HttpContext.Session.GetString("_User") != null)
                return RedirectToAction("Index", "Locations");
            return await Task.FromResult(View());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("UsernameLogin", "PasswordLogin")] LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _service.VerifyLogin(loginModel);
                if (user != null)
                {
                    HttpContext.Session.SetString("_User", user.Id.ToString());
                    HttpContext.Session.SetString("_Role", user.UserType.Name);
                    return RedirectToAction("Index", "Locations");
                }
            }
            loginModel.PasswordLogin = null;
            ModelState.AddModelError("LoginError", "Failed to log in.");
            return View(loginModel);
        }

        public async Task<IActionResult> Register(RegisterModel registerModel, bool notUsed)
        {
            if (HttpContext.Session.GetString("_User") != null)
                return RedirectToAction("Index", "Locations");
            return await Task.FromResult(View(registerModel));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Username", "Password", "PasswordConfirmation", "FirstName", "LastName")] RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var message = await _service.RegisterUser(registerModel);
                if (message != "")
                {
                    ModelState.AddModelError("CustomMessage", message);
                    registerModel.Password = null;
                    registerModel.PasswordConfirmation = null;
                    return View(registerModel);
                }
                return RedirectToAction("Login");
            }
            ModelState.AddModelError("Password", "Enter Password Again");
            registerModel.Password = null;
            registerModel.PasswordConfirmation = null;
            return RedirectToAction("Register", registerModel);
        }
    }
}
