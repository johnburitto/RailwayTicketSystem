using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult AccessDenied(string returnUrl)
        {
            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = requestCulture?.RequestCulture.Culture.ToString().Substring(0, 2);

            return User.IsInRole("User") ? Redirect($"/{culture}/Home/Index") : Redirect($"/{culture}/AdminPanel/Index");
        }
    }
}
