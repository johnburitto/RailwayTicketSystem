using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Security.Dto;
using Security.Entities;
using Security.Services.Interfaces;

namespace Security.Controllers
{
    [EnableCors("CORSPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IConfiguration _conf;

        public UserController(IUserService service, IConfiguration conf)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _conf = conf ?? throw new ArgumentNullException(nameof(conf));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<User>>> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<User>> GetByIdAsync(string id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpGet("roles/{id}")]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<string>>> GetUserRolesAsync(string id)
        {
            return Ok(await _service.GetUserRolesAsync(id));
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> RegisterAsync([FromBody] UserRegistrationDto dto)
        {
            return Ok((await _service.RegisterAsync(dto)).ToString());
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> LoginAsync([FromBody] UserLoginDto dto)
        {
            return Ok((await _service.LoginAsync(dto)).ToString());
        }

        [HttpGet("delete/{id}")]
        public async Task<IActionResult> DeleteByIdAsync(string id)
        {
            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = requestCulture?.RequestCulture.Culture.ToString().Substring(0, 2);

            await _service.DeleteByIdAsync(id);

            return Redirect($"{_conf["WebUIString"]}/{culture}/User/Index");
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromForm] UserCreateDto dto)
        {
            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = requestCulture?.RequestCulture.Culture.ToString().Substring(0, 2);

            if (ModelState.IsValid)
            {
                if ((await _service.CreateAsync(dto)) == ResponseType.InternalError)
                {
                    return Redirect($"{_conf["WebUIString"]}/{culture}/User/Create");
                }

                return Redirect($"{_conf["WebUIString"]}/{culture}/User/Index");
            }

            return Redirect($"{_conf["WebUIString"]}/{culture}/User/Create");
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateAsync([FromForm] UserUpdateDto dto)
        {
            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = requestCulture?.RequestCulture.Culture.ToString().Substring(0, 2);

            if (ModelState.IsValid)
            {
                if ((await _service.UpdateAsync(dto)) == ResponseType.InternalError)
                {
                    return Redirect($"{_conf["WebUIString"]}/{culture}/User/Update/{dto.Id}");
                }

                return Redirect($"{_conf["WebUIString"]}/{culture}/User/Index");
            }

            return Redirect($"{_conf["WebUIString"]}/{culture}/User/Update/{dto.Id}");
        }

        [HttpPost("register-ui")]
        public async Task<IActionResult> RegisterUIAsync([FromForm] UserRegistrationDto dto)
        {
            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = requestCulture?.RequestCulture.Culture.ToString().Substring(0, 2);

            if (ModelState.IsValid)
            {
                if ((await _service.RegisterAsync(dto)) == ResponseType.InternalError)
                {
                    return Redirect($"{_conf["WebUIString"]}/{culture}/User/Register");
                }

                return Redirect($"{_conf["WebUIString"]}/{culture}/Redirect/LoginPoint");
            }

            return Redirect($"{_conf["WebUIString"]}/{culture}/User/Register");
        }
    }
}
