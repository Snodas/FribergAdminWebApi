namespace FribergAdminWebApi.Models
{
    public class WorkEntry
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan WorkDuration { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; } //kolla det här med AI
        public decimal HourlyRateAtTimeOfWork { get; set; }
    }
}
