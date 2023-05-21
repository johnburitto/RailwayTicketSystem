using AutoMapper;
using Core.Dtos.Create;
using Core.Dtos.Update;
using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Impls
{
    public class RouteService : IRouteService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public RouteService(AppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Route> CreateAsync(RouteCreateDto dto)
        {
            var route = _mapper.Map<Route>(dto);

            await _context.AddAsync(route);
            await _context.SaveChangesAsync();

            return route;
        }

        public async Task DeleteAsync(Route entity)
        {
            _context.Routes.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Route>> GetAllAsync()
        {
            return await _context.Routes
                .Include(route => route.Trains)
                .ThenInclude(train => train.TrainCars)
                .ThenInclude(trainCar => trainCar.Places).ToListAsync();
        }

        public async Task<Route?> GetByIdAsync(int id)
        {
            return await _context.Routes
                .Where(route => route.Id == id)
                .Include(route => route.Trains)
                .ThenInclude(train => train.TrainCars)
                .ThenInclude(trainCar => trainCar.Places).FirstOrDefaultAsync();
        }

        public async Task<Route> UpdateAsync(RouteUpdateDto dto)
        {
            var route = await GetByIdAsync(dto.Id);

            _mapper.Map(dto, route ?? throw new ArgumentNullException(nameof(route)));
            _context.Entry(route).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return route;
        }
    }
}
