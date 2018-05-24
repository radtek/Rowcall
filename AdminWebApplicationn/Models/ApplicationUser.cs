using Microsoft.AspNetCore.Identity;
using RowcallBackend.Models;
using System.Collections.Generic;

namespace MomensoBackend.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<UserToken> UserTokens { get; set; }
        public ICollection<UserClass> UserClass { get; set; } 
    }
}
