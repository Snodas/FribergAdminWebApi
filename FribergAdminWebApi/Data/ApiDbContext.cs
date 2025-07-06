using FribergAdminWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FribergAdminWebApi.Data
{
    public class ApiDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<WorkEntry> WorkEntries { get; set; }


        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }
        

    }
}
