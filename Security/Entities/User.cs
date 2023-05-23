using Microsoft.AspNetCore.Identity;

namespace Security.Entities
{
    public class User : IdentityUser
    {
        public User() 
        { 
        
        }

        public User(string id) : base(id) 
        {
        
        }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
    }
}
