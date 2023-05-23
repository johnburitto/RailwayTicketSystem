using Microsoft.AspNetCore.Mvc;
using Security.Dto;
using Security.Entities;
using Security.Services.Interfaces;

namespace Security.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<User>>> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
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
    }
}
