using FribergAdminWebApi.Constants;
using FribergAdminWebApi.Models;
using Microsoft.AspNetCore.Identity;

namespace FribergAdminWebApi.Data.Seeding
{
    public static class UserAndRoleSeeding
    {
        public static async Task SeedUsersAndEmployeesAsync(UserManager<ApiUser> userManager, ApiDbContext context)
        {
            //Admin Seeding

            var admin1 = new ApiUser
            {
                UserName = "admin1@friberg.com",
                Email = "admin1@friberg.com",
                NormalizedUserName = "ADMIN1@FRIBERG.COM",
                NormalizedEmail = "ADMIN1@FRIBERG.COM",
                EmailConfirmed = true,
                FirstName = "Viktor",
                LastName = "Hansson"
            };

            var admin1Result = await userManager.CreateAsync(admin1, "Admin123!");

            if (admin1Result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin1, ApiRoles.Admin);
                var admin1Entity = new Admin
                {
                    FirstName = "Viktor",
                    LastName = "Hansson",
                    Email = admin1.Email,
                    ApiUserId = admin1.Id
                };
                context.Admins.Add(admin1Entity);
            }

            //Admin 2

            var admin2 = new ApiUser
            {
                UserName = "admin2@friberg.com",
                Email = "admin2@friberg.com",
                NormalizedUserName = "ADMIN2@FRIBERG.COM",
                NormalizedEmail = "ADMIN2@FRIBERG.COM",
                EmailConfirmed = true,
                FirstName = "Micke",
                LastName = "Hansson"
            };

            var admin2result = await userManager.CreateAsync(admin2, "Admin123!");

            if (admin2result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin2, ApiRoles.Admin);
                var admin2Entity = new Admin
                {
                    FirstName = "Micke",
                    LastName = "Hansson",
                    Email = admin2.Email,
                    ApiUserId = admin2.Id
                };
                context.Admins.Add(admin2Entity);
            }

            //Employee Seeding

            var employee1 = new ApiUser
            {
                UserName = "employee1@friberg.com",
                Email = "employee1@friberg.com",
                NormalizedUserName = "EMPLOYEE1@FRIBERG.COM",
                NormalizedEmail = "EMPLOYEE1@FRIBERG.COM",
                EmailConfirmed = true,
                FirstName = "Alexander",
                LastName = "Eriksson"
            };

            var result1 = await userManager.CreateAsync(employee1, "Employee123!");

            if (result1.Succeeded)
            {
                await userManager.AddToRoleAsync(employee1, ApiRoles.Employee);
                var emp1 = new Employee
                {
                    FirstName = "Alexander",
                    LastName = "Eriksson",
                    Email = employee1.Email,
                    HourlyRate = 50,
                    SocialSecurityNumber = "1234567890",
                    PhoneNumber = "0701234567",
                    Address = "123 Main St, City, Country",
                    EmergencyContactName = "Mom",
                    EmergencyContactPhone = "0707654321",
                    ApiUserId = employee1.Id
                };
                context.Employees.Add(emp1);
            }

            //Emp 2

            var employee2 = new ApiUser
            {
                UserName = "employee2@friberg.com",
                Email = "employee2@friberg.com",
                NormalizedUserName = "EMPLOYEE2@FRIBERG.COM",
                NormalizedEmail = "EMPLOYEE2@FRIBERG.COM",
                EmailConfirmed = true,
                FirstName = "Hugo",
                LastName = "Rosdhal"
            };

            var result2 = await userManager.CreateAsync(employee2, "Employee123!");

            if (result2.Succeeded)
            {
                await userManager.AddToRoleAsync(employee2, ApiRoles.Employee);
                var emp2 = new Employee
                {
                    FirstName = "Hugo",
                    LastName = "Rosdhal",
                    Email = employee2.Email,
                    HourlyRate = 200,
                    SocialSecurityNumber = "0987654321",
                    PhoneNumber = "0709876543",
                    Address = "456 Elm St, City, Country",
                    EmergencyContactName = "Hadok",
                    EmergencyContactPhone = "0701234567",
                    ApiUserId = employee2.Id
                };
                context.Employees.Add(emp2);
            }

            //Emp 3

            var employee3 = new ApiUser
            {
                UserName = "employee3@friberg.com",
                Email = "employee3@friberg.com",
                NormalizedUserName = "EMPLOYEE3@FRIBERG.COM",
                NormalizedEmail = "EMPLOYEE3@FRIBERG.COM",
                EmailConfirmed = true,
                FirstName = "Jim",
                LastName = "Svedin"
            };

            var result3 = await userManager.CreateAsync(employee3, "Employee123!");

            if (result3.Succeeded)
            {
                await userManager.AddToRoleAsync(employee3, ApiRoles.Employee);
                var emp3 = new Employee
                {
                    FirstName = "Jim",
                    LastName = "Svedin",
                    Email = employee3.Email,
                    HourlyRate = 150,
                    SocialSecurityNumber = "1122334455",
                    PhoneNumber = "0701122334",
                    Address = "789 Oak St, City, Country",
                    EmergencyContactName = "Girlfriend",
                    EmergencyContactPhone = "0705566778",
                    ApiUserId = employee3.Id
                };
                context.Employees.Add(emp3);
            }

            await context.SaveChangesAsync();
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
