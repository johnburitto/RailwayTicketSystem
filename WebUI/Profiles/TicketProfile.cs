using AutoMapper;
using Core.Dtos.Create;
using Core.Dtos.Read;
using Core.Dtos.Update;
using Core.Entities;

namespace WebUI.Profiles
{
    public class TicketProfile : Profile
    {
        public TicketProfile() 
        { 
            CreateMap<TicketCreateDto, Ticket>()
                .ForMember(dest => dest.ActionUser, options => options.MapFrom(src => Environment.UserName))
                .ForMember(dest => dest.CreatedAt, options => options.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, options => options.MapFrom(src => DateTime.UtcNow));
            CreateMap<TicketUpdateDto, Ticket>()
                .ForMember(dest => dest.ActionUser, options => options.MapFrom(src => Environment.UserName))
                .ForMember(dest => dest.UpdatedAt, options => options.MapFrom(src => DateTime.UtcNow));
            CreateMap<Ticket, TicketUpdateDto>();
            CreateMap<Ticket, TicketReadDto>();
        }
    }
}
