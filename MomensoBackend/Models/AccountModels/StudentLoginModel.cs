using System.ComponentModel.DataAnnotations;

namespace RowcallBackend.Models.AccountModels
{
    public class StudentLoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string TokenValue { get; set; }

    }
}
