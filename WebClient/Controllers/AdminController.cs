using Microsoft.AspNetCore.Mvc;

namespace WebClient.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
