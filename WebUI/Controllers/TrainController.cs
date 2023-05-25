using AutoMapper;
using Core.Dtos.Create;
using Core.Dtos.Update;
using Core.Entities;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class TrainController : Controller
    {
        private readonly ITrainService _service;
        private readonly IMapper _mapper;

        public TrainController(ITrainService service, IMapper mapper)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IActionResult> Index()
        {
            return View(new RawDataModel<Train> { Data = await _service.GetAllRawAsync() });
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAction(TrainCreateDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateAsync(dto);
            }

            return Redirect("/Train/Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            var train = await _service.GetByIdAsync(id);

            if (train == null)
            {
                return Redirect("/Train/Index");
            }

            return View(_mapper.Map<TrainUpdateDto>(train));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAction(TrainUpdateDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.UpdateAsync(dto);
            }

            return Redirect("/Train/Index");
        } 

        public async Task<IActionResult> Delete(int id)
        {
            var train = await _service.GetByIdAsync(id);

            if (train == null)
            {
                return Redirect("/Train/Index");
            }

            await _service.DeleteAsync(train);

            return Redirect("/Train/Index");
        }
    }
}
