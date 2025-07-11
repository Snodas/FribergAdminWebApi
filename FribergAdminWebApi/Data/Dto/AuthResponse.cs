using Microsoft.Extensions.Primitives;

namespace FribergAdminWebApi.Data.Dto
{
    public class AuthResponse
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
    }
}
