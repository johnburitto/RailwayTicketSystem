using Security.Dto;
using Security.Entities;

namespace Security.Services.Interfaces
{
    public interface IUserService
    {
        public Task<List<User>> GetAllAsync();
        public Task<ResponseType> RegisterAsync(UserRegistrationDto dto);
        public Task<ResponseType> LoginAsync(UserLoginDto dto);
    }

    public enum ResponseType 
    {
        Logined,
        BadCredentials,
        Registered,
        AlreadyRegistered,
        NotRegistered,
        InternalError
    }
}
