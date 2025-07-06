using FribergAdminWebApi.Data;
using System.ComponentModel.DataAnnotations;

namespace FribergAdminWebApi.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public decimal HourlyRate { get; set; }
        public string SocialSecurityNumber { get; set; } = string.Empty;
        
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;
        
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;      
        public string? ApiUserId { get; set; }
        public ApiUser? ApiUser { get; set; }

        //Emergency Contatct
        public string EmergencyContactName { get; set; } = string.Empty;

        [Phone]
        public string EmergencyContactPhone { get; set; } = string.Empty;
    }
}
