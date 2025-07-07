using FribergAdminWebApi.Models;
using Microsoft.AspNetCore.Identity;

namespace FribergAdminWebApi.Data
{
    public class ApiUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public virtual Employee? Employee { get; set; }
        public virtual Admin? Admin { get; set; }
    }
}
