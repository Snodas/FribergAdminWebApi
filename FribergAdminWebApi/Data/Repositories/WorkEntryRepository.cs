using FribergAdminWebApi.Data.Interfaces;
using FribergAdminWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FribergAdminWebApi.Data.Repositories
{
    public class WorkEntryRepository : GenericRepository<WorkEntry, ApiDbContext>, IWorkEntryRepository
    {
        private readonly ApiDbContext _context;
        public WorkEntryRepository(ApiDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<WorkEntry>> GetWorkEntriesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.WorkEntries
                .Include(we => we.Employee)
                .Where(we => we.Date >= startDate && we.Date <= endDate)
                .OrderByDescending(we => we.Date)
                .ToListAsync();
        }

        public async Task<List<WorkEntry>> GetWorkEntriesByEmployeeAndDateRangeAsync(int employeeId, DateTime startDate, DateTime endDate)
        {
            return await _context.WorkEntries
                .Include(we => we.Employee)
                .Where(we => we.EmployeeId == employeeId && we.Date >= startDate && we.Date <= endDate)
                .OrderByDescending(we => we.Date)
                .ToListAsync();
        }

        public async Task<List<WorkEntry>> GetWorkEntriesByEmployeeIdAsync(int employeeId)
        {
            return await _context.WorkEntries
                .Include(we => we.Employee)
                .Where(we => we.EmployeeId == employeeId)
                .OrderByDescending(we => we.Date)
                .ToListAsync();
        }
    }
}
