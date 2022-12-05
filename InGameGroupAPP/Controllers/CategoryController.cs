using EntityLayer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace InGameGroupAPP.Controllers
{
    public class CategoryController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            HttpClient client = GetClient();

            var endPoint = "https://localhost:7298/api/category";

            var response = await client.GetAsync(endPoint);
            var responseBody = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<List<Category>>(responseBody);
            return View(responseObject);
        }
        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Category category)
        {
            try
            {
                HttpClient client = GetClient();

                var endPoint = "https://localhost:7298/api/Category/Add";

                var newPostJsonPayment = JsonConvert.SerializeObject(category);
                var payloadPayment = new StringContent(newPostJsonPayment, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(endPoint, payloadPayment);
                var responseBody = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<Product>(responseBody);

                return RedirectToAction("ProductAdd", "Product");
            }
            catch (Exception)
            {

                return RedirectToAction("Add", "Product");
            }

        }
        [HttpPost("Update")]
        public async Task<JsonResult> Update(Category category)
        {
            HttpClient client = GetClient();

            var endPoint = "https://localhost:7298/api/Category/Update";
            var newPostJsonPayment = JsonConvert.SerializeObject(category);
            var payloadPayment = new StringContent(newPostJsonPayment, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(endPoint, payloadPayment);
            var responseBody = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<Product>(responseBody);

            return Json(responseObject);
        }
        [HttpDelete("Delete")]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                HttpClient client = GetClient();

                var endPoint = "https://localhost:7298/api/Category/CategoryDelete" + id.ToString();

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
