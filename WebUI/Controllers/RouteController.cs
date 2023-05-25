using AutoMapper;
using Core.Dtos.Create;
using Core.Dtos.Update;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;

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

        public async Task<IActionResult> Index()
        {
            return View(new RawDataModel<Core.Entities.Route> { Data = await _service.GetAllRawAsync() });
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAction(RouteCreateDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateAsync(dto);
            }

            return Redirect("/Route/Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            var route = await _service.GetByIdAsync(id);

            if (route  == null)
            {
                return Redirect("/Route/Index");
            }

            return View(_mapper.Map<RouteUpdateDto>(route));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAction(RouteUpdateDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.UpdateAsync(dto);
            }

            return Redirect("/Route/Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var route = await _service.GetByIdAsync(id);

            if (route == null)
            {
                return Redirect("/Route/Index");
            }

            await _service.DeleteAsync(route);

            return Redirect("/Route/Index");
        }
    }
}
