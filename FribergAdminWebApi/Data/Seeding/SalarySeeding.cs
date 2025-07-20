using FribergAdminWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FribergAdminWebApi.Data.Seeding
{
    public static class SalarySeeding
    {
        public static async Task SeedSalariesAsync(ApiDbContext context)
        {
            var employees = await context.Employees.ToListAsync();
            if (!employees.Any()) return;

            var salaries = new List<Salary>();

            for (int monthsBack = 3; monthsBack >= 1; monthsBack--)
            {
                var targetDate = DateTime.Now.AddMonths(-monthsBack);
                var year = targetDate.Year;
                var month = targetDate.Month;

                var periodStart = new DateTime(year, month, 1);
                var periodEnd = periodStart.AddMonths(1).AddDays(-1);

                foreach (var employee in employees)
                {
                    var workEntries = await context.WorkEntries
                        .Where(w => w.EmployeeId == employee.Id &&
                                    w.Date >= periodStart &&
                                    w.Date <= periodEnd)
                        .ToListAsync();

                    if (!workEntries.Any()) continue;

                    var totalHours = workEntries.Sum(w => (decimal)w.WorkDuration.TotalHours);
                    var grossWages = workEntries.Sum(w =>
                        (decimal)w.WorkDuration.TotalHours * w.HourlyRateAtTimeOfWork);

                    var taxRate = 0.3m; 
                    var pensionRate = 0.07m;

                    var taxDeduction = grossWages * taxRate;
                    var pensionDeduction = grossWages * pensionRate;
                    var netWages = grossWages - taxDeduction - pensionDeduction;

                    var salary = new Salary
                    {
                        EmployeeId = employee.Id,
                        Year = year,
                        Month = month,
                        PeriodStart = periodStart,
                        PeriodEnd = periodEnd,
                        TotalHours = Math.Round(totalHours, 2),
                        GrossWages = Math.Round(grossWages, 2),
                        TaxDeduction = Math.Round(taxDeduction, 2),
                        PensionDeduction = Math.Round(pensionDeduction, 2),
                        NetWages = Math.Round(netWages, 2),
                        CreatedAt = periodEnd.AddDays(5),
                        IsPaid = monthsBack > 1,
                        PaidDate = monthsBack > 1 ? periodEnd.AddDays(10) : null
                    };

                    salaries.Add(salary);
                }
            }

            await context.Salaries.AddRangeAsync(salaries);
            await context.SaveChangesAsync();
        }
    }
}
