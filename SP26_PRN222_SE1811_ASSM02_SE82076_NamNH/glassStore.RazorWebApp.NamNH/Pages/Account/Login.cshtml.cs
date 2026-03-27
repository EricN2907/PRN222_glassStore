using glassStore.Entites.NamNH.Models;
using glassStore.Service.NamNH;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace glassStore.RazorWebApp.Pages.Account

{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SystemUserAccountService _userAccountService;

        public LoginModel() => _userAccountService ??= new SystemUserAccountService();

        [BindProperty]
        public string UserName { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string? ReturnUrl { get; set; }


        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            var userAccount = await _userAccountService.GetUserAsync(UserName, Password);
            if (userAccount == null)
            {
                TempData["Message"] = "Login fail: Wrong email or password!";
                return Page();
            }
            if (!userAccount.IsActive)
            {
                TempData["Message"] = "Your account has been BANNED. Please contact admin.";
                return Page();
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, UserName),
                new Claim(ClaimTypes.Role, userAccount.RoleId.ToString()),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
            var cookieValue = userAccount.UserName ?? "";
            Response.Cookies.Append("UserName", cookieValue);

            if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }

            return RedirectToPage("/OrdersNamNhs/Index");
        }
    }    
}
