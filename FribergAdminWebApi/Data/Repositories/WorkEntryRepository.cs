using FribergAdminWebApi.Data.Interfaces;
using FribergAdminWebApi.Models;

namespace FribergAdminWebApi.Data.Repositories
{
    public class WorkEntryRepository : GenericRepository<WorkEntry, ApiDbContext>, IWorkEntryRepository
    {
        public WorkEntryRepository(ApiDbContext context) : base(context)
        {
        }

        public async Task<List<WorkEntry>> GetWorkEntriesByEmployeeIdAsync(int employeeId)
        {
            throw new NotImplementedException();
        }
    }
}
