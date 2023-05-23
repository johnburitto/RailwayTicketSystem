using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestAuthController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public string TestAuth()
        {
            return "Hello, you are authorized.";
        }

        [HttpGet("user")]
        [Authorize(Roles = "User")]
        public string TestUserAuth()
        {
            return "Hello, you are authorized User.";
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public string TestAdminAuth()
        {
            return "Hello, you are authorized Admin.";
        }
    }
}
