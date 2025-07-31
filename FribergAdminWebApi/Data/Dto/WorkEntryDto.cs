using System.ComponentModel.DataAnnotations;

namespace FribergAdminWebApi.Data.Dto
{
    public class WorkEntryDto
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan WorkDuration { get; set; }

        public int EmployeeId { get; set; }

        public decimal HourlyRateAtTimeOfWork { get; set; }

        public decimal TotalHours => (decimal)WorkDuration.TotalHours;

        public string EmployeeName { get; set; } = string.Empty;
    }
}
