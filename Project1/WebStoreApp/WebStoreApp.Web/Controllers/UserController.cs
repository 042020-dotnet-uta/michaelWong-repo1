using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using WebStoreApp.Web.Models;
using WebStoreApp.Web.Services;

namespace WebStoreApp.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserModelService _service;

        public UserController(ILogger<UserController> logger, IUserModelService userModelService)
        {
            this._logger = logger;
            this._service = userModelService;
        }

        public async Task<IActionResult> Index()
        {
            Guid userId;
            if (!Guid.TryParse(HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.Sid)?.Value, out userId))
                return RedirectToAction("Login");

            var userModel = await _service.GetUserDetails(userId);
            var ordersModel = await _service.GetUserOrders(userId);
            var userViewModel = new UserViewModel
            {
                UserModel = userModel,
                OrdersModel = ordersModel
            };
            
            return View(userViewModel);
        }

        public async Task<IActionResult> Login()
        {
            Guid userId;
            if (Guid.TryParse(HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.Sid)?.Value, out userId))
                return RedirectToAction("Index", "Locations");
            return await Task.FromResult(View(new LoginRegisterViewModel()));
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("UsernameLogin", "PasswordLogin")] LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _service.VerifyLogin(loginModel);
                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.LoginInfo.Username),
                        new Claim(ClaimTypes.Role, user.UserType.Name),
                        new Claim(ClaimTypes.Sid, user.Id.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    return RedirectToAction("Index", "Locations");
                }
                ModelState.AddModelError("LoginError", "Username and password not found.");
            }
            loginModel.PasswordLogin = null;
            return View("Login", new LoginRegisterViewModel { LoginModel = loginModel });
        }

        [HttpPost]
        public async Task<IActionResult> Register([Bind("Username", "Password", "PasswordConfirmation", "FirstName", "LastName")] RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var message = await _service.RegisterUser(registerModel);
                if (message == null)
                    return RedirectToAction("Login");
                else
                    ModelState.AddModelError("RegisterError", message);
            }
            registerModel.Password = null;
            registerModel.PasswordConfirmation = null;
            return View("Index", new LoginRegisterViewModel { RegisterModel = registerModel });
        }

        public async Task<IActionResult> LogOut()
        {
            Guid userId;
            if (Guid.TryParse(HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.Sid)?.Value, out userId))
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            return RedirectToAction("Login");
        }
    }
}