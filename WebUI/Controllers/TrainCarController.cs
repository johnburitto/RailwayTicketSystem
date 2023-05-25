using AutoMapper;
using Core.Dtos.Create;
using Core.Dtos.Update;
using Core.Entities;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;

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

        public async Task<IActionResult> Index()
        {
            return View(new RawDataModel<TrainCar> { Data = await _service.GetAllRawAsync() });
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAction(TrainCarCreateDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateAsync(dto);
            }

            return Redirect("/TrainCar/Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            var trainCar = await _service.GetByIdAsync(id);

            if (trainCar == null)
            {
                return Redirect("/TrainCar/Index");
            }

            return View(_mapper.Map<TrainCarUpdateDto>(trainCar));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAction(TrainCarUpdateDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.UpdateAsync(dto);
            }

            return Redirect("/TrainCar/Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var trainCar = await _service.GetByIdAsync(id);

            if (trainCar == null)
            {
                return Redirect("/TrainCar/Index");
            }

            await _service.DeleteAsync(trainCar);

            return Redirect("/TrainCar/Index");
        }
    }
}
