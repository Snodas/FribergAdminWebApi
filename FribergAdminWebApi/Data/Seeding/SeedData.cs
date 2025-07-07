using Microsoft.AspNetCore.Identity;

namespace FribergAdminWebApi.Data.Seeding
{
    public class SeedData
    {
        public static async Task SeedAsync(ApiDbContext context, UserManager<ApiUser> userManager, RoleManager<IdentityRole> identityRole)
        {
            await UserAndRoleSeeding.SeedRolesAsync(identityRole);

            if (!context.Employees.Any())
            {
                await UserAndRoleSeeding.SeedUsersAndEmployeesAsync(userManager, context);
            }

            if (!context.WorkEntries.Any())
            {
                await WorkEntrySeeding.SeedWorkEntriesAsync()
            }
        }

    }
}
