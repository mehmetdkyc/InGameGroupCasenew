using Microsoft.AspNetCore.Mvc;

namespace InGameGroupAPP.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
