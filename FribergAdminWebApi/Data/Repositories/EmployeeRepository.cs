using FribergAdminWebApi.Data.Interfaces;
using FribergAdminWebApi.Models;

namespace FribergAdminWebApi.Data.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee, ApiDbContext>, IEmployeeRepository
    {
        public EmployeeRepository(ApiDbContext context) : base(context)
        {
        }
    }
}
