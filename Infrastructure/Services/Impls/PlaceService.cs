using AutoMapper;
using Core.Dtos.Create;
using Core.Dtos.Update;
using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Impls
{
    public class PlaceService : IPlaceService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PlaceService(AppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Place> CreateAsync(PlaceCreateDto dto)
        {
            var place = _mapper.Map<Place>(dto);

            await _context.Places.AddAsync(place);
            await _context.SaveChangesAsync();

            return place;
        }

        public async Task DeleteAsync(Place entity)
        {
            _context.Places.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Place>> GetAllAsync()
        {
            return await _context.Places
                .Include(place => place.TrainCar)
                .ThenInclude(trainCar => trainCar.Train)
                .ThenInclude(train => train.Route)
                .Include(place => place.Tickets).ToListAsync();
        }

        public async Task<List<Place>> GetAllRawAsync()
        {
            return await _context.Places.ToListAsync();
        }

        public async Task<Place?> GetByIdAsync(int id)
        {
            return await _context.Places
                .Where(place => place.Id == id)
                .Include(place => place.TrainCar)
                .ThenInclude(trainCar => trainCar.Train)
                .ThenInclude(train => train.Route)
                .Include(place => place.Tickets).FirstOrDefaultAsync();
        }

        public async Task<Place> UpdateAsync(PlaceUpdateDto dto)
        {
            var place = await GetByIdAsync(dto.Id);

            _mapper.Map(dto, place ?? throw new ArgumentNullException(nameof(place)));
            _context.Update(place);
            await _context.SaveChangesAsync();

            return place;
        }

        public async Task EnablePlaceAsync(int id)
        {
            var place = await GetByIdAsync(id) ?? throw (new ArgumentNullException("Such palce doesn't exist"));

            place.IsAvaliable = true;
            _context.Update(place);
            await _context.SaveChangesAsync();
        }

        public async Task DisablePlaceAsync(int id)
        {
            var place = await GetByIdAsync(id) ?? throw(new ArgumentNullException("Such palce doesn't exist"));

            place.IsAvaliable = false;
            _context.Update(place);
            await _context.SaveChangesAsync();
        }

        public Task<int> GetNumberOfPlacesInTrainCarByPlaceTypeAsync(int trainCarId, PlaceType placetype)
        {
            return Task.FromResult(_context.Places.Where(place => place.TrainCarId == trainCarId && 
                                                                  place.PlaceType == placetype &&
                                                                  place.IsAvaliable).Count());
        }

        public async Task<List<Place>> GetPlacesOfTrainCarAsync(int trainCarId)
        {
            return await _context.Places.Where(place => place.TrainCarId == trainCarId).ToListAsync();
        }
    }
}
