﻿using System.ComponentModel.DataAnnotations;

namespace AchillService.Models
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public bool Gender { get; set; }

        [Required]
        public int Type { get; set; }
    }
}
