using Core.Dtos.Create;
using Core.Dtos.Update;
using Core.Entities;

namespace Infrastructure.Services.Interfaces
{
    public interface ITrainService : IService<Train, TrainCreateDto, TrainUpdateDto>
    {

    }
}
