using FribergAdminWebApi.Models;

namespace FribergAdminWebApi.Data
{
    public class ApiUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public virtual Employee? Employee { get; set; }
        public virtual Admin? Admin { get; set; }
    }
}
