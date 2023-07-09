using Core.Dtos.Create;
using Core.Dtos.Update;
using Core.Entities;
using Infrastructure.Dtos;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [EnableCors("CORSPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class RouteController : ControllerBase
    {
        private readonly IRouteService _service;

        public RouteController(IRouteService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        [Authorize("read")]
        [ProducesResponseType(typeof(List<Core.Entities.Route>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Core.Entities.Route>>> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}", Name = "GetRouteById")]
        [Authorize("read")]
        [ProducesResponseType(typeof(Core.Entities.Route), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Core.Entities.Route>> GetByIdAsync(int id)
        {
            var route = await _service.GetByIdAsync(id);

            return route == null ? NotFound() : Ok(route);
        }

        [HttpPost]
        [Authorize("write")]
        [ProducesResponseType(typeof(Core.Entities.Route), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Core.Entities.Route>> CreateAsync([FromBody] RouteCreateDto dto)
        {
            var route = await _service.CreateAsync(dto);

            return CreatedAtRoute("GetRouteById", new { route.Id }, route);
        }

        [HttpPut("{id}")]
        [Authorize("write")]
        [ProducesResponseType(typeof(Core.Entities.Route), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Core.Entities.Route>> UpdateAsync(int id, [FromBody] RouteUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var route = await _service.GetByIdAsync(id);

            return route == null ? NotFound() : Ok(await _service.UpdateAsync(dto));
        }

        [HttpDelete("{id}")]
        [Authorize("write")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var route = await _service.GetByIdAsync(id);

            if (route == null)
            {
                return NotFound();
            }

            await _service.DeleteAsync(route);

            return Ok();
        }

        [HttpPost("search")]
        [Authorize("read")]
        [ProducesResponseType(typeof(List<Core.Entities.Route>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Core.Entities.Route>>> SearchRoutesAsync([FromBody] SearchRoutesDto dto)
        {
            return Ok(await _service.SearchRoutesAsync(dto));
        }
    }
}
