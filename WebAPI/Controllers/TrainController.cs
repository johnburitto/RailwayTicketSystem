using Core.Dtos.Create;
using Core.Dtos.Update;
using Core.Entities;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainController : ControllerBase
    {
        private readonly ITrainService _service;

        public TrainController(ITrainService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Train>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Train>>> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}", Name = "GetTrainById")]
        [ProducesResponseType(typeof(Train), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Train>> GetByIdAsync(int id)
        {
            var train = await _service.GetByIdAsync(id);

            return train == null ? NotFound() : Ok(train);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Train), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Train>> CreateAsync([FromBody] TrainCreateDto dto)
        {
            var train = await _service.CreateAsync(dto);

            return CreatedAtRoute("GetTrainById", new { train.Id }, train);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Train), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Train>> UpdateAsync(int id, [FromBody] TrainUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var train = await _service.GetByIdAsync(id);

            return train == null ? NotFound() : Ok(await _service.UpdateAsync(dto));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var train = await _service.GetByIdAsync(id);

            if (train == null)
            {
                return NotFound();
            }

            await _service.DeleteAsync(train);

            return Ok();
        }
    }
}
