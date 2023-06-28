using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Security.Dto;
using Security.Services.Interfaces;
using System.Text;
using System.Web;

namespace Security.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _service;

        public AuthController(IUserService service, IConfiguration conf)
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
            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = requestCulture?.RequestCulture.Culture;

            if (ModelState.IsValid)
            {
                var result = await _service.LoginAsync(dto);

                return result == ResponseType.Logined ? Redirect(dto.ReturnUrl) 
                    : Redirect($"/{culture}/Auth/Login?ReturnUrl={HttpUtility.UrlEncode(Encoding.Default.GetBytes(dto.ReturnUrl))}");
            }

            return Redirect($"/{culture}/Auth/Login?ReturnUrl={HttpUtility.UrlEncode(Encoding.Default.GetBytes(dto.ReturnUrl))}");
        }
    }
}
