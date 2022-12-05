using APIIngame.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace InGameGroupAPP.Controllers
{
    [AllowAnonymous]
    public class UserController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> ForgotPassword(string email)
        {
            try
            {
                HttpClient client = GetClient();

                var endPoint = "https://localhost:7298/api/user/ForgotPassword";

                var newPostJsonPayment = JsonConvert.SerializeObject(email);
                var payloadPayment = new StringContent(newPostJsonPayment, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(endPoint, payloadPayment);
                var responseBody = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<UserManagerResponse>(responseBody);

                return Json(true);
            }
            catch (Exception)
            {

                return Json(false);
            }
        }
        [HttpPost]
        public async Task<JsonResult> ChangePassword(string email,string oldPass,string newPass)
        {
            try
            {
                HttpClient client = GetClient();

                var endPoint = "https://localhost:7298/api/user/ChangePassword";
                ChangePasswordModel newModel = new ChangePasswordModel
                {
                    Email = email,
                    OldPasword = oldPass,
                    NewPassword = newPass
                };
                var newPostJsonPayment = JsonConvert.SerializeObject(newModel);
                var payloadPayment = new StringContent(newPostJsonPayment, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(endPoint, payloadPayment);
                var responseBody = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<UserManagerResponse>(responseBody);

                return Json(true);
            }
            catch (Exception)
            {

                return Json(false);
            }
        }
        private HttpClient GetClient()
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            return client;
        }
    }
}
