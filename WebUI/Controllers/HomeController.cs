using AutoMapper;
using Core.Dtos.Read;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITrainService _service;
        private readonly IMapper _mapper;

        public HomeController(ITrainService service, IMapper mapper)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Book(int trainId)
        {
            return View(_mapper.Map<TrainReadDto>(await _service.GetByIdAsync(trainId)));
        }
    }
}
