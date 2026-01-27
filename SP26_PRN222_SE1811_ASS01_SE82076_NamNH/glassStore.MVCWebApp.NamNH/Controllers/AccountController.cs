using glassStore.Entites.NamNH.Models;
using glassStore.Service.NamNH;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace glassStore.MVCWebApp.NamNH.Controllers
{
    public class AccountController : Controller
    {
        private readonly SystemUserAccountService _userAccountService;

        public AccountController(SystemUserAccountService systemUserAccountService) => _userAccountService = systemUserAccountService;

        public IActionResult Index()
        {
            return RedirectToAction("Login");
            //return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password)
        {
            try
            {
                var userAccount = await _userAccountService.LoginAsync(userName, password);

                if (userAccount != null)
                {
                    var claims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.Name, userAccount.FullName),
                                    new Claim(ClaimTypes.Role, userAccount.RoleId.ToString())
                                };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                    Response.Cookies.Append("UserName", userAccount.FullName);
                    Response.Cookies.Append("Role", userAccount.RoleId.ToString());

                    return RedirectToAction("Index", "OrdersNamNhs");
                }
            }
            catch (Exception ex)
            {
                if(ex.Message == "ACCOUNT_LOCKED")
                {
                    ModelState.AddModelError("", "Account is locked");
                    return View();
                }else if(ex.Message == "INCORRECT_PASSWORD")
                {
                    ModelState.AddModelError("", "Incorrect password");
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", "Login failure");
                    return View();
                }
            }

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ModelState.AddModelError("", "Login failure");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> Forbidden()
        {
            return View();
        }
    }
}
