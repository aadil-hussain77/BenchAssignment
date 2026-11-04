using Microsoft.EntityFrameworkCore;
using MyAppBenchAssignment.Models;

namespace MyAppBenchAssignment.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(AppDbContext context)
    {
        if (await context.Employees.AnyAsync())
        {
            return;
        }

        context.Employees.AddRange(
            new Employee
            {
                EmployeeCode = "EMP001",
                FirstName = "Aisha",
                LastName = "Khan",
                Email = "aisha.khan@example.com",
                Department = "Engineering",
                Designation = "Software Engineer"
            },
            new Employee
            {
                EmployeeCode = "EMP002",
                FirstName = "David",
                LastName = "Miller",
                Email = "david.miller@example.com",
                Department = "Operations",
                Designation = "Operations Manager"
            });

        await context.SaveChangesAsync();
    }
}
