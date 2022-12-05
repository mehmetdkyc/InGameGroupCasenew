using APIIngame.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace InGameGroupAPP.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(LoginModel loginModel)
        {
            HttpClient client = new HttpClient();

            var endPoint = new Uri("https://localhost:7298/api/auth/login");
            var newPostJson = JsonConvert.SerializeObject(loginModel);
            var payload = new StringContent(newPostJson, Encoding.UTF8, "application/json");
            var jsonResult = client.PostAsync(endPoint, payload).Result.Content.ReadAsStringAsync().Result;

            //responseu deserialize ediyoruz.
            var data = JsonConvert.DeserializeObject<UserManagerResponse>(jsonResult);
            if (data.IsSuccess)
            {
                HttpContext.Session.SetString("JWToken", data.Message);
                HttpContext.Session.SetString("UserEmail", loginModel.Email);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email,loginModel.Email)
                };
                var userIdentity = new ClaimsIdentity(claims, "a");
                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(principal);
                return RedirectToAction("Index", "Product");
            }
                
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");

        }
    }
}
