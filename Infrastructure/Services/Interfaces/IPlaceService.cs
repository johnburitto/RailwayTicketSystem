using Core.Dtos.Create;
using Core.Dtos.Update;
using Core.Entities;

namespace Infrastructure.Services.Interfaces
{
    public interface IPlaceService : IService<Place, PlaceCreateDto, PlaceUpdateDto>
    {
        public Task DisablePlaceAsync(int id);
    }
}
