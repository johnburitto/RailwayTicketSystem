using AutoMapper;
using Core.Dtos.Create;
using Core.Dtos.Update;
using Core.Entities;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicketService _service;
        private readonly IMapper _mapper;

        public TicketController(ITicketService service, IMapper mapper)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IActionResult> Index()
        {
            return View(new RawDataModel<Ticket> { Data = await _service.GetAllRawAsync() });
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> CreateAction(TicketCreateDto dto)
        {
            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = requestCulture?.RequestCulture.Culture;

            if (ModelState.IsValid)
            {
                await _service.CreateAsync(dto);
            }

            return Redirect($"/{culture}/Ticket/Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = requestCulture?.RequestCulture.Culture;
            var ticket = await _service.GetByIdAsync(id);

            if (ticket == null)
            {
                return Redirect($"/{culture}/Ticket/Index");
            }

            return View(_mapper.Map<TicketUpdateDto>(ticket));
        }

        public async Task<IActionResult> UpdateAction(TicketUpdateDto dto)
        {
            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = requestCulture?.RequestCulture.Culture;

            if (ModelState.IsValid)
            {
                await _service.UpdateAsync(dto);
            }

            return Redirect($"/{culture}/Ticket/Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = requestCulture?.RequestCulture.Culture;
            var ticket = await _service.GetByIdAsync(id);

            if (ticket == null)
            {
                return Redirect($"/{culture}/Ticket/Index");
            }

            await _service.DeleteAsync(ticket);

            return Redirect($"/{culture}/Ticket/Index");
        }
    }
}
