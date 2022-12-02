using APIIngame.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace InGameGroupAPP.Controllers
{
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(RegisterModel registerModel)
        {
            HttpClient client = new HttpClient();
            var endPoint = new Uri("https://localhost:7298/api/auth/register");
            var newPostJson = JsonConvert.SerializeObject(registerModel);
            var payload = new StringContent(newPostJson, Encoding.UTF8, "application/json");
            var jsonResult = client.PostAsync(endPoint, payload).Result.Content.ReadAsStringAsync().Result;

            //responseu deserialize ediyoruz.
            var data = JsonConvert.DeserializeObject<IdentityResult>(jsonResult);
            if (data.Succeeded)
                return RedirectToAction("Index", "Login");
            return RedirectToAction("Index");
        }
    }
}
