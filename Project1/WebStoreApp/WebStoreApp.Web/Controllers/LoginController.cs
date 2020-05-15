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
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IUserModelService _service;

        public LoginController(ILogger<LoginController> logger, IUserModelService userModelService)
        {
            this._logger = logger;
            this._service = userModelService;
        }

        public async Task<IActionResult> Index()
        {
            Guid? userId = Guid.Parse(HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.Sid).Value);
            if (userId != null) return RedirectToAction("Index", "Locations");
            return await Task.FromResult(View());
        }

        [HttpPost]
        [ActionName("Index")]
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
            }
            loginModel.PasswordLogin = null;
            return View(loginModel);
        }
    }
}