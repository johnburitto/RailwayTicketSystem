using AutoMapper;
using Core.Dtos.Create;
using Core.Dtos.Update;
using Core.Entities;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class PlaceController : Controller
    {
        private readonly IPlaceService _service;
        private readonly IMapper _mapper;

        public PlaceController(IPlaceService service, IMapper mapper)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IActionResult> Index()
        {
            return View(new RawDataModel<Place> { Data = await _service.GetAllRawAsync() });
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAction(PlaceCreateDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateAsync(dto);
            }

            return Redirect("/Place/Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            var place = await _service.GetByIdAsync(id);

            if (place == null)
            {
                return Redirect("/Place/Index");
            }

            return View(_mapper.Map<PlaceUpdateDto>(place));
        }

        public async Task<IActionResult> UpdateAction(PlaceUpdateDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.UpdateAsync(dto);
            }

            return Redirect("/Place/Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var place = await _service.GetByIdAsync(id);

            if (place == null)
            {
                return Redirect("/Place/Index");
            }

            await _service.DeleteAsync(place);

            return Redirect("/Place/Index");
        }
    }
}
