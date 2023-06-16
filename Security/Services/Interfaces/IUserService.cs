using Security.Dto;
using Security.Entities;

namespace Security.Services.Interfaces
{
    public interface IUserService
    {
        public Task<List<User>> GetAllAsync();
        public Task<User?> GetByIdAsync(string id);
        public Task<ResponseType> RegisterAsync(UserRegistrationDto dto);
        public Task<ResponseType> CreateAsync(UserCreateDto dto);
        public Task<ResponseType> UpdateAsync(UserUpdateDto dto);
        public Task<ResponseType> LoginAsync(UserLoginDto dto);
        public Task<List<string>> GetUserRolesAsync(string id);
        public Task DeleteByIdAsync(string id);
    }

    public enum ResponseType 
    {
        Created,
        Updated,
        Logined,
        BadCredentials,
        Registered,
        AlreadyRegistered,
        NotRegistered,
        InternalError,
        InternalErrorRoleCause,
        InternalErrorPasswordCause,
        BadRole
    }
}
