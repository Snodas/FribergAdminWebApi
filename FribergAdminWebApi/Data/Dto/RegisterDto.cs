﻿using System.ComponentModel.DataAnnotations;

namespace FribergAdminWebApi.Data.Dto
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }
    }
}
