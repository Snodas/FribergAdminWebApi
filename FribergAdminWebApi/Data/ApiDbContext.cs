using FribergAdminWebApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FribergAdminWebApi.Data
{
    public class ApiDbContext : IdentityDbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<WorkEntry> WorkEntries { get; set; }


        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure the primary key for the Employee table
            modelBuilder.Entity<Employee>()
                .HasKey(e => e.Id);
            // Configure the primary key for the Admin table
            modelBuilder.Entity<Admin>()
                .HasKey(a => a.Id);
            // Configure the primary key for the WorkEntry table
            modelBuilder.Entity<WorkEntry>()
                .HasKey(w => w.Id);
            // Configure relationships if necessary
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.WorkEntries)
                .WithOne(w => w.Employee)
                .HasForeignKey(w => w.EmployeeId);

            modelBuilder.Entity<Employee>()
                .HasOne(r => r.ApiUser)
                .WithOne(a => a.Employee)
                .HasForeignKey<Employee>(r => r.ApiUserId)
                .OnDelete(DeleteBehavior.Restrict);

            //Admin till apiuser
            modelBuilder.Entity<Admin>()
                .HasOne(a => a.ApiUser)
                .WithOne(a => a.Admin)
                .HasForeignKey<Admin>(a => a.ApiUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
