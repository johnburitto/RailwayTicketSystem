using AutoMapper;
using Core.Dtos.Create;
using Core.Dtos.Update;
using Core.Entities;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using WebUI.Paging;

namespace WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RouteController : Controller
    {
        private readonly IRouteService _service;
        private readonly IMapper _mapper;

        public RouteController(IRouteService service, IMapper mapper)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IActionResult> Index([FromQuery] PagingParams pagingParams)
        {
            return View(PagingListModel<Core.Entities.Route>.Create(await _service.GetAllRawAsync(), pagingParams));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAction(RouteCreateDto dto)
        {
            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = requestCulture?.RequestCulture.Culture;

            if (ModelState.IsValid)
            {
                await _service.CreateAsync(dto);
            }

            return Redirect($"/{culture}/Route/Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = requestCulture?.RequestCulture.Culture;
            var route = await _service.GetByIdAsync(id);

            if (route  == null)
            {
                return Redirect($"/{culture}/Route/Index");
            }

            return View(_mapper.Map<RouteUpdateDto>(route));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAction(RouteUpdateDto dto)
        {
            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = requestCulture?.RequestCulture.Culture;

            if (ModelState.IsValid)
            {
                await _service.UpdateAsync(dto);
            }

            return Redirect($"/{culture}/Route/Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = requestCulture?.RequestCulture.Culture;
            var route = await _service.GetByIdAsync(id);

            if (route == null)
            {
                return Redirect($"/{culture}/Route/Index");
            }

            await _service.DeleteAsync(route);

            return Redirect($"/{culture}/Route/Index");
        }
    }
}
