using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Security.Configurations;
using Security.Dto;
using Security.Services.Interfaces;

namespace Security.Services.Impls
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _manager;
        private readonly IMapper _mapper;

        public RoleService(RoleManager<IdentityRole> manager, IMapper mapper)
        {
            _manager = manager ?? throw new ArgumentNullException(nameof(manager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
        }

        public async Task<List<IdentityRole>> GetAllAsync()
        {
            return await _manager.Roles.ToListAsync();
        }

        public async Task<string> AddAsync(RoleDto dto)
        {
            if (!Enum.GetNames(typeof(Authorization.Role)).Contains(dto.RoleName))
            {
                return $"System doesn't have role {dto.RoleName}.";
            }

            var expectedRole = await _manager.Roles.Where(role => role.Name == dto.RoleName).FirstOrDefaultAsync();

            if (expectedRole != null)
            {
                return $"Role {dto.RoleName} already exist in database.";
            }

            var role = _mapper.Map<IdentityRole>(dto);
            var result = await _manager.CreateAsync(role);

            return result.Succeeded ? $"Role {dto.RoleName} successfully added to database." : $"Some internal error has occured.";
        }

        public async Task<string> RemoveAsync(RoleDto dto)
        {
            if (!Enum.GetNames(typeof(Authorization.Role)).Contains(dto.RoleName))
            {
                return $"System doesn't have role {dto.RoleName}.";
            }

            var role = await _manager.Roles.Where(role => role.Name == dto.RoleName).FirstOrDefaultAsync();

            if (role == null)
            {
                return $"Role {dto.RoleName} doesn't exist in database.";
            }

            var result = await _manager.DeleteAsync(role);

            return result.Succeeded ? $"Role {dto.RoleName} successfully removed from database." : $"Some internal error has occured.";
        }
    }
}
