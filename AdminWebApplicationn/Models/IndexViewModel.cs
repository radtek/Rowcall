using MomensoBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminWebApplicationn.Models
{
    public class IndexViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public List<ApplicationUser> ApplicationUserList { get; set; }
    }
}
