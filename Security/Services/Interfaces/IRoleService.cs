using Microsoft.AspNetCore.Identity;
using Security.Dto;

namespace Security.Services.Interfaces
{
    public interface IRoleService
    {
        public Task<List<IdentityRole>> GetAllAsync();
        public Task<string> AddAsync(RoleDto dto);
        public Task<string> RemoveAsync(RoleDto dto);
    }
}
