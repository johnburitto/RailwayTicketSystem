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
        }
    }
}
