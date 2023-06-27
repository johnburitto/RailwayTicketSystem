using Core.Dtos.Create;
using Core.Dtos.Update;
using Core.Entities;
using Infrastructure.Dtos;

namespace Infrastructure.Services.Interfaces
{
    public interface IRouteService : IService<Route, RouteCreateDto, RouteUpdateDto>
    {
        public Task<List<Route>> SearchRoutesAsync(SearchRoutesDto dto);
    }
}
