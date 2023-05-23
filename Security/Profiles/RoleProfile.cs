using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Security.Dto;

namespace Security.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile() 
        {
            CreateMap<RoleDto, IdentityRole>()
                .ForMember(dest => dest.Name, options => options.MapFrom(src => src.RoleName))
                .ForMember(dest => dest.NormalizedName, options => options.MapFrom(src => src.RoleName.ToUpper()))
                .ForMember(dest => dest.ConcurrencyStamp, options => options.MapFrom(src => Guid.NewGuid().ToString()));
        }
    }
}
