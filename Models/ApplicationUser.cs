using Microsoft.AspNetCore.Identity;

namespace School.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime BirthDay { get; set; }
    }
}
