using FribergAdminWebApi.Models;

namespace FribergAdminWebApi.Data.Interfaces
{
    public interface IWorkEntryRepository : IRepository<WorkEntry>
    {
        Task<List<WorkEntry>> GetWorkEntriesByEmployeeIdAsync(int employeeId);
        Task<List<WorkEntry>> GetWorkEntriesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<WorkEntry>> GetWorkEntriesByEmployeeAndDateRangeAsync(int employeeId, DateTime startDate, DateTime endDate);
    }
}
