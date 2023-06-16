using AutoMapper;
using Core.Dtos.Create;
using Core.Dtos.Read;
using Core.Dtos.Update;

namespace RailwayTicketSystemTests.Profiles
{
    public class RouteProfile : Profile
    {
        public RouteProfile() 
        {
            CreateMap<RouteCreateDto, Core.Entities.Route>()
                .ForMember(dest => dest.ActionUser, options => options.MapFrom(src => Environment.UserName))
                .ForMember(dest => dest.CreatedAt, options => options.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, options => options.MapFrom(src => DateTime.UtcNow));
            CreateMap<RouteUpdateDto, Core.Entities.Route>()
                .ForMember(dest => dest.ActionUser, options => options.MapFrom(src => Environment.UserName))
                .ForMember(dest => dest.UpdatedAt, options => options.MapFrom(src => DateTime.UtcNow));
            CreateMap<Core.Entities.Route, RouteReadDto>();
        }
    }
}
