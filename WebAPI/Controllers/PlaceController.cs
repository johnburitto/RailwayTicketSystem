﻿using Core.Dtos.Create;
using Core.Dtos.Update;
using Core.Entities;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [EnableCors("CORSPolicy")]
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
        [Authorize("read")]
        [ProducesResponseType(typeof(List<Place>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Place>>> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}", Name = "GetPlaceById")]
        [Authorize("read")]
        [ProducesResponseType(typeof(Place), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Place>> GetByIdAsync(int id)
        {
            var place = await _service.GetByIdAsync(id);

            return place == null ? NotFound() : Ok(place);
        }

        [HttpPost]
        [Authorize("write")]
        [ProducesResponseType(typeof(Place), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Place>> CreateAsync([FromBody] PlaceCreateDto dto)
        {
            var place = await _service.CreateAsync(dto);

            return CreatedAtRoute("GetPlaceById", new { place.Id }, place);
        }

        [HttpPut("{id}")]
        [Authorize("write")]
        [ProducesResponseType(typeof(Place), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
        [Authorize("write")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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

        [HttpGet("{trainCarId}/{placeType}/count")]
        [Authorize("read")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> GetNumberOfPlacesInTrainCarByPlaceTypeAsync(int trainCarId, PlaceType placeType)
        {
            return await _service.GetNumberOfPlacesInTrainCarByPlaceTypeAsync(trainCarId, placeType);
        }

        [HttpGet("{trainCarId}/places")]
        [Authorize("read")]
        [ProducesResponseType(typeof(List<Place>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Place>>> GetPlacesOfTrainCarAsync(int trainCarId)
        {
            return await _service.GetPlacesOfTrainCarAsync(trainCarId);
        }
    }
}
