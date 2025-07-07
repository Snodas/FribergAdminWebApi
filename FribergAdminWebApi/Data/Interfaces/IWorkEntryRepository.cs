using FribergAdminWebApi.Models;

namespace FribergAdminWebApi.Data.Interfaces
{
    public interface IWorkEntryRepository : IRepository<WorkEntry>
    {
        Task<List<WorkEntry>> GetWorkEntriesByEmployeeIdAsync(int employeeId);
    }
}
