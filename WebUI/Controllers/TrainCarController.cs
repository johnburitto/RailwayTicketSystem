using AutoMapper;
using Core.Dtos.Create;
using Core.Dtos.Update;
using Core.Entities;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;
using WebUI.Paging;

namespace WebUI.Controllers
{
    public class TrainCarController : Controller
    {
        private readonly ITrainCarService _service;
        private readonly IMapper _mapper;

        public TrainCarController(ITrainCarService service, IMapper mapper)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IActionResult> Index([FromQuery] PagingParams pagingParams)
        {
            return View(PagingListModel<TrainCar>.Create(await _service.GetAllRawAsync(), pagingParams));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAction(TrainCarCreateDto dto)
        {
            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = requestCulture?.RequestCulture.Culture;

            if (ModelState.IsValid)
            {
                await _service.CreateAsync(dto);
            }

            return Redirect($"/{culture}/TrainCar/Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = requestCulture?.RequestCulture.Culture;
            var trainCar = await _service.GetByIdAsync(id);

            if (trainCar == null)
            {
                return Redirect($"/{culture}/TrainCar/Index");
            }

            return View(_mapper.Map<TrainCarUpdateDto>(trainCar));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAction(TrainCarUpdateDto dto)
        {
            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = requestCulture?.RequestCulture.Culture;

            if (ModelState.IsValid)
            {
                await _service.UpdateAsync(dto);
            }

            return Redirect($"/{culture}/TrainCar/Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = requestCulture?.RequestCulture.Culture;
            var trainCar = await _service.GetByIdAsync(id);

            if (trainCar == null)
            {
                return Redirect($"/{culture}/TrainCar/Index");
            }

            await _service.DeleteAsync(trainCar);

            return Redirect($"/{culture}/TrainCar/Index");
        }
    }
}
