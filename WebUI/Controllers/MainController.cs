using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class MainController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
