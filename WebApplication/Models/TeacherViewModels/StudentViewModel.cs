using ClassroomAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models.TeacherViewModels
{
    public class StudentViewModel
    {
        public string ClassroomId { get; set; }
        public ICollection<string> Users { get; set; }
    }
}
