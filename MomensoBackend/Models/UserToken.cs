using MomensoBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RowcallBackend.Models
{
    public class UserToken
    {
        public string ApplicationUserId { get; set; }
        public int TokenId { get; set; }

        public virtual Token Token { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
