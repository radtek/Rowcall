using Microsoft.AspNetCore.Identity;
using TokenAPI.Models;
using System.Collections.Generic;

namespace TokenAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<UserToken> UserTokens { get; set; }
        public ICollection<UserClass> UserClass { get; set; } 
    }
}
