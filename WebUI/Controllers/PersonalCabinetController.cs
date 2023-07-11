using AutoMapper;
using Core.Entities;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Security.Dto;
using System.Security.Claims;
using WebUI.Paging;

namespace WebUI.Controllers
{
    [Authorize(Roles = "User")]
    public class PersonalCabinetController : Controller
    {
        private readonly ITicketService _service;

        public PersonalCabinetController(ITicketService service, IMapper mapper)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Update(string id)
        {
            return View(new UserUpdateDto { Id = id });
        }

        public async Task<IActionResult> UserTickets(string id, [FromQuery] PagingParams pagingParams)
        {
            return View(PagingListModel<Ticket>.Create(await _service.GetUserTicketsAsync(id), pagingParams));
        }

        public async Task<IActionResult> Return(int id)
        {
            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = requestCulture?.RequestCulture.Culture;
            var ticket = await _service.GetByIdAsync(id);

            if (ticket == null)
            {
                return Redirect($"/{culture}/PersonalCabinet/UserTickets?id={User.FindFirstValue(ClaimTypes.NameIdentifier)}&{new PagingParams()}");
            }

            await _service.DeleteAsync(ticket);

            return Redirect($"/{culture}/PersonalCabinet/UserTickets?id={User.FindFirstValue(ClaimTypes.NameIdentifier)}&{new PagingParams()}");
        }
    }
}
