using AutoMapper;
using Core.Dtos.Create;
using Core.Dtos.Update;
using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Impls
{
    public class TrainService : ITrainService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TrainService(AppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Train> CreateAsync(TrainCreateDto dto)
        {
            var train = _mapper.Map<Train>(dto);

            await _context.Trains.AddAsync(train);
            await _context.SaveChangesAsync();

            return train;
        }

        public async Task DeleteAsync(Train entity)
        {
            _context.Trains.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Train>> GetAllAsync()
        {
            return await _context.Trains
                .Include(train => train.Route)
                .Include(train => train.TrainCars)
                .ThenInclude(trainCar => trainCar.Places).ToListAsync();
        }

        public async Task<Train?> GetByIdAsync(int id)
        {
            return await _context.Trains
                .Where(train => train.Id == id)
                .Include(train => train.Route)
                .Include(train => train.TrainCars)
                .ThenInclude(trainCar => trainCar.Places).FirstOrDefaultAsync();
        }

        public async Task<Train> UpdateAsync(TrainUpdateDto dto)
        {
            var train = await GetByIdAsync(dto.Id);

            _mapper.Map(dto, train ?? throw new ArgumentNullException(nameof(train)));
            _context.Entry(train).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return train;
        }
    }
}
