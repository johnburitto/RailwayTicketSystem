using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Security.Dto;
using Security.Services.Interfaces;

namespace Security.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _service;

        public RoleController(IRoleService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<IdentityRole>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<IdentityRole>>> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpPost("add")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> AddAsync([FromBody] RoleDto dto)
        {
            return Ok(await _service.AddAsync(dto));
        }

        [HttpPost("remove")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> RemoveRole([FromBody] RoleDto dto)
        {
            return Ok(await _service.RemoveAsync(dto));
        }
    }
}
