using Microsoft.AspNetCore.Identity;
using AccountAPI.Models;
using System.Collections.Generic;

namespace AccountAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<UserToken> UserTokens { get; set; }
        public ICollection<UserClass> UserClass { get; set; } 
    }
}
