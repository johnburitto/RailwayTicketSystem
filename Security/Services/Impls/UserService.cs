using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Security.Configurations;
using Security.Dto;
using Security.Entities;
using Security.Services.Interfaces;

namespace Security.Services.Impls
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _manager;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> manager, IMapper mapper)
        {
            _manager = manager ?? throw new ArgumentNullException(nameof(manager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _manager.Users.ToListAsync();
        }

        public async Task<ResponseType> RegisterAsync(UserRegistrationDto dto)
        {
            var expectedUser = await _manager.FindByEmailAsync(dto.Email);

            if (expectedUser != null)
            {
                return ResponseType.AlreadyRegistered;
            }

            var user = _mapper.Map<User>(dto);
            var result = await _manager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                await _manager.AddToRoleAsync(user, Authorization.DEFAULT_ROLE.ToString());

                return ResponseType.Registered;
            }
            else
            {
                return ResponseType.InternalError;
            }
        }

        public async Task<ResponseType> LoginAsync(UserLoginDto dto)
        {
            var expectedUser = await _manager.FindByNameAsync(dto.UserName);

            if (expectedUser != null)
            {
                return await _manager.CheckPasswordAsync(expectedUser, dto.Password) ? ResponseType.Logined : ResponseType.BadCredentials;
            }
            else
            {
                return ResponseType.NotRegistered;
            }
        }
    }
}
