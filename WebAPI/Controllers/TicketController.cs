﻿using Core.Dtos.Create;
using Core.Dtos.Update;
using Core.Entities;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _service;

        public TicketController(ITicketService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Ticket>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Ticket>>> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}", Name = "GetTicketById")]
        [ProducesResponseType(typeof(Ticket), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Ticket>> GetByIdAsync(int id)
        {
            var ticket = await _service.GetByIdAsync(id);

            return ticket == null ? NotFound() : Ok(ticket);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Ticket), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Ticket>> CreateAsync([FromBody] TicketCreateDto dto)
        {
            var ticket = await _service.CreateAsync(dto);

            return CreatedAtRoute("GetTicketById", new { ticket.Id }, ticket);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Ticket), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Ticket>> UpdateAsync(int id, [FromBody] TicketUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var ticket = await _service.GetByIdAsync(id);

            return ticket == null ? NotFound() : Ok(await _service.UpdateAsync(dto)); 
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var ticket = await _service.GetByIdAsync(id);
            
            if (ticket == null)
            {
                return NotFound();
            }

            await _service.DeleteAsync(ticket);

            return Ok();
        }
    }
}
