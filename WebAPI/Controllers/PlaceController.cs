using Core.Dtos.Create;
using Core.Dtos.Update;
using Core.Entities;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaceController : ControllerBase
    {
        private readonly IPlaceService _service;

        public PlaceController(IPlaceService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Place>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Place>>> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}", Name = "GetPlaceById")]
        [ProducesResponseType(typeof(Place), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Place>> GetByIdAsync(int id)
        {
            var place = await _service.GetByIdAsync(id);

            return place == null ? NotFound() : Ok(place);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Place), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Place>> CreateAsync([FromBody] PlaceCreateDto dto)
        {
            var place = await _service.CreateAsync(dto);

            return CreatedAtRoute("GetPlaceById", new { place.Id }, place);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Place), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Place>> UpdateAsync(int id, [FromBody] PlaceUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var place = await _service.GetByIdAsync(id);

            return place == null ? NotFound() : Ok(await _service.UpdateAsync(dto));
        }

        [HttpDelete("id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var palce = await _service.GetByIdAsync(id);

            if (palce == null)
            {
                return NotFound();
            }

            await _service.DeleteAsync(palce);

            return Ok();
        }
    }
}
