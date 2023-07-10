using Core.Dtos.Create;
using Core.Dtos.Update;
using Core.Entities;

namespace Infrastructure.Services.Interfaces
{
    public interface ITicketService : IService<Ticket, TicketCreateDto, TicketUpdateDto>
    {
        Task<List<Ticket>> CreateAsync(List<TicketCreateDto> dtos);
    }
}
