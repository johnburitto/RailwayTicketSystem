using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [Authorize(Roles = "User")]
    public class PersonalCabinetController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
