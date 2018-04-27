using System.ComponentModel.DataAnnotations;

namespace MomensoBackend.Models.AccountModels
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; } 
    }
}
