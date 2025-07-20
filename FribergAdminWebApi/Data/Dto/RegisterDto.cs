using System.ComponentModel.DataAnnotations;

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

        [Phone]
        public string? PhoneNumber { get; set; }

        public decimal HourlyRate { get; set; }

        [Required]
        public string SocialSecurityNumber { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string EmergencyContactName { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string EmergencyContactPhone { get; set; } = string.Empty;
    }
}
