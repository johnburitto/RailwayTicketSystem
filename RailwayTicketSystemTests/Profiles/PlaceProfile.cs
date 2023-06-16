using AutoMapper;
using Core.Dtos.Create;
using Core.Dtos.Read;
using Core.Dtos.Update;
using Core.Entities;

namespace RailwayTicketSystemTests.Profiles
{
    public class PlaceProfile : Profile
    {
        public PlaceProfile() 
        {
            CreateMap<PlaceCreateDto, Place>()
                .ForMember(dest => dest.ActionUser, options => options.MapFrom(src => Environment.UserName))
                .ForMember(dest => dest.CreatedAt, options => options.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, options => options.MapFrom(src => DateTime.UtcNow));
            CreateMap<PlaceUpdateDto, Place>()
                .ForMember(dest => dest.ActionUser, options => options.MapFrom(src => Environment.UserName))
                .ForMember(dest => dest.UpdatedAt, options => options.MapFrom(src => DateTime.UtcNow));
            CreateMap<Place, PlaceReadDto>();
        }
    }
}
