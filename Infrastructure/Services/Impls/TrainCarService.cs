using AutoMapper;
using Core.Dtos.Create;
using Core.Dtos.Update;
using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Impls
{
    public class TrainCarService : ITrainCarService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TrainCarService(AppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<TrainCar> CreateAsync(TrainCarCreateDto dto)
        {
            var trainCar = _mapper.Map<TrainCar>(dto);

            await _context.TrainCars.AddAsync(trainCar);
            await _context.SaveChangesAsync();

            return trainCar;
        }

        public async Task DeleteAsync(TrainCar entity)
        {
            _context.TrainCars.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TrainCar>> GetAllAsync()
        {
            return await _context.TrainCars
                .Include(trainCar => trainCar.Train)
                .ThenInclude(train => train.Route)
                .Include(trainCar => trainCar.Places).ToListAsync();
        }

        public Task<List<TrainCar>> GetAllRawAsync()
        {
            return _context.TrainCars.ToListAsync();
        }

        public async Task<TrainCar?> GetByIdAsync(int id)
        {
            return await _context.TrainCars
                .Where(trainCar => trainCar.Id == id)
                .Include(trainCar => trainCar.Train)
                .ThenInclude(train => train.Route)
                .Include(trainCar => trainCar.Places).FirstOrDefaultAsync();
        }

        public async Task<TrainCar> UpdateAsync(TrainCarUpdateDto dto)
        {
            var trainCar = await GetByIdAsync(dto.Id);

            _mapper.Map(dto, trainCar ?? throw new ArgumentNullException(nameof(trainCar)));
            _context.Update(trainCar);
            await _context.SaveChangesAsync();

            return trainCar;
        }
    }
}
