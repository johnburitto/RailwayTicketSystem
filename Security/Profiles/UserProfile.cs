using AutoMapper;
using Security.Dto;
using Security.Entities;

namespace Security.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<UserRegistrationDto, User>();
            CreateMap<UserCreateDto, User>();
            //CreateMap<UserUpdateDto, User>()
            //    .ForMember(dest => dest.Id, options => options.Ignore())
            //    .ForMember(dest => dest.SecurityStamp, options => options.Ignore())
            //    .ForMember(dest => dest.ConcurrencyStamp, options => options.Ignore())
            //    .ForMember(dest => dest.PasswordHash, options => options.Ignore())
            //    .ForMember(dest => dest.FirstName, options => options.MapFrom(src => src.FirstName))
            //    .ForMember(dest => dest.MiddleName, options => options.MapFrom(src => src.MiddleName))
            //    .ForMember(dest => dest.LastName, options => options.MapFrom(src => src.LastName))
            //    .ForMember(dest => dest.UserName, options => options.MapFrom(src => src.UserName))
            //    .ForMember(dest => dest.Email, options => options.MapFrom(src => src.Email))
            //    .ForMember(dest => dest.PhoneNumber, options => options.MapFrom(src => src.PhoneNumber));
        }
    }
}
