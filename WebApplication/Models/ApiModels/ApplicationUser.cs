using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ClassroomAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<UserToken> UserTokens { get; set; }
        public ICollection<UserClass> UserClass { get; set; } 
    }
}
