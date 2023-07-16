using Core.Dtos.Create;
using Core.Dtos.Update;
using Core.Entities;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Extensions;
using WebAPI.Paging;
using System.Text;
using Shared.QR;
using Shared.Email;

namespace WebAPI.Controllers
{
    [EnableCors("CORSPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _service;
        private readonly SmtpEmailSender _emailSender;
        private readonly IConfiguration _conf;

        public TicketController(ITicketService service, SmtpEmailSender emailSender, IConfiguration conf)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _conf = conf ?? throw new ArgumentNullException(nameof(conf));
        }

        [HttpGet]
        [Authorize("read")]
        [ProducesResponseType(typeof(List<Ticket>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Ticket>>> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}", Name = "GetTicketById")]
        [Authorize("read")]
        [ProducesResponseType(typeof(Ticket), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Ticket>> GetByIdAsync(int id)
        {
            var ticket = await _service.GetByIdAsync(id);

            return ticket == null ? NotFound() : Ok(ticket);
        }

        [HttpPost]
        [Authorize("write")]
        [ProducesResponseType(typeof(Ticket), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Ticket>> CreateAsync([FromBody] TicketCreateDto dto)
        {
            var ticket = await _service.CreateAsync(dto);

            return CreatedAtRoute("GetTicketById", new { ticket.Id }, ticket);
        }

        [HttpPost("range")]
        [Authorize("write")]
        [ProducesResponseType(typeof(List<Ticket>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Ticket>>> CreateRangeAsync([FromBody] List<TicketCreateDto> dto)
        {
            return Ok(await _service.CreateAsync(dto));
        }

        [HttpPut("{id}")]
        [Authorize("write")]
        [ProducesResponseType(typeof(Ticket), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
        [Authorize("write")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        [HttpGet("user/{userId}")]
        [Authorize("read")]
        [ProducesResponseType(typeof(List<Ticket>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Ticket>>> GetUserTicketsAsync(string userId)
        {
            return Ok(await _service.GetUserTicketsAsync(userId));
        }

        [HttpGet("send/{email}/{id}/{userId}/{culture}")]
        public async Task<IActionResult> SendTicketAsync(string email, int id, string userId, string culture)
        {
            var ticket = await _service.GetByIdAsync(id);

            await _emailSender.SendEmailAsync(email, "Квиток", ticket.ToHTMLForm());

            return Redirect($"{_conf["WebUIString"]}/{culture}/PersonalCabinet/UserTickets?id={userId}&{new PagingParams()}");
        }

        [HttpGet("qr/dowload/{id}")]
        public async Task<IActionResult> DownloadTicketAsync(int id)
        {
            var ticket = await _service.GetByIdAsync(id);
            byte[] fileBytes = QRCodeGenerator.Generate(ticket);

            return File(fileBytes, "application/force-download", "qr.png");
        }
    }
}
