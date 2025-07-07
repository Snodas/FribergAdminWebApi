using FribergAdminWebApi.Constants;
using Microsoft.AspNetCore.Identity;

namespace FribergAdminWebApi.Data.Seeding
{
    public static class UserAndRoleSeeding
    {
        public static async Task SeedUsersAndEmployeesAsync(UserManager<ApiUser> userManager, ApiDbContext context)
        {

        }


        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = new[]
            {
                ApiRoles.User,
                ApiRoles.Admin,
                ApiRoles.Employee
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
