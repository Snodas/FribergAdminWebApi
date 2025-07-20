namespace FribergAdminWebApi.Data.Dto
{
    public class SalaryDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public int Year { get; set; }
        public int Month { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public decimal TotalHours { get; set; }
        public decimal GrossWages { get; set; }
        public decimal TaxDeduction { get; set; }
        public decimal PensionDeduction { get; set; }
        public decimal NetWages { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PaidDate { get; set; }
        public bool IsPaid { get; set; }
    }
}
