using ClassroomAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models.TeacherViewModels
{
    public class TokenViewModel
    {
        public ICollection<Token> Tokens { get; set; } 
    }
}
