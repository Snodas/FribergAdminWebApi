using FribergAdminWebApi.Data.Interfaces;
using FribergAdminWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FribergAdminWebApi.Data.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee, ApiDbContext>, IEmployeeRepository
    {
        public EmployeeRepository(ApiDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees
                .Include(a => a.ApiUser)
                .ToListAsync();
        }

        public async Task<Employee?> GetByUserIdAsync(string userId)
        {
            return await _context.Employees
                .Include(e => e.ApiUser)
                .FirstOrDefaultAsync(e => e.ApiUserId == userId);
        }

        public async Task<IEnumerable<Employee>> GetEmployeesWithRecentWorkEntriesAsync()
        {
            var lastThirtyDays = DateTime.Now.AddDays(-30);
            return await _context.Employees
                .Include(e => e.ApiUser)
                .Include(e => e.WorkEntries.Where(w => w.Date >= lastThirtyDays))
                .ToListAsync();
        }
    }
}
