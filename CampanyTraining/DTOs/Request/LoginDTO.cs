﻿using System.ComponentModel.DataAnnotations;

namespace CompanyTraining.DTOs.Request
{
    public class LoginDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string Role { get; set; } = null!;
    }
}
