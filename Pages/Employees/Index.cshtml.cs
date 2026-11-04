using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyAppBenchAssignment.Data;
using MyAppBenchAssignment.Models;

namespace MyAppBenchAssignment.Pages.Employees;

public class IndexModel(AppDbContext context) : PageModel
{
    public IList<Employee> Employees { get; private set; } = [];

    [TempData]
    public string? StatusMessage { get; set; }

    public async Task OnGetAsync()
    {
        Employees = await context.Employees
            .AsNoTracking()
            .OrderBy(employee => employee.LastName)
            .ThenBy(employee => employee.FirstName)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var employee = await context.Employees.FindAsync(id);
        if (employee is not null)
        {
            context.Employees.Remove(employee);
            await context.SaveChangesAsync();
            StatusMessage = "Employee deleted.";
        }

        return RedirectToPage();
    }
}
