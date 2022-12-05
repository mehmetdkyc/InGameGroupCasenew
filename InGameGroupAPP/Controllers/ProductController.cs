using DataAccessLayer.Concrete;
using EntityLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public async Task<IActionResult> UpdateProduct(int id)
        {
            HttpClient client = GetClient();

            var endPoint = "https://localhost:7298/api/product/" + id;

            var response = await client.GetAsync(endPoint);
            var responseBody = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<Product>(responseBody);
            var categoryValues = await GetCategoryList();
            ViewBag.CategoryValues = categoryValues;
            return View(responseObject);
        }
        public IActionResult ProductAdd()
        {
            var categoryValues = GetCategoryList();
            ViewBag.CategoryValues = categoryValues;
            return View();
        }
        //[Authorize(Roles = "Admin")]
        [Route("Product/ProductDetail/{productName}/{id}")]
        public async Task<IActionResult> ProductDetail(string productName, int id)
        {
            HttpClient client = GetClient();

            var endPoint = "https://localhost:7298/api/product/"+id;

            var response = await client.GetAsync(endPoint);
            var responseBody = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<Product>(responseBody);

            return View(responseObject);
        }
        public async Task<List<SelectListItem>> GetCategoryList()
        {
            HttpClient client = GetClient();
            var endPoint = "https://localhost:7298/api/product/GetCategoryList";

            var response = await client.GetAsync(endPoint);
            var responseBody = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<List<SelectListItem>>(responseBody);

            return responseObject;
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
            try
            {
                HttpClient client = GetClient();

                var endPoint = "https://localhost:7298/api/product/ProductAdd";
                var newPostJsonPayment = JsonConvert.SerializeObject(product);
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
        [HttpPut]
        public async Task<JsonResult> Update(Product product)
        {
            HttpClient client = GetClient();

            var endPoint = "https://localhost:7298/api/product/UpdateProduct";
            var newPostJsonPayment = JsonConvert.SerializeObject(product);
            var payloadPayment = new StringContent(newPostJsonPayment, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(endPoint, payloadPayment);
            var responseBody = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<Product>(responseBody);

            return Json(responseObject);
        }

        [Route("Product/Delete/{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                HttpClient client = GetClient();

                var endPoint = "https://localhost:7298/api/product/ProductDelete" + id.ToString();

                var newPostJsonPayment = JsonConvert.SerializeObject(id);
                var payloadPayment = new StringContent(newPostJsonPayment, Encoding.UTF8, "application/json");

                var response = await client.DeleteAsync(endPoint);
                var responseBody = await response.Content.ReadAsStringAsync();

                return RedirectToAction("Index", "Product");
            }
            catch
            {
                return RedirectToAction("Index", "Product");
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
