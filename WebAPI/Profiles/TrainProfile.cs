using AutoMapper;
using Core.Dtos.Create;
using Core.Dtos.Read;
using Core.Dtos.Update;
using Core.Entities;

namespace WebAPI.Profiles
{
    public class TrainProfile : Profile
    {
        public TrainProfile() 
        {
            CreateMap<TrainCreateDto, Train>()
                .ForMember(dest => dest.ActionUser, options => options.MapFrom(src => Environment.UserName))
                .ForMember(dest => dest.CreatedAt, options => options.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, options => options.MapFrom(src => DateTime.UtcNow));
            CreateMap<TrainUpdateDto, Train>()
                .ForMember(dest => dest.ActionUser, options => options.MapFrom(src => Environment.UserName))
                .ForMember(dest => dest.UpdatedAt, options => options.MapFrom(src => DateTime.UtcNow));
            CreateMap<Train, TrainReadDto>();
        }
    }
}
