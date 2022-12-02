using Microsoft.AspNetCore.Mvc;

namespace InGameGroupAPP.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
