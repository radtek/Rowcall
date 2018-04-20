using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RowcallBackend.Models.ClassRoomModels
{
    public class AddUserDto
    {
        public string Email { get; set; }
        public int ClassRoomId { get; set; } 
    }
}
