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
        private readonly IConfiguration _conf;

        public AuthController(IUserService service, IConfiguration conf)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _conf = conf ?? throw new ArgumentNullException(nameof(conf));
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

                return result == ResponseType.Logined ? Redirect(dto.ReturnUrl) 
                    : Redirect($"/Auth/Login?ReturnUrl={HttpUtility.UrlEncode(Encoding.Default.GetBytes(dto.ReturnUrl))}");
            }

            return Redirect($"/Auth/Login?ReturnUrl={HttpUtility.UrlEncode(Encoding.Default.GetBytes(dto.ReturnUrl))}");
        }
    }
}
