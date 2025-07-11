
using FribergAdminWebApi.Data;
using FribergAdminWebApi.Data.Interfaces;
using FribergAdminWebApi.Data.Repositories;
using FribergAdminWebApi.Data.Seeding;
using FribergAdminWebApi.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

namespace FribergAdminWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddDbContext<ApiDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));           

            //Injecting

            builder.Services.AddScoped<IWorkEntryRepository, WorkEntryRepository>();
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            //builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

            //Identity
            builder.Services.AddIdentityCore<ApiUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApiDbContext>();

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    b => b.AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin());
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            //Seed
            using (var scope = app.Services.CreateScope())
            {
                var apiDbContext = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApiUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                SeedData.SeedAsync(apiDbContext, userManager, roleManager).Wait();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
