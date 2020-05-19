using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
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
            if (!Guid.TryParse(User.FindFirst(ClaimTypes.Sid)?.Value, out userId))
                return RedirectToAction("Login");

            try
            {
                var userModel = await _service.GetUserDetails(userId);
                var ordersModel = await _service.GetUserOrders(userId);
                var userViewModel = new UserViewModel
                {
                    UserModel = userModel,
                    OrdersModel = ordersModel
                };
                ViewData["Role"] = User.FindFirst(ClaimTypes.Role)?.Value;
                return View(userViewModel);
            }
            catch
            {
                return RedirectToAction("Login");
            }

        }

        public async Task<IActionResult> Login()
        {
            Guid userId;
            if (Guid.TryParse(User.FindFirst(ClaimTypes.Sid)?.Value, out userId))
                return RedirectToAction("Index", "Locations");
            return await Task.FromResult(View(new LoginRegisterViewModel()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("UsernameLogin", "PasswordLogin")] LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _service.VerifyLogin(loginModel);
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
                catch (Exception ex)
                {
                    ModelState.AddModelError("LoginError", ex.Message);
                }
            }
            loginModel.PasswordLogin = null;
            return View("Login", new LoginRegisterViewModel { LoginModel = loginModel });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Username", "Password", "PasswordConfirmation", "FirstName", "LastName")] RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _service.RegisterUser(registerModel);
                    ModelState.AddModelError("RegisterMessage", "User registered.");
                    return await Task.FromResult(View(new LoginRegisterViewModel()));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("RegisterError", ex.Message);
                }
            }
            registerModel.Password = null;
            registerModel.PasswordConfirmation = null;
            return View("Login", new LoginRegisterViewModel { RegisterModel = registerModel });
        }

        public async Task<IActionResult> LogOut()
        {
            Guid userId;
            if (Guid.TryParse(User.FindFirst(ClaimTypes.Sid)?.Value, out userId))
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Search(string firstName = "", string lastName = "")
        {
            string role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role != "Admin") return RedirectToAction("Index");

            ViewData["Role"] = role;

            if (firstName == "" && lastName == "")
            {
                return View(new List<UserViewModel>());
            }

            var users = await _service.SearchUsers(firstName, lastName);
            var userViewModels = new List<UserViewModel>();
            foreach (var user in users)
            {
                try
                {
                    var userDetails = await _service.GetUserDetails(user.Id);
                    var userOrders = await _service.GetUserOrders(user.Id);
                    userViewModels.Add(new UserViewModel
                    {
                        UserModel = userDetails,
                        OrdersModel = userOrders
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }

            return View(userViewModels);
        }
    }
}