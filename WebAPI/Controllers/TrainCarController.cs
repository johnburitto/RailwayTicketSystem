using AutoMapper;
using Core.Dtos.Create;
using Core.Dtos.Update;
using Core.Entities;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainCarController : ControllerBase
    {
        private readonly ITrainCarService _service;

        public TrainCarController(ITrainCarService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<TrainCar>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<TrainCar>>> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync()); 
        }

        [HttpGet("{id}", Name = "GetTrainCarById")]
        [ProducesResponseType(typeof(TrainCar), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TrainCar>> GetByIdAsync(int id)
        {
            var trainCar = await _service.GetByIdAsync(id);

            return trainCar == null ? NotFound() : Ok(trainCar);
        }

        [HttpPost]
        [ProducesResponseType(typeof(TrainCar), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TrainCar>> CreateAsync([FromBody] TrainCarCreateDto dto)
        {
            var trainCar = await _service.CreateAsync(dto);

            return CreatedAtRoute("GetTrainCarById", new { trainCar.Id }, trainCar);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TrainCar), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TrainCar>> UpdateAsync(int id, [FromBody] TrainCarUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var trainCar = await _service.GetByIdAsync(id);

            return trainCar == null ? NotFound() : Ok(await _service.UpdateAsync(dto)); 
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var trainCar = await _service.GetByIdAsync(id);

            if (trainCar == null)
            {
                return NotFound();
            }

            await _service.DeleteAsync(trainCar);

            return Ok();
        }
    }
}
