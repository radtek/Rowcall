using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models.HomeViewModels
{
    public class StudentViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
