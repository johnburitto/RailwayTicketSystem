using AutoMapper;
using Core.Dtos.Create;
using Core.Dtos.Read;
using Core.Dtos.Update;
using Core.Entities;

namespace WebUI.Profiles
{
    public class TrainCarProfile : Profile
    {
        public TrainCarProfile()
        {
            CreateMap<TrainCarCreateDto, TrainCar>()
                .ForMember(dest => dest.ActionUser, options => options.MapFrom(src => Environment.UserName))
                .ForMember(dest => dest.CreatedAt, options => options.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, options => options.MapFrom(src => DateTime.UtcNow));
            CreateMap<TrainCarUpdateDto, TrainCar>()
                .ForMember(dest => dest.ActionUser, options => options.MapFrom(src => Environment.UserName))
                .ForMember(dest => dest.UpdatedAt, options => options.MapFrom(src => DateTime.UtcNow));
            CreateMap<TrainCar, TrainCarUpdateDto>();
            CreateMap<TrainCar, TrainCarReadDto>();
        }
    }
}
