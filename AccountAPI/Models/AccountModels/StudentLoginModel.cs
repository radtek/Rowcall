﻿using System.ComponentModel.DataAnnotations;

namespace AccountAPI.Models.AccountModels
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

        [Required]
        public double Longtitude { get; set; }

        [Required]
        public double Latitude { get; set; }
    }
}
