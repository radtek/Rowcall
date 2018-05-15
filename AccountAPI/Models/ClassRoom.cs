using AccountAPI.Models;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountAPI.Models
{
    public class ClassRoom
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("ApplicationUser")]
        public string TeacherId { get; set; }
        public virtual ApplicationUser Teacher { get; set; } 

        public ICollection<Token> Tokens { get; set; }
        public ICollection<UserClass> Students { get; set; }

    }
}
