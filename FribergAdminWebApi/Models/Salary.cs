namespace FribergAdminWebApi.Models
{
    public class Salary
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; } // Navigation property to Employee

    }
}
