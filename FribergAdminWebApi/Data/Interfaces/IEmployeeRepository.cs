using FribergAdminWebApi.Models;

namespace FribergAdminWebApi.Data.Interfaces
{
    public interface IEmployeeRepository : IRepository<Employee>
    {            
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<IEnumerable<Employee>> GetEmployeesWithRecentWorkEntriesAsync();
        Task<Employee?> GetByUserIdAsync(string userId);
        Task<Employee?> GetEmployeeByUserIdAsync(string userId);
    }
}
