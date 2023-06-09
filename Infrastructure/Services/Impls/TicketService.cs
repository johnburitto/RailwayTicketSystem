﻿using AutoMapper;
using Core.Dtos.Create;
using Core.Dtos.Update;
using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Impls
{
    public class TicketService : ITicketService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TicketService(AppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Ticket> CreateAsync(TicketCreateDto dto)
        {
            var ticket = _mapper.Map<Ticket>(dto);

            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();

            return ticket;
        }

        public async Task DeleteAsync(Ticket entity)
        {
            _context.Tickets.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public Task<List<Ticket>> GetAllAsync()
        {
            return _context.Tickets
                .Include(ticket => ticket.Place)
                .ThenInclude(place => place.TrainCar)
                .ThenInclude(trainCar => trainCar.Train)
                .ThenInclude(train => train.Route).ToListAsync();
        }

        public async Task<Ticket?> GetByIdAsync(int id)
        {
            return await _context.Tickets
                .Where(ticket => ticket.Id == id)
                .Include(ticket => ticket.Place)
                .ThenInclude(place => place.TrainCar)
                .ThenInclude(trainCar => trainCar.Train)
                .ThenInclude(train => train.Route).FirstOrDefaultAsync();
        }

        public async Task<Ticket> UpdateAsync(TicketUpdateDto dto)
        {
            var ticket = await GetByIdAsync(dto.Id);

            _mapper.Map(dto, ticket ?? throw new ArgumentNullException(nameof(ticket)));
            _context.Entry(ticket).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return ticket;
        }
    }
}
