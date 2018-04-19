using Microsoft.AspNetCore.Identity;
using RowcallBackend.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MomensoBackend.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<UserToken> UserTokens { get; set; }
        public ICollection<UserClass> UserClass { get; set; } 
    }
}
