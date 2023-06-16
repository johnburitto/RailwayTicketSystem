using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Security.Entities;

namespace Security.Data
{
    public class SecurityDbContext : IdentityDbContext<User>
    {
        public SecurityDbContext()
        {

        }

        public SecurityDbContext(DbContextOptions<SecurityDbContext> options) : base(options) 
        {
            
        }
    }
}
