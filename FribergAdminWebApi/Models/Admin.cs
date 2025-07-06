using FribergAdminWebApi.Data;
using System.ComponentModel.DataAnnotations;

namespace FribergAdminWebApi.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string? ApiUserId { get; set; }
        public ApiUser? ApiUser { get; set; }
    }
}
