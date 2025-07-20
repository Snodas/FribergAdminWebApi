using FribergAdminWebApi.Models;

namespace FribergAdminWebApi.Data.Interfaces
{
    public interface ISalaryRepository : IRepository<Salary>
    {
        Task<IEnumerable<Salary>> GetSalariesByEmployeeAndYearAsync(int employeeId, int year);
        Task<IEnumerable<Salary>> GetSalariesByIdAndYearAsync(string userId, int year);
        Task<IEnumerable<Salary>> GetSalariesByEmployeeIdAsync(int employeeId); 
    }
}
