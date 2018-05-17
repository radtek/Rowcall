using System.ComponentModel.DataAnnotations;

namespace AccountAPI.Models.AccountModels
{
    public class StudentLoginModel:LoginModel
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public double Longtitude { get; set; }

        [Required]
        public double Latitude { get; set; }
    }
}
