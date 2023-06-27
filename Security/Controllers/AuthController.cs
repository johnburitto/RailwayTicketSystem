using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Security.Dto;
using Security.Entities;
using Security.Services.Interfaces;

namespace Security.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _service;

        public AuthController(IUserService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new UserLoginDto { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> LoginAction(UserLoginDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.LoginAsync(dto);

                return result == ResponseType.Logined ? Redirect(dto.ReturnUrl) : Redirect("/Auth/Login");
            }

            User.IsInRole("Admin")

            return Redirect("/Auth/Login");
        }
    }
}
