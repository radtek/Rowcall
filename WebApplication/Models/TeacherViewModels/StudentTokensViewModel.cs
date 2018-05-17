using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models.TeacherViewModels
{
    public class StudentTokensViewModel
    {
        public ICollection<string> Present { get; set; }
        public ICollection<string> NotPresent { get; set; } 
    }
}
