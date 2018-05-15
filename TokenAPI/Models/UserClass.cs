﻿using TokenAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace TokenAPI.Models
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
