using EntityLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace InGameGroupAPP.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            HttpClient client = GetClient();

            var endPoint = "https://localhost:7298/api/product";

            var response =  await client.GetAsync(endPoint);
            var responseBody = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<List<Product>>(responseBody);

            return View(responseObject);
        }

        [HttpPost]
        public async Task<JsonResult> Index(Product product)
        {
            HttpClient client = GetClient();

            var endPoint = "https://localhost:7298/api/product";
            var newPostJsonPayment = JsonConvert.SerializeObject(product);
            var payloadPayment = new StringContent(newPostJsonPayment, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(endPoint, payloadPayment);
            var responseBody = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<Product>(responseBody);

            return Json(responseObject);
        }
        [HttpPut]
        public async Task<JsonResult> Update(Product product)
        {
            HttpClient client = GetClient();

            var endPoint = "https://localhost:7298/api/product";
            var newPostJsonPayment = JsonConvert.SerializeObject(product);
            var payloadPayment = new StringContent(newPostJsonPayment, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(endPoint, payloadPayment);
            var responseBody = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<Product>(responseBody);

            return Json(responseObject);
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                HttpClient client = GetClient();

                var endPoint = "https://localhost:7298/api/product/" + id.ToString();

                var newPostJsonPayment = JsonConvert.SerializeObject(id);
                var payloadPayment = new StringContent(newPostJsonPayment, Encoding.UTF8, "application/json");

                var response = await client.DeleteAsync(endPoint);
                var responseBody = await response.Content.ReadAsStringAsync();

                return Json(true);
            }
            catch
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
