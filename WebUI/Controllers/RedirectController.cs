using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class RedirectController : Controller
    {
        [Authorize]
        public IActionResult LoginPoint()
        {
            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = requestCulture?.RequestCulture.Culture;

            return User.IsInRole("Admin") ? Redirect($"/{culture}/AdminPanel/Index") :
                User.IsInRole("User") ? Redirect($"/{culture}/Main/Index") : Redirect($"/{culture}/Home/Index");
        }
    }
}
