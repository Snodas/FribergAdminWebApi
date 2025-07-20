using FribergAdminWebApi.Data.Interfaces;
using FribergAdminWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FribergAdminWebApi.Data.Repositories
{
    public class SalaryRepository : GenericRepository<Salary, ApiDbContext>, ISalaryRepository
    {
        public SalaryRepository(ApiDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Salary>> GetSalariesByEmployeeAndYearAsync(int employeeId, int year)
        {
            return await _context.Salaries
                .Include(s => s.Employee)
                .Where(s => s.EmployeeId == employeeId && s.Year == year)
                .OrderBy(s => s.Month)
                .ToListAsync();
        }

        public async Task<IEnumerable<Salary>> GetSalariesByEmployeeIdAsync(int employeeId)
        {
            return await _context.Salaries
                .Include(s => s.Employee)
                .Where(s => s.EmployeeId == employeeId)
                .OrderByDescending(s => s.Year)
                .ThenByDescending(s => s.Month)
                .ToListAsync();
        }

        public async Task<IEnumerable<Salary>> GetSalariesByIdAndYearAsync(string userId, int year)
        {
            return await _context.Salaries
                .Include(s => s.Employee)
                .Where(s => s.Employee.ApiUserId == userId && s.Year == year)
                .OrderBy(s => s.Month)
                .ToListAsync();
        }
    }
}
