using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RowcallBackend.Models
{
    public class Token
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("ClassRoom")]
        public int ClassId { get; set; }    

        public string Value { get; set; }
        public int Duration { get; set; } 
        public DateTime CreatedDateTime { get; set; }

        public virtual ClassRoom ClassRoom { get; set; }
        public virtual ICollection<UserToken> UserToken { get; set; }
    }
}
