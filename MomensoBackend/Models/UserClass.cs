using MomensoBackend.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RowcallBackend.Models
{
    public class UserClass
    {
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public int ClassRoomId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ClassRoom ClassRoom { get; set; }

    }
}
