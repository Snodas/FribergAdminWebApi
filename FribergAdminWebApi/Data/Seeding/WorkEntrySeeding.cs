using FribergAdminWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FribergAdminWebApi.Data.Seeding
{
    public static class WorkEntrySeeding
    {
        public static async Task SeedWorkEntriesAsync(ApiDbContext context)
        {
            var employees = await context.Employees.ToListAsync();
            if (!employees.Any()) return;

            var workEntries = new List<WorkEntry>();
            var random = new Random();

            foreach (var employee in employees)
            {
                for (int i = 1; i <= 150; i++)
                {
                    var workDate = DateTime.Now.AddDays(-i);

                    if (workDate.DayOfWeek == DayOfWeek.Saturday || workDate.DayOfWeek == DayOfWeek.Sunday)
                        continue;

                    if (random.Next(1, 11) <= 2)
                        continue;

                    var workEntry = new WorkEntry
                    {
                        EmployeeId = employee.Id,
                        Date = workDate.Date,
                        WorkDuration = TimeSpan.FromHours(random.Next(6, 9))
                                     + TimeSpan.FromMinutes(random.Next(0, 60)),
                        HourlyRateAtTimeOfWork = employee.HourlyRate
                    };

                    workEntries.Add(workEntry);
                }
            }

            await context.WorkEntries.AddRangeAsync(workEntries);
            await context.SaveChangesAsync();
        }
    }
}