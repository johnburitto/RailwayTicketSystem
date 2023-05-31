using Microsoft.AspNetCore.Mvc;
using Security.Dto;

namespace WebUI.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Update(string id)
        {
            return View(new UserUpdateDto { Id = id });
        }
    }
}
